using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Xml.Linq;

namespace Haondt.Web.UI.Services;

public interface ILucideIconService
{
    string RenderIcon(string name, int? size = null, string? cssClass = null, string? style = null);
}

public class LucideIconOptions
{
    public string IconBasePath { get; set; } = "shared/vendored/lucide-static/icons/";
    public int DefaultSize { get; set; } = 14;

}

public record SvgTemplate(XElement SvgElement, IEnumerable<XElement> Paths);

public class LucideIconService : ILucideIconService
{
    private readonly IWebHostEnvironment _environment;
    private readonly IMemoryCache _cache;
    private readonly LucideIconOptions _options;
    private static readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(30);

    public LucideIconService(IWebHostEnvironment environment, IMemoryCache cache, IOptions<LucideIconOptions> options)
    {
        _environment = environment;
        _cache = cache;
        _options = options.Value;
    }

    public string RenderIcon(string name, int? size = null, string? cssClass = null, string? style = null)
    {
        var template = GetSvgTemplate(name);
        if (template == null)
            return $"<!-- Icon {name} not found -->";

        return RenderSvgFromTemplate(template, size ?? _options.DefaultSize, cssClass, style);
    }

    private SvgTemplate? GetSvgTemplate(string name)
    {
        var cacheKey = $"lucide_icon_{name}";

        return _cache.GetOrCreate(cacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheExpiry;

            var fileProvider = _environment.WebRootFileProvider;
            var iconPath = $"{_options.IconBasePath}{name}.svg";
            var fileInfo = fileProvider.GetFileInfo(iconPath);

            if (!fileInfo.Exists)
                return null;

            using var stream = fileInfo.CreateReadStream();
            var svgDoc = XDocument.Load(stream);
            var svgElement = svgDoc.Root;

            if (svgElement?.Name.LocalName != "svg")
                return null;

            // Extract paths and other drawable elements
            var paths = svgElement.Elements().Where(e =>
                e.Name.LocalName == "path" ||
                e.Name.LocalName == "circle" ||
                e.Name.LocalName == "rect" ||
                e.Name.LocalName == "line" ||
                e.Name.LocalName == "polyline" ||
                e.Name.LocalName == "polygon");

            return new SvgTemplate(svgElement, paths);
        });
    }

    private static string RenderSvgFromTemplate(SvgTemplate template, int size, string? cssClass, string? style)
    {
        var svg = new XElement("svg",
            new XAttribute("width", size),
            new XAttribute("height", size),
            new XAttribute("viewBox", template.SvgElement.Attribute("viewBox")?.Value ?? "0 0 24 24"),
            new XAttribute("fill", "none"),
            new XAttribute("stroke", "currentColor"),
            new XAttribute("stroke-width", "2"),
            new XAttribute("stroke-linecap", "round"),
            new XAttribute("stroke-linejoin", "round")
        );

        // Add CSS class
        var classValue = "lucide";
        if (!string.IsNullOrEmpty(cssClass))
            classValue = $"{cssClass} {classValue}";
        svg.Add(new XAttribute("class", classValue));

        // Add style
        if (!string.IsNullOrEmpty(style))
            svg.Add(new XAttribute("style", style));

        // Add paths
        foreach (var path in template.Paths)
        {
            svg.Add(new XElement(path));
        }

        return svg.ToString();
    }
}