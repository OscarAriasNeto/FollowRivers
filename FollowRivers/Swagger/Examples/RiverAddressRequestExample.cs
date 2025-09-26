using FollowRivers.DTO;
using Swashbuckle.AspNetCore.Filters;

namespace FollowRivers.Swagger.Examples;

public class RiverAddressRequestExample : IExamplesProvider<RiverAddressRequestDto>
{
    public RiverAddressRequestDto GetExamples() => new()
    {
        Address = "Margem esquerda do Rio Tietê, km 23",
        CanCauseFlood = true,
        PersonId = 1
    };
}
