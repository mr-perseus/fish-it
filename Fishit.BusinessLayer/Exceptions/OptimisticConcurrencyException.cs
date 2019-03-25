using System;

namespace Fishit.BusinessLayer.Exceptions
{
    [Serializable]
    public class OptimisticConcurrencyException<T>
        : Exception
    {
        public OptimisticConcurrencyException()
        {
        }

        public OptimisticConcurrencyException(string message) : base(message)
        {
        }

        public OptimisticConcurrencyException(string message, T mergedEntity) : base(message)
        {
            MergedEntity = mergedEntity;
        }

        public T MergedEntity { get; set; }
    }
}