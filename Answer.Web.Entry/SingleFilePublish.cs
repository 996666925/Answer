using Furion;
using System.Reflection;

namespace Answer.Web.Entry;

public class SingleFilePublish : ISingleFilePublish
{
    public Assembly[] IncludeAssemblies()
    {
        return Array.Empty<Assembly>();
    }

    public string[] IncludeAssemblyNames()
    {
        return new[]
        {
            "Answer.Application",
            "Answer.Core",
            "Answer.Web.Core"
        };
    }
}