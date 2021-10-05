using System;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using MetadataExtractCore.Analysis;
using MetadataExtractCore.Utilities;
using MetadataExtractCore.Diagrams;

namespace MetadataExtractCore.Extractors
{
    public class SVGDocument : DocumentExtractor
    {
        public SVGDocument(Stream stm):base(stm)
        {
        }

        public override FileMetadata AnalyzeFile()
        {
            XmlTextReader avgReader = null;

            try
            {
                this.foundMetadata = new FileMetadata();
                avgReader = new XmlTextReader(this.fileStream) {XmlResolver = null};
                avgReader.Read();

                while (avgReader.Read())
                {
                    // node's value, example: <a>/home/user/file</a>
                    if (CheckPath(avgReader.Value))
                    {
                        var cleanPath = PathAnalysis.CleanPath(avgReader.Value);
                        var user = PathAnalysis.ExtractUserFromPath(cleanPath);
                        if (user != string.Empty)
                            this.foundMetadata.Add(new User(user, true));
                        this.foundMetadata.Add(new Diagrams.Path(cleanPath, true));
                    }

                    while (avgReader.MoveToNextAttribute())
                    {
                        // attribute's value, example: <a atrib="/home/user/file"/>
                        if (!CheckPath(avgReader.Value)) continue;
                        var cleanPath = PathAnalysis.CleanPath(avgReader.Value);
                        var user = PathAnalysis.ExtractUserFromPath(cleanPath);
                        if (user != string.Empty)
                            this.foundMetadata.Add(new User(user, true));
                        this.foundMetadata.Add(new Diagrams.Path(cleanPath, true));
                    }

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("error: " + ex.Message);
            }
            finally
            {
                avgReader?.Close();
            }
            return this.foundMetadata;
        }

        private static bool CheckPath(string path)
        {
            return Regex.Matches(path, @"^([a-z]:|\\\\|/)", RegexOptions.IgnoreCase).Count > 0;
        }
    }
}
