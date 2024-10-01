using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using GlobalRoutes.Infrastructure.Contexts;
using GlobalRoutes.SharedKernel.Entities;
using GlobalRoutes.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GlobalRoutes.Infrastructure.Repositories
{
    public class AsyncRepository<T> : RepositoryBase<T>, IAsyncRepository<T> where T : BaseEntity
    {
        private readonly GlobalRoutesContext _dbContext;

        public AsyncRepository(GlobalRoutesContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AnyAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().AnyAsync(e => e.Id == id, cancellationToken);
        }

        public IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluator.Default.GetQuery(_dbContext.Set<T>().AsQueryable(), specification);
        }

        public void Attach(object entity)
        {
            _dbContext.Attach(entity);
        }

        public void Clear()
        {
            _dbContext.ChangeTracker.Clear();
        }
    }
}
