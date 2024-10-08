using IntegrateMe.Core;

namespace IntegrateMe.Azure.ContainerApp;

public static class ContainerAppContext
{
    public static ContainerAppAbstractStep ContainerApp(this AbstractStep parent, string? name = null)
    {
        if (name == null) return new ContainerAppAbstractStep(parent);
        if (parent.MainDsl.Entities.TryGetValue(name, out var step))
        {
            if (step is not ContainerAppAbstractStep containerAppStep)
            {
                throw new InvalidOperationException();
            }

            return containerAppStep;
        }

        var next = new ContainerAppAbstractStep(parent);
        parent.MainDsl.Entities.Add(name, next);
        return next;
    }
}