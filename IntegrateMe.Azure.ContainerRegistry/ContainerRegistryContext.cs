using IntegrateMe.Core;

namespace IntegrateMe.Azure.ContainerRegistry;

public static class ContainerRegistryContext
{
    public static ContainerRegistryAbstractStep ContainerRegistry(this AbstractStep parent, string? name = null)
    {
        if (name == null) return new ContainerRegistryAbstractStep(parent);
        if (parent.MainDsl.Entities.TryGetValue(name, out var step))
        {
            if (step is not ContainerRegistryAbstractStep containerRegistryStep)
            {
                throw new InvalidOperationException();
            }

            return containerRegistryStep;
        }

        var next = new ContainerRegistryAbstractStep(parent);
        parent.MainDsl.Entities.Add(name, next);
        return next;
    }
}