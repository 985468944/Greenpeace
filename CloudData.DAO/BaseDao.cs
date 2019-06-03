using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloudDataMar.DAO
{
    public class BaseDao<T>
                  where T : class,
                 new()
    {
        ObjectContext objectContext = ObjectContextFactory.GetCurrentObjectContext() as ObjectContext;//获取EF上下文

        /// <summary>
        /// 加载实体集合
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public virtual IQueryable<T> LoadEntites()
        {
            return objectContext.CreateObjectSet<T>().AsQueryable<T>();
        }
        /// <summary>
        /// 加载实体集合
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public virtual IQueryable<T> LoadEntites(Func<T, bool> whereLambda)
        {
             return objectContext.CreateObjectSet<T>().Where<T>(whereLambda).AsQueryable<T>();
        }

        /// <summary>
        /// 分页加载数据
        /// </summary>
        /// <param name="whereLambda">过滤条件</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns></returns>
        public virtual IQueryable<T> LoadEntites(Func<T, bool> whereLambda, int pageStart, int pageSize, out int totalCount)
        {
            var tmp = objectContext.CreateObjectSet<T>().Where<T>(whereLambda);
            totalCount = tmp.Count();

            return tmp.Skip<T>(pageStart)//跳过行数，最终生成的sql语句是Top(n)
                      .Take<T>(pageSize) //返回指定数量的行
                      .AsQueryable<T>();
        }
        public virtual IQueryable<T> LoadEntites(Func<T, bool> whereLambda, string orderitem, bool isasc, int pageStart, int pageSize, out int totalCount)
        {
            IQueryable<T> queryable = this.LoadEntites(whereLambda);

            dynamic keyselector = GetLambdaExpression(orderitem);
            if (isasc)
            {
                queryable = Queryable.OrderBy(queryable, keyselector);
            }
            else
            {
                queryable = Queryable.OrderByDescending(queryable, keyselector);
            }


            totalCount = queryable.Count();

            return queryable.Skip<T>(pageStart)//跳过行数，最终生成的sql语句是Top(n)
                      .Take<T>(pageSize) //返回指定数量的行
                      .AsQueryable<T>();
        }

        public virtual IQueryable<T> Orderby(IQueryable<T> queryable, string propertyName, bool isasc)
        {
            dynamic keyselector = GetLambdaExpression(propertyName);
            if (isasc)
            {
                return Queryable.OrderBy(queryable, keyselector);
            }
            else
            {
                return Queryable.OrderByDescending(queryable, keyselector);
            }
        }

        private static LambdaExpression GetLambdaExpression(string propertyName)
        {

            var param = Expression.Parameter(typeof(T));
            var body = Expression.Property(param, propertyName);
            var keySelector = Expression.Lambda(body, param);

            return keySelector;
        }
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>返回更新后的实体</returns>
        public virtual T AddEntity(T entity)
        {
            objectContext.CreateObjectSet<T>().AddObject(entity);
            objectContext.SaveChanges();
            return entity;
        }
        public virtual List<T> AddEntity(List<T> listentity)
        {
            for (int i = 0; i < listentity.Count; i++)
            {
                T entity = listentity[i];
                objectContext.CreateObjectSet<T>().AddObject(entity);
                listentity[i] = entity;
            }

            objectContext.SaveChanges();
            return listentity;
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>返回更新后的实体</returns>
        public virtual T UpdateEntity(T entity)
        {
            using (ObjectContext objectContext = new ObjectContext("name=CloudDataContainer"))
            {
                objectContext.CreateObjectSet<T>().Attach(entity);
                objectContext.ObjectStateManager.ChangeObjectState(entity, System.Data.EntityState.Modified);//将附加的对象状态更改为修改
                objectContext.SaveChanges();
            }
            return entity;
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool DelEntity(T entity)
        {
            objectContext.CreateObjectSet<T>().Attach(entity);
            objectContext.ObjectStateManager.ChangeObjectState(entity, System.Data.EntityState.Deleted);//将附加的实体状态更改为删除
            if (objectContext.SaveChanges() > 0)
            {
                return true;//删除成功
            }
            else
            {
                return false;//删除失败
            }
        }

        /// <summary>
        /// 根据条件删除对象
        /// </summary>
        /// <param name="whereLambda">条件</param>
        /// <returns></returns>
        public virtual bool DelEntityByWhere(Func<T, bool> whereLambda)
        {
            var tmp = objectContext.CreateObjectSet<T>().Where<T>(whereLambda);//根据条件从数据库中获取对象集合
            foreach (var entity in tmp)
            {
                objectContext.CreateObjectSet<T>().DeleteObject(entity);//标记对象为删除状态删除
            }
            if (objectContext.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}
