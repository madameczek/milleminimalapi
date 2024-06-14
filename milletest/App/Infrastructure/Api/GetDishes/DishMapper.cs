using FastEndpoints;
using milletest.Shared.App.Models;

namespace milletest.App.Infrastructure.Api.GetDishes;

public class DishMapper : ResponseMapper<IEnumerable<ResponseDto>, IEnumerable<Dish>>
{
    public override IEnumerable<ResponseDto> FromEntity(IEnumerable<Dish> dishes)
    {
        return dishes.Select(dish => new ResponseDto
        {
            Id = dish.Id,
            Name = dish.Name
        });
    }
}