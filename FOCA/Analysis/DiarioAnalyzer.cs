﻿using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;

namespace FOCA.Analysis
{
    public class DiarioAnalyzer
    {
        private const int MaxRetries = 3;
        private const int FileNotFoundErrorCode = 406;
        private const string Analyzed = "A";
        private const string Processing = "P";
        private const string Failed = "F";

        private static readonly TimeSpan DelayBetweenRetries = TimeSpan.FromSeconds(3);
        private DiarioSDKNet.Diario sdk;

        private TaskScheduler currentScheduler;

        public static readonly string[] SupportedExtensions = new string[] { ".docx", ".xlsx", ".doc", ".xls", ".pdf" };

        public DiarioAnalyzer(string apiKey, string secret)
        {
            if (String.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentNullException(nameof(apiKey));

            if (String.IsNullOrWhiteSpace(secret))
                throw new ArgumentNullException(nameof(secret));

            this.sdk = new DiarioSDKNet.Diario(apiKey, secret);
            this.currentScheduler = new QueuedTaskScheduler(TaskScheduler.Default, Environment.ProcessorCount);
        }

        private static string NormalizeExtension(string extension)
        {
            if (String.IsNullOrWhiteSpace(extension))
                throw new ArgumentNullException(nameof(extension));

            if (!extension.StartsWith("."))
                extension = "." + extension;

            return extension.ToLowerInvariant().Trim();
        }

        public static bool IsSupportedExtension(string extension)
        {
            if (String.IsNullOrWhiteSpace(extension))
                return false;

            string normalizedExtension = NormalizeExtension(extension);
            return SupportedExtensions.Any(p => p.Equals(normalizedExtension));
        }

        public void CheckMalware(string filePath, Action<DiarioFileAnalysis> finishCallback, CancellationToken token = default(CancellationToken))
        {
            this.CheckMalware(new DiarioFileAnalysis(filePath, finishCallback, token));
        }

        public void CheckMalware(DiarioFileAnalysis file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            Task.Factory.StartNew(async () => await CheckFile(file), file.CancelToken, TaskCreationOptions.DenyChildAttach | TaskCreationOptions.HideScheduler, this.currentScheduler);
        }

        private async Task CheckFile(DiarioFileAnalysis file)
        {
            try
            {
                if (file.Retries > MaxRetries || file.CancelToken.IsCancellationRequested)
                {
                    file.Completed = true;
                    file.Callback(file);
                }
                else
                {
                    string fileHash;
                    byte[] fileContent;
                    using (SHA256Managed sha256 = new SHA256Managed())
                    {
                        fileContent = File.ReadAllBytes(file.FilePath);
                        fileHash = BitConverter.ToString(sha256.ComputeHash(fileContent)).Replace("-", "").ToLowerInvariant();
                    }

                    BaseSDK.ApiResponse<dynamic> diarioResponse = this.sdk.Search(fileHash);
                    if (diarioResponse != null)
                    {
                        if (diarioResponse.Error != null)
                        {
                            if (diarioResponse.Error?.Code == FileNotFoundErrorCode)
                            {
                                diarioResponse = this.sdk.Upload(fileContent, Path.GetFileName(file.FilePath));
                                file.Retries++;
                                await Task.Delay(DelayBetweenRetries);
                                this.CheckMalware(file);
                            }
                            else
                            {
                                file.Error = diarioResponse.Error.Message;
                                file.Callback(file);
                            }
                        }
                        else if (diarioResponse.Data?["status"] == Analyzed)
                        {
                            file.Prediction = DiarioSDKNet.Diario.GetPredictonFromString(diarioResponse.Data["prediction"]);
                            file.Completed = true;
                            file.Callback(file);
                        }
                        else if (diarioResponse.Data?["status"] == Processing)
                        {
                            file.Retries++;
                            await Task.Delay(DelayBetweenRetries);
                            this.CheckMalware(file);
                        }
                        else if (diarioResponse.Data?["status"] == Failed)
                        {
                            file.Prediction = DiarioSDKNet.Diario.Prediction.Unknown;
                            file.Callback(file);
                        }
                    }
                    else
                    {
                        file.Completed = false;
                        file.Callback(file);
                    }
                }
            }
            catch (Exception)
            {
                file.Completed = false;
                file.Callback(file);
            }
        }
    }
}
