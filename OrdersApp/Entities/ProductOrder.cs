namespace OrdersApp.Entities;

public class ProductOrder
{
    public int ProductId { get; set; }
    public int OrderId { get; set; }
    public int Amount { get; set; }

    public Product Product { get; set; } = null!;
    public Order Order { get; set; } = null!;
}
