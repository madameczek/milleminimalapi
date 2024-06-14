using FastEndpoints;
using milletest.App.Interfaces;

namespace milletest.App.Infrastructure.Api.CreateDish;

public class AddDishEndpoint : Endpoint<RequestDto, ResponseDto, DishMapper>
{
    private readonly IDishRepository _dishRepository;

    public AddDishEndpoint(IDishRepository dishRepository)
    {
        _dishRepository = dishRepository;
    }

    public override async Task HandleAsync(RequestDto requestDto, CancellationToken ct)
    {
        if(string.IsNullOrEmpty(requestDto.Name))
            ThrowError("Name can not be null", 400);
        
        var result = await _dishRepository.AddDish(Map.ToEntity(requestDto), ct);

        result.Match(
            success: dish => Response = Map.FromEntity(dish),
            error: exception =>
            {
                ThrowError("Application error", 500);
            });
        
        HttpContext.Response.Headers.Append("Location", $"/api/v1/dishes/{Response.Id}");
        await SendAsync(Response, 200, ct);
    }
    
    public override void Configure()
    {
        Post("/dishes");
        Version(1);
        Description(builder => builder
            .Produces(200)
            .Produces(400)
            .ProducesValidationProblem()
            .Produces(500));
        AllowAnonymous();
    }


    private class CreatePanelEndpointSummary : Summary<AddDishEndpoint>
    {
        public CreatePanelEndpointSummary()
        {
            Summary = "Endpoint for create user panels";
            Description = "Created resource location can be found in header with \"Location\" key";
            Response(201, "Created");
        }
    }
}

