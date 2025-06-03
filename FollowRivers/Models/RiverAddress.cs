using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FollowRivers.Models
{
    public class RiverAddress
    {
        [Key]
        public int RiverAddressId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        public bool CanCauseFlood { get; set; }

        // Foreign key to Person
        public int PersonId { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }
    }
}
