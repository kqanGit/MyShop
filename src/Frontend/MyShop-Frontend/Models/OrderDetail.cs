namespace MyShop_Frontend.Models
{
    /// <summary>
    /// OrderDetail Entity - Pure domain model
    /// </summary>
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal CurrentCost { get; set; }
        public decimal TotalLine { get; set; }

        // Navigation properties
        public OrderEntity? Order { get; set; }
        public Product? Product { get; set; }

        // Computed property
        public string ProductName => Product?.ProductName ?? "Unknown";
    }
}
