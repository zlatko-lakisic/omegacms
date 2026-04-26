using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Base;
using MD.CMS.BusinessLogic.Core.Helpers.Collections;
using System.Collections.Generic;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner.CustomAttributes;
using System;
using MD.Tools.Helpers.Core.Logging;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{

    public class Folder<T> : BaseHierarchycEntity<Folder<T>, long>, IHierarchyData
        where T : Content
    {
        #region Attributes
        private long _parentId;
        private string _name;
        private string _description;
        private Folder<T> _parent;
        private string _folderPath;
        private bool _inherit;
        private int _childrenTotalCount;
        private int _contentsTotalCount;
        private int _mediaContentTotalCount;

        private List<Folder<T>> _children;
        private List<T> _contents;
        private List<ContentTypeDefinition<ContentTypeDefinitionField>> _contentTypeDefinitions;
        private List<FolderMetaDataField> _metaDataFields;
        private List<MediaContent> _mediacontent;
        private List<FolderMediaContentMetaDataField> _mediaContentmetaDataFields;
        private List<ContentTypeDefinitionFolder> _contentTypeDefinitionFolder;
        private List<ProfileType> _profilePermissions;
        private List<User> _notAuthorizedUsers;
        private long _contentTypeDefinitionsId;
		private List<Template> _templates;
		private List<ContentTypeDefinitionFolderDataBoundCondition> _contentTypeDefinitionFolderDataBoundCondition;
		
		#endregion

		#region Properties

		[OmitPropertyFromReport]
        public List<ProfileType> ProfilePermissions
        {
            get
            {
                if (_profilePermissions == null) _profilePermissions = new List<ProfileType>();
                return _profilePermissions;
            }
            set { _profilePermissions = value; }
        }

        [OmitPropertyFromReport]
        public List<User> NotAuthorizedUsers
        {
            get
            {
                if (_notAuthorizedUsers == null) _notAuthorizedUsers = new List<User>();
                return _notAuthorizedUsers;
            }
            set { _notAuthorizedUsers = value; }
        }

        [OmitPropertyFromReport]
        public List<T> Contents
        {
            get
            {
                if (_contents == null)
                {
                    _contents = new List<T>();
                }
                return _contents;
            }
            set { _contents = value; }
        }

        [OmitPropertyFromReport]
        public List<ContentTypeDefinitionFolder> ContentTypeDefinitionFolder
        {
            get
            {
                if (_contentTypeDefinitionFolder == null)
                {
                    _contentTypeDefinitionFolder = new List<ContentTypeDefinitionFolder>();
                }
                return _contentTypeDefinitionFolder;
            }
            set { _contentTypeDefinitionFolder = value; }
		}

		[OmitPropertyFromReport]
		public List<ContentTypeDefinition<ContentTypeDefinitionField>> ContentTypeDefinitions
		{
			get
			{
				if (_contentTypeDefinitions == null)
				{
					_contentTypeDefinitions = new List<ContentTypeDefinition<ContentTypeDefinitionField>>();
				}
				return _contentTypeDefinitions;
			}
			set { _contentTypeDefinitions = value; }
		}

		[OmitPropertyFromReport]
		public List<ContentTypeDefinitionFolderDataBoundCondition> ContentTypeDefinitionFolderDataBoundCondition { get => _contentTypeDefinitionFolderDataBoundCondition; set => _contentTypeDefinitionFolderDataBoundCondition = value; }


		[OmitPropertyFromReport]
        public List<MediaContent> MediaContent
        {
            get
            {
                if (_mediacontent == null)
                {
                    _mediacontent = new List<MediaContent>();
                }
                return _mediacontent;
            }
            set { _mediacontent = value; }
        }

        [OmitPropertyFromReport]
        public List<FolderMetaDataField> MetaDataFields
        {
            get
            {
                if (_metaDataFields == null)
                {
                    _metaDataFields = new List<FolderMetaDataField>();
                }
                return _metaDataFields;
            }
            set { _metaDataFields = value; }
        }

        [OmitPropertyFromReport]
        public List<FolderMediaContentMetaDataField> FolderMediaContentMetaDataField
        {
            get
            {
                if (_mediaContentmetaDataFields == null)
                {
                    _mediaContentmetaDataFields = new List<FolderMediaContentMetaDataField>();
                }
                return _mediaContentmetaDataFields;
            }
            set { _mediaContentmetaDataFields = value; }
        }

        [OmitPropertyFromReport]
        public List<Template> Templates
        {
            get
            {
                if (_templates == null)
                {
                    _templates = new List<Template>();
                }
                return _templates;
            }
            set { _templates = value; }
        }

        public long ContentTypeDefinitionsId
        {
            get { return _contentTypeDefinitionsId; }
            set { _contentTypeDefinitionsId = value; }
        }

        public long ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }
        public bool Inherit
        {
            get { return _inherit; }
            set { _inherit = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public override Folder<T> Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public string FolderPath
        {
            get { return _folderPath; }
            set { _folderPath = value; }
        }

        [OmitPropertyFromReport]
        public override List<Folder<T>> Children
        {
            get
            {
                if (_children == null)
                {
                    _children = new List<Folder<T>>();
                }
                return _children;
            }
            set { _children = value; }
        }

        [OmitPropertyFromReport]
        public bool IsNew
        {
            get
            {
                return Id.Equals(default(long));
            }
        }

        public int ChildrenTotalCount
        {
            get
            {
                return _childrenTotalCount;
            }
            set
            {
                _childrenTotalCount = value;
            }
        }

        public int ContentsTotalCount
        {
            get
            {
                return _contentsTotalCount;
            }
            set
            {
                _contentsTotalCount = value;
            }
        }

        public int MediaContentTotalCount
        {
            get { return _mediaContentTotalCount; }
            set { _mediaContentTotalCount = value; }
        }

        public bool IsHidden
        {
            get
            {
                try
                {
                    return !string.IsNullOrEmpty(Name) && Name.StartsWith('.');
                } 
                catch(Exception e)
                {
                    typeof(Folder<T>).Log(e);
                }
                return false;
            }
        }

		public override string ToString()
        {
            return Name;
        }
      
        #endregion
    }
}
