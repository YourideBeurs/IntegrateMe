﻿using IntegrateMe.Core;

namespace IntegrateMe.Azure.ContainerApp;

public static class ContainerAppExtensions
{
    public static ContainerAppStep ContainerApp(this AbstractStep parent, string? name = null)
    {
        if (name == null) return new ContainerAppStep(parent);
        if (parent.MainDsl.Entities.TryGetValue(name, out var step))
        {
            if (step is not ContainerAppStep containerAppStep)
            {
                throw new InvalidOperationException();
            }

            return containerAppStep;
        }

        var next = new ContainerAppStep(parent);
        parent.MainDsl.Entities.Add(name, next);
        return next;
    }
}