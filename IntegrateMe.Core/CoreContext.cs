namespace IntegrateMe.Core;

public static class CoreContext
{
    public static Dsl Given()
    {
        return new Dsl(null!);
    }

    public static AbstractStep Parallel(this AbstractStep abstractStep, Action<AbstractStep> action1,
        Action<AbstractStep> action2)
    {
        return abstractStep;
    }

    public static AbstractStep Parallel(this AbstractStep abstractStep, params Action<AbstractStep>[] actions)
    {
        return abstractStep;
    }
}