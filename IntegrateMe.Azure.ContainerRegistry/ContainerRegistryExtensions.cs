using IntegrateMe.Core;

namespace IntegrateMe.Azure.ContainerRegistry;

public static class ContainerRegistryExtensions
{
    public static ContainerRegistryStep ContainerRegistry(this AbstractStep parent, string? name = null)
    {
        if (name == null) return new ContainerRegistryStep(parent);
        if (parent.MainDsl.Entities.TryGetValue(name, out var step))
        {
            if (step is not ContainerRegistryStep containerRegistryStep)
            {
                throw new InvalidOperationException();
            }

            return containerRegistryStep;
        }

        var next = new ContainerRegistryStep(parent);
        parent.MainDsl.Entities.Add(name, next);
        return next;
    }
}