using FastEndpoints;
using milletest.Shared.App.Models;

namespace milletest.App.Infrastructure.Api.CreateDish;

public class DishMapper : Mapper<RequestDto, ResponseDto, Dish>
{
    public override Dish ToEntity(RequestDto requestDto) => new(new Guid(), requestDto.Name);

    public override ResponseDto FromEntity(Dish dish) => new()
    {
        Id = dish.Id,
        Name = dish.Name
    };
}