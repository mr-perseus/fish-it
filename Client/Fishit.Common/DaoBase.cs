using Fishit.Common.Exceptions;
using Fishit.Dal;
using Fishit.Logging;
using Microsoft.EntityFrameworkCore;

namespace Fishit.Common
{
    public class DaoBase
    {
        private readonly ILogger _logger;

        protected DaoBase()
        {
            _logger = LogManager.GetLogger(nameof(DaoBase));
        }

        protected void HandleDbUpdateException<T>(DbUpdateException exception, FishitContext context, T entity)
            where T : class
        {
            _logger.Error(nameof(HandleDbUpdateException) + "; entity; " + entity, exception);

            if (exception is DbUpdateConcurrencyException)
                throw CreateOptimisticConcurrencyException(context, entity);

            throw exception;
        }

        private OptimisticConcurrencyException<T> CreateOptimisticConcurrencyException<T>(
            DbContext context, T entity)
            where T : class
        {
            _logger.Error(nameof(HandleDbUpdateException) + "; Start; " + "entity; " + entity);

            T dbEntity = (T) context.Entry(entity)
                .GetDatabaseValues()
                .ToObject();

            OptimisticConcurrencyException<T> optimisticConcurrencyException =
                new OptimisticConcurrencyException<T>($"Update {typeof(T).Name}: Concurrency-Error", dbEntity);

            _logger.Error(nameof(HandleDbUpdateException) + "; End; " + "optimisticConcurrencyException; " +
                          optimisticConcurrencyException);

            return optimisticConcurrencyException;
        }
    }
}