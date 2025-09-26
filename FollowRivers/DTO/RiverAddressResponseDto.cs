using Swashbuckle.AspNetCore.Annotations;

namespace FollowRivers.DTO;

/// <summary>
/// Dados expostos pela API para um endereço de rio monitorado.
/// </summary>
public class RiverAddressResponseDto
{
    [SwaggerSchema(Description = "Identificador único do ponto monitorado.", Example = 1)]
    public long RiverAddressId { get; set; }

    [SwaggerSchema(Description = "Descrição do endereço de monitoramento.", Example = "Margem direita do Rio X, km 12")]
    public string Address { get; set; } = string.Empty;

    [SwaggerSchema(Description = "Indica se o ponto pode causar alagamentos.", Example = true)]
    public bool CanCauseFlood { get; set; }

    [SwaggerSchema(Description = "Identificador da pessoa responsável pelo ponto.", Example = 1)]
    public long PersonId { get; set; }
}
