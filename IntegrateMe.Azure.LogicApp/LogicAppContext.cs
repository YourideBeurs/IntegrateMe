using IntegrateMe.Core;

namespace IntegrateMe.Azure.LogicApp;

public static class LogicAppContext
{
    public static LogicAppStep LogicApp(this IStep parent, string? name = null)
    {
        if (name == null)
        {
            name = Guid.NewGuid().ToString();
        }
        else
        {
            if (parent.MainDsl.Entities.TryGetValue(name, out var step))
            {
                if (step is not LogicAppStep logicAppStep)
                {
                    throw new InvalidOperationException();
                }

                return logicAppStep;
            }
        }

        var next = new LogicAppStep(parent);
        parent.MainDsl.Entities.Add(name, next);
        return next;
    }
}