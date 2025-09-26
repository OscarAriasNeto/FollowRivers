using System.Linq;
using FollowRivers.Data;
using FollowRivers.DTO;
using FollowRivers.Models;
using FollowRivers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace FollowRivers.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly FollowRiversContext _context;

    public PersonController(FollowRiversContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lista as pessoas cadastradas no sistema.
    /// </summary>
    [HttpGet(Name = nameof(GetPersonsAsync))]
    [SwaggerOperation(Summary = "Lista paginada de pessoas", Description = "Retorna uma lista paginada das pessoas cadastradas com links HATEOAS.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<HateoasResource<PersonResponseDto>>))]
    public async Task<ActionResult<PagedResponse<HateoasResource<PersonResponseDto>>>> GetPersonsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        pageNumber = Math.Max(pageNumber, 1);
        pageSize = Math.Clamp(pageSize, 1, 50);

        var query = _context.Persons.AsNoTracking().OrderBy(p => p.PersonId);
        var totalItems = await query.CountAsync();
        var persons = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        var resources = persons.Select(MapToResource);
        var response = new PagedResponse<HateoasResource<PersonResponseDto>>(resources, pageNumber, pageSize, totalItems);
        AppendPaginationLinks(response, nameof(GetPersonsAsync));
        return Ok(response);
    }

    /// <summary>
    /// Retorna uma pessoa específica.
    /// </summary>
    [HttpGet("{id}", Name = nameof(GetPersonByIdAsync))]
    [SwaggerOperation(Summary = "Consulta pessoa por identificador")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HateoasResource<PersonResponseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HateoasResource<PersonResponseDto>>> GetPersonByIdAsync(long id)
    {
        var person = await _context.Persons.AsNoTracking().FirstOrDefaultAsync(p => p.PersonId == id);
        if (person is null)
        {
            return NotFound();
        }

        return Ok(MapToResource(person));
    }

    /// <summary>
    /// Cria uma nova pessoa.
    /// </summary>
    [HttpPost(Name = nameof(CreatePersonAsync))]
    [SwaggerOperation(Summary = "Cria uma nova pessoa")]
    [SwaggerRequestExample(typeof(PersonRequestDto), typeof(FollowRivers.Swagger.Examples.PersonRequestExample))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(HateoasResource<PersonResponseDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<HateoasResource<PersonResponseDto>>> CreatePersonAsync([FromBody] PersonRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var emailAlreadyRegistered = await _context.Persons.AnyAsync(p => p.Email == request.Email);
        if (emailAlreadyRegistered)
        {
            ModelState.AddModelError(nameof(request.Email), "E-mail já cadastrado para outra pessoa.");
            return ValidationProblem(ModelState);
        }

        var person = new Person
        {
            Name = request.Name,
            Email = request.Email,
            Senha = request.Senha
        };

        _context.Persons.Add(person);
        await _context.SaveChangesAsync();

        var resource = MapToResource(person);
        var location = Url.Link(nameof(GetPersonByIdAsync), new { id = person.PersonId });
        return Created(location!, resource);
    }

    /// <summary>
    /// Atualiza os dados de uma pessoa existente.
    /// </summary>
    [HttpPut("{id}", Name = nameof(UpdatePersonAsync))]
    [SwaggerOperation(Summary = "Atualiza uma pessoa existente")]
    [SwaggerRequestExample(typeof(PersonRequestDto), typeof(FollowRivers.Swagger.Examples.PersonRequestExample))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePersonAsync(long id, [FromBody] PersonRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var person = await _context.Persons.FirstOrDefaultAsync(p => p.PersonId == id);
        if (person is null)
        {
            return NotFound();
        }

        var emailUsedByAnother = await _context.Persons.AnyAsync(p => p.Email == request.Email && p.PersonId != id);
        if (emailUsedByAnother)
        {
            ModelState.AddModelError(nameof(request.Email), "E-mail já cadastrado para outra pessoa.");
            return ValidationProblem(ModelState);
        }

        person.Name = request.Name;
        person.Email = request.Email;
        person.Senha = request.Senha;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Remove uma pessoa e seus registros associados.
    /// </summary>
    [HttpDelete("{id}", Name = nameof(DeletePersonAsync))]
    [SwaggerOperation(Summary = "Remove uma pessoa")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePersonAsync(long id)
    {
        var person = await _context.Persons.Include(p => p.RiverAddresses).FirstOrDefaultAsync(p => p.PersonId == id);
        if (person is null)
        {
            return NotFound();
        }

        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private HateoasResource<PersonResponseDto> MapToResource(Person person)
    {
        var dto = new PersonResponseDto
        {
            PersonId = person.PersonId,
            Name = person.Name,
            Email = person.Email
        };

        var links = new List<LinkDto>
        {
            new LinkDto(Url.Link(nameof(GetPersonByIdAsync), new { id = person.PersonId })!, "self", HttpMethods.Get),
            new LinkDto(Url.Link(nameof(UpdatePersonAsync), new { id = person.PersonId })!, "update", HttpMethods.Put),
            new LinkDto(Url.Link(nameof(DeletePersonAsync), new { id = person.PersonId })!, "delete", HttpMethods.Delete)
        };

        return new HateoasResource<PersonResponseDto>(dto, links);
    }

    private void AppendPaginationLinks<TResource>(PagedResponse<TResource> response, string routeName)
    {
        response.Links.Add(new LinkDto(Url.Link(routeName, new { pageNumber = response.PageNumber, pageSize = response.PageSize })!, "self", HttpMethods.Get));

        if (response.HasPrevious)
        {
            response.Links.Add(new LinkDto(Url.Link(routeName, new { pageNumber = response.PageNumber - 1, pageSize = response.PageSize })!, "previous", HttpMethods.Get));
        }

        if (response.HasNext)
        {
            response.Links.Add(new LinkDto(Url.Link(routeName, new { pageNumber = response.PageNumber + 1, pageSize = response.PageSize })!, "next", HttpMethods.Get));
        }
    }
}
