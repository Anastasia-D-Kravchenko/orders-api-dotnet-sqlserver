namespace OrdersApp.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? FulfilledAt { get; set; }   // nullable per diagram
    public int ClientId { get; set; }
    public int StatusId { get; set; }

    public Client Client { get; set; } = null!;
    public Status Status { get; set; } = null!;
    public ICollection<ProductOrder> ProductOrders { get; set; } = new List<ProductOrder>();
}
