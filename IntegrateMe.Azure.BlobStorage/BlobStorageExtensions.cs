using IntegrateMe.Core;

namespace IntegrateMe.Azure.BlobStorage;

public static class BlobStorageExtensions
{
    public static BlobStorageStep BlobStorage(this AbstractStep parent, string? name = null)
    {
        if (name == null) return new BlobStorageStep(parent);
        if (parent.MainDsl.Entities.TryGetValue(name, out var step))
        {
            if (step is not BlobStorageStep storageStep)
            {
                throw new InvalidOperationException();
            }

            return storageStep;
        }

        var next = new BlobStorageStep(parent);
        parent.MainDsl.Entities.Add(name, next);
        return next;
    }
}