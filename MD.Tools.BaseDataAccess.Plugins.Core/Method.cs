using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MD.Tools.BaseDataAccess.Plugins.Core
{
    public class Method : PagedMethod, IMethodStatus, IDisposable
    {
        #region Delegates
        public delegate void OnBeforeExecuteHandler(string args);
        public delegate void OnAfterExecuteHandler(string args);
        #endregion

        #region Events
        public event OnBeforeExecuteHandler OnBeforeExecute;
        public event OnAfterExecuteHandler OnAfterExecute;
        #endregion

        #region Attributes
        private Mapping.Entities _entity;
        private int _id;
        private List<IMethodProperty> _properties;
        private Mapping.MethodTypes _methodType;
        private bool _operationStarted;
        private bool? _onAfterCompleted;
        private bool? _onBeforeCompleted;
        private bool _clearCache;
        //findme
        private bool _reindexingFinished;
        private bool _endCalled;
        #endregion

        #region Properties
        /// <summary>
        /// Method Entity
        /// </summary>
        public Mapping.Entities Entity
        {
            get { return _entity; }
            set { _entity = value; }
        }
        /// <summary>
        /// Method Id
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// Method properties
        /// </summary>
        public List<IMethodProperty> Properties
        {
            get 
            { 
                if(_properties == null){
                    _properties = new List<IMethodProperty>();
                }
                return _properties; 
            }
            set { _properties = value; }
        }
        /// <summary>
        /// Method type
        /// </summary>
        public Mapping.MethodTypes MethodType
        {
            get { return _methodType; }
            set { _methodType = value; }
        }
        /// <summary>
        /// Operation status
        /// </summary>
        public bool OperationStarted
        {
            get { return _operationStarted; }
        }

        public bool? OnAfterCompleted
        {
            get { return _onAfterCompleted; }
            set { _onAfterCompleted = value; }
        }

        public bool? OnBeforeCompleted
        {
            get { return _onBeforeCompleted; }
            set { _onBeforeCompleted = value; }
        }
        /// <summary>
        /// Clear the cache
        /// </summary>
        public bool ClearCache { get => _clearCache; set => _clearCache = value; }
        #endregion

        #region Operators
        /// <summary>
        /// Sets or gets method properties by name
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IMethodProperty this[int key]
        {
            get
            {
                return _properties.FirstOrDefault(mp => mp.Id == key);
            }
            set
            {
                IMethodProperty obj = this[key];
                if (obj != null)
                {
                    obj = value;
                }
                else
                {
                    _properties.Add(value);
                }
            }
        }
        #endregion

        #region Methods
        public Method()
        {
            _operationStarted = true;
            //findme
            _reindexingFinished = false;
            _onAfterCompleted = null;
            _onBeforeCompleted = null;
            _endCalled = false;
        }

        public void End()
        {
            _operationStarted = false;
            _endCalled = true;
        }

        public void Dispose()
        {
            if (!_endCalled)
            {
                End();
            }
        }

        public void WaitForOnAfterCompleted()
        {
            //if (_onAfterCompleted != null)
            //{
            //    while (_onAfterCompleted == false)
            //    {
            //        Thread.Sleep(20);
            //    }
            //}
        }

        //public void waitforonbeforecompleted()
        //{
        //    //if (_onbeforecompleted != null)
        //    //{
        //    //    while (_onbeforecompleted == false)
        //    //    {
        //    //        thread.sleep(10);
        //    //    }
        //    //}
        //}
        #endregion
    }
}
