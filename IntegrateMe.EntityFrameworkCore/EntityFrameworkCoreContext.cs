using IntegrateMe.Core;

namespace IntegrateMe.EntityFramework.Core;

public static class EntityFrameworkCoreContext
{
    public static EntityFrameworkCoreStep EntityFramework(this IStep parent, string? name = null)
    {
        if (name == null)
        {
            name = Guid.NewGuid().ToString();
        }
        else
        {
            if (parent.MainDsl.Entities.TryGetValue(name, out var step))
            {
                if (step is not EntityFrameworkCoreStep entityFrameworkCoreStep)
                {
                    throw new InvalidOperationException();
                }

                return entityFrameworkCoreStep;
            }
        }

        var next = new EntityFrameworkCoreStep(parent);
        parent.MainDsl.Entities.Add(name, next);
        return next;
    }
}