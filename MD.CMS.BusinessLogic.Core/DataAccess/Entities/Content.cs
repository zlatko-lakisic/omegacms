using MD.CMS.BusinessLogic.Core.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using MD.Tools.BaseDataAccess.Core.Entities;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner.CustomAttributes;
using System.Diagnostics.CodeAnalysis;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class Content : BaseEntity<string>, IComparable<Content>
    {
        #region Attributes
        private int _LCID;
        private DateTime _dateCreated;
        private string _authorId;
        private long _folderId;
        private string _title;
        private string _path;
        private string _html;
        private User _author;
        private ContentTypeDefinition<ContentTypeDefinitionFieldValue> _contentType;
        private List<MetaDataFieldValue> _metaDataFieldValues;
        private List<Taxonomy> _taxonomy;
        private List<Menu> _menu;
        private long _taxonomyId;
        private List<ContentAlias> _contentAliases;
        private long _contentTypeDefinitionId;
        private Template _template;
        private bool _isPublished;
        private bool _approvalPending;
        private bool? _isNew;
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

        public string DateCreated
        {
            get
            {
                return _dateCreated.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _dateCreated = DateTime.Parse(value);
                }
                else
                {
                    _dateCreated = DateTime.UtcNow;
                }

            }
        }

        public bool IsDataBound
        {
            get
            {
                return ContentType != null && ContentType.Fields.Any(f => f.DataBound);
            }
        }

        [OmitPropertyFromReport]
        public List<ContentAlias> ContentAliases
        {
            get
            {
                if (_contentAliases == null)
                {
                    _contentAliases = new List<ContentAlias>();
                }
                return _contentAliases;
            }
            set { _contentAliases = value; }
        }
        public string AuthorId
        {
            get { return _authorId; }
            set { _authorId = value; }
        }
        public long ContentTypeDefinitionId
        {
            get { return _contentTypeDefinitionId; }
            set { _contentTypeDefinitionId = value; }
        }
        public long FolderId
        {
            get { return _folderId; }
            set { _folderId = value; }
        }
        public long TaxonomyId
        {
            get { return _taxonomyId; }
            set { _taxonomyId = value; }
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        public string Path
        {
            get { return _path; }
            set { _path = value; }
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

		//[JsonIgnore : JsonNotIgnore]      
		public string Html
		{
			get { return _html; }
			set { _html = value; }
		}
		[OmitPropertyFromReport]
		public User Author
		{
			get { return _author; }
			set { _author = value; }
		}

		[OmitPropertyFromReport]
		public ContentTypeDefinition<ContentTypeDefinitionFieldValue> ContentType
		{
			get { return _contentType; }
			set { _contentType = value; }
		}

		[OmitPropertyFromReport]
		public bool IsNew
		{
			get
			{
                if(_isNew != null)
                {
                    return _isNew.Value;
                }
				return string.IsNullOrEmpty(Id);
			}
            set
            {
                _isNew = value;
            }
		}

		[OmitPropertyFromReport]
		public List<Taxonomy> Taxonomy
		{
			get { return _taxonomy; }
			set { _taxonomy = value; }
		}

		[OmitPropertyFromReport]
		public List<Menu> Menu
		{
			get { return _menu; }
			set { _menu = value; }
		}

		[OmitPropertyFromReport]
		public List<MetaDataFieldValue> MetaDataFieldValues
		{
			get { return _metaDataFieldValues; }
			set { _metaDataFieldValues = value; }
		}

		[OmitPropertyFromReport]
		public Template Template
		{
			get { return _template; }
			set { _template = value; }
		}
		/// <summary>
		/// Is Published?
		/// </summary>
		public bool IsPublished
		{
			get { return _isPublished; }
			set { _isPublished = value; }
		}
		public bool ApprovalPending
		{
			get { return _approvalPending; }
			set { _approvalPending = value; }
		}
		#endregion

		#region Methods
        public Content() : base()
        {

        }

        public Content(Content obj) : base(obj)
        {
            DateCreated = obj.DateCreated;
            LCID = obj.LCID;
            ContentAliases = obj.ContentAliases;
            AuthorId = obj.AuthorId;
            Author = obj.Author;
            ContentTypeDefinitionId = obj.ContentTypeDefinitionId;
            ContentType = obj.ContentType;
            FolderId = obj.FolderId;
            TaxonomyId = obj.TaxonomyId;
            Title = obj.Title;
            Path = obj.Path;
            Html = obj.Html;
            IsNew = obj.IsNew;
            Taxonomy = obj.Taxonomy;
            Menu = obj.Menu;
            MetaDataFieldValues = obj.MetaDataFieldValues;
            Template = obj.Template;
            IsPublished = obj.IsPublished;
            ApprovalPending = obj.ApprovalPending;
        }

		public virtual bool ShouldSerializeLCID()
		{
			return true;
		}

		public virtual bool ShouldSerializeDateCreated()
		{
			return true;
		}

		public virtual bool ShouldSerializeAuthorId()
		{
			return true;
		}

		public virtual bool ShouldSerializeAuthor()
		{
			return true;
		}

		public virtual bool ShouldSerializeFolderId()
		{
			return true;
		}

		public virtual bool ShouldSerializeTaxonomyId()
		{
			return true;
		}

		public virtual bool ShouldSerializeTitle()
		{
			return true;
		}
		public virtual bool ShouldSerializePath()
		{
			return true;
		}

		public virtual bool ShouldSerializeHtml()
		{
			return true;
		}

		public virtual bool ShouldSerializeContentType()
		{
			return true;
		}

		public virtual bool ShouldSerializeContentAlias()
		{
			return true;
		}

		public virtual bool ShouldSerializeIsNew()
		{
			return true;
		}

		public virtual bool ShouldSerializeTaxonomy()
		{
			return true;
		}

		public virtual bool ShouldSerializeMenu()
		{
			return true;
		}

		public virtual bool ShouldSerializeMetaDataFieldValues()
		{
			return true;
		}

        public override string GetPermissionEntityId()
        {
            return string.Format("{0}-{1}", Id, LCID);
        }

        public static int Compare([AllowNull] Content x, [AllowNull] Content y)
        {
            if (x == null)
            {
                return 1;
            }

            if (y == null)
            {
                return 1;
            }

            return x._dateCreated.CompareTo(y._dateCreated);
        }

        public int CompareTo([AllowNull] Content other)
        {
            if(other == null)
            {
                return 1;
            }

            return _dateCreated.CompareTo(other._dateCreated);
        }
        #endregion
    }
}
