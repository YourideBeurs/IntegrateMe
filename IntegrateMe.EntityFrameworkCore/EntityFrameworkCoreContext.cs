using IntegrateMe.Core;

namespace IntegrateMe.EntityFramework.Core;

public static class EntityFrameworkCoreContext
{
    public static EntityFrameworkCoreAbstractStep EntityFramework(this AbstractStep parent, string? name = null)
    {
        if (name == null)
        {
            name = Guid.NewGuid().ToString();
        }
        else
        {
            if (parent.MainDsl.Entities.TryGetValue(name, out var step))
            {
                if (step is not EntityFrameworkCoreAbstractStep entityFrameworkCoreStep)
                {
                    throw new InvalidOperationException();
                }

                return entityFrameworkCoreStep;
            }
        }

        var next = new EntityFrameworkCoreAbstractStep(parent);
        parent.MainDsl.Entities.Add(name, next);
        return next;
    }
}