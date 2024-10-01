
using Ardalis.Specification;

namespace GlobalRoutes.SharedKernel.Interfaces
{
    public interface IAsyncRepository<T> : IRepositoryBase<T> where T : class
    {
        IQueryable<T> ApplySpecification(ISpecification<T> specification);

        public Task<bool> AnyAsync(int id, CancellationToken cancellationToken = default);

        public void Attach(object entity);

        public void Clear();
    }
}
