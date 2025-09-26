using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace FollowRivers.DTO;

/// <summary>
/// Dados enviados para criação e atualização de alertas de inundação.
/// </summary>
public class FloodAlertRequestDto
{
    [Required]
    [MaxLength(120)]
    [SwaggerSchema(Description = "Título curto e objetivo do alerta.", Example = "Risco alto de inundação")]
    public required string Title { get; set; }

    [Required]
    [MaxLength(500)]
    [SwaggerSchema(Description = "Descrição detalhada do alerta.", Example = "Chuvas fortes nas últimas 24h aumentaram o nível do Rio X.")]
    public required string Description { get; set; }

    [Required]
    [SwaggerSchema(Description = "Nível de severidade do alerta.", Example = "Alto")]
    public required string Severity { get; set; }

    [Required]
    [SwaggerSchema(Description = "Identificador do ponto monitorado onde o alerta foi registrado.", Example = 1)]
    public long RiverAddressId { get; set; }

    [Required]
    [SwaggerSchema(Description = "Identificador da pessoa que registrou o alerta.", Example = 1)]
    public long PersonId { get; set; }
}
