using FollowRivers.DTO;
using Swashbuckle.AspNetCore.Filters;

namespace FollowRivers.Swagger.Examples;

public class PersonRequestExample : IExamplesProvider<PersonRequestDto>
{
    public PersonRequestDto GetExamples() => new()
    {
        Name = "Mariana Costa",
        Email = "mariana.costa@email.com",
        Senha = "Senha@2024"
    };
}
