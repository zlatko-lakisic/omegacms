using Google.Apis.Auth.OAuth2;
using MD.Tools.Helpers.Core.Serializer;
using System;

namespace MD.CMS.BusinessLogic.GoogleCloud.Core.FileProviders
{
    public class GoogleCloudStorageFileProviderOptions
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Bucket { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool CacheFiles { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CacheLocation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public dynamic CredentialsJson { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CredentialsJsonLocation { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GoogleCredential GetCredentials()
        {
            if (CredentialsJson != null)
            {
                return GoogleCredential.FromJson(OmegaJsonSerializer.SerializeObject(CredentialsJson));
            }
            if (!string.IsNullOrEmpty(CredentialsJsonLocation))
            {
                return GoogleCredential.FromFile(CredentialsJsonLocation);
            }
            throw new ArgumentNullException("Could not generate Google Credentials, Json String or Json File Location required!");
        }
        #endregion
    }
}
