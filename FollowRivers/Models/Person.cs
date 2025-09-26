using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FollowRivers.Models;

/// <summary>
/// Representa uma pessoa responsável por acompanhar pontos monitorados de rios.
/// </summary>
public class Person
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long PersonId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Senha { get; set; } = string.Empty;

    public ICollection<RiverAddress> RiverAddresses { get; set; } = new List<RiverAddress>();
    public ICollection<FloodAlert> FloodAlerts { get; set; } = new List<FloodAlert>();
}
