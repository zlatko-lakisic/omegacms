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
/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var taxonomyContent = (function (_super) {
                __extends(taxonomyContent, _super);
                function taxonomyContent() {
                    var _this = _super.call(this) || this;
                    _this.Id = 0;
                    _this.LCID = 0;
                    _this.DateCreated = new Date();
                    _this.TaxonomyId = 0;
                    _this.Title = '';
                    _this.Type = '';
                    _this.Path = '';
                    return _this;
                }
                taxonomyContent.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Id = 0;
                    this.LCID = this.getValue(data, 'LCID', 0);
                    this.DateCreated = this.getValue(data, 'DateCreated', null);
                    this.TaxonomyId = this.getValue(data, 'TaxonomyId', 0);
                    this.Title = this.getValue(data, 'Title', '');
                    this.Type = this.getValue(data, 'Type', '');
                    this.Path = this.getValue(data, 'Path', '');
                };
                return taxonomyContent;
            }(entities.base.BaseEntity));
            entities.taxonomyContent = taxonomyContent;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=taxonomyContent.js.map