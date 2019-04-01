using System;
using System.Runtime.Serialization;

namespace Fishit.Common.Exceptions
{
    [Serializable]
    public class OptimisticConcurrencyException<T>
        : Exception
    {
        private OptimisticConcurrencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public OptimisticConcurrencyException(string message, T mergedEntity) : base(message)
        {
            MergedEntity = mergedEntity;
        }

        public T MergedEntity { get; set; }
    }
}