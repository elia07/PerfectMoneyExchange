using LinqKit;
using RockCandy.Web.Framework.Core.Enumerations;
using RockCandy.Web.Framework.Core.Interfaces;
using RockCandy.Web.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.RepositoryDefinition
{
    public abstract class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : class
    {
        bool savePermission = true;
        public Saraf365Entities db;
        public DbSet<T> dbSet;

        public GenericRepository(Saraf365Entities dbInstance = null, bool disableLazyLoading = false)
        {
            if (dbInstance == null)
            {
                this.db = new Saraf365Entities();
            }
            else
            {
                savePermission = false;
                this.db = dbInstance;
            }

            this.dbSet = db.Set<T>();

            if (disableLazyLoading)
            {
                db.Configuration.LazyLoadingEnabled = false;
            }

        }

        public IQueryable<T> GetAll(bool NoTracking = false)
        {
            IQueryable<T> objT = dbSet;
            if (NoTracking)
            {
                return objT.AsNoTracking();
            }
            else
            {
                return objT;
            }

        }

        public IQueryable<T> GetAll<TKey>(bool NoTracking = false)
        {
            IQueryable<T> objT = dbSet;
            if (NoTracking)
            {
                return objT.AsNoTracking().OrderByDescending(GetExpression<T, TKey>());
            }
            else
            {
                return objT.OrderByDescending(GetExpression<T, TKey>());
            }

        }

        public IQueryable<T> GetAll(Pagination pagination, bool NoTracking = false)
        {
            IQueryable<T> objT = dbSet;
            if (NoTracking)
            {
                return objT.AsNoTracking().Skip(pagination.CurrentPage - 1).Take(pagination.ItemsPerPage);
            }
            else
            {
                return objT.Skip(pagination.CurrentPage - 1).Take(pagination.ItemsPerPage);
            }

        }

        public IQueryable<T> GetAll<TKey>(Pagination pagination, OrderByType orderByType, string OrderByFieldName = "xID", bool NoTracking = false)
        {
            IQueryable<T> objT = dbSet;

            if (NoTracking)
            {
                objT = objT.AsNoTracking();
            }

            if (orderByType == OrderByType.Desc)
            {
                if (pagination.ItemsPerPage != 0)
                {
                    return objT.OrderByDescending(GetExpression<T, TKey>(OrderByFieldName)).Skip((pagination.CurrentPage - 1) * pagination.ItemsPerPage).Take(pagination.ItemsPerPage);
                }
                else
                {
                    return objT.OrderByDescending(GetExpression<T, TKey>(OrderByFieldName));
                }
            }
            else
            {
                if (pagination.ItemsPerPage != 0)
                {
                    return objT.OrderBy(GetExpression<T, TKey>(OrderByFieldName)).Skip((pagination.CurrentPage - 1) * pagination.ItemsPerPage).Take(pagination.ItemsPerPage);
                }
                else
                {
                    return objT.OrderBy(GetExpression<T, TKey>(OrderByFieldName));
                }
            }

        }

        public long Count()
        {
            return dbSet.Count();
        }

        public T GetByID(object id, bool NoTracking = false)
        {
            if (NoTracking)
            {
                var entity = dbSet.Find(id);
                db.Entry(entity).State = EntityState.Detached;
                return entity;
            }
            else
            {
                return dbSet.Find(id);
            }

        }

        public T GetByID(long id, bool NoTracking = false)
        {
            if (NoTracking)
            {
                var entity = dbSet.Find(id);
                db.Entry(entity).State = EntityState.Detached;
                return entity;
            }
            else
            {
                return dbSet.Find(id);
            }

        }

        public long Count(Expression<Func<T, bool>> predicate)
        {
            try
            {
                if(predicate==null)
                {
                    return dbSet.Count();
                }
                else
                {
                    return dbSet.AsExpandable().Where(predicate).Count();
                }
                

            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public IQueryable<T> FilterByParam(SearchModelManager searchModelManager, Expression<Func<T, bool>> predicate, Pagination pagination, OrderByType orderByType, string OrderByFieldName = "xID")
        {

            if (searchModelManager.OrderByFieldNameType == null)
            {
                return FilterByParam<long>(predicate, pagination, searchModelManager.OrderByType);
            }
            else if (searchModelManager.OrderByFieldNameType == typeof(string))
            {
                return FilterByParam<string>(predicate, pagination, searchModelManager.OrderByType, searchModelManager.OrderByFieldName);
            }
            else if (searchModelManager.OrderByFieldNameType == typeof(DateTime))
            {
                return FilterByParam<DateTime>(predicate, pagination, searchModelManager.OrderByType, searchModelManager.OrderByFieldName);
            }
            else if (searchModelManager.OrderByFieldNameType.IsEnum || searchModelManager.OrderByFieldNameType == typeof(Int16))
            {
                return FilterByParam<Int16>(predicate, pagination, searchModelManager.OrderByType, searchModelManager.OrderByFieldName);
            }
            else if (searchModelManager.OrderByFieldNameType == typeof(long))
            {
                return FilterByParam<long>(predicate, pagination, searchModelManager.OrderByType, searchModelManager.OrderByFieldName);
            }
            else if (searchModelManager.OrderByFieldNameType == typeof(int))
            {
                return FilterByParam<int>(predicate, pagination, searchModelManager.OrderByType, searchModelManager.OrderByFieldName);
            }
            else
            {
                return null;
            }
        }

        public IQueryable<T> FilterByParam<TKey>(Expression<Func<T, bool>> predicate, Pagination pagination, OrderByType orderByType, string OrderByFieldName = "xID", bool NoTracking = false)
        {

            if (NoTracking)
            {
                try
                {
                    if (orderByType == OrderByType.Desc)
                    {
                        if (predicate != null)
                        {
                            if (pagination.ItemsPerPage != 0)
                            {
                                return dbSet.AsNoTracking().AsExpandable().Where(predicate).Distinct().OrderByDescending(GetExpression<T, TKey>(OrderByFieldName)).Skip((pagination.CurrentPage - 1) * pagination.ItemsPerPage).Take(pagination.ItemsPerPage);
                            }
                            else
                            {
                                return dbSet.AsNoTracking().AsExpandable().Where(predicate).Distinct().OrderByDescending(GetExpression<T, TKey>(OrderByFieldName));
                            }
                        }
                        else
                        {
                            return GetAll<TKey>(pagination, orderByType, OrderByFieldName, NoTracking);
                        }
                    }
                    else
                    {
                        if (predicate != null)
                        {
                            if (pagination.ItemsPerPage != 0)
                            {
                                return dbSet.AsNoTracking().AsExpandable().Where(predicate).Distinct().OrderBy(GetExpression<T, TKey>(OrderByFieldName)).Skip((pagination.CurrentPage - 1) * pagination.ItemsPerPage).Take(pagination.ItemsPerPage);
                            }
                            else
                            {
                                return dbSet.AsNoTracking().AsExpandable().Where(predicate).Distinct().OrderBy(GetExpression<T, TKey>(OrderByFieldName));
                            }
                        }
                        else
                        {
                            return GetAll<TKey>(pagination, orderByType, OrderByFieldName, NoTracking);
                        }
                    }

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            else
            {

                try
                {
                    if (orderByType == OrderByType.Desc)
                    {
                        if (predicate != null)
                        {
                            if (pagination.ItemsPerPage != 0)
                            {
                                return dbSet.AsExpandable().Where(predicate).Distinct().OrderByDescending(GetExpression<T, TKey>(OrderByFieldName)).Skip((pagination.CurrentPage - 1) * pagination.ItemsPerPage).Take(pagination.ItemsPerPage);
                            }
                            else
                            {
                                return dbSet.AsExpandable().Where(predicate).Distinct().OrderByDescending(GetExpression<T, TKey>(OrderByFieldName));
                            }
                        }
                        else
                        {
                            return GetAll<TKey>(pagination, orderByType, OrderByFieldName, NoTracking);
                        }
                    }
                    else
                    {
                        if (predicate != null)
                        {
                            if (pagination.ItemsPerPage != 0)
                            {
                                return dbSet.AsExpandable().Where(predicate).Distinct().OrderBy(GetExpression<T, TKey>(OrderByFieldName)).Skip((pagination.CurrentPage - 1) * pagination.ItemsPerPage).Take(pagination.ItemsPerPage);
                            }
                            else
                            {
                                return dbSet.AsExpandable().Where(predicate).Distinct().OrderBy(GetExpression<T, TKey>(OrderByFieldName));
                            }
                        }
                        else
                        {
                            return GetAll<TKey>(pagination, orderByType, OrderByFieldName, NoTracking);
                        }
                    }

                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }




        }

        public void Insert(T obj)
        {
            dbSet.Add(obj);
        }

        public void Update(T obj)
        {
            try
            {
                dbSet.Attach(obj);
                db.Entry(obj).State = EntityState.Modified;
            }
            catch { };
        }

        public void Delete(object id)
        {


            T ObjT = dbSet.Find(id);
            if (ObjT != null)
            {
                Delete(ObjT);
            }

        }

        public void Delete(long id)
        {


            T ObjT = dbSet.Find(id);
            if (ObjT != null)
            {
                Delete(ObjT);
            }

        }

        public virtual void Delete(T obj)
        {


            if (db.Entry(obj).State == EntityState.Detached)
            {
                dbSet.Attach(obj);
            }
            dbSet.Remove(obj);
        }

        public void Save()
        {

            if (!savePermission)
            {
                return;
            }
            try
            {
                db.SaveChanges();
            }

            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Dispose()
        {
            if (db != null)
            {
                db.Dispose();
                db = null;
                GC.Collect();
            }
        }

        void IDisposable.Dispose()
        {
            if (db != null)
            {
                Save();
                Dispose();
                GC.Collect();
            }
        }


        public Expression<Func<T, TKey>> GetExpression<T, TKey>(string OrderByFieldName = "xID")
        {
            var param = Expression.Parameter(typeof(T), typeof(T).Name);
            return Expression.Lambda<Func<T, TKey>>(Expression.Convert(Expression.Property(param, OrderByFieldName), typeof(TKey)), param);
        }
    }
}
