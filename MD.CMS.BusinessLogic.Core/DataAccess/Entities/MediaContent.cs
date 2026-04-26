using MD.Tools.BaseDataAccess.Core.Entities;
using MD.CMS.BusinessLogic.Core.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner.CustomAttributes;
using System.Globalization;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class MediaContent : BaseEntity<long>
    {  
        
        #region Attributes
        private int _LCID;
        private long _FolderId;
        private int _FileType;
        private string _Size;
        private string _Path;
        private string _Name;
        private string _Description; 
        private EnumType _type;
        private EnumInputType _inputType;
        private string _previewUrl;
        private string _fullNameFile;
        private List<MediaContentMetaDataFieldValues> _mediaContentMetaDataFieldValues;
        private DateTime _dateCreated;
        private bool _isPublished;
        #endregion

        #region Properties

        public int LCID
        {

            get
            {
                if (_LCID.Equals(default(int)))
                {
                    _LCID = Settings.Default.DefaultLcid;
                }
                return _LCID;
            }
            set { _LCID = value; }
        }
        public long FolderId
        {
            get { return _FolderId; }
            set { _FolderId = value; }
        }
        public int FileType
        {
            get { return _FileType; }
            set { _FileType = value; }
        
        }
        public string Size
        {
            get { return _Size; }
            set { _Size = value; }
        }


        public string DateCreated
        {
            get
            {
                return _dateCreated.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _dateCreated = DateTime.Parse(value, CultureInfo.InvariantCulture);
                }
                else
                {
                    _dateCreated = DateTime.UtcNow;
                }

            }
        }

        public string Path
        {
            get { return _Path; }
            set { _Path = value; }
        }
        public string PreviewUrl
        {
            get { return _previewUrl; }
            set { _previewUrl = value; }
        }
        public string FullNameFile
        {
            get { return _fullNameFile; }
            set { _fullNameFile = value; }
        }
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public bool IsNew
        {
            get
            {
                return Id.Equals(default(long));
            }
        }

        [OmitPropertyFromReport]
        public List<MediaContentMetaDataFieldValues> MediaContentMetaDataFieldValues
        {
            get { return _mediaContentMetaDataFieldValues; }
            set { _mediaContentMetaDataFieldValues = value; }
        }

        [OmitPropertyFromReport]
        public EnumType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        [OmitPropertyFromReport]
        public EnumInputType InputType
        {
            get { return _inputType; }
            set { _inputType = value; }
        }
        /// <summary>
        /// Is Published?
        /// </summary>
        public bool IsPublished
        {
            get { return _isPublished; }
            set { _isPublished = value; }
        }
        public string UniqueId
        {
            get
            {
                if (!this.IsNew)
                {
                    return string.Format("{0}-{1}", this.Id, this.LCID);
                }
                return string.Empty;
            }
        }
        #endregion

        //Constructors
        public MediaContent(string path, string nameFile)
            : base()
        {
            this.Path = path;
            this.Name = nameFile;

        }
        public MediaContent()
        {
            this.Path = default(string);
            this.LCID = default(int);
            this.Name = default(string);
            this.Size = default(string);
            this.FileType = (default(int));
            this.Description = default(string);
            this.FolderId = default(int);

        }

        #region Enums
        public enum EnumType : int
        {
           
            Int = 1
           

        }
        public enum EnumInputType : int
        {
            jpg=1,
			txt = 2,
			mp4 =3,
            JPG = 4,
            png = 5,
            PNG = 6,
            flv = 7,
            mkv = 8,
            jpeg=9,
            JPEG=10,
            pdf = 11,
            docx = 12,
            xls = 13,
            xlsx = 14
        }

      
        #endregion
    }
}
