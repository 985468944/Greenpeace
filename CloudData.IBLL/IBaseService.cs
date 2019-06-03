using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudDataMar.IBLL
{
    public interface IBaseService<T>
                       where T : class,
                       new()
    {
        IQueryable<T> LoadEntites();
        //根据条件获取实体对象集合
        IQueryable<T> LoadEntites(Func<T, bool> whereLambda);

        //根据条件获取实体对象集合分页
        IQueryable<T> LoadEntites(Func<T, bool> whereLambda, int pageIndex, int pageSize, out int totalCount);

        IQueryable<T> LoadEntites(Func<T, bool> whereLambda, string orderitem, bool isasc, int pageStart,
            int pageSize, out int totalCount);
        IQueryable<T> Orderby(IQueryable<T> queryable, string propertyName, bool isasc);
        //增加
        T AddEntity(T entity);

       List<T> AddEntity(List<T> listentity);
        //更新
        T UpdateEntity(T entity);

        //删除
        bool DelEntity(T entity);

        //根据条件删除
        bool DelEntityByWhere(Func<T, bool> whereLambda);
    }
}
