using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Repositories;

namespace BEZNgCore.EntityFrameworkCore.Repositories;

/// <summary>
/// Base class for custom repositories of the application.
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
/// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
public abstract class BEZNgCoreRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<BEZNgCoreDbContext, TEntity, TPrimaryKey>
    where TEntity : class, IEntity<TPrimaryKey>
{
    protected BEZNgCoreRepositoryBase(IDbContextProvider<BEZNgCoreDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    //add your common methods for all repositories
}

/// <summary>
/// Base class for custom repositories of the application.
/// This is a shortcut of <see cref="BEZNgCoreRepositoryBase{TEntity,TPrimaryKey}"/> for <see cref="int"/> primary key.
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
public abstract class BEZNgCoreRepositoryBase<TEntity> : BEZNgCoreRepositoryBase<TEntity, int>
    where TEntity : class, IEntity<int>
{
    protected BEZNgCoreRepositoryBase(IDbContextProvider<BEZNgCoreDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    //do not add any method here, add to the class above (since this inherits it)!!!
}

