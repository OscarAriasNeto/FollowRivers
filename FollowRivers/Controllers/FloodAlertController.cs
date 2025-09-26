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
public class FloodAlertController : ControllerBase
{
    private readonly FollowRiversContext _context;

    public FloodAlertController(FollowRiversContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lista os alertas de inundação registrados.
    /// </summary>
    [HttpGet(Name = nameof(GetFloodAlertsAsync))]
    [SwaggerOperation(Summary = "Lista paginada de alertas de inundação")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<HateoasResource<FloodAlertResponseDto>>))]
    public async Task<ActionResult<PagedResponse<HateoasResource<FloodAlertResponseDto>>>> GetFloodAlertsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        pageNumber = Math.Max(pageNumber, 1);
        pageSize = Math.Clamp(pageSize, 1, 50);

        var query = _context.FloodAlerts.AsNoTracking().OrderBy(a => a.FloodAlertId);
        var totalItems = await query.CountAsync();
        var alerts = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        var resources = alerts.Select(MapToResource);
        var response = new PagedResponse<HateoasResource<FloodAlertResponseDto>>(resources, pageNumber, pageSize, totalItems);
        AppendPaginationLinks(response, nameof(GetFloodAlertsAsync));
        return Ok(response);
    }

    /// <summary>
    /// Obtém um alerta de inundação específico.
    /// </summary>
    [HttpGet("{id}", Name = nameof(GetFloodAlertByIdAsync))]
    [SwaggerOperation(Summary = "Consulta alerta de inundação por identificador")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HateoasResource<FloodAlertResponseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HateoasResource<FloodAlertResponseDto>>> GetFloodAlertByIdAsync(long id)
    {
        var alert = await _context.FloodAlerts.AsNoTracking().FirstOrDefaultAsync(a => a.FloodAlertId == id);
        if (alert is null)
        {
            return NotFound();
        }

        return Ok(MapToResource(alert));
    }

    /// <summary>
    /// Cria um novo alerta de inundação.
    /// </summary>
    [HttpPost(Name = nameof(CreateFloodAlertAsync))]
    [SwaggerOperation(Summary = "Cria alerta de inundação")]
    [SwaggerRequestExample(typeof(FloodAlertRequestDto), typeof(FollowRivers.Swagger.Examples.FloodAlertRequestExample))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(HateoasResource<FloodAlertResponseDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<HateoasResource<FloodAlertResponseDto>>> CreateFloodAlertAsync([FromBody] FloodAlertRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var personExists = await _context.Persons.AnyAsync(p => p.PersonId == request.PersonId);
        if (!personExists)
        {
            ModelState.AddModelError(nameof(request.PersonId), "Pessoa informada não foi encontrada.");
            return ValidationProblem(ModelState);
        }

        var riverAddressExists = await _context.RiverAddresses.AnyAsync(r => r.RiverAddressId == request.RiverAddressId);
        if (!riverAddressExists)
        {
            ModelState.AddModelError(nameof(request.RiverAddressId), "Ponto monitorado informado não foi encontrado.");
            return ValidationProblem(ModelState);
        }

        var alert = new FloodAlert
        {
            Title = request.Title,
            Description = request.Description,
            Severity = request.Severity,
            RiverAddressId = request.RiverAddressId,
            PersonId = request.PersonId
        };

        _context.FloodAlerts.Add(alert);
        await _context.SaveChangesAsync();

        var resource = MapToResource(alert);
        var location = Url.Link(nameof(GetFloodAlertByIdAsync), new { id = alert.FloodAlertId });
        return Created(location!, resource);
    }

    /// <summary>
    /// Atualiza as informações de um alerta de inundação.
    /// </summary>
    [HttpPut("{id}", Name = nameof(UpdateFloodAlertAsync))]
    [SwaggerOperation(Summary = "Atualiza alerta de inundação")]
    [SwaggerRequestExample(typeof(FloodAlertRequestDto), typeof(FollowRivers.Swagger.Examples.FloodAlertRequestExample))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFloodAlertAsync(long id, [FromBody] FloodAlertRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var alert = await _context.FloodAlerts.FirstOrDefaultAsync(a => a.FloodAlertId == id);
        if (alert is null)
        {
            return NotFound();
        }

        var personExists = await _context.Persons.AnyAsync(p => p.PersonId == request.PersonId);
        if (!personExists)
        {
            ModelState.AddModelError(nameof(request.PersonId), "Pessoa informada não foi encontrada.");
            return ValidationProblem(ModelState);
        }

        var riverAddressExists = await _context.RiverAddresses.AnyAsync(r => r.RiverAddressId == request.RiverAddressId);
        if (!riverAddressExists)
        {
            ModelState.AddModelError(nameof(request.RiverAddressId), "Ponto monitorado informado não foi encontrado.");
            return ValidationProblem(ModelState);
        }

        alert.Title = request.Title;
        alert.Description = request.Description;
        alert.Severity = request.Severity;
        alert.PersonId = request.PersonId;
        alert.RiverAddressId = request.RiverAddressId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Remove um alerta de inundação.
    /// </summary>
    [HttpDelete("{id}", Name = nameof(DeleteFloodAlertAsync))]
    [SwaggerOperation(Summary = "Remove alerta de inundação")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFloodAlertAsync(long id)
    {
        var alert = await _context.FloodAlerts.FindAsync(id);
        if (alert is null)
        {
            return NotFound();
        }

        _context.FloodAlerts.Remove(alert);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private HateoasResource<FloodAlertResponseDto> MapToResource(FloodAlert alert)
    {
        var dto = new FloodAlertResponseDto
        {
            FloodAlertId = alert.FloodAlertId,
            Title = alert.Title,
            Description = alert.Description,
            Severity = alert.Severity,
            CreatedAt = alert.CreatedAt,
            RiverAddressId = alert.RiverAddressId,
            PersonId = alert.PersonId
        };

        var links = new List<LinkDto>
        {
            new LinkDto(Url.Link(nameof(GetFloodAlertByIdAsync), new { id = alert.FloodAlertId })!, "self", HttpMethods.Get),
            new LinkDto(Url.Link(nameof(UpdateFloodAlertAsync), new { id = alert.FloodAlertId })!, "update", HttpMethods.Put),
            new LinkDto(Url.Link(nameof(DeleteFloodAlertAsync), new { id = alert.FloodAlertId })!, "delete", HttpMethods.Delete)
        };

        return new HateoasResource<FloodAlertResponseDto>(dto, links);
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
