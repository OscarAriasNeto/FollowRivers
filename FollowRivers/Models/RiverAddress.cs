using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FollowRivers.Models;

/// <summary>
/// Representa um ponto de monitoramento em um rio.
/// </summary>
public class RiverAddress
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long RiverAddressId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Address { get; set; } = string.Empty;

    public bool CanCauseFlood { get; set; }

    [ForeignKey(nameof(Person))]
    public long PersonId { get; set; }

    [JsonIgnore]
    public Person? Person { get; set; }

    public ICollection<FloodAlert> FloodAlerts { get; set; } = new List<FloodAlert>();
}
