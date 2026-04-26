using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class ContentTypeDefinitionFolderDataBoundSync
    {
        #region Enum
        public enum ContentTypeDefinitionFolderDataBoundSyncType
        {
            NoSync = 0,
            RemoteToOmega = 1,
            OmegaToRemote = 2,
            Bidirectional = 3
        }
        #endregion

        #region Attributes
        private long _folderId;
        private long _contentTypeDefinitionId;
        private DateTime _startTime;
        private DateTime? _endTime;
        private TimeSpan _frequency;
        private bool _enabled;
        private ContentTypeDefinitionFolderDataBoundSyncType _syncType;
        private long? _deltaFieldId;
        #endregion

        #region Properties
        public long FolderId { get => _folderId; set => _folderId = value; }
        public long ContentTypeDefinitionId { get => _contentTypeDefinitionId; set => _contentTypeDefinitionId = value; }
        public DateTime StartTime { get => _startTime; set => _startTime = value; }
        public DateTime? EndTime { get => _endTime; set => _endTime = value; }
        public TimeSpan Frequency { get => _frequency; set => _frequency = value; }
        public bool Enabled { get => _enabled; set => _enabled = value; }
        public ContentTypeDefinitionFolderDataBoundSyncType SyncType { get => _syncType; set => _syncType = value; }
        public long? DeltaFieldId { get => _deltaFieldId; set => _deltaFieldId = value; }
        #endregion
    }
}
