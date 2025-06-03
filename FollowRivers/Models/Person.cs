using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FollowRivers.Models;

namespace FollowRivers.Models
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        // Navigation property - one person can have many river addresses
        public ICollection<RiverAddress> RiverAddresses { get; set; }
    }
}
