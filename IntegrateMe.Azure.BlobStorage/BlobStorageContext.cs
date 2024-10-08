using IntegrateMe.Core;

namespace IntegrateMe.Azure.BlobStorage;

public static class BlobStorageContext
{
    public static BlobStorageAbstractStep BlobStorage(this AbstractStep parent, string? name = null)
    {
        if (name == null) return new BlobStorageAbstractStep(parent);
        if (parent.MainDsl.Entities.TryGetValue(name, out var step))
        {
            if (step is not BlobStorageAbstractStep storageStep)
            {
                throw new InvalidOperationException();
            }

            return storageStep;
        }

        var next = new BlobStorageAbstractStep(parent);
        parent.MainDsl.Entities.Add(name, next);
        return next;
    }
}