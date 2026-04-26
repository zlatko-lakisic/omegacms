namespace MD.CMS.BusinessLogic.Aws.Core.FileProviders.S3
{
    /// <summary>
    /// 
    /// </summary>
    public class AWSS3FileProviderOptions
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public string BucketName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AccessKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SecretKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RegionDisplayName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool CacheFiles { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CacheLocation { get; set; }
        #endregion
    }
}
