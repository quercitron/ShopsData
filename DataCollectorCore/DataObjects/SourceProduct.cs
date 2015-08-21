namespace DataCollectorCore.DataObjects
{
    public class SourceProduct
    {
        public int SourceProductId { get; set; }

        public int ProductId { get; set; }

        public int DataSourceId { get; set; }

        public string Key { get; set; }

        public string Name { get; set; }

        public string OriginalName { get; set; }
    }
}
