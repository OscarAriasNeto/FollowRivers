using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FollowRivers.Models
{
    public class RiverAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RiverAddressId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        public string CanCauseFlood { get; set; }

        [ForeignKey("PersonId")]
        [JsonIgnore]
        public Person Person { get; set; }
    }
}
