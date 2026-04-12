using System.Reflection;

namespace Solster.AspNetCore.Components;

public sealed class AssemblyTemplateProvider(Assembly assembly) : ITemplateProvider
{
    public Type[] GetTemplates()
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.OfType<Type>().ToArray();
        }
    }
}