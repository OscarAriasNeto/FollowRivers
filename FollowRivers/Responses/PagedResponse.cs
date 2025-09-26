using System;
using System.Linq;

namespace FollowRivers.Responses;

/// <summary>
/// Estrutura responsável por encapsular respostas paginadas de forma consistente.
/// </summary>
/// <typeparam name="T">Tipo dos itens retornados.</typeparam>
public class PagedResponse<T>
{
    public PagedResponse(IEnumerable<T> items, int pageNumber, int pageSize, int totalItems)
    {
        Items = items.ToList();
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = totalItems;
        Links = new List<LinkDto>();
    }

    public IReadOnlyCollection<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalItems { get; }
    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;
    public List<LinkDto> Links { get; }
}
