using System.Data;
using MD.Tools.BaseDataAccess.Core.Interfaces;
using MD.Tools.BaseDataAccess.Core.Entities;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.Helpers.Core;

namespace MD.Tools.BaseDataAccess.Core.Controllers
{
    public abstract class BaseController<T> : Singleton<T>
        where T : class, IBaseControllerSettings, new()
    {
        #region Attributes
        private bool _getDeleted;
        private bool _partialLoad;
        #endregion

        #region Properties
        /// <summary>
        /// Get deleted items
        /// </summary>
        public bool GetDeleted
        {
            get { return _getDeleted; }
            set { _getDeleted = value; }
        }
        /// <summary>
        /// Partially load the object
        /// </summary>
        public bool PartialLoad
        {
            get { return _partialLoad; }
            set { _partialLoad = value; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Create an instance of the base entity class
        /// </summary>
        /// <typeparam name="E">Type that inherits the BaseEntity class</typeparam>
        /// <typeparam name="K">Id property type</typeparam>
        /// <param name="row">Data row for entity</param>
        /// <param name="idColumnName">Name of the column for the Id property</param>
        /// <param name="isDeleteColumName">Name of the column for the IsDeleted property</param>
        /// <returns>Instance of class</returns>
        public virtual E Create<E, K>(DataRow row, string idColumnName = "", string isDeleteColumName = "")
            where E : BaseEntity<K>, new()
        {
            E obj = null;
            if (row != null)
            {
                obj = new E();
                if (!string.IsNullOrEmpty(idColumnName))
                {
                    obj.Id = row.GetValue<K>(idColumnName);
                }

                if (!string.IsNullOrEmpty(isDeleteColumName))
                {
                    obj.IsDeleted = row.GetValue<bool>(isDeleteColumName);
                }

            }
            return obj;
        }
        /// <summary>
        /// Create an instance of the base entity class
        /// </summary>
        /// <typeparam name="E">Type that inherits the BaseEntity class</typeparam>
        /// <param name="row">Data row for entity</param>
        /// <param name="idColumnName">Name of the column for the Id property</param>
        /// <param name="isDeleteColumName">Name of the column for the IsDeleted property</param>
        /// <returns>Instance of class</returns>
        public virtual E Create<E>(DataRow row, string idColumnName = "", string isDeleteColumName = "")
            where E : BaseEntity, new()
        {
            return Create<E, int>(row, idColumnName, isDeleteColumName);
        }
        #endregion
    }
}
