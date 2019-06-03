



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudDataMar.IDAO;

namespace CloudDataMar.DAO
{
  public partial class DBSession : IDBSession
  {
        
        private Idata_accessory_infoDao _data_accessory_infoDao;
        public Idata_accessory_infoDao data_accessory_infoDao
        {
            get
            {
                if (_data_accessory_infoDao == null)
                {
                    _data_accessory_infoDao = new data_accessory_infoDao();
                }
                return _data_accessory_infoDao;
            }
            set { _data_accessory_infoDao = value; }
        }
        
        private Idata_announce_infoDao _data_announce_infoDao;
        public Idata_announce_infoDao data_announce_infoDao
        {
            get
            {
                if (_data_announce_infoDao == null)
                {
                    _data_announce_infoDao = new data_announce_infoDao();
                }
                return _data_announce_infoDao;
            }
            set { _data_announce_infoDao = value; }
        }
        
        private Idata_applicationDao _data_applicationDao;
        public Idata_applicationDao data_applicationDao
        {
            get
            {
                if (_data_applicationDao == null)
                {
                    _data_applicationDao = new data_applicationDao();
                }
                return _data_applicationDao;
            }
            set { _data_applicationDao = value; }
        }
        
        private Idata_detail_infoDao _data_detail_infoDao;
        public Idata_detail_infoDao data_detail_infoDao
        {
            get
            {
                if (_data_detail_infoDao == null)
                {
                    _data_detail_infoDao = new data_detail_infoDao();
                }
                return _data_detail_infoDao;
            }
            set { _data_detail_infoDao = value; }
        }
        
        private Idata_infoDao _data_infoDao;
        public Idata_infoDao data_infoDao
        {
            get
            {
                if (_data_infoDao == null)
                {
                    _data_infoDao = new data_infoDao();
                }
                return _data_infoDao;
            }
            set { _data_infoDao = value; }
        }
        
        private Idata_specialDao _data_specialDao;
        public Idata_specialDao data_specialDao
        {
            get
            {
                if (_data_specialDao == null)
                {
                    _data_specialDao = new data_specialDao();
                }
                return _data_specialDao;
            }
            set { _data_specialDao = value; }
        }
        
        private Idata_statisticsDao _data_statisticsDao;
        public Idata_statisticsDao data_statisticsDao
        {
            get
            {
                if (_data_statisticsDao == null)
                {
                    _data_statisticsDao = new data_statisticsDao();
                }
                return _data_statisticsDao;
            }
            set { _data_statisticsDao = value; }
        }
        
        private Idata_type_infoDao _data_type_infoDao;
        public Idata_type_infoDao data_type_infoDao
        {
            get
            {
                if (_data_type_infoDao == null)
                {
                    _data_type_infoDao = new data_type_infoDao();
                }
                return _data_type_infoDao;
            }
            set { _data_type_infoDao = value; }
        }
        
        private Iuser_infoDao _user_infoDao;
        public Iuser_infoDao user_infoDao
        {
            get
            {
                if (_user_infoDao == null)
                {
                    _user_infoDao = new user_infoDao();
                }
                return _user_infoDao;
            }
            set { _user_infoDao = value; }
        }
        
        private Iuser_roleDao _user_roleDao;
        public Iuser_roleDao user_roleDao
        {
            get
            {
                if (_user_roleDao == null)
                {
                    _user_roleDao = new user_roleDao();
                }
                return _user_roleDao;
            }
            set { _user_roleDao = value; }
        }
    
 }
}