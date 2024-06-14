using System.Data;
using FastEndpoints;
using milletest.App.Interfaces;

namespace milletest.App.Infrastructure.Api.GetDishes;

public class GetDishesEndpoint : EndpointWithoutRequest <IEnumerable<ResponseDto>, DishMapper>
{
    private readonly ILogger<GetDishesEndpoint> _logger;
    private readonly IDishRepository _dishRepository;

    public GetDishesEndpoint(ILogger<GetDishesEndpoint> logger, IDishRepository dishRepository)
    {
        _logger = logger;
        _dishRepository = dishRepository;
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _dishRepository.GetDishes(ct);
        
        result.Match(
            success: dishes => SendAsync(Map.FromEntity(dishes), cancellation: ct),
            error: exception =>
            {
                if (exception is RowNotInTableException)
                    ThrowError($"{exception.Message}", 404);
                ThrowError("Application error", 500);
            });
    }
    
    public override void Configure()
    {
        Get("/dishes");
        Description(builder => builder
            .Produces(500));
        Version(1);
        AllowAnonymous();
    }
    
    private class CreatePanelEndpointSummary : Summary<GetDishesEndpoint>
    {
        public CreatePanelEndpointSummary()
        {
            Summary = "Get all dishes. No pagination";
            Response(200, "OK");
        }
    }
}