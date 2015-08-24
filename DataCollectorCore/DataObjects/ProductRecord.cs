using System;

namespace DataCollectorCore.DataObjects
{
    public class ProductRecord
    {
        public int ProductRecordId { get; set; }

        public int SourceProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public float Rating { get; set; }

        public int AmountAvailable { get; set; }

        public DateTime Timestamp { get; set; }

        public int LocationId { get; set; }

        public string ExternalId { get; set; }

        public string Brand { get; set; }

        public override string ToString()
        {
            return string.Format("Name: {0}, Price: {1}, Description: {2}, Rating: {3}", Name, Price, Description, Rating);
        }
    }
}