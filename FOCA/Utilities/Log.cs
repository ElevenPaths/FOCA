using System;

namespace FOCA
{
    public class Log
    {
        public enum LogType
        {
            debug = 0,
            error = 1,
            low = 2,
            medium = 3,
            high = 4
        }

        public enum ModuleType
        {
            FingingerPrinting = 0,
            ShodanSearch = 1,
            Shodan = 2,
            ScanIPRangeICMP = 3,
            DNSSearch = 4,
            IPBingSearch = 5,
            MultipleChoices = 6,
            TransferZone = 7,
            FOCA = 8,
            WebSearch = 9,
            Crawling = 10,
            Fuzzer = 11,
            DNS = 12,
            TechnologyRecognition = 13,
            MetadataSearch = 14,
            DNSCommonNames = 15,
            IPRangeSearch = 16,
            ProxySearch = 17,
            AutoSave = 18,
        }

        public ModuleType module;
        public string text;
        public string time;
        public LogType type;

        public Log(ModuleType module, string text, LogType type)
        {
            time = DateTime.Now.ToLongTimeString();
            this.type = type;
            this.text = text;
            this.module = module;
        }
    }
}