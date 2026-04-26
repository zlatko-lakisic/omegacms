using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace MD.Tools.Helpers.Core.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class MDEntityUnauthorizedException : Exception
    {
        #region Child Classes
        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "<Pending>")]
        [Serializable]
        public class MDExceptionEntityMapping
        {
            #region Attributes
            private string _entity;
            private string[] _accessType;
            #endregion

            #region Properties
            /// <summary>
            /// 
            /// </summary>
            public string Entity { get => _entity; }
            /// <summary>
            /// 
            /// </summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "<Pending>")]
            public string[] AccessType { get => _accessType; }
            #endregion

            #region Methods
            /// <summary>
            /// 
            /// </summary>
            /// <param name="entity"></param>
            /// <param name="accessType"></param>
            public MDExceptionEntityMapping(string entity, params string[] accessType)
            {
                _entity = entity;
                _accessType = accessType;
            }
            #endregion
        }
        #endregion

        #region Attributes
        MDExceptionEntityMapping[] _mappings;
        private string _userId;
        private string _username;
        private HttpStatusCode _errorCode;
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "<Pending>")]
        public MDExceptionEntityMapping[] Mappings => _mappings;
        /// <summary>
        /// 
        /// </summary>
        public string UserId => _userId;
        /// <summary>
        /// 
        /// </summary>
        public HttpStatusCode ErrorCode { get => _errorCode; }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
#pragma warning disable CA1303 // Do not pass literals as localized parameters
        public MDEntityUnauthorizedException() : base("There is no user authenticated to make this call!")
#pragma warning restore CA1303 // Do not pass literals as localized parameters
        {
            _errorCode = HttpStatusCode.Unauthorized;
            Console.WriteLine(this.StackTrace);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <param name="mappings"></param>
        public MDEntityUnauthorizedException(string userId, string username, params MDExceptionEntityMapping[] mappings) : base("The current user does not have sufficient permission to access this resource!")
        {
            _errorCode = HttpStatusCode.Forbidden;

            _userId = userId;
            _username = username;
            _mappings = mappings;

            this.Data.Add("User Id", userId);
            this.Data.Add("Username", username);
            this.Data.Add("Mappings", mappings);
            Console.WriteLine(this.StackTrace);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <param name="innerException"></param>
        /// <param name="mappings"></param>
        public MDEntityUnauthorizedException(string userId, string username, Exception innerException, params MDExceptionEntityMapping[] mappings) : base("The current user does not have sufficient permission to access this resource!", innerException)
        {
            _errorCode = HttpStatusCode.Forbidden;

            _userId = userId;
            _username = username;
            _mappings = mappings;

            this.Data.Add("User Id", userId);
            this.Data.Add("Username", username);
            this.Data.Add("Mappings", mappings);
            Console.WriteLine(this.StackTrace);
        }
        #endregion
    }
}
