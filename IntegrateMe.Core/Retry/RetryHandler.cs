namespace IntegrateMe.Core.Retry;

public class RetryHandler
{
    public int Delay { get; set; } = 0;
    public int Iterations { get; set; } = 1;
    private bool _success;

    public static RetryHandler DefaultRetryHandler()
    {
        return new RetryHandler();
    }

    public void Success()
    {
        _success = true;
    }

    public bool IsSuccess()
    {
        return _success;
    }

    public async Task RunAsync(Func<Task> task)
    {
        for (var i = 0; i < Iterations; i++)
        {
            await task();
            if (_success)
            {
                break;
            }

            await Task.Delay(Delay);
        }
    }
}