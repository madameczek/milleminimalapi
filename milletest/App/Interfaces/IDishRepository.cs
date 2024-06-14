using milletest.Shared.App.Models;
using milletest.Shared.Common;

namespace milletest.App.Interfaces;

public interface IDishRepository
{
    public Task<Result<IEnumerable<Dish>>> GetDishes(CancellationToken cancellationToken);
    public Task<Result<Dish>> AddDish(Dish dish, CancellationToken cancellationToken);
}