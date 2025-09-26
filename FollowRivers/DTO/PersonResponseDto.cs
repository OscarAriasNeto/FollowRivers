using Swashbuckle.AspNetCore.Annotations;

namespace FollowRivers.DTO;

/// <summary>
/// Representa os dados retornados pela API para uma pessoa cadastrada.
/// </summary>
public class PersonResponseDto
{
    [SwaggerSchema(Description = "Identificador único da pessoa.", Example = 1)]
    public long PersonId { get; set; }

    [SwaggerSchema(Description = "Nome completo da pessoa.", Example = "João da Silva")]
    public string Name { get; set; } = string.Empty;

    [SwaggerSchema(Description = "E-mail da pessoa.", Example = "joao@email.com")]
    public string Email { get; set; } = string.Empty;
}
