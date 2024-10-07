namespace IntegrateMe.Core;

public static class CoreContext
{
    public static Dsl Given()
    {
        return new Dsl();
    }

    public static IStep Parallel(this IStep step, Action<IStep> action1, Action<IStep> action2)
    {
        return step;
    }

    public static IStep Parallel(this IStep step, params Action<IStep>[] actions)
    {
        return step;
    }
}