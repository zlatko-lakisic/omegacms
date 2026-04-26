/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./mediaContentMetaDataFeldValues.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var mediaContent = (function (_super) {
                __extends(mediaContent, _super);
                function mediaContent() {
                    var _this = _super.call(this) || this;
                    _this.Id = 0;
                    _this.LCID = 0;
                    _this.Size = '';
                    _this.Path = '';
                    _this.FileType = 0;
                    _this.FolderId = 0;
                    _this.Name = '';
                    _this.Description = '';
                    _this.Type = null;
                    _this.InputType = null;
                    _this.MediaContentMetaDataFieldValues = new Array();
                    _this.PreviewUrl = '';
                    _this.FullNameFile = '';
                    _this.Icon = '';
                    _this.DateCreated = new Date();
                    return _this;
                }
                mediaContent.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Id = this.getValue(data, "Id", 0);
                    this.LCID = this.getValue(data, "LCID", 0);
                    this.Size = this.getValue(data, "Size", '');
                    this.Path = this.getValue(data, "Path", '');
                    this.FileType = this.getValue(data, "FileType", 0);
                    this.FolderId = this.getValue(data, "FolderId", 0);
                    this.Name = this.getValue(data, "Name", '');
                    this.Description = this.getValue(data, "Description", '');
                    this.Type = this.getValue(data, "Type", 0);
                    this.InputType = this.getValue(data, "InputType", 0);
                    this.MediaContentMetaDataFieldValues = this.getArrayConstructEntityValue(data, "MediaContentMetaDataFieldValues", new Array(), new entities.mediaContentMetaDataFeldValues());
                    this.PreviewUrl = this.getValue(data, "PreviewUrl", '');
                    this.FullNameFile = this.getValue(data, "FullNameFile", '');
                    this.Icon = this.getValue(data, "Icon", '');
                    this.DateCreated = this.getValue(data, "DateCreated", new Date());
                };
                return mediaContent;
            }(entities.base.BaseEntity));
            entities.mediaContent = mediaContent;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=mediaContent.js.map