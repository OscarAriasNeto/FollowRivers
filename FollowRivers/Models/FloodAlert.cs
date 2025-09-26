using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FollowRivers.Models;

/// <summary>
/// Representa um alerta de inundação associado a um ponto monitorado de rio.
/// </summary>
public class FloodAlert
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long FloodAlertId { get; set; }

    [Required]
    [MaxLength(120)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Severity { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(RiverAddress))]
    public long RiverAddressId { get; set; }

    [JsonIgnore]
    public RiverAddress? RiverAddress { get; set; }

    [ForeignKey(nameof(Person))]
    public long PersonId { get; set; }

    [JsonIgnore]
    public Person? Person { get; set; }
}
