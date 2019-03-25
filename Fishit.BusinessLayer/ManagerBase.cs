using Fishit.BusinessLayer.Exceptions;
using Fishit.Dal;
using Microsoft.EntityFrameworkCore;

namespace Fishit.BusinessLayer
{
    public abstract class ManagerBase
    {
        protected void HandleDbUpdateException<T>(DbUpdateException exception, FishitContext context, T entity)
            where T : class
        {
            if (exception is DbUpdateConcurrencyException)
                throw CreateOptimisticConcurrencyException(context, entity);

            throw exception;
        }

        protected static OptimisticConcurrencyException<T> CreateOptimisticConcurrencyException<T>(
            FishitContext context, T entity)
            where T : class
        {
            T dbEntity = (T) context.Entry(entity)
                .GetDatabaseValues()
                .ToObject();

            return new OptimisticConcurrencyException<T>($"Update {typeof(T).Name}: Concurrency-Fehler", dbEntity);
        }
    }
}