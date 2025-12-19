using Abp.Dependency;

namespace BEZNgCore.Web.Xss;

public interface IHtmlSanitizer : ITransientDependency
{
    string Sanitize(string html);
}

