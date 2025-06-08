using FollowRivers.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FollowRivers.DTO
{
    public class RiverAddressDTO
    {
        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        public string CanCauseFlood { get; set; }

        public long PersonId { get; set; }
    }
}
