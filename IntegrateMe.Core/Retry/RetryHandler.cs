﻿namespace IntegrateMe.Core.Retry;

public class RetryHandler
{
    public TimeSpan Delay { get; set; } = TimeSpan.Zero;
    public int MaxRetries { get; set; } = 1;
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
        for (var i = 0; i < MaxRetries; i++)
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