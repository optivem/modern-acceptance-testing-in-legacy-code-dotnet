using Optivem.AtddAccelerator.EShop.Monolith.Core.Entities;

namespace Optivem.AtddAccelerator.EShop.Monolith.Core.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }

    public Order? FindById(string orderNumber)
    {
        return _context.Orders.Find(orderNumber);
    }

    public void Save(Order order)
    {
        var existing = _context.Orders.Find(order.OrderNumber);
        if (existing == null)
        {
            _context.Orders.Add(order);
        }
        else
        {
            _context.Entry(existing).CurrentValues.SetValues(order);
        }
        _context.SaveChanges();
    }
}
