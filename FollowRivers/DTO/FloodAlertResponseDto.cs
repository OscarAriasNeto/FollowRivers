using Swashbuckle.AspNetCore.Annotations;

namespace FollowRivers.DTO;

/// <summary>
/// Representa um alerta de inundação retornado pela API.
/// </summary>
public class FloodAlertResponseDto
{
    [SwaggerSchema(Description = "Identificador único do alerta.", Example = 10)]
    public long FloodAlertId { get; set; }

    [SwaggerSchema(Description = "Título do alerta.", Example = "Risco alto de inundação")]
    public string Title { get; set; } = string.Empty;

    [SwaggerSchema(Description = "Descrição detalhada sobre o alerta.", Example = "Chuvas fortes nas últimas 24h aumentaram o nível do Rio X.")]
    public string Description { get; set; } = string.Empty;

    [SwaggerSchema(Description = "Nível de severidade informado.", Example = "Alto")]
    public string Severity { get; set; } = string.Empty;

    [SwaggerSchema(Description = "Data e hora em que o alerta foi registrado em UTC.", Example = "2024-08-01T12:00:00Z")]
    public DateTime CreatedAt { get; set; }

    [SwaggerSchema(Description = "Identificador do ponto monitorado relacionado ao alerta.", Example = 1)]
    public long RiverAddressId { get; set; }

    [SwaggerSchema(Description = "Identificador da pessoa que cadastrou o alerta.", Example = 1)]
    public long PersonId { get; set; }
}
