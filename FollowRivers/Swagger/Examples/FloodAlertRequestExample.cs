using FollowRivers.DTO;
using Swashbuckle.AspNetCore.Filters;

namespace FollowRivers.Swagger.Examples;

public class FloodAlertRequestExample : IExamplesProvider<FloodAlertRequestDto>
{
    public FloodAlertRequestDto GetExamples() => new()
    {
        Title = "Risco crítico de inundação",
        Description = "Volume de chuvas acumulado em 48h ultrapassou o limite seguro. Evacuação recomendada.",
        Severity = "Crítico",
        PersonId = 1,
        RiverAddressId = 1
    };
}
