using System.ComponentModel.DataAnnotations;

namespace FollowRivers.DTO
{
    public class PersonDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }
        public string Senha { get; set; }

    }
}
