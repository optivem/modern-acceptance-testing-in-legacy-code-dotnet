using Optivem.AtddAccelerator.EShop.Monolith.Core.Entities;

namespace Optivem.AtddAccelerator.EShop.Monolith.Core.Repositories;

public interface IOrderRepository
{
    Order? FindById(string orderNumber);
    void Save(Order order);
}
