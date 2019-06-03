using CloudDataMar.DAO;
using CloudDataMar.IDAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudDataMar.BLL
{
    public abstract class BaseService<T>
                   where T : class,
                    new()
    {
        //构造函数
        public BaseService()
        {
            //调用SetCurrentDao()方法，要求子类必须实现
            SetCurrentDao();
        }

        //获取EF实体工厂
        IDBSessionFactory _dbSessionFactory;
        IDBSession _dbSession;

        public IDBSessionFactory DbSessionFactory
        {
            get
            {
                if (_dbSessionFactory == null)
                {
                    _dbSessionFactory = new DBSessionFactory();
                }
                return _dbSessionFactory;
            }
            set { _dbSessionFactory = value; }
        }


        public IDBSession DbSession
        {
            get
            {
                if (_dbSession == null)
                {
                    _dbSession = DbSessionFactory.GetCurrentDBSession();//通过数据访问层提供的工厂获取EF实体对象
                }
                return _dbSession;
            }
            set { _dbSession = value; }
        }
        //数据访问层基接口类型可以接收数据访问层的所有实体Dao
        public IBaseDao<T> CurrentDao { get; set; }

        //该方法用于子类实现，其作用是设置相应的实体Dao
        public abstract void SetCurrentDao();

        //以下是CRUD实现
        public virtual IQueryable<T> LoadEntites(Func<T, bool> whereLambda)
        {
            return this.CurrentDao.LoadEntites(whereLambda);
        }

        public virtual IQueryable<T> LoadEntites()
        {
            return this.CurrentDao.LoadEntites();
        }

        public virtual IQueryable<T> LoadEntites(Func<T, bool> whereLambda, int pageIndex, int pageSize, out int totalCount)
        {
            return this.CurrentDao.LoadEntites(whereLambda, pageIndex, pageSize, out totalCount);
        }
        public virtual IQueryable<T> LoadEntites(Func<T, bool> whereLambda, string orderitem,bool isasc,int pageIndex, int pageSize, out int totalCount)
        {
          
            if (!string.IsNullOrEmpty(orderitem))
            {
                return this.CurrentDao.LoadEntites(whereLambda, orderitem, isasc, pageIndex, pageSize, out totalCount);
            }
            else
            {
                return this.CurrentDao.LoadEntites(whereLambda, pageIndex, pageSize, out totalCount);
            }
        }

        public virtual IQueryable<T> Orderby(IQueryable<T> queryable,string propertyName,bool isasc)
        {
            return this.Orderby(queryable, propertyName, isasc);
        }

        public virtual T AddEntity(T entity)
        {
            var tmp = this.CurrentDao.AddEntity(entity);
            this.DbSession.SaveChange();
            return tmp;
        }
      public virtual  List<T>  AddEntity(List<T> listentity)
        {
            listentity = this.CurrentDao.AddEntity(listentity);
            this.DbSession.SaveChange();
            return listentity;
        }

        public virtual T UpdateEntity(T entity)
        {
            var tmp = this.CurrentDao.UpdateEntity(entity);
            this.DbSession.SaveChange();
            return tmp;
        }


        public virtual bool DelEntity(T entity)
        {
            return this.CurrentDao.DelEntity(entity);
        }


        public virtual bool DelEntityByWhere(Func<T, bool> whereLambda)
        {
            return this.CurrentDao.DelEntityByWhere(whereLambda);
        }
    }
}
