namespace DataCollectorCore.DataObjects
{
    public class UserProduct
    {
        public int UserProductId { get; set; }

        public int UserId { get; set; }

        public int? ProductId { get; set; }

        public string ProductName { get; set; }
    }
}
