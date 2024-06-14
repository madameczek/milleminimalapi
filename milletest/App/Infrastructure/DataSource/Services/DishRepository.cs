using Microsoft.EntityFrameworkCore;
using milletest.App.Infrastructure.DataSource.DbContexts;
using milletest.App.Interfaces;
using milletest.Shared.App.Models;
using milletest.Shared.Common;

namespace milletest.App.Infrastructure.DataSource.Services;

public class DishRepository : IDishRepository
{
    private readonly DishesDbContext _dbContext;

    public DishRepository(DishesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<IEnumerable<Dish>>> GetDishes(CancellationToken ct = default)
    {
        try
        {
            var dishes = await _dbContext.Dishes
                //.Where(d => name == null || d.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync(ct);
            return Result<IEnumerable<Dish>>.Success(dishes);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Result<Dish>> AddDish(Dish dish, CancellationToken cancellationToken)
    {
        try
        {
            _dbContext.Add(dish);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result<Dish>.Success(dish);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}