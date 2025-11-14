using Microsoft.EntityFrameworkCore;
using Optivem.EShop.Monolith.Core.Entities;

namespace Optivem.EShop.Monolith.Core.Repositories;

public class OrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task<Order?> FindByIdAsync(string orderNumber)
    {
        return await _context.Orders.FindAsync(orderNumber);
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }
}
