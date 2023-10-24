using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BrowerFileBrowser.Containers;

public abstract class StateContainer
{
    public event Action OnChange;

    protected void NotifyStateChanged() => OnChange?.Invoke();

    public static void RegisterAllDerived(IServiceCollection services)
    {
        var baseType = typeof(StateContainer);
        var derivedTypes = Assembly.GetAssembly(baseType)
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(baseType));

        foreach (var type in derivedTypes)
        {
            services.AddScoped(type);
        }
    }
}
