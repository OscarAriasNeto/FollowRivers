namespace FollowRivers.Responses;

/// <summary>
/// Representa um link HATEOAS retornado nos recursos da API.
/// </summary>
public class LinkDto
{
    public LinkDto(string href, string rel, string method)
    {
        Href = href;
        Rel = rel;
        Method = method;
    }

    public string Href { get; }
    public string Rel { get; }
    public string Method { get; }
}
