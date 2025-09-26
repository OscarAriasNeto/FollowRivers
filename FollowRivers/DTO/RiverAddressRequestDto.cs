using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace FollowRivers.DTO;

/// <summary>
/// Dados necessários para criação ou atualização de um ponto monitorado de rio.
/// </summary>
public class RiverAddressRequestDto
{
    [Required]
    [MaxLength(200)]
    [SwaggerSchema(Description = "Descrição detalhada do ponto monitorado do rio.", Example = "Margem direita do Rio X, km 12")]
    public required string Address { get; set; }

    [SwaggerSchema(Description = "Informa se o ponto possui histórico de alagamentos.", Example = true)]
    public bool CanCauseFlood { get; set; }

    [Required]
    [SwaggerSchema(Description = "Identificador da pessoa responsável pelo monitoramento.", Example = 1)]
    public long PersonId { get; set; }
}
