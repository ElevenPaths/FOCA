using System;

namespace FOCA.Database.Entities
{
    [Serializable]
    public class MetaData
    {

        public int Id { get; set; }
        public string Title { get; set; }

        public Applications Applications { get; set; }

        public string Subject { get; set; }

        public string DataBase { get; set; }

        public string Category { get; set; }

        public string Codification { get; set; }

        public string Comments { get; set; }

        public string Company { get; set; }

        public string Description { get; set; }

        public string Statistic { get; set; }

        public string Language { get; set; }

        public string UserInfo { get; set; }

        public decimal VersionNumber { get; set; }

        public string Keywords { get; set; }

        public string Template { get; set; }

        public string OperativeSystem { get; set; }

        public decimal EditTime { get; set; }

        public string Model { get; set; }

        public MetaData()
        {
            Applications = new Applications();
        }
    }
}
