namespace IntegrateMe.Core;

public interface IContainerRegistry
{
    string GetRepository();
    string LatestTag();
}