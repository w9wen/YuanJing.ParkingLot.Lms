using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using w9wen.SharedKernel;
using w9wen.SharedKernel.Interfaces;

namespace w9wen.dotnet.Template.Infrastructure.Data;

// inherit from Ardalis.Specification type
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : BaseEntity, IAggregateRoot
{
  private readonly AppDbContext _dbContext;

  public EfRepository(AppDbContext dbContext) : base(dbContext)
  {
    _dbContext = dbContext;
  }
  public override async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
  {
    var list = await _dbContext.Set<T>().AsQueryable().Where(x => x.ValidFlag).ToListAsync();
    return list;
  }

  public override async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    var specificationResult = ApplySpecification(specification);
    return await specificationResult.ToListAsync(cancellationToken);
  }

  public override async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
  {
    var keyValues = new object[] { id };
    var entity = await _dbContext.Set<T>().FindAsync(keyValues, cancellationToken);

    if (entity != null && entity.ValidFlag)
    {
      return entity;
    }
    else
    {
      return null;
    }
  }

  public override async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
  {
    var entityPropertieList = entity.GetType().GetProperties();

    var createdUserPropertyInfo = entityPropertieList.Where(property => property.Name.Contains("CreatedUser")).FirstOrDefault();

    if (createdUserPropertyInfo != null)
    {
      var now = DateTime.UtcNow;
      // var userName = this._appUserManager.CurrentName ?? "System";
      var userName = "System";

      entity.CreatedUser = userName;
      entity.CreatedDateTime = now;
      entity.UpdatedUser = userName;
      entity.UpdatedDateTime = now;
      entity.ValidFlag = true;
    }

    await _dbContext.Set<T>().AddAsync(entity);
    await _dbContext.SaveChangesAsync(cancellationToken);

    return entity;
  }

  public override async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
  {
    var entityPropertieList = entity.GetType().GetProperties();

    var updatedUserPropertyInfo = entityPropertieList.Where(property => property.Name.Contains("UpdatedUser")).FirstOrDefault();

    if (updatedUserPropertyInfo != null)
    {
      var now = DateTime.UtcNow;
      // var userName = this._appUserManager.CurrentName ?? "System";
      var userName = "System";

      entity.UpdatedUser = userName;
      entity.UpdatedDateTime = now;
      entity.UpdatedTimes++;
    }

    _dbContext.Entry(entity).State = EntityState.Modified;
    await _dbContext.SaveChangesAsync(cancellationToken);
  }

  public override async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
  {
    // var item = await GetByIdAsync(entity.Id);
    entity.ValidFlag = false;
    await UpdateAsync(entity, cancellationToken);
  }

  protected override IQueryable<T> ApplySpecification(ISpecification<T> specification, bool evaluateCriteriaOnly = false)
  {
    var evaluator = new SpecificationEvaluator();
    return evaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), specification).Where(x => x.ValidFlag);
  }
}
