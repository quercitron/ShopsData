using System;

namespace DataCollectorCore.DataObjects
{
    public class Product
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Class { get; set; }

        public int ProductTypeId { get; set; }

        public DateTime Created { get; set; }

        public string Code { get; set; }
    }
}
