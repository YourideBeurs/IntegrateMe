namespace IntegrateMe.Core;

public static class CoreExtensions
{
    public static Dsl Given()
    {
        var dsl = new Dsl(null!);
        dsl.MainDsl = dsl;
        return dsl;
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