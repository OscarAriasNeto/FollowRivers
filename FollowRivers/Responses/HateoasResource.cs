using System.Linq;

namespace FollowRivers.Responses;

/// <summary>
/// Estrutura padrão para resposta de recursos com links HATEOAS.
/// </summary>
/// <typeparam name="T">Tipo do recurso retornado.</typeparam>
public class HateoasResource<T>
{
    public HateoasResource(T data, IEnumerable<LinkDto> links)
    {
        Data = data;
        Links = links.ToList();
    }

    public T Data { get; }
    public IReadOnlyCollection<LinkDto> Links { get; }
}
