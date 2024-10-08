using IntegrateMe.Core;

namespace IntegrateMe.Azure.LogicApp;

public static class LogicAppContext
{
    public static LogicAppAbstractStep LogicApp(this AbstractStep parent, string? name = null)
    {
        if (name == null)
        {
            name = Guid.NewGuid().ToString();
        }
        else
        {
            if (parent.MainDsl.Entities.TryGetValue(name, out var step))
            {
                if (step is not LogicAppAbstractStep logicAppStep)
                {
                    throw new InvalidOperationException();
                }

                return logicAppStep;
            }
        }

        var next = new LogicAppAbstractStep(parent);
        parent.MainDsl.Entities.Add(name, next);
        return next;
    }
}