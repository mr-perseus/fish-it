using System;
using System.Runtime.Serialization;

namespace Fishit.Common.Exceptions
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

        protected OptimisticConcurrencyException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {

        }

        public T MergedEntity { get; set; }
    }
}