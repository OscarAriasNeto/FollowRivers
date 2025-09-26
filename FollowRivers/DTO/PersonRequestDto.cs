using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace FollowRivers.DTO;

/// <summary>
/// Payload utilizado para criação e atualização de pessoas monitoradas pela aplicação.
/// </summary>
public class PersonRequestDto
{
    [Required]
    [MaxLength(100)]
    [SwaggerSchema(Description = "Nome completo da pessoa cadastrada na plataforma.", Example = "João da Silva")]
    public required string Name { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    [SwaggerSchema(Description = "E-mail utilizado para autenticação e contato.", Example = "joao@email.com")]
    public required string Email { get; set; }

    [Required]
    [MinLength(6)]
    [SwaggerSchema(Description = "Senha utilizada para acesso ao sistema.", Example = "Senha@123")]
    public required string Senha { get; set; }
}
