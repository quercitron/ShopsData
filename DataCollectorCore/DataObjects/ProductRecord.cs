using System;

namespace DataCollectorCore.DataObjects
{
    public class ProductRecord
    {
        public int ProductRecordId { get; set; }

        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public float Rating { get; set; }

        public int AmountAvailable { get; set; }

        public DateTime Timestamp { get; set; }
    }
}