using System;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using MetadataExtractCore.Analysis;
using MetadataExtractCore.Utilities;

namespace MetadataExtractCore.Metadata
{
    public class SVGDocument : MetaExtractor
    {
        public SVGDocument() { }

        public SVGDocument(Stream stm)
        {
            this.stm = new MemoryStream();
            Functions.CopyStream(stm, this.stm);
        }

        public override void analyzeFile()
        {
            XmlTextReader avgReader = null;

            try
            {
                avgReader = new XmlTextReader(this.stm) {XmlResolver = null};
                avgReader.Read();

                while (avgReader.Read())
                {
                    // node's value, example: <a>/home/user/file</a>
                    if (CheckPath(avgReader.Value))
                    {
                        var cleanPath = PathAnalysis.CleanPath(avgReader.Value);
                        var user = PathAnalysis.ExtractUserFromPath(cleanPath);
                        if (user != string.Empty)
                            FoundUsers.AddUniqueItem(user, true);
                        FoundPaths.AddUniqueItem(cleanPath, true);
                    }

                    while (avgReader.MoveToNextAttribute())
                    {
                        // attribute's value, example: <a atrib="/home/user/file"/>
                        if (!CheckPath(avgReader.Value)) continue;
                        var cleanPath = PathAnalysis.CleanPath(avgReader.Value);
                        var user = PathAnalysis.ExtractUserFromPath(cleanPath);
                        if (user != string.Empty)
                            FoundUsers.AddUniqueItem(user, true);
                        FoundPaths.AddUniqueItem(cleanPath, true);
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
        }

        private static bool CheckPath(string path)
        {
            return Regex.Matches(path, @"^([a-z]:|\\\\|/)", RegexOptions.IgnoreCase).Count > 0;
        }
    }
}
