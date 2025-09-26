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
public class RiverAddressController : ControllerBase
{
    private readonly FollowRiversContext _context;

    public RiverAddressController(FollowRiversContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna os pontos monitorados de rios.
    /// </summary>
    [HttpGet(Name = nameof(GetRiverAddressesAsync))]
    [SwaggerOperation(Summary = "Lista paginada de pontos monitorados", Description = "Retorna os pontos monitorados com suporte a paginação e HATEOAS.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<HateoasResource<RiverAddressResponseDto>>))]
    public async Task<ActionResult<PagedResponse<HateoasResource<RiverAddressResponseDto>>>> GetRiverAddressesAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        pageNumber = Math.Max(pageNumber, 1);
        pageSize = Math.Clamp(pageSize, 1, 50);

        var query = _context.RiverAddresses.AsNoTracking().OrderBy(r => r.RiverAddressId);
        var totalItems = await query.CountAsync();
        var riverAddresses = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        var resources = riverAddresses.Select(MapToResource);
        var response = new PagedResponse<HateoasResource<RiverAddressResponseDto>>(resources, pageNumber, pageSize, totalItems);
        AppendPaginationLinks(response, nameof(GetRiverAddressesAsync));
        return Ok(response);
    }

    /// <summary>
    /// Busca um ponto monitorado específico.
    /// </summary>
    [HttpGet("{id}", Name = nameof(GetRiverAddressByIdAsync))]
    [SwaggerOperation(Summary = "Consulta ponto monitorado por identificador")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HateoasResource<RiverAddressResponseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HateoasResource<RiverAddressResponseDto>>> GetRiverAddressByIdAsync(long id)
    {
        var riverAddress = await _context.RiverAddresses.AsNoTracking().FirstOrDefaultAsync(r => r.RiverAddressId == id);
        if (riverAddress is null)
        {
            return NotFound();
        }

        return Ok(MapToResource(riverAddress));
    }

    /// <summary>
    /// Cria um novo ponto monitorado de rio.
    /// </summary>
    [HttpPost(Name = nameof(CreateRiverAddressAsync))]
    [SwaggerOperation(Summary = "Cria ponto monitorado")]
    [SwaggerRequestExample(typeof(RiverAddressRequestDto), typeof(FollowRivers.Swagger.Examples.RiverAddressRequestExample))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(HateoasResource<RiverAddressResponseDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<HateoasResource<RiverAddressResponseDto>>> CreateRiverAddressAsync([FromBody] RiverAddressRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var person = await _context.Persons.FindAsync(request.PersonId);
        if (person is null)
        {
            ModelState.AddModelError(nameof(request.PersonId), "Pessoa informada não foi encontrada.");
            return ValidationProblem(ModelState);
        }

        var riverAddress = new RiverAddress
        {
            Address = request.Address,
            CanCauseFlood = request.CanCauseFlood,
            PersonId = request.PersonId
        };

        _context.RiverAddresses.Add(riverAddress);
        await _context.SaveChangesAsync();

        var resource = MapToResource(riverAddress);
        var location = Url.Link(nameof(GetRiverAddressByIdAsync), new { id = riverAddress.RiverAddressId });
        return Created(location!, resource);
    }

    /// <summary>
    /// Atualiza as informações de um ponto monitorado.
    /// </summary>
    [HttpPut("{id}", Name = nameof(UpdateRiverAddressAsync))]
    [SwaggerOperation(Summary = "Atualiza ponto monitorado")]
    [SwaggerRequestExample(typeof(RiverAddressRequestDto), typeof(FollowRivers.Swagger.Examples.RiverAddressRequestExample))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRiverAddressAsync(long id, [FromBody] RiverAddressRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var riverAddress = await _context.RiverAddresses.FirstOrDefaultAsync(r => r.RiverAddressId == id);
        if (riverAddress is null)
        {
            return NotFound();
        }

        var personExists = await _context.Persons.AnyAsync(p => p.PersonId == request.PersonId);
        if (!personExists)
        {
            ModelState.AddModelError(nameof(request.PersonId), "Pessoa informada não foi encontrada.");
            return ValidationProblem(ModelState);
        }

        riverAddress.Address = request.Address;
        riverAddress.CanCauseFlood = request.CanCauseFlood;
        riverAddress.PersonId = request.PersonId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Remove um ponto monitorado.
    /// </summary>
    [HttpDelete("{id}", Name = nameof(DeleteRiverAddressAsync))]
    [SwaggerOperation(Summary = "Remove ponto monitorado")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRiverAddressAsync(long id)
    {
        var riverAddress = await _context.RiverAddresses.FindAsync(id);
        if (riverAddress is null)
        {
            return NotFound();
        }

        _context.RiverAddresses.Remove(riverAddress);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private HateoasResource<RiverAddressResponseDto> MapToResource(RiverAddress riverAddress)
    {
        var dto = new RiverAddressResponseDto
        {
            RiverAddressId = riverAddress.RiverAddressId,
            Address = riverAddress.Address,
            CanCauseFlood = riverAddress.CanCauseFlood,
            PersonId = riverAddress.PersonId
        };

        var links = new List<LinkDto>
        {
            new LinkDto(Url.Link(nameof(GetRiverAddressByIdAsync), new { id = riverAddress.RiverAddressId })!, "self", HttpMethods.Get),
            new LinkDto(Url.Link(nameof(UpdateRiverAddressAsync), new { id = riverAddress.RiverAddressId })!, "update", HttpMethods.Put),
            new LinkDto(Url.Link(nameof(DeleteRiverAddressAsync), new { id = riverAddress.RiverAddressId })!, "delete", HttpMethods.Delete)
        };

        return new HateoasResource<RiverAddressResponseDto>(dto, links);
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
