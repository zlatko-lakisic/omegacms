var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var settings;
    (function (settings) {
        settings.debug = false;
        settings.code = '';
        settings.lcid = 0;
        settings.apiBase = '';
        settings.apiBaseSeparator = '/';
        settings.appBase = '';
        settings.uploadsBase = '';
        settings.apiAllowCrossOrigin = false;
        settings.isAdministration = false;
        settings.packageWebSocketInBody = false;
        settings.authorizationHeader = 'authorization';
    })(settings = mdBusinessLogic.settings || (mdBusinessLogic.settings = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var helpers;
    (function (helpers) {
        var entityHelper = (function () {
            function entityHelper() {
            }
            entityHelper.parseDateAndTimezoneToString = function (date, timezone, delimiter) {
                if (date === void 0) { date = new Date(); }
                if (timezone === void 0) { timezone = moment.tz.guess(); }
                if (delimiter === void 0) { delimiter = ';'; }
                return moment(date).utc().format() + delimiter + timezone;
            };
            entityHelper.parseDateStringValue = function (data, defaultValue, delimiter) {
                if (defaultValue === void 0) { defaultValue = new Date(); }
                if (delimiter === void 0) { delimiter = ';'; }
                var returnValue = moment(defaultValue).utc().format();
                try {
                    if (data !== undefined && data !== undefined) {
                        returnValue = data.split(delimiter)[0];
                    }
                }
                catch (e) {
                    if (mdBusinessLogic.settings.debug) {
                        console.warn(e);
                    }
                }
                return returnValue;
            };
            entityHelper.parseDateValue = function (data, defaultValue, delimiter) {
                if (defaultValue === void 0) { defaultValue = new Date(); }
                if (delimiter === void 0) { delimiter = ';'; }
                var returnValue = defaultValue;
                try {
                    if (data !== undefined && data !== undefined) {
                        returnValue = moment(data.split(delimiter)[0]).tz(this.parseTimeZoneValue(data)).toDate();
                    }
                }
                catch (e) {
                    if (mdBusinessLogic.settings.debug) {
                        console.warn(e);
                    }
                }
                return returnValue;
            };
            entityHelper.parseTimeZoneValue = function (data, defaultValue, delimiter) {
                if (defaultValue === void 0) { defaultValue = moment.tz.guess(); }
                if (delimiter === void 0) { delimiter = ';'; }
                var returnValue = defaultValue;
                try {
                    if (data !== undefined && data !== undefined && data.split(delimiter)[1] !== undefined) {
                        returnValue = data.split(delimiter)[1];
                    }
                }
                catch (e) {
                    if (mdBusinessLogic.settings.debug) {
                        console.warn(e);
                    }
                }
                return returnValue;
            };
            entityHelper.getDateValue = function (data, fieldName, defaultValue, delimiter) {
                if (delimiter === void 0) { delimiter = ';'; }
                return this.parseDateValue(data[fieldName], defaultValue, delimiter);
            };
            entityHelper.getTimeZoneValue = function (data, fieldName, defaultValue, delimiter) {
                if (delimiter === void 0) { delimiter = ';'; }
                return this.parseTimeZoneValue(data[fieldName], defaultValue, delimiter);
            };
            entityHelper.getValue = function (data, fieldName, defaultValue) {
                var returnValue = defaultValue;
                try {
                    if (data !== undefined && data[fieldName] !== undefined) {
                        if (defaultValue instanceof Date) {
                            returnValue = moment(data[fieldName]).toDate();
                        }
                        else {
                            returnValue = data[fieldName];
                        }
                    }
                }
                catch (e) {
                    if (mdBusinessLogic.settings.debug) {
                        console.warn(e);
                    }
                }
                return returnValue;
            };
            entityHelper.getConstructEntityValue = function (data, fieldName, defaultValue, returnNullIfInvalid) {
                if (returnNullIfInvalid === undefined) {
                    returnNullIfInvalid = true;
                }
                var returnValue = defaultValue;
                try {
                    var parsedJson = this.getValue(data, fieldName, null);
                    if (parsedJson != null && returnValue != null) {
                        returnValue.construct(parsedJson);
                        return returnValue.clone();
                    }
                    else if (returnNullIfInvalid) {
                        return null;
                    }
                }
                catch (e) {
                    if (mdBusinessLogic.settings.debug) {
                        console.warn(e);
                    }
                }
                return defaultValue;
            };
            entityHelper.getConstructValue = function (data, fieldName, defaultValue) {
                var returnValue = defaultValue;
                try {
                    var parsedJson = this.getValue(data, fieldName, null);
                    if (parsedJson != null && returnValue != null) {
                        returnValue.construct(parsedJson);
                    }
                }
                catch (e) {
                    if (mdBusinessLogic.settings.debug) {
                        console.warn(e);
                    }
                }
                return returnValue.clone();
            };
            entityHelper.getArrayConstructEntityValue = function (data, fieldName, defaultValue, defaultTypeValue, returnNullIfInvalid) {
                if (returnNullIfInvalid === undefined) {
                    returnNullIfInvalid = true;
                }
                var returnValue = defaultValue;
                try {
                    var parsedJson = this.getValue(data, fieldName, null);
                    if (parsedJson != null && parsedJson instanceof Array) {
                        for (var i = 0; i < parsedJson.length; i++) {
                            returnValue.push(this.getConstructEntityValue(parsedJson, i.toString(), defaultTypeValue, returnNullIfInvalid));
                        }
                    }
                }
                catch (e) {
                    if (mdBusinessLogic.settings.debug) {
                        console.warn(e);
                    }
                }
                return returnValue.filter(function (item) {
                    return item !== undefined && item != null;
                });
            };
            entityHelper.getArrayConstructValue = function (data, fieldName, defaultValue, defaultTypeValue) {
                var returnValue = defaultValue;
                try {
                    var parsedJson = this.getValue(data, fieldName, null);
                    if (parsedJson != null && parsedJson instanceof Array) {
                        for (var i = 0; i < parsedJson.length; i++) {
                            returnValue.push(this.getConstructValue(parsedJson, i.toString(), defaultTypeValue));
                        }
                    }
                }
                catch (e) {
                    if (mdBusinessLogic.settings.debug) {
                        console.warn(e);
                    }
                }
                return returnValue.filter(function (item) {
                    return item !== undefined && item != null;
                });
            };
            return entityHelper;
        }());
        helpers.entityHelper = entityHelper;
    })(helpers = mdBusinessLogic.helpers || (mdBusinessLogic.helpers = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var base;
            (function (base) {
                var BaseEntity = (function () {
                    function BaseEntity(obj) {
                        this.Id = 0;
                        this.IsDeleted = false;
                        if (obj !== undefined && obj != null) {
                            this.Id = obj.Id;
                            this.IsDeleted = obj.IsDeleted;
                        }
                    }
                    BaseEntity.getDateValue = function (data, fieldName, defaultValue) {
                        return mdBusinessLogic.helpers.entityHelper.getDateValue(data, fieldName, defaultValue);
                    };
                    BaseEntity.prototype.getDateValue = function (data, fieldName, defaultValue) {
                        return mdBusinessLogic.helpers.entityHelper.getDateValue(data, fieldName, defaultValue);
                    };
                    BaseEntity.getTimeZoneValue = function (data, fieldName, defaultValue) {
                        return mdBusinessLogic.helpers.entityHelper.getTimeZoneValue(data, fieldName, defaultValue);
                    };
                    BaseEntity.prototype.getTimeZoneValue = function (data, fieldName, defaultValue) {
                        return mdBusinessLogic.helpers.entityHelper.getTimeZoneValue(data, fieldName, defaultValue);
                    };
                    BaseEntity.getValue = function (data, fieldName, defaultValue) {
                        return mdBusinessLogic.helpers.entityHelper.getValue(data, fieldName, defaultValue);
                    };
                    BaseEntity.prototype.getValue = function (data, fieldName, defaultValue) {
                        return mdBusinessLogic.helpers.entityHelper.getValue(data, fieldName, defaultValue);
                    };
                    BaseEntity.getConstructValue = function (data, fieldName, defaultValue) {
                        return mdBusinessLogic.helpers.entityHelper.getConstructValue(data, fieldName, defaultValue);
                    };
                    BaseEntity.prototype.getConstructValue = function (data, fieldName, defaultValue) {
                        return mdBusinessLogic.helpers.entityHelper.getConstructValue(data, fieldName, defaultValue);
                    };
                    BaseEntity.getConstructEntityValue = function (data, fieldName, defaultValue, returnNullIfInvalid) {
                        return mdBusinessLogic.helpers.entityHelper.getConstructEntityValue(data, fieldName, defaultValue, returnNullIfInvalid);
                    };
                    BaseEntity.prototype.getConstructEntityValue = function (data, fieldName, defaultValue, returnNullIfInvalid) {
                        return mdBusinessLogic.helpers.entityHelper.getConstructEntityValue(data, fieldName, defaultValue, returnNullIfInvalid);
                    };
                    BaseEntity.getArrayConstructValue = function (data, fieldName, defaultValue, defaultTypeValue) {
                        return mdBusinessLogic.helpers.entityHelper.getArrayConstructValue(data, fieldName, defaultValue, defaultTypeValue);
                    };
                    BaseEntity.prototype.getArrayConstructValue = function (data, fieldName, defaultValue, defaultTypeValue) {
                        return mdBusinessLogic.helpers.entityHelper.getArrayConstructValue(data, fieldName, defaultValue, defaultTypeValue);
                    };
                    BaseEntity.getArrayConstructEntityValue = function (data, fieldName, defaultValue, defaultTypeValue, returnNullIfInvalid) {
                        return mdBusinessLogic.helpers.entityHelper.getArrayConstructEntityValue(data, fieldName, defaultValue, defaultTypeValue, returnNullIfInvalid);
                    };
                    BaseEntity.prototype.getArrayConstructEntityValue = function (data, fieldName, defaultValue, defaultTypeValue, returnNullIfInvalid) {
                        return mdBusinessLogic.helpers.entityHelper.getArrayConstructEntityValue(data, fieldName, defaultValue, defaultTypeValue, returnNullIfInvalid);
                    };
                    BaseEntity.prototype.construct = function (data) {
                        this.Id = this.getValue(data, 'Id', 0);
                        this.IsDeleted = this.getValue(data, 'IsDeleted', false);
                    };
                    return BaseEntity;
                }());
                base.BaseEntity = BaseEntity;
            })(base = entities.base || (entities.base = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var user = (function (_super) {
                __extends(user, _super);
                function user(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Username = '';
                    _this.ProfileTypes = new Array();
                    _this.ProfileTypeId = 0;
                    _this.Token = '';
                    _this.DateRefreshToken = new Date();
                    _this.RWDPermissions = new Array();
                    _this.AdministrationAllowed = false;
                    _this.IsRoot = false;
                    _this.AuthenticationProvider = '';
                    _this.ReferenceId = '';
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                user.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Username = this.getValue(data, 'Username', '');
                    this.ProfileTypes = this.getArrayConstructEntityValue(data, 'ProfileTypes', new Array(), new entities.profileType());
                    this.ProfileTypeId = this.getValue(data, 'ProfileTypeId', 0);
                    this.Token = this.getValue(data, 'Token', '');
                    this.DateRefreshToken = this.getValue(data, 'DateRefresh', new Date());
                    this.RWDPermissions = this.getValue(data, 'RWDPermissions', new Array());
                    this.AdministrationAllowed = this.getValue(data, 'AdministrationAllowed', false);
                    this.IsRoot = this.getValue(data, 'IsRoot', false);
                    this.AuthenticationProvider = this.getValue(data, 'AuthenticationProvider', '');
                    this.ReferenceId = this.getValue(data, 'ReferenceId', '');
                };
                user.prototype.clone = function () {
                    return new user(this);
                };
                user.prototype.getProfileType = function (query) {
                    if (isNaN(query)) {
                        return this.ProfileTypes.filter(function (profile) { return profile.Name == query; })[0];
                    }
                    return this.ProfileTypes.filter(function (profile) { return profile.Id == query; })[0];
                };
                return user;
            }(entities.base.BaseEntity));
            entities.user = user;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var loggedOnUser = (function (_super) {
                __extends(loggedOnUser, _super);
                function loggedOnUser(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.SessionId = '';
                    _this.SessionId = '';
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                loggedOnUser.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.SessionId = this.getValue(data, 'SessionId', '');
                };
                loggedOnUser.prototype.clone = function () {
                    return new loggedOnUser(this);
                };
                loggedOnUser.prototype.toString = function () {
                    return JSON.stringify({
                        SessionId: this.SessionId,
                        Id: this.Id,
                        Username: this.Username,
                        Token: this.Token,
                        DateRefreshToken: this.DateRefreshToken
                    });
                };
                return loggedOnUser;
            }(entities.user));
            entities.loggedOnUser = loggedOnUser;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var helpers;
    (function (helpers) {
        var encoder;
        (function (encoder) {
            var base64;
            (function (base64) {
                base64.encode = function (input) {
                    if (input === undefined || input == null) {
                        return input;
                    }
                    return window.Base64.encode(input);
                };
                base64.decode = function (input) {
                    if (input === undefined || input == null) {
                        return input;
                    }
                    return window.Base64.decode(input);
                };
            })(base64 = encoder.base64 || (encoder.base64 = {}));
        })(encoder = helpers.encoder || (helpers.encoder = {}));
    })(helpers = mdBusinessLogic.helpers || (mdBusinessLogic.helpers = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var helpers;
    (function (helpers) {
        helpers.oopHelper = function (child, parent) {
            child.prototype = Object.create(parent.prototype);
        };
        function loadParentArray(obj, parentName, parentLinkName, parentArray) {
            if (parentName === undefined) {
                parentName = 'Name';
            }
            if (parentArray === undefined) {
                parentArray = new Array();
            }
            if (obj !== undefined && obj !== null) {
                if (obj[parentName] !== undefined && obj[parentName] != null) {
                    if (parentLinkName !== undefined && obj[parentLinkName] !== undefined && obj[parentLinkName] != null) {
                        var objInArray = {};
                        objInArray[parentName] = obj[parentName];
                        objInArray[parentLinkName] = obj[parentLinkName];
                        parentArray.unshift(objInArray);
                    }
                    else {
                        parentArray.unshift(obj[parentName]);
                    }
                }
                if (obj.Parent !== undefined && obj.Parent != null) {
                    loadParentArray(obj.Parent, parentName, parentLinkName, parentArray);
                }
            }
            return parentArray;
        }
        helpers.loadParentArray = loadParentArray;
    })(helpers = mdBusinessLogic.helpers || (mdBusinessLogic.helpers = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            entities.isFunction = function (functionToCheck) {
                var getType = {};
                return functionToCheck && getType.toString.call(functionToCheck) === '[object Function]';
            };
            entities.isArray = function (obj) {
                return (!!obj) && (obj.constructor === Array);
            };
            entities.isObject = function (obj) {
                return (!!obj) && (obj.constructor === Object);
            };
            var entitiesEnum;
            (function (entitiesEnum) {
                entitiesEnum[entitiesEnum["Content"] = 1] = "Content";
                entitiesEnum[entitiesEnum["AttributeTypeDefinition"] = 2] = "AttributeTypeDefinition";
                entitiesEnum[entitiesEnum["ContentTypeDefinition"] = 3] = "ContentTypeDefinition";
                entitiesEnum[entitiesEnum["ContentTypeDefinitionField"] = 4] = "ContentTypeDefinitionField";
                entitiesEnum[entitiesEnum["ContentTypeDefinitionFieldValue"] = 5] = "ContentTypeDefinitionFieldValue";
                entitiesEnum[entitiesEnum["ContentTypeDefinitionFolder"] = 6] = "ContentTypeDefinitionFolder";
                entitiesEnum[entitiesEnum["Folder"] = 7] = "Folder";
                entitiesEnum[entitiesEnum["FolderMediaContentMetaDataField"] = 8] = "FolderMediaContentMetaDataField";
                entitiesEnum[entitiesEnum["FolderMetaDataField"] = 9] = "FolderMetaDataField";
                entitiesEnum[entitiesEnum["MediaContentMetaDataFieldValues"] = 10] = "MediaContentMetaDataFieldValues";
                entitiesEnum[entitiesEnum["MediaContent"] = 11] = "MediaContent";
                entitiesEnum[entitiesEnum["LCID"] = 12] = "LCID";
                entitiesEnum[entitiesEnum["Culture"] = 13] = "Culture";
                entitiesEnum[entitiesEnum["MenuContent"] = 14] = "MenuContent";
                entitiesEnum[entitiesEnum["ContentAlias"] = 15] = "ContentAlias";
                entitiesEnum[entitiesEnum["Menu"] = 16] = "Menu";
                entitiesEnum[entitiesEnum["MetaDataField"] = 17] = "MetaDataField";
                entitiesEnum[entitiesEnum["MetaDataFieldValue"] = 18] = "MetaDataFieldValue";
                entitiesEnum[entitiesEnum["Permissions"] = 19] = "Permissions";
                entitiesEnum[entitiesEnum["Profile"] = 20] = "Profile";
                entitiesEnum[entitiesEnum["ProfileType"] = 21] = "ProfileType";
                entitiesEnum[entitiesEnum["ProfileTypeField"] = 22] = "ProfileTypeField";
                entitiesEnum[entitiesEnum["ProfileTypeFieldValue"] = 23] = "ProfileTypeFieldValue";
                entitiesEnum[entitiesEnum["Session"] = 24] = "Session";
                entitiesEnum[entitiesEnum["TaxonomyContent"] = 25] = "TaxonomyContent";
                entitiesEnum[entitiesEnum["Taxonomy"] = 26] = "Taxonomy";
                entitiesEnum[entitiesEnum["Template"] = 27] = "Template";
                entitiesEnum[entitiesEnum["User"] = 28] = "User";
                entitiesEnum[entitiesEnum["RWDPermission"] = 29] = "RWDPermission";
                entitiesEnum[entitiesEnum["Report"] = 30] = "Report";
                entitiesEnum[entitiesEnum["ReportDefinition"] = 31] = "ReportDefinition";
                entitiesEnum[entitiesEnum["ReportData"] = 32] = "ReportData";
                entitiesEnum[entitiesEnum["ReportScheduler"] = 33] = "ReportScheduler";
                entitiesEnum[entitiesEnum["ReportSchedulerAction"] = 34] = "ReportSchedulerAction";
                entitiesEnum[entitiesEnum["ApprovalChain"] = 35] = "ApprovalChain";
                entitiesEnum[entitiesEnum["Step"] = 36] = "Step";
                entitiesEnum[entitiesEnum["StepAction"] = 37] = "StepAction";
                entitiesEnum[entitiesEnum["StepUser"] = 38] = "StepUser";
                entitiesEnum[entitiesEnum["MessageFolder"] = 39] = "MessageFolder";
                entitiesEnum[entitiesEnum["Message"] = 40] = "Message";
                entitiesEnum[entitiesEnum["ApprovalChainApproval"] = 41] = "ApprovalChainApproval";
                entitiesEnum[entitiesEnum["ContentTypeDefinitionDataSource"] = 42] = "ContentTypeDefinitionDataSource";
                entitiesEnum[entitiesEnum["ContentTypeDefinitionDataSourceJoin"] = 44] = "ContentTypeDefinitionDataSourceJoin";
                entitiesEnum[entitiesEnum["ContentTypeDefinitionFolderDataBoundCondition"] = 45] = "ContentTypeDefinitionFolderDataBoundCondition";
                entitiesEnum[entitiesEnum["ContentTypeDefinitionFolderDataBoundSync"] = 46] = "ContentTypeDefinitionFolderDataBoundSync";
            })(entitiesEnum = entities.entitiesEnum || (entities.entitiesEnum = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var permissions;
            (function (permissions) {
                var permissionAccessTypeEnum;
                (function (permissionAccessTypeEnum) {
                    permissionAccessTypeEnum[permissionAccessTypeEnum["Read"] = 1] = "Read";
                    permissionAccessTypeEnum[permissionAccessTypeEnum["Write"] = 2] = "Write";
                    permissionAccessTypeEnum[permissionAccessTypeEnum["Delete"] = 3] = "Delete";
                })(permissionAccessTypeEnum = permissions.permissionAccessTypeEnum || (permissions.permissionAccessTypeEnum = {}));
            })(permissions = entities.permissions || (entities.permissions = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var permissions;
            (function (permissions) {
                var entityPermission = (function () {
                    function entityPermission() {
                        this.AccessTypes = new Array();
                    }
                    return entityPermission;
                }());
                permissions.entityPermission = entityPermission;
            })(permissions = entities.permissions || (entities.permissions = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var permissions;
            (function (permissions) {
                var objectPermission = (function () {
                    function objectPermission() {
                        this.AccessTypes = new Array();
                    }
                    return objectPermission;
                }());
                permissions.objectPermission = objectPermission;
            })(permissions = entities.permissions || (entities.permissions = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var permissions;
            (function (permissions) {
                var permissionsBase = (function (_super) {
                    __extends(permissionsBase, _super);
                    function permissionsBase(obj) {
                        var _this = _super.call(this) || this;
                        _this.EntityPermissions = new Array();
                        _this.ObjectPermissions = new Array();
                        if (obj !== undefined && obj != null) {
                            _this.EntityPermissions = obj.EntityPermissions;
                            _this.ObjectPermissions = obj.ObjectPermissions;
                        }
                        return _this;
                    }
                    return permissionsBase;
                }(entities.base.BaseEntity));
                permissions.permissionsBase = permissionsBase;
            })(permissions = entities.permissions || (entities.permissions = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var permissions;
            (function (permissions) {
                var profileTypePermissions = (function (_super) {
                    __extends(profileTypePermissions, _super);
                    function profileTypePermissions(obj) {
                        var _this = _super.call(this, obj) || this;
                        _this.ProfileId = 0;
                        if (obj !== undefined && obj != null) {
                            _this.ProfileId = obj.ProfileId;
                        }
                        return _this;
                    }
                    profileTypePermissions.prototype.construct = function (data) {
                        this.ProfileId = data.ProfileId;
                        this.EntityPermissions = data.EntityPermissions;
                        this.ObjectPermissions = data.ObjectPermissions;
                    };
                    profileTypePermissions.prototype.clone = function () {
                        return new profileTypePermissions(this);
                    };
                    return profileTypePermissions;
                }(permissions.permissionsBase));
                permissions.profileTypePermissions = profileTypePermissions;
            })(permissions = entities.permissions || (entities.permissions = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var permissions;
            (function (permissions) {
                var userPermissions = (function (_super) {
                    __extends(userPermissions, _super);
                    function userPermissions(obj) {
                        var _this = _super.call(this, obj) || this;
                        _this.UserId = 0;
                        if (obj !== undefined && obj != null) {
                            _this.UserId = obj.UserId;
                        }
                        return _this;
                    }
                    userPermissions.prototype.construct = function (data) {
                        this.UserId = data.UserId;
                        this.EntityPermissions = data.EntityPermissions;
                        this.ObjectPermissions = data.ObjectPermissions;
                    };
                    userPermissions.prototype.clone = function () {
                        return new userPermissions(this);
                    };
                    return userPermissions;
                }(permissions.permissionsBase));
                permissions.userPermissions = userPermissions;
            })(permissions = entities.permissions || (entities.permissions = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var globals;
    (function (globals) {
        globals.loggedOnUser = null;
        globals.selectedLanguage = '';
        globals.systemName = '';
        globals.systemVersion = '';
        globals.numberAwsSocketRetries = 5;
        globals.enabledAuthenticationProviders = new Array();
        globals.loggedOnProfileTypePermissions = new Array();
        globals.loggedOnUserPermissions = new Array();
    })(globals = mdBusinessLogic.globals || (mdBusinessLogic.globals = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var base;
            (function (base) {
                var BaseController_helpers = (function () {
                    function BaseController_helpers() {
                    }
                    BaseController_helpers.prototype.loadParentNamesAsArray = function (nameArray, obj, parentName, parentLinkName) {
                        if (parentName === undefined) {
                            parentName = 'Name';
                        }
                        if (obj[parentName] !== undefined && obj[parentName] != null) {
                            if (parentLinkName !== undefined && obj[parentLinkName] !== undefined && obj[parentLinkName] != null) {
                                var customObj = new Object();
                                customObj[parentName] = obj[parentName];
                                customObj[parentLinkName] = obj[parentLinkName];
                                nameArray.push(customObj);
                            }
                            else {
                                nameArray.push(obj[parentName]);
                            }
                        }
                        if (obj.Parent !== undefined && obj.Parent != null) {
                            this.loadParentNamesAsArray(nameArray, obj.Parent, parentName, parentLinkName);
                        }
                    };
                    BaseController_helpers.prototype.parseUrl = function (url) {
                        var l = document.createElement("a");
                        l.href = url;
                        return l;
                    };
                    BaseController_helpers.prototype.getAddress = function (endpoint, data) {
                        var address = endpoint;
                        if (data !== undefined) {
                            if (data instanceof Array) {
                                if (address[address.length - 1] != '/') {
                                    address += '/';
                                }
                                for (var i = 0; i < data.length; i++) {
                                    if (data[i] !== undefined && data[i] !== null) {
                                        address += data[i].toString();
                                    }
                                    if (i < data.length - 1) {
                                        address += '/';
                                    }
                                }
                            }
                            else {
                                if (address[address.length - 1] != '?') {
                                    address += '?';
                                }
                                var counter = 0;
                                for (var key in data) {
                                    if (counter > 0) {
                                        address += '&';
                                    }
                                    if (data[key] !== undefined && data[key] !== null) {
                                        address += encodeURIComponent(key) + '=' + encodeURIComponent((data[key] instanceof Array) ? data[key].join(',') : data[key]);
                                        counter++;
                                    }
                                }
                            }
                        }
                        return address;
                    };
                    return BaseController_helpers;
                }());
                base.BaseController_helpers = BaseController_helpers;
            })(base = controllers.base || (controllers.base = {}));
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var helpers;
    (function (helpers) {
        var mdException = (function () {
            function mdException(message, errorData, innerException, stackTrace) {
                this.settings = mdBusinessLogic.settings;
                this.errorData = errorData;
                this.innerException = innerException;
                this.stackTrace = stackTrace !== undefined ? stackTrace : (new Error()).stack;
                this.message = message;
                if (this.settings.debug) {
                    console.log('Error occurred: ' + this.message + (this.errorData !== undefined && this.errorData != null ? ' data(' + JSON.stringify(this.errorData) + ')' + (this.stackTrace !== undefined ? ', stacktrace(' + this.stackTrace + ')' : '') : ''));
                }
            }
            return mdException;
        }());
        helpers.mdException = mdException;
    })(helpers = mdBusinessLogic.helpers || (mdBusinessLogic.helpers = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var base;
            (function (base) {
                var AjaxMethodData = (function () {
                    function AjaxMethodData(requestId) {
                        this.responseData = null;
                        this.requestData = null;
                        this.controller = null;
                        this.exception = null;
                        this.responseDataArray = null;
                        this.requestId = requestId !== undefined && requestId != null && requestId.trim().length > 0 ? requestId : mdBusinessLogic.helpers.Guid.create().toString();
                        this.requestIdAutoGenerated = !(requestId !== undefined && requestId != null && requestId.trim().length > 0);
                    }
                    AjaxMethodData.prototype.getRequestId = function () {
                        return this.requestId;
                    };
                    AjaxMethodData.prototype.getRequestIdAutoGenerated = function () {
                        return this.requestIdAutoGenerated;
                    };
                    return AjaxMethodData;
                }());
                base.AjaxMethodData = AjaxMethodData;
                var AjaxMethodDataSocket = (function (_super) {
                    __extends(AjaxMethodDataSocket, _super);
                    function AjaxMethodDataSocket(requestId) {
                        var _this = _super.call(this, requestId) || this;
                        _this.socket = null;
                        return _this;
                    }
                    return AjaxMethodDataSocket;
                }(AjaxMethodData));
                base.AjaxMethodDataSocket = AjaxMethodDataSocket;
                var AjaxMethodOptions = (function (_super) {
                    __extends(AjaxMethodOptions, _super);
                    function AjaxMethodOptions(requestId) {
                        var _this = _super.call(this, requestId) || this;
                        _this.onSuccess = _this._onsuccess;
                        _this.onClose = _this._onClose;
                        _this.onError = _this._onerror;
                        _this.includeAuthHeader = false;
                        _this.isJsonArray = false;
                        _this.isAdministration = mdBusinessLogic.settings.isAdministration;
                        _this.isFormData = false;
                        _this.contentType = null;
                        _this.showLoading = true;
                        _this.address = '';
                        _this.method = null;
                        _this.headers = new Array();
                        _this.lcid = mdBusinessLogic.settings.lcid;
                        _this.clearCache = false;
                        _this.isInitCall = false;
                        return _this;
                    }
                    AjaxMethodOptions.prototype.getFullUrl = function (prefix) {
                        if (prefix === void 0) { prefix = ''; }
                        return mdBusinessLogic.settings.apiBase + prefix + this.getAddressWithCacheFlag(this.address, this.clearCache);
                    };
                    AjaxMethodOptions.prototype.getAddressWithCacheFlag = function (address, clearCache) {
                        if (clearCache) {
                            if (address[address.length - 1] != '/') {
                                address += '/';
                            }
                            if (address.indexOf('?') >= 0) {
                                address += '?';
                            }
                            else {
                                address += '&';
                            }
                            address += encodeURIComponent('_') + '=' + encodeURIComponent(new Date().getTime().toString());
                        }
                        return address;
                    };
                    AjaxMethodOptions.prototype.getPartialUrl = function (prefix) {
                        if (prefix === void 0) { prefix = ''; }
                        var apiBase = mdBusinessLogic.settings.apiBase;
                        if (apiBase[apiBase.length - 1] != mdBusinessLogic.settings.apiBaseSeparator) {
                            apiBase = apiBase.substr(0, apiBase.length - 1) + mdBusinessLogic.settings.apiBaseSeparator;
                        }
                        return apiBase + prefix;
                    };
                    AjaxMethodOptions.prototype.getMethodTypeString = function () {
                        switch (this.method) {
                            case AjaxMethodType.GET:
                                return 'GET';
                            case AjaxMethodType.POST:
                                return 'POST';
                            case AjaxMethodType.PUT:
                                return 'PUT';
                            case AjaxMethodType.DELETE:
                                return 'DELETE';
                            case AjaxMethodType.SOCKET:
                                return 'SOCKET';
                        }
                    };
                    AjaxMethodOptions.prototype._onsuccess = function (data) {
                    };
                    AjaxMethodOptions.prototype._onClose = function (data) {
                    };
                    AjaxMethodOptions.prototype._onerror = function (data) {
                    };
                    AjaxMethodOptions.prototype.getRequestId = function () {
                        if (_super.prototype.getRequestIdAutoGenerated.call(this) && this.method != AjaxMethodType.SOCKET) {
                            this.requestId = '';
                            return mdBusinessLogic.helpers.crypto.md5(JSON.stringify(this));
                        }
                        return _super.prototype.getRequestId.call(this);
                    };
                    return AjaxMethodOptions;
                }(AjaxMethodData));
                base.AjaxMethodOptions = AjaxMethodOptions;
                var AjaxMethodType;
                (function (AjaxMethodType) {
                    AjaxMethodType[AjaxMethodType["GET"] = 1] = "GET";
                    AjaxMethodType[AjaxMethodType["POST"] = 2] = "POST";
                    AjaxMethodType[AjaxMethodType["PUT"] = 3] = "PUT";
                    AjaxMethodType[AjaxMethodType["DELETE"] = 4] = "DELETE";
                    AjaxMethodType[AjaxMethodType["SOCKET"] = 5] = "SOCKET";
                })(AjaxMethodType = base.AjaxMethodType || (base.AjaxMethodType = {}));
                var AjaxMethodHeader = (function () {
                    function AjaxMethodHeader(name, value) {
                        this.name = '';
                        this.value = '';
                        this.name = name;
                        this.value = value;
                    }
                    return AjaxMethodHeader;
                }());
                base.AjaxMethodHeader = AjaxMethodHeader;
            })(base = controllers.base || (controllers.base = {}));
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var exceptions;
            (function (exceptions) {
                var netException = (function () {
                    function netException(obj) {
                        this.StackTrace = '';
                        this.Source = '';
                        this.Message = null;
                        this.InnerException = null;
                        this.HResult = 0;
                        this.Data = null;
                        if (obj !== undefined && obj != null) {
                            this.StackTrace = obj.StackTrace;
                            this.Source = obj.Source;
                            this.Message = obj.Message;
                            this.InnerException = obj.InnerException;
                            this.HResult = obj.HResult;
                            this.Data = obj.Data;
                        }
                    }
                    netException.prototype.construct = function (data) {
                        this.StackTrace = mdBusinessLogic.helpers.entityHelper.getValue(data, "StackTrace", '');
                        this.Source = mdBusinessLogic.helpers.entityHelper.getValue(data, "Source", '');
                        this.Message = mdBusinessLogic.helpers.entityHelper.getValue(data, "Message", '');
                        this.InnerException = mdBusinessLogic.helpers.entityHelper.getConstructValue(data, "InnerException", null);
                        this.HResult = mdBusinessLogic.helpers.entityHelper.getValue(data, "HResult", 0);
                        this.Data = mdBusinessLogic.helpers.entityHelper.getValue(data, "Data", null);
                    };
                    netException.prototype.clone = function () {
                        return new netException(this);
                    };
                    return netException;
                }());
                exceptions.netException = netException;
            })(exceptions = entities.exceptions || (entities.exceptions = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var exceptions;
            (function (exceptions) {
                var errorDetails = (function () {
                    function errorDetails(obj) {
                        this.StatusCode = 0;
                        this.Message = '';
                        this.InnerException = null;
                        if (obj !== undefined && obj != null) {
                            this.StatusCode = obj.StatusCode;
                            this.Message = obj.Message;
                            this.InnerException = obj.InnerException;
                        }
                    }
                    errorDetails.prototype.construct = function (data) {
                        this.StatusCode = mdBusinessLogic.helpers.entityHelper.getValue(data, "StatusCode", 0);
                        this.Message = mdBusinessLogic.helpers.entityHelper.getValue(data, "Message", '');
                        this.InnerException = mdBusinessLogic.helpers.entityHelper.getConstructValue(data, "InnerException", null);
                    };
                    errorDetails.prototype.clone = function () {
                        return new errorDetails(this);
                    };
                    return errorDetails;
                }());
                exceptions.errorDetails = errorDetails;
            })(exceptions = entities.exceptions || (entities.exceptions = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var comm;
            (function (comm) {
                var socketModel = (function () {
                    function socketModel(obj) {
                        this.message = '';
                        this.connectionId = '';
                        if (obj !== undefined && obj != null) {
                            this.construct(obj);
                        }
                    }
                    socketModel.prototype.construct = function (data) {
                        this.message = mdBusinessLogic.helpers.entityHelper.getValue(data, "message", null);
                        this.connectionId = mdBusinessLogic.helpers.entityHelper.getValue(data, "connectionId", null);
                    };
                    socketModel.prototype.clone = function () {
                        return new socketModel(this);
                    };
                    return socketModel;
                }());
                comm.socketModel = socketModel;
            })(comm = entities.comm || (entities.comm = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var comm;
            (function (comm) {
                var awsSocketModel = (function (_super) {
                    __extends(awsSocketModel, _super);
                    function awsSocketModel(obj) {
                        var _this = _super.call(this, obj) || this;
                        _this.requestId = '';
                        if (obj !== undefined && obj != null) {
                            _this.construct(obj);
                        }
                        return _this;
                    }
                    awsSocketModel.prototype.construct = function (data) {
                        this.requestId = mdBusinessLogic.helpers.entityHelper.getValue(data, "requestId", null);
                    };
                    awsSocketModel.prototype.clone = function () {
                        return new awsSocketModel(this);
                    };
                    return awsSocketModel;
                }(comm.socketModel));
                comm.awsSocketModel = awsSocketModel;
                var executionScheduleType;
                (function (executionScheduleType) {
                    executionScheduleType[executionScheduleType["Manual"] = 0] = "Manual";
                    executionScheduleType[executionScheduleType["Recurring"] = 1] = "Recurring";
                })(executionScheduleType = comm.executionScheduleType || (comm.executionScheduleType = {}));
            })(comm = entities.comm || (entities.comm = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var base;
            (function (base) {
                var BaseController = (function (_super) {
                    __extends(BaseController, _super);
                    function BaseController(controllerBase) {
                        var _this = _super.call(this) || this;
                        _this.controllerBase = controllerBase;
                        return _this;
                    }
                    BaseController.prototype.getAddress = function (endpoint, data, includeBase) {
                        if (includeBase === undefined) {
                            includeBase = true;
                        }
                        if (includeBase && this.controllerBase === undefined || this.controllerBase == '') {
                            throw new mdBusinessLogic.helpers.mdException('The controllerBase property is missing!');
                        }
                        if (endpoint === undefined) {
                            throw new mdBusinessLogic.helpers.mdException('The endpoint argument is missing!');
                        }
                        var address = includeBase ? this.controllerBase + endpoint : endpoint;
                        return _super.prototype.getAddress.call(this, address, data);
                    };
                    BaseController.prototype.generateNonSecureRequestSocket = function (options, requestId) {
                        var socket;
                        try {
                            var numberOfTries_1 = 0;
                            var response_1 = new base.AjaxMethodDataSocket();
                            response_1.controller = options.controller;
                            var parsedUrl = mdBusinessLogic.settings.packageWebSocketInBody ? this.parseUrl(options.getPartialUrl('web-sockets/')) : this.parseUrl(options.getFullUrl('web-sockets/'));
                            var url = parsedUrl.href.replace(parsedUrl.protocol, 'https:' == document.location.protocol ? 'wss:' : 'ws:');
                            if (!mdBusinessLogic.settings.packageWebSocketInBody) {
                                var headers = options.headers;
                                headers.push(new base.AjaxMethodHeader("connectionId", requestId));
                                if (url.indexOf('?') >= 0) {
                                    url += '&' + headers.map(function (header) {
                                        return header.name + '=' + header.value;
                                    }).join('&');
                                }
                                else {
                                    url += '?' + headers.map(function (header) {
                                        return header.name + '=' + header.value;
                                    }).join('&');
                                }
                            }
                            socket = mdBusinessLogic.settings.ajax.connections.getSocket(requestId);
                            if (socket === undefined || socket == null) {
                                socket = new WebSocket(url);
                            }
                            socket.onmessage = function (data) {
                                var shouldRetrySocket = false;
                                try {
                                    var awsSocketResponse = new dataAccess.entities.comm.awsSocketModel(JSON.parse(data.data));
                                    if (awsSocketResponse && awsSocketResponse.message == 'Endpoint request timed out' && numberOfTries_1 < mdBusinessLogic.globals.numberAwsSocketRetries) {
                                        shouldRetrySocket = true;
                                        numberOfTries_1++;
                                    }
                                }
                                catch (e) {
                                    shouldRetrySocket = false;
                                }
                                if (shouldRetrySocket) {
                                    sendCallback_1();
                                }
                                else {
                                    var socketModelResponse = new dataAccess.entities.comm.socketModel(JSON.parse(data.data));
                                    if (options.isJsonArray) {
                                        var jsonData = JSON.parse(socketModelResponse.message);
                                        response_1.responseData = options.responseData.clone();
                                        response_1.responseDataArray = new Array();
                                        for (var i = 0; i < jsonData.length; i++) {
                                            response_1.responseData.construct(jsonData[i]);
                                            response_1.responseDataArray.push(response_1.responseData);
                                            response_1.responseData = options.responseData.clone();
                                        }
                                    }
                                    else {
                                        response_1.responseData = options.responseData;
                                        response_1.responseData.construct(JSON.parse(socketModelResponse.message));
                                    }
                                    response_1.socket = socket;
                                    options.onSuccess(response_1);
                                    numberOfTries_1 = 0;
                                }
                            };
                            socket.onclose = function (data) {
                                response_1.socket = socket;
                                options.onClose(response_1);
                                mdBusinessLogic.settings.ajax.connections.removeSocket(requestId);
                            };
                            socket.onerror = function (data) {
                                response_1.socket = socket;
                                response_1.exception = new mdBusinessLogic.helpers.mdException('The web socket has closed!', data, new Error());
                                options.onError(response_1);
                                mdBusinessLogic.settings.ajax.connections.removeSocket(requestId);
                            };
                            mdBusinessLogic.settings.ajax.connections.addSocket({ id: requestId, obj: socket });
                            var sendCallback_1 = function () {
                                setTimeout(function () {
                                    var socketModelData = new dataAccess.entities.comm.socketModel();
                                    socketModelData.connectionId = requestId;
                                    socketModelData.message = (typeof options.requestData == 'string' || options.requestData instanceof String) ? options.requestData.toString() : JSON.stringify(options.requestData);
                                    if (socket.readyState == WebSocket.OPEN) {
                                        if (mdBusinessLogic.settings.packageWebSocketInBody) {
                                            var queryStrings = {};
                                            for (var i = 0; i < options.headers.length; i++) {
                                                queryStrings[options.headers[i].name] = options.headers[i].value;
                                            }
                                            socket.send(JSON.stringify({
                                                action: 'sendmessage',
                                                data: {
                                                    address: options.address,
                                                    queryStrings: queryStrings,
                                                    data: options.requestData
                                                }
                                            }));
                                        }
                                        else {
                                            socket.send(JSON.stringify(socketModelData));
                                        }
                                    }
                                    else {
                                        sendCallback_1();
                                    }
                                }, 100);
                            };
                            sendCallback_1();
                        }
                        catch (e) {
                            mdBusinessLogic.settings.ajax.connections.removeSocket(requestId);
                        }
                    };
                    BaseController.prototype.generateNonSecureRequestXhr = function (options, requestId) {
                        var xhrExists = true;
                        var xhr = mdBusinessLogic.settings.ajax.connections.getRequestObject(requestId);
                        if (xhr == null) {
                            xhr = {
                                id: requestId,
                                obj: new XMLHttpRequest(),
                                successEvents: [options.onSuccess],
                                errorEvents: [options.onError]
                            };
                            xhrExists = false;
                        }
                        if (xhrExists) {
                            if (xhr.successEvents === undefined) {
                                xhr.successEvents = [];
                            }
                            xhr.successEvents.push(options.onSuccess);
                            if (xhr.errorEvents === undefined) {
                                xhr.errorEvents = [];
                            }
                            xhr.errorEvents.push(options.onError);
                        }
                        else {
                            var response_2 = new base.AjaxMethodData();
                            response_2.controller = options.controller;
                            xhr.obj.open(options.getMethodTypeString(), options.getFullUrl(), true);
                            if (!options.isFormData) {
                                if (options.contentType === undefined || options.contentType == null) {
                                    options.contentType = new base.AjaxMethodHeader('Content-Type', 'application/json; charset=UTF-8');
                                }
                                options.headers.push(options.contentType);
                            }
                            for (var i = 0; i < options.headers.length; i++) {
                                xhr.obj.setRequestHeader(options.headers[i].name, options.headers[i].value);
                            }
                            xhr.obj.addEventListener('load', function (event) {
                                switch (this.status) {
                                    case 401:
                                        var returnedExceptionUnauthorized = new dataAccess.entities.exceptions.errorDetails(JSON.parse(this.responseText));
                                        var errorUnauthorized = new mdBusinessLogic.helpers.mdException(returnedExceptionUnauthorized.Message, event, returnedExceptionUnauthorized);
                                        mdBusinessLogic.settings.admin.onEvent(mdBusinessLogic.settings.adminEventTypes.ajaxOnUnauthorized, this, event, errorUnauthorized);
                                        break;
                                    case 403:
                                        var returnedExceptionForbidden = new dataAccess.entities.exceptions.errorDetails(JSON.parse(this.responseText));
                                        var errorForbidden = new mdBusinessLogic.helpers.mdException(returnedExceptionForbidden.Message, event, returnedExceptionForbidden);
                                        mdBusinessLogic.settings.admin.onEvent(mdBusinessLogic.settings.adminEventTypes.ajaxOnForbidden, this, event, errorForbidden);
                                        break;
                                    case 404:
                                        var returnedExceptionNotFound = new dataAccess.entities.exceptions.errorDetails(JSON.parse(this.responseText));
                                        response_2.exception = new mdBusinessLogic.helpers.mdException(returnedExceptionNotFound.Message, event, returnedExceptionNotFound);
                                        break;
                                    case 200:
                                        try {
                                            if (this.responseText != undefined && this.responseText.length > 0) {
                                                if (options.isJsonArray) {
                                                    var jsonData = JSON.parse(this.responseText);
                                                    response_2.responseDataArray = new Array();
                                                    for (var i_1 = 0; i_1 < jsonData.length; i_1++) {
                                                        var responseObj = options.responseData.clone();
                                                        responseObj.construct(jsonData[i_1]);
                                                        if (responseObj instanceof dataAccess.entities.primitiveType) {
                                                            response_2.responseDataArray.push(responseObj.Value);
                                                        }
                                                        else {
                                                            response_2.responseDataArray.push(_.cloneDeep(responseObj));
                                                        }
                                                    }
                                                }
                                                else {
                                                    response_2.responseData = options.responseData;
                                                    if (options.responseData instanceof dataAccess.entities.primitiveType) {
                                                        response_2.responseData.construct(this.responseText);
                                                    }
                                                    else {
                                                        response_2.responseData.construct(JSON.parse(this.responseText));
                                                    }
                                                }
                                            }
                                        }
                                        catch (exception) {
                                            response_2.exception = new mdBusinessLogic.helpers.mdException(exception.message, event, exception);
                                            throw response_2.exception;
                                        }
                                        if (xhr.successEvents !== undefined) {
                                            for (var i = 0; i < xhr.successEvents.length; i++) {
                                                xhr.successEvents[i](response_2);
                                            }
                                        }
                                        break;
                                    default:
                                        var returnExceptionOther = new dataAccess.entities.exceptions.errorDetails(JSON.parse(this.responseText));
                                        response_2.exception = new mdBusinessLogic.helpers.mdException(returnExceptionOther.Message, event, returnExceptionOther);
                                }
                                mdBusinessLogic.settings.ajax.connections.removeRequest(requestId);
                            });
                            xhr.obj.addEventListener('loadend', function (event) {
                                mdBusinessLogic.settings.admin.onEvent(mdBusinessLogic.settings.adminEventTypes.ajaxOnComplete, this, event);
                            });
                            xhr.obj.addEventListener('error', function (event) {
                                var returnedException = new dataAccess.entities.exceptions.errorDetails(JSON.parse(this.responseText));
                                var error = new mdBusinessLogic.helpers.mdException(returnedException.Message, event, returnedException);
                                switch (this.status) {
                                    case 401:
                                        mdBusinessLogic.globals.loggedOnUser = null;
                                        mdBusinessLogic.settings.admin.onEvent(mdBusinessLogic.settings.adminEventTypes.ajaxOnUnauthorized, this, event, error);
                                        break;
                                    case 403:
                                        mdBusinessLogic.settings.admin.onEvent(mdBusinessLogic.settings.adminEventTypes.ajaxOnForbidden, this, event, error);
                                        break;
                                    default:
                                        response_2.exception = error;
                                        if (xhr.errorEvents !== undefined) {
                                            for (var i = 0; i < xhr.errorEvents.length; i++) {
                                                xhr.errorEvents[i](response_2);
                                            }
                                        }
                                        break;
                                }
                                mdBusinessLogic.settings.ajax.connections.removeRequest(requestId);
                            });
                            xhr.obj.addEventListener('readystatechange', function (event) {
                                if (this.readyState === 4) {
                                    if (this.status !== 200) {
                                        if ((this.responseText !== undefined && this.responseText.length > 0)) {
                                            var returnedException = new dataAccess.entities.exceptions.errorDetails(JSON.parse(this.responseText));
                                            response_2.exception = new mdBusinessLogic.helpers.mdException(returnedException.Message, event, returnedException);
                                        }
                                        else {
                                            response_2.exception = new mdBusinessLogic.helpers.mdException('An error occurred while executing the ' + options.getMethodTypeString() + ' request! status(' + this.status.toString() + ')', event, (this.responseText !== undefined && this.responseText.length > 0) ? JSON.parse(this.responseText) : new Error());
                                        }
                                        options.onError(response_2);
                                    }
                                }
                            });
                            mdBusinessLogic.settings.admin.onEvent(mdBusinessLogic.settings.adminEventTypes.ajaxOnBeforeSend, xhr.obj).then(function (data) {
                                switch (options.method) {
                                    case base.AjaxMethodType.POST:
                                    case base.AjaxMethodType.DELETE:
                                        if (!options.isFormData) {
                                            if (options.contentType != null && options.contentType.value.indexOf("application/json") >= 0) {
                                                mdBusinessLogic.settings.admin.onEvent(mdBusinessLogic.settings.adminEventTypes.ajaxOnJsonSerialize, options.requestData).then(function (data) {
                                                    var resultData = data[0];
                                                    if (resultData == null) {
                                                        resultData = undefined;
                                                    }
                                                    xhr.obj.send(JSON.stringify(resultData));
                                                });
                                            }
                                            else {
                                                xhr.obj.send(this.prepareFormData(options.requestData));
                                            }
                                        }
                                        else {
                                            xhr.obj.send(options.requestData);
                                        }
                                        break;
                                    default:
                                        xhr.obj.send();
                                }
                            });
                            mdBusinessLogic.settings.ajax.connections.addRequest(xhr);
                        }
                    };
                    BaseController.prototype.generateNonSecureRequest = function (options) {
                        options = this.setHeaders(options);
                        var requestId = options.getRequestId();
                        var obj = this;
                        if (options.method == base.AjaxMethodType.SOCKET) {
                            obj.generateNonSecureRequestSocket(options, requestId);
                        }
                        else {
                            if (!options.isInitCall) {
                                controllers.systemInfoController.processPreInit(function () {
                                    obj.generateNonSecureRequestXhr(options, requestId);
                                });
                            }
                            else {
                                obj.generateNonSecureRequestXhr(options, requestId);
                            }
                        }
                    };
                    BaseController.prototype.setHeaders = function (options) {
                        if (options.includeAuthHeader && mdBusinessLogic.globals.loggedOnUserToken != null) {
                            options.headers.push(new base.AjaxMethodHeader(mdBusinessLogic.settings.authorizationHeader, mdBusinessLogic.globals.loggedOnUserToken));
                        }
                        if (mdBusinessLogic.settings.apiAllowCrossOrigin) {
                            options.headers.push(new base.AjaxMethodHeader('Access-Control-Allow-Origin', '*'));
                            options.headers.push(new base.AjaxMethodHeader('Access-Control-Allow-Methods', '*'));
                        }
                        if (mdBusinessLogic.settings.lcid != undefined && mdBusinessLogic.settings.lcid != 0) {
                            options.headers.push(new base.AjaxMethodHeader('LCID', mdBusinessLogic.settings.lcid.toString()));
                        }
                        if (options.isAdministration) {
                            options.headers.push(new base.AjaxMethodHeader('Administration', 'true'));
                        }
                        return options;
                    };
                    BaseController.prototype.prepareFormData = function (data) {
                        var formData = new Array();
                        if (data !== null && typeof data === 'object') {
                            for (var key in data) {
                                if (data !== null && !(data[key] instanceof Date) && (typeof data[key] === 'object' || Array.isArray(data))) {
                                    this.prepareSubFormItems(formData, data[key], key);
                                }
                                else if (data !== null && (data[key] instanceof Date || typeof data[key] === 'string' || typeof data[key] === 'number' || typeof data[key] === 'boolean')) {
                                    formData.push({
                                        name: key,
                                        value: encodeURIComponent((data[key] instanceof Date) ? moment(data[key]).format('YYYY-MM-DD HH:mm:ss') : data[key])
                                    });
                                }
                            }
                        }
                        return formData.map(function (item) {
                            return item.name + '=' + item.value;
                        }).join('&');
                    };
                    BaseController.prototype.prepareSubFormItems = function (formData, data, namePrefix) {
                        if (data !== null && typeof data === 'object') {
                            for (var key in data) {
                                if (data !== null && !(data[key] instanceof Date) && (typeof data[key] === 'object' || Array.isArray(data[key]))) {
                                    this.prepareSubFormItems(formData, data[key], namePrefix + '[' + key + ']');
                                }
                                else if (data !== null && (data[key] instanceof Date || typeof data[key] === 'string' || typeof data[key] === 'number' || typeof data[key] === 'boolean')) {
                                    formData.push({
                                        name: namePrefix + '[' + key + ']',
                                        value: encodeURIComponent((data[key] instanceof Date) ? moment(data[key]).format('YYYY-MM-DD HH:mm:ss') : data[key])
                                    });
                                }
                            }
                        }
                    };
                    BaseController.prototype._get = function (options) {
                        options.method = base.AjaxMethodType.GET;
                        this.generateNonSecureRequest(options);
                    };
                    BaseController.prototype._post = function (options) {
                        options.method = base.AjaxMethodType.POST;
                        this.generateNonSecureRequest(options);
                    };
                    BaseController.prototype._put = function (options) {
                        options.method = base.AjaxMethodType.PUT;
                        this.generateNonSecureRequest(options);
                    };
                    BaseController.prototype._delete = function (options) {
                        options.method = base.AjaxMethodType.DELETE;
                        this.generateNonSecureRequest(options);
                    };
                    BaseController.prototype._socket = function (options) {
                        options.method = base.AjaxMethodType.SOCKET;
                        this.generateNonSecureRequest(options);
                    };
                    return BaseController;
                }(base.BaseController_helpers));
                base.BaseController = BaseController;
            })(base = controllers.base || (controllers.base = {}));
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var attributeTypeDefinition = (function (_super) {
                __extends(attributeTypeDefinition, _super);
                function attributeTypeDefinition(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Name = '';
                    _this.DefaultValue = '';
                    _this.Type = null;
                    _this.InputType = null;
                    if (obj !== undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                attributeTypeDefinition.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Name = this.getValue(data, 'Name', '');
                    this.DefaultValue = this.getValue(data, 'DefaultValue', '');
                    this.Type = this.getValue(data, 'Type', 0);
                    this.InputType = this.getValue(data, 'InputType', 0);
                };
                attributeTypeDefinition.prototype.clone = function () {
                    return new attributeTypeDefinition(this);
                };
                return attributeTypeDefinition;
            }(entities.base.BaseEntity));
            entities.attributeTypeDefinition = attributeTypeDefinition;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var genericContent;
            (function (genericContent) {
                var baseField = (function (_super) {
                    __extends(baseField, _super);
                    function baseField(obj) {
                        var _this = _super.call(this, obj) || this;
                        _this.AttributeTypeDefinitionId = 0;
                        _this.AttributeTypeDefinition = null;
                        _this.Name = '';
                        _this.IsRequired = false;
                        _this.DefaultValue = '';
                        _this.Delimiter = '';
                        _this.ListValue = '';
                        _this.FriendlyName = '';
                        _this.UniqueId = '';
                        _this.IsReadOnly = false;
                        if (obj != undefined && obj != null) {
                            _this.construct(obj);
                        }
                        else {
                            if (_this.ListValue == '[]') {
                                _this.ListValue = '';
                            }
                            if (_this.DefaultValue == '[]') {
                                _this.DefaultValue = '';
                            }
                            if ((_this.DefaultValue === undefined || _this.DefaultValue == null || _this.DefaultValue == '') && !(_this.ListValue === undefined || _this.ListValue == null || _this.ListValue == '')) {
                                _this.DefaultValue = _this.getListValueAsArray()[0] || '';
                            }
                        }
                        return _this;
                    }
                    baseField.prototype.construct = function (data) {
                        _super.prototype.construct.call(this, data);
                        this.AttributeTypeDefinitionId = this.getValue(data, 'AttributeTypeDefinitionId', 0);
                        this.AttributeTypeDefinition = this.getConstructEntityValue(data, 'AttributeTypeDefinition', new entities.attributeTypeDefinition());
                        this.Name = this.getValue(data, 'Name', '');
                        this.IsRequired = this.getValue(data, 'IsRequired', false);
                        this.DefaultValue = this.getValue(data, 'DefaultValue', '');
                        this.FriendlyName = this.getValue(data, 'FriendlyName', '');
                        this.UniqueId = this.getValue(data, 'UniqueId', '');
                        this.Delimiter = this.getValue(data, 'Delimiter', '');
                        this.ListValue = this.getValue(data, 'ListValue', '');
                        this.IsReadOnly = this.getValue(data, 'IsReadOnly', false);
                        if (this.ListValue == '[]') {
                            this.ListValue = '';
                        }
                        if (this.DefaultValue == '[]') {
                            this.DefaultValue = '';
                        }
                        if ((this.DefaultValue === undefined || this.DefaultValue == null || this.DefaultValue == '') && !(this.ListValue === undefined || this.ListValue == null || this.ListValue == '')) {
                            this.DefaultValue = this.getListValueAsArray()[0] || '';
                        }
                    };
                    baseField.prototype.getListValueAsArray = function () {
                        if (this.ListValue === undefined || this.ListValue == null) {
                            this.ListValue = '';
                        }
                        if (this.Delimiter === undefined || this.Delimiter == null) {
                            this.Delimiter = '';
                        }
                        return this.ListValue.split(this.Delimiter);
                    };
                    return baseField;
                }(entities.base.BaseEntity));
                genericContent.baseField = baseField;
            })(genericContent = entities.genericContent || (entities.genericContent = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var genericContent;
            (function (genericContent) {
                var genericContentField = (function (_super) {
                    __extends(genericContentField, _super);
                    function genericContentField(obj) {
                        var _this = _super.call(this, obj) || this;
                        _this.Description = '';
                        _this.SafeName = '';
                        _this.Order = 0;
                        _this.Options = '';
                        _this.JsonField = new genericContent.genericContentFieldJsonField();
                        _this.OptionsJson = {};
                        _this.DataBound = false;
                        _this.DataSourceId = 0;
                        _this.DataSourceField = '';
                        _this.DataBoundReadOnly = false;
                        _this.IsDataBoundPrimaryKey = false;
                        if (obj !== undefined && obj != null) {
                            _this.construct(obj);
                        }
                        return _this;
                    }
                    genericContentField.prototype.construct = function (data) {
                        _super.prototype.construct.call(this, data);
                        this.Description = this.getValue(data, 'Description', '');
                        this.SafeName = this.getValue(data, 'SafeName', '');
                        this.Order = this.getValue(data, 'Order', 0);
                        this.Options = this.getValue(data, 'Options', '');
                        this.JsonField = this.getConstructValue(data, 'JsonField', new genericContent.genericContentFieldJsonField());
                        this.OptionsJson = this.getValue(data, 'OptionsJson', {});
                        this.DataBound = this.getValue(data, 'DataBound', false);
                        this.DataSourceId = this.getValue(data, 'DataSourceId', 0);
                        this.DataSourceField = this.getValue(data, 'DataSourceField', '');
                        this.DataBoundReadOnly = this.getValue(data, 'DataBoundReadOnly', false);
                        this.IsDataBoundPrimaryKey = this.getValue(data, 'IsDataBoundPrimaryKey', false);
                    };
                    genericContentField.prototype.setOptions = function (optionsJson) {
                        this.Options = JSON.stringify(optionsJson);
                    };
                    return genericContentField;
                }(genericContent.baseField));
                genericContent.genericContentField = genericContentField;
            })(genericContent = entities.genericContent || (entities.genericContent = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var genericContent;
            (function (genericContent) {
                var genericContentFieldValue = (function (_super) {
                    __extends(genericContentFieldValue, _super);
                    function genericContentFieldValue(obj) {
                        var _this = _super.call(this, obj) || this;
                        _this.Value = '';
                        if (obj != undefined && obj != null) {
                            _this.construct(obj);
                        }
                        else {
                            if ((_this.Value === undefined || _this.Value == null || _this.Value == '') && (_this.DefaultValue !== undefined && _this.DefaultValue != null)) {
                                _this.Value = _this.DefaultValue;
                            }
                        }
                        return _this;
                    }
                    genericContentFieldValue.prototype.construct = function (data) {
                        _super.prototype.construct.call(this, data);
                        this.Value = this.getValue(data, 'Value', '');
                        if ((this.Value === undefined || this.Value == null || this.Value == '') && (this.DefaultValue !== undefined && this.DefaultValue != null)) {
                            this.Value = this.DefaultValue;
                        }
                    };
                    return genericContentFieldValue;
                }(genericContent.genericContentField));
                genericContent.genericContentFieldValue = genericContentFieldValue;
            })(genericContent = entities.genericContent || (entities.genericContent = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var metaDataField = (function (_super) {
                __extends(metaDataField, _super);
                function metaDataField(obj) {
                    return _super.call(this, obj) || this;
                }
                metaDataField.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                };
                metaDataField.prototype.clone = function () {
                    return new metaDataField(this);
                };
                return metaDataField;
            }(entities.genericContent.genericContentField));
            entities.metaDataField = metaDataField;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var metaDataFieldValue = (function (_super) {
                __extends(metaDataFieldValue, _super);
                function metaDataFieldValue(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.ContentId = 0;
                    _this.LCID = 0;
                    _this.DateCreated = new Date();
                    _this.MetaDataFieldId = 0;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                metaDataFieldValue.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ContentId = this.getValue(data, 'ContentId', 0);
                    this.LCID = this.getValue(data, 'LCID', 0);
                    this.DateCreated = this.getValue(data, 'DateCreated', new Date());
                    this.MetaDataFieldId = this.getValue(data, 'MetaDataFieldId', 0);
                };
                metaDataFieldValue.prototype.clone = function () {
                    return new metaDataFieldValue(this);
                };
                return metaDataFieldValue;
            }(entities.genericContent.genericContentFieldValue));
            entities.metaDataFieldValue = metaDataFieldValue;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var template = (function (_super) {
                __extends(template, _super);
                function template(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Name = '';
                    _this.Description = '';
                    _this.TemplateUrl = '';
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                template.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Name = this.getValue(data, 'Name', '');
                    this.Description = this.getValue(data, 'Description', '');
                    this.TemplateUrl = this.getValue(data, 'TemplateUrl', '');
                };
                template.prototype.clone = function () {
                    return new template(this);
                };
                return template;
            }(entities.base.BaseEntity));
            entities.template = template;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var taxonomyContent = (function (_super) {
                __extends(taxonomyContent, _super);
                function taxonomyContent(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.LCID = 0;
                    _this.DateCreated = new Date();
                    _this.TaxonomyId = 0;
                    _this.Title = '';
                    _this.Type = '';
                    _this.Path = '';
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                taxonomyContent.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.LCID = this.getValue(data, 'LCID', 0);
                    this.DateCreated = this.getValue(data, 'DateCreated', null);
                    this.TaxonomyId = this.getValue(data, 'TaxonomyId', 0);
                    this.Title = this.getValue(data, 'Title', '');
                    this.Type = this.getValue(data, 'Type', '');
                    this.Path = this.getValue(data, 'Path', '');
                };
                taxonomyContent.prototype.clone = function () {
                    return new taxonomyContent(this);
                };
                return taxonomyContent;
            }(entities.base.BaseEntity));
            entities.taxonomyContent = taxonomyContent;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var taxonomy = (function (_super) {
                __extends(taxonomy, _super);
                function taxonomy(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.ParentId = 0;
                    _this.Name = '';
                    _this.Description = '';
                    _this.Parent = null;
                    _this.Children = new Array();
                    _this.Items = new Array();
                    _this.FreeTextField = '';
                    _this.Lcid = 0;
                    _this.FolderId = 0;
                    _this.TaxonomyPath = '';
                    _this.Contents = new Array();
                    _this.ParentArray = new Array();
                    _this.ChildrenTotalCount = 0;
                    _this.ItemsTotalCount = 0;
                    _this.Order = 0;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                taxonomy.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ParentId = this.getValue(data, 'ParentId', 0);
                    this.Name = this.getValue(data, 'Name', '');
                    this.Description = this.getValue(data, 'Description', '');
                    this.Parent = this.getValue(data, 'Parent', new taxonomy());
                    this.Children = this.getArrayConstructEntityValue(data, 'Children', new Array(), new taxonomy());
                    this.Items = this.getArrayConstructEntityValue(data, 'Items', new Array(), new entities.taxonomyContent());
                    this.FreeTextField = this.getValue(data, 'FreeTextField', '');
                    this.Lcid = this.getValue(data, 'Lcid', 0);
                    this.FolderId = this.getValue(data, 'FolderId', 0);
                    this.TaxonomyPath = this.getValue(data, 'TaxonomyPath', '');
                    this.Contents = this.getArrayConstructEntityValue(data, 'Contents', new Array(), new entities.content());
                    this.ParentArray = mdBusinessLogic.helpers.loadParentArray(this, "Name", "TaxonomyPath");
                    this.ChildrenTotalCount = this.getValue(data, "ChildrenTotalCount", 0);
                    this.ItemsTotalCount = this.getValue(data, "ItemsTotalCount", 0);
                    this.Order = this.getValue(data, "Order", 0);
                };
                taxonomy.prototype.clone = function () {
                    return new taxonomy(this);
                };
                return taxonomy;
            }(entities.base.BaseEntity));
            entities.taxonomy = taxonomy;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var contentAlias = (function (_super) {
                __extends(contentAlias, _super);
                function contentAlias(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.LCID = 0;
                    _this.DateCreated = new Date();
                    _this.ContentId = 0;
                    _this.Alias = '';
                    if (obj !== undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                contentAlias.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.LCID = this.getValue(data, 'LCID', 0);
                    this.DateCreated = this.getValue(data, 'DateCreated', null);
                    this.ContentId = this.getValue(data, 'ContentId', 0);
                    this.Alias = this.getValue(data, 'Alias', '');
                };
                contentAlias.prototype.clone = function () {
                    return new contentAlias(this);
                };
                return contentAlias;
            }(entities.base.BaseEntity));
            entities.contentAlias = contentAlias;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var contentTypeDefinition = (function (_super) {
                __extends(contentTypeDefinition, _super);
                function contentTypeDefinition(instance, obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Name = '';
                    _this.Description = '';
                    _this.Fields = new Array();
                    _this.Options = '';
                    _this.JsonOptions = null;
                    _this.IsEditable = true;
                    _this.Icon = '';
                    _this.Instance = new entities.contentTypeDefinitionField();
                    _this.DataSources = new Array();
                    _this.Joins = new Array();
                    if (instance !== undefined) {
                        _this.Instance = instance;
                    }
                    if (obj !== undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                contentTypeDefinition.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Name = this.getValue(data, 'Name', '');
                    this.Description = this.getValue(data, 'Description', '');
                    this.Fields = this.getArrayConstructEntityValue(data, 'Fields', new Array(), this.Instance);
                    this.Options = this.getValue(data, 'Options', '');
                    this.JsonOptions = this.getValue(data, 'JsonOptions', null);
                    this.IsEditable = this.getValue(data, 'IsEditable', true);
                    this.Icon = this.getValue(data, 'Icon', '');
                    this.DataSources = this.getArrayConstructEntityValue(data, 'DataSources', new Array(), new entities.contentTypeDataSource());
                    this.Joins = this.getArrayConstructEntityValue(data, 'Joins', new Array(), new entities.contentTypeDataSourceJoin());
                };
                contentTypeDefinition.prototype.clone = function () {
                    return new contentTypeDefinition(this.Instance, this);
                };
                contentTypeDefinition.prototype.convertToFieldValue = function () {
                    this.Fields = this.Fields.map(function (item) {
                        var fieldValue = new entities.contentTypeDefinitionFieldValue();
                        fieldValue.construct(item);
                        fieldValue.ContentTypeDefinitionFieldId = item.Id;
                        return fieldValue;
                    });
                    return this;
                };
                contentTypeDefinition.prototype.setFieldValue = function (value, fieldName) {
                    if (this.Fields != null) {
                        for (var i in this.Fields) {
                            if (this.Fields[i].Name == fieldName) {
                                this.Fields[i]['Value'] = value;
                                break;
                            }
                        }
                    }
                };
                contentTypeDefinition.prototype.getFieldValue = function (fieldName) {
                    if (this.Fields != null) {
                        for (var i in this.Fields) {
                            if (this.Fields[i].Name == fieldName && this.Fields[i]['Value'] !== undefined) {
                                return this.Fields[i]['Value'];
                            }
                        }
                    }
                    return null;
                };
                contentTypeDefinition.prototype.getField = function (fieldName) {
                    if (this.Fields != null) {
                        for (var i in this.Fields) {
                            if (this.Fields[i].Name == fieldName) {
                                return this.Fields[i];
                            }
                        }
                    }
                    return null;
                };
                contentTypeDefinition.prototype.hasLinkToTitle = function () {
                    return this.Fields.filter(function (f) { return f.JsonField.linkToTitle; }).length > 0;
                };
                contentTypeDefinition.prototype.getLinkToTitle = function () {
                    if (this.hasLinkToTitle()) {
                        return this.Fields.filter(function (f) { return f.JsonField.linkToTitle; })[0];
                    }
                    return null;
                };
                contentTypeDefinition.prototype.setJsonOptions = function (jsonOptions) {
                    this.JsonOptions = jsonOptions;
                    this.Options = JSON.stringify(jsonOptions);
                };
                return contentTypeDefinition;
            }(entities.base.BaseEntity));
            entities.contentTypeDefinition = contentTypeDefinition;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var contentTypeDefinitionFieldValue = (function (_super) {
                __extends(contentTypeDefinitionFieldValue, _super);
                function contentTypeDefinitionFieldValue(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.ContentId = '0';
                    _this.LCID = 0;
                    _this.DateCreated = new Date();
                    _this.ContentTypeDefinitionFieldId = 0;
                    _this.ContentTypeDefinitionId = 0;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                contentTypeDefinitionFieldValue.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ContentId = this.getValue(data, 'ContentId', '0');
                    this.LCID = this.getValue(data, 'LCID', 0);
                    this.DateCreated = this.getValue(data, 'DateCreated', new Date());
                    this.ContentTypeDefinitionFieldId = this.getValue(data, 'ContentTypeDefinitionFieldId', 0);
                    this.ContentTypeDefinitionId = this.getValue(data, 'ContentTypeDefinitionId', 0);
                };
                contentTypeDefinitionFieldValue.prototype.clone = function () {
                    return new contentTypeDefinitionFieldValue(this);
                };
                return contentTypeDefinitionFieldValue;
            }(entities.genericContent.genericContentFieldValue));
            entities.contentTypeDefinitionFieldValue = contentTypeDefinitionFieldValue;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var content = (function (_super) {
                __extends(content, _super);
                function content(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.LCID = 0;
                    _this.DateCreated = new Date();
                    _this.AuthorId = 0;
                    _this.FolderId = 0;
                    _this.Title = "";
                    _this.Path = "";
                    _this.Html = null;
                    _this.Author = null;
                    _this.ContentType = null;
                    _this.Taxonomy = new Array();
                    _this.MetaDataFieldValues = new Array();
                    _this.ContentAliases = new Array();
                    _this.Template = null;
                    _this.IsNew = true;
                    _this.IsPublished = false;
                    _this.IsDataBound = false;
                    _this.UniqueId = "";
                    _this.ContentTypeDefinitionId = 0;
                    if (obj !== undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                content.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.LCID = this.getValue(data, 'LCID', 0);
                    this.DateCreated = this.getValue(data, 'DateCreated', null);
                    this.AuthorId = this.getValue(data, 'AuthorId', 0);
                    this.FolderId = this.getValue(data, 'FolderId', 0);
                    this.Title = this.getValue(data, 'Title', '');
                    this.Path = this.getValue(data, 'Path', '');
                    this.Html = this.getValue(data, 'Html', '');
                    this.ContentTypeDefinitionId = this.getValue(data, 'ContentTypeDefinitionId', 0);
                    this.Author = this.getConstructEntityValue(data, 'Author', new entities.user());
                    this.ContentType = this.getConstructEntityValue(data, 'ContentType', new entities.contentTypeDefinition(new entities.contentTypeDefinitionFieldValue()));
                    this.Taxonomy = this.getArrayConstructEntityValue(data, 'Taxonomy', new Array(), new entities.taxonomy());
                    this.MetaDataFieldValues = this.getArrayConstructEntityValue(data, 'MetaDataFieldValues', new Array(), new entities.metaDataFieldValue());
                    if (data.ContentAliases !== undefined && data.ContentAliases != null && data.ContentAliases.length > 0 && (typeof data.ContentAliases[0] === 'string' || data.ContentAliases[0] instanceof String)) {
                        var thisObj_1 = this;
                        data.ContentAliases = data.ContentAliases.map(function (alias) {
                            var al = new entities.contentAlias();
                            al.construct({
                                LCID: thisObj_1.LCID,
                                DateCreated: thisObj_1.DateCreated,
                                ContentId: thisObj_1.Id,
                                Alias: alias
                            });
                            return al;
                        });
                    }
                    this.ContentAliases = this.getArrayConstructEntityValue(data, 'ContentAliases', new Array(), new entities.contentAlias());
                    this.Template = this.getConstructEntityValue(data, 'Template', new entities.template());
                    this.IsNew = this.getValue(data, 'IsNew', false);
                    this.IsPublished = this.getValue(data, 'IsPublished', false);
                    this.IsDataBound = this.getValue(data, 'IsDataBound', false);
                    this.UniqueId = this.getValue(data, 'UniqueId', '');
                };
                content.prototype.clone = function () {
                    return new content(this);
                };
                return content;
            }(entities.base.BaseEntity));
            entities.content = content;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var options;
            (function (options) {
                var v2;
                (function (v2) {
                    var enums;
                    (function (enums) {
                        var contentEnum;
                        (function (contentEnum) {
                            contentEnum[contentEnum["ContentId"] = 0] = "ContentId";
                            contentEnum[contentEnum["LCID"] = 1] = "LCID";
                            contentEnum[contentEnum["DateCreated"] = 2] = "DateCreated";
                            contentEnum[contentEnum["AuthorId"] = 3] = "AuthorId";
                            contentEnum[contentEnum["FolderId"] = 4] = "FolderId";
                            contentEnum[contentEnum["Title"] = 5] = "Title";
                            contentEnum[contentEnum["Html"] = 6] = "Html";
                            contentEnum[contentEnum["SearchTerm"] = 7] = "SearchTerm";
                            contentEnum[contentEnum["ContentTypeDefinitionId"] = 8] = "ContentTypeDefinitionId";
                            contentEnum[contentEnum["ContentCount"] = 9] = "ContentCount";
                            contentEnum[contentEnum["Alias"] = 10] = "Alias";
                            contentEnum[contentEnum["TaxonomyId"] = 11] = "TaxonomyId";
                            contentEnum[contentEnum["IsPublished"] = 12] = "IsPublished";
                            contentEnum[contentEnum["Folderpath"] = 13] = "Folderpath";
                            contentEnum[contentEnum["ApprovalPending"] = 14] = "ApprovalPending";
                        })(contentEnum = enums.contentEnum || (enums.contentEnum = {}));
                    })(enums = v2.enums || (v2.enums = {}));
                })(v2 = options.v2 || (options.v2 = {}));
            })(options = controllers.options || (controllers.options = {}));
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var contentController = (function (_super) {
                __extends(contentController, _super);
                function contentController(controllerBase) {
                    if (controllerBase === void 0) { controllerBase = 'Content/'; }
                    return _super.call(this, controllerBase) || this;
                }
                contentController.prototype.get = function (opts, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('', opts);
                    options.responseData = new dataAccess.entities.paginationEntity(dataAccess.entities.content);
                    options.lcid = opts.Lcid;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentController.prototype.getById = function (id, loadAuthor, lcid, fillFields, isDataBound, contentTypeDefinitionId, onSuccess, onError) {
                    if (fillFields === void 0) { fillFields = true; }
                    if (isDataBound === void 0) { isDataBound = false; }
                    if (contentTypeDefinitionId === void 0) { contentTypeDefinitionId = 0; }
                    this.get({
                        ContentIds: [id],
                        FillFields: fillFields,
                        FillMetaData: loadAuthor,
                        Lcid: lcid
                    }, function (result) {
                        onSuccess(result.Items[0]);
                    }, function (error) {
                        onError(error);
                    });
                };
                contentController.prototype.getByIds = function (ids, loadAuthor, lcid, fillFields, isDataBound, contentTypeDefinitionId, onSuccess, onError) {
                    if (fillFields === void 0) { fillFields = true; }
                    if (isDataBound === void 0) { isDataBound = false; }
                    if (contentTypeDefinitionId === void 0) { contentTypeDefinitionId = 0; }
                    this.get({
                        ContentIds: ids,
                        FillFields: fillFields,
                        LoadAuthor: loadAuthor,
                        Lcid: lcid
                    }, function (result) {
                        onSuccess(result.Items);
                    }, function (error) {
                        onError(error);
                    });
                };
                contentController.prototype.getByRequest = function (request, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByRequest');
                    options.requestData = request;
                    options.responseData = new dataAccess.entities.paginationEntity(dataAccess.entities.content);
                    options.headers.push(new controllers.base.AjaxMethodHeader('loadAuthor', request.LoadAuthor.toString()));
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                contentController.prototype.taxonomyContentGetContentByTaxonomy = function (id, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('TaxonomyContentGetContentByTaxonomy/', [id]);
                    options.responseData = new dataAccess.entities.content;
                    options.isJsonArray = true;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentController.prototype.taxonomyContentGetContentByTaxonomyFullMeta = function (id, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('TaxonomyContentGetContentByTaxonomy/', [id, true, false]);
                    options.responseData = new dataAccess.entities.content;
                    options.isJsonArray = true;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentController.prototype.menuContentGetContentByMenu = function (id, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('MenuContentGetContentByMenu/', [id]);
                    options.responseData = new dataAccess.entities.content;
                    options.isJsonArray = true;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentController.prototype.translate = function (content, targetLcid, lcid, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Translate/');
                    options.requestData = content;
                    options.responseData = new dataAccess.entities.content;
                    options.lcid = lcid;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentController.prototype.selectAllCount = function (id, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('SelectAllCount/', [id]);
                    options.responseData = new dataAccess.entities.content;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentController.prototype.getByFolderId = function (id, loadAuthor, lcid, loadFields, loadMetaDataFields, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByFolderId/', [id, loadAuthor, lcid, loadFields, loadMetaDataFields]);
                    options.responseData = new dataAccess.entities.content;
                    options.isJsonArray = true;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentController.prototype.paginationGetByFolderId = function (paginationData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('PaginationGetByFolderId/', paginationData);
                    options.responseData = new dataAccess.entities.paginationEntity(dataAccess.entities.content);
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentController.prototype.getByFolderIdCount = function (countData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByFolderIdCount/', countData);
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.onSuccess = function (response) {
                        onSuccess(response.responseData.Value);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentController.prototype.getAll = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetAll/');
                    options.responseData = new dataAccess.entities.content;
                    options.isJsonArray = true;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentController.prototype.getAllVersion = function (id, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetAllVersion/', [id]);
                    options.responseData = new dataAccess.entities.content;
                    options.isJsonArray = true;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentController.prototype.getByAll = function (obj, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByAll/');
                    options.responseData = new dataAccess.entities.content;
                    options.requestData = obj;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                contentController.prototype.search = function (searchData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('Search', searchData);
                    options.responseData = new dataAccess.entities.content();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentController.prototype.getBySearchTerm = function (searchTerm, loadAuthor, lcid, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetBySearchTerm/', [searchTerm]);
                    options.responseData = new dataAccess.entities.content;
                    options.isJsonArray = true;
                    options.lcid = lcid;
                    options.headers.push(new controllers.base.AjaxMethodHeader('loadAuthor', loadAuthor.toString()));
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentController.prototype.save = function (obj, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save/');
                    options.responseData = new dataAccess.entities.content;
                    options.requestData = obj;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                contentController.prototype.del = function (id, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete/', [id]);
                    options.responseData = new dataAccess.entities.content;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                contentController.prototype.deleteByAll = function (id, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('DeleteByAll/', [id]);
                    options.responseData = new dataAccess.entities.content;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                contentController.prototype.selectByContentTypeDefinitionCount = function (id, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('SelectByContentTypeDefinitionCount/', [id]);
                    options.responseData = new dataAccess.entities.content;
                    options.isJsonArray = true;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentController.prototype.doesContentExist = function (contents, content) {
                    var contentIndex = -1;
                    for (var i = 0; i < contents.length; i++) {
                        if (contents[i].Id == content.Id) {
                            contentIndex = i;
                            break;
                        }
                    }
                    return contentIndex;
                };
                return contentController;
            }(controllers.base.BaseController));
            controllers.contentController = contentController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var approvalChain = (function (_super) {
                __extends(approvalChain, _super);
                function approvalChain(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.FolderId = 0;
                    _this.IsActive = false;
                    _this.Steps = [];
                    _this.ChainId = 0;
                    if (obj !== undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                approvalChain.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.FolderId = this.getValue(data, "FolderId", 0);
                    this.IsActive = this.getValue(data, "IsActive", false);
                    this.Steps = this.getValue(data, "Steps", []);
                    this.ChainId = this.getValue(data, "ChainId", 0);
                };
                approvalChain.prototype.clone = function () {
                    return new approvalChain(this);
                };
                return approvalChain;
            }(entities.base.BaseEntity));
            entities.approvalChain = approvalChain;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var approvalChainStepAction = (function (_super) {
                __extends(approvalChainStepAction, _super);
                function approvalChainStepAction(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.StepId = 0;
                    _this.UserId = 0;
                    _this.Action = 0;
                    _this.Type = 0;
                    _this.RedirectTo = 0;
                    if (obj !== undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                approvalChainStepAction.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.StepId = this.getValue(data, "StepId", 0);
                    this.UserId = this.getValue(data, "UserId", 0);
                    this.Action = this.getValue(data, "Action", 0);
                    this.Type = this.getValue(data, "Type", 0);
                    this.RedirectTo = this.getValue(data, "RedirectTo", 0);
                };
                approvalChainStepAction.prototype.clone = function () {
                    return new approvalChainStepAction(this);
                };
                return approvalChainStepAction;
            }(entities.base.BaseEntity));
            entities.approvalChainStepAction = approvalChainStepAction;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var approvalChainStep = (function (_super) {
                __extends(approvalChainStep, _super);
                function approvalChainStep(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.ApprovalChainId = 0;
                    _this.ComboOperator = 0;
                    _this.Order = 0;
                    _this.UserIds = new Array();
                    _this.Actions = new Array();
                    if (obj !== undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                approvalChainStep.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ApprovalChainId = this.getValue(data, "ApprovalChainId", 0);
                    this.ComboOperator = this.getValue(data, "ComboOperator", 0);
                    this.Order = this.getValue(data, "Order", 0);
                    this.UserIds = this.getArrayConstructEntityValue(data, "UserIds", new Array(), new entities.primitiveType());
                    this.Actions = this.getArrayConstructEntityValue(data, "Actions", new Array(), new entities.approvalChainStepAction());
                };
                approvalChainStep.prototype.clone = function () {
                    return new approvalChainStep(this);
                };
                return approvalChainStep;
            }(entities.base.BaseEntity));
            entities.approvalChainStep = approvalChainStep;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var approvalChainApproval = (function (_super) {
                __extends(approvalChainApproval, _super);
                function approvalChainApproval(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.ApprovalType = 1;
                    _this.ReviewDate = null;
                    _this.Content = null;
                    _this.Comment = '';
                    _this.User = null;
                    _this.Step = null;
                    if (obj !== undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                approvalChainApproval.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ApprovalType = this.getValue(data, "ApprovalType", 1);
                    this.ReviewDate = this.getValue(data, "ReviewDate", null);
                    this.Content = this.getValue(data, "Content", new entities.content());
                    this.Comment = this.getValue(data, "Comment", '');
                    this.User = this.getValue(data, "User", new entities.user());
                    this.Step = this.getValue(data, "Step", new entities.approvalChainStep());
                };
                approvalChainApproval.prototype.clone = function () {
                    return new approvalChainApproval(this);
                };
                return approvalChainApproval;
            }(entities.base.BaseEntity));
            entities.approvalChainApproval = approvalChainApproval;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var approvalChainController = (function (_super) {
                __extends(approvalChainController, _super);
                function approvalChainController() {
                    return _super.call(this, 'ApprovalChain/') || this;
                }
                approvalChainController.prototype.getById = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetById', [id]);
                    options.responseData = new dataAccess.entities.approvalChain();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                approvalChainController.prototype.getByFolderId = function (folderId, onSuccess, onError) {
                    folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByFolderId', [folderId]);
                    options.responseData = new dataAccess.entities.approvalChain();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                approvalChainController.prototype.save = function (approvalChain, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.approvalChain();
                    options.requestData = approvalChain;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                approvalChainController.prototype['delete'] = function (approvalChain, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete');
                    options.responseData = new dataAccess.entities.approvalChain();
                    options.requestData = approvalChain;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                approvalChainController.prototype.getStepById = function (id, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetStepById', [id]);
                    options.responseData = new dataAccess.entities.approvalChain();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                approvalChainController.prototype.getStepsByApprovalChainId = function (id, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetStepsByApprovalChainId', [id]);
                    options.responseData = new dataAccess.entities.approvalChain();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                approvalChainController.prototype.addStep = function (step, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('AddStep');
                    options.responseData = new dataAccess.entities.approvalChainStep();
                    options.requestData = step;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                approvalChainController.prototype.deleteStep = function (step, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('DeleteStep');
                    options.responseData = new dataAccess.entities.approvalChain();
                    options.requestData = step;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                approvalChainController.prototype.getStepActionById = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetStepActionById', [id]);
                    options.responseData = new dataAccess.entities.approvalChainStepAction();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                approvalChainController.prototype.getStepActionsByStepId = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetStepActionsByStepId', [id]);
                    options.responseData = new dataAccess.entities.approvalChainStepAction();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                approvalChainController.prototype.addStepAction = function (stepAction, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('AddStepAction');
                    options.responseData = new dataAccess.entities.approvalChainStepAction();
                    options.requestData = stepAction;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                approvalChainController.prototype.deleteStepAction = function (stepAction, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('DeleteStepAction');
                    options.responseData = new dataAccess.entities.approvalChainStepAction();
                    options.requestData = stepAction;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                approvalChainController.prototype.addApproval = function (approval, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('AddApproval');
                    options.responseData = new dataAccess.entities.approvalChainApproval();
                    options.requestData = approval;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                return approvalChainController;
            }(controllers.base.BaseController));
            controllers.approvalChainController = approvalChainController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var attributeTypeDefinitionController = (function (_super) {
                __extends(attributeTypeDefinitionController, _super);
                function attributeTypeDefinitionController() {
                    return _super.call(this, 'AttributeTypeDefinition/') || this;
                }
                attributeTypeDefinitionController.prototype.getById = function (id, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetById', [id]);
                    options.responseData = new dataAccess.entities.attributeTypeDefinition();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                attributeTypeDefinitionController.prototype.getByInputTypeId = function (id, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByInputTypeId', [id]);
                    options.responseData = new dataAccess.entities.attributeTypeDefinition();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                attributeTypeDefinitionController.prototype.getAll = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAll');
                    options.responseData = new dataAccess.entities.attributeTypeDefinition();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                return attributeTypeDefinitionController;
            }(controllers.base.BaseController));
            controllers.attributeTypeDefinitionController = attributeTypeDefinitionController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var omegaCachingObject = (function () {
                function omegaCachingObject(obj) {
                    this.ByteSize = 0;
                    this.CacheKey = '';
                    this.Timeout = '';
                    this.CacheTime = new Date();
                    this.CacheValue = '';
                    if (obj != undefined && obj != null) {
                        this.construct(obj);
                    }
                }
                omegaCachingObject.prototype.construct = function (data) {
                    this.ByteSize = mdBusinessLogic.helpers.entityHelper.getValue(data, "ByteSize", 0);
                    this.CacheSource = mdBusinessLogic.helpers.entityHelper.getValue(data, "CacheSource", '');
                    this.CacheKey = mdBusinessLogic.helpers.entityHelper.getValue(data, "CacheKey", '');
                    this.Timeout = mdBusinessLogic.helpers.entityHelper.getValue(data, "Timeout", '');
                    this.CacheTime = mdBusinessLogic.helpers.entityHelper.getValue(data, "CacheTime", new Date());
                    this.CacheValue = mdBusinessLogic.helpers.entityHelper.getValue(data, "CacheValue", '');
                };
                omegaCachingObject.prototype.clone = function () {
                    return new omegaCachingObject(this);
                };
                return omegaCachingObject;
            }());
            entities.omegaCachingObject = omegaCachingObject;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var cacheResponse = (function () {
                function cacheResponse(obj) {
                    this.ProviderName = '';
                    this.CacheObjects = new Array();
                    if (obj != undefined && obj != null) {
                        this.construct(obj);
                    }
                }
                cacheResponse.prototype.construct = function (data) {
                    this.ProviderName = mdBusinessLogic.helpers.entityHelper.getValue(data, "ProviderName", '');
                    this.CacheObjects = mdBusinessLogic.helpers.entityHelper.getArrayConstructValue(data, 'CacheObjects', new Array(), new entities.omegaCachingObject());
                };
                cacheResponse.prototype.clone = function () {
                    return new cacheResponse(this);
                };
                return cacheResponse;
            }());
            entities.cacheResponse = cacheResponse;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var cacheController = (function (_super) {
                __extends(cacheController, _super);
                function cacheController() {
                    return _super.call(this, 'Cache/') || this;
                }
                cacheController.prototype.getAllDataCache = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetDataCache');
                    options.responseData = new dataAccess.entities.cacheResponse();
                    options.isJsonArray = true;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                cacheController.prototype.invalidateDataCache = function (provider, cacheKey, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('InvalidateDataCache', [provider, encodeURI(cacheKey)]);
                    options.responseData = new dataAccess.entities.cacheResponse();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                return cacheController;
            }(controllers.base.BaseController));
            controllers.cacheController = cacheController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var contentAliasController = (function (_super) {
                __extends(contentAliasController, _super);
                function contentAliasController() {
                    return _super.call(this, 'ContentAlias/') || this;
                }
                contentAliasController.prototype.getById = function (id, lcid, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.lcid = lcid;
                    options.address = this.getAddress('GetById', [id]);
                    options.responseData = new dataAccess.entities.contentAlias();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentAliasController.prototype.getAll = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAll');
                    options.responseData = new dataAccess.entities.contentAlias();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentAliasController.prototype.getAllByContent = function (content, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAllAliasesByContent');
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.requestData = content;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                contentAliasController.prototype.del = function (id, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [id]);
                    options.responseData = new dataAccess.entities.contentAlias();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                contentAliasController.prototype.save = function (contentAlias, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.contentAlias();
                    options.requestData = contentAlias;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                return contentAliasController;
            }(controllers.base.BaseController));
            controllers.contentAliasController = contentAliasController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var editable = (function () {
                function editable(edit) {
                    this.Edit = edit;
                }
                editable.prototype.construct = function (data) {
                    this.Edit = entities.base.BaseEntity.getValue(data, 'Edit', false);
                };
                editable.prototype.clone = function () {
                    return this;
                };
                return editable;
            }());
            entities.editable = editable;
            var length = (function (_super) {
                __extends(length, _super);
                function length(obj) {
                    var _this = _super.call(this, false) || this;
                    _this.Length = 1;
                    if (obj != undefined && obj != null) {
                        _this = _super.call(this, obj.Edit) || this;
                        _this.construct(obj);
                    }
                    return _this;
                }
                length.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Length = entities.base.BaseEntity.getValue(data, 'Length', 1);
                };
                length.prototype.clone = function () {
                    return new length(this);
                };
                return length;
            }(editable));
            entities.length = length;
            var casing = (function (_super) {
                __extends(casing, _super);
                function casing(obj) {
                    var _this = _super.call(this, false) || this;
                    _this.UpperCase = true;
                    _this.LowerCase = true;
                    if (obj != undefined && obj != null) {
                        _this = _super.call(this, obj.Edit) || this;
                        _this.construct(obj);
                    }
                    return _this;
                }
                casing.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.UpperCase = entities.base.BaseEntity.getValue(data, 'UpperCase', true);
                    this.LowerCase = entities.base.BaseEntity.getValue(data, 'LowerCase', true);
                };
                casing.prototype.clone = function () {
                    return new casing(this);
                };
                return casing;
            }(editable));
            entities.casing = casing;
            var specialCharacters = (function (_super) {
                __extends(specialCharacters, _super);
                function specialCharacters(obj) {
                    var _this = _super.call(this, false) || this;
                    _this.Included = new Array();
                    if (obj != undefined && obj != null) {
                        _this = _super.call(this, obj.Edit) || this;
                        _this.construct(obj);
                    }
                    return _this;
                }
                specialCharacters.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Included = entities.base.BaseEntity.getValue(data, 'Included', new Array());
                };
                specialCharacters.prototype.clone = function () {
                    return new specialCharacters(this);
                };
                return specialCharacters;
            }(editable));
            entities.specialCharacters = specialCharacters;
            var numbers = (function (_super) {
                __extends(numbers, _super);
                function numbers(obj) {
                    var _this = _super.call(this, false) || this;
                    _this.From = 0;
                    _this.To = 1;
                    if (obj != undefined && obj != null) {
                        _this = _super.call(this, obj.Edit) || this;
                        _this.construct(obj);
                    }
                    return _this;
                }
                numbers.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.From = entities.base.BaseEntity.getValue(data, 'From', 0);
                    this.To = entities.base.BaseEntity.getValue(data, 'To', 1);
                };
                numbers.prototype.clone = function () {
                    return new numbers(this);
                };
                return numbers;
            }(editable));
            entities.numbers = numbers;
            var characterTypes = (function (_super) {
                __extends(characterTypes, _super);
                function characterTypes(obj) {
                    var _this = _super.call(this, false) || this;
                    _this.Casing = new casing();
                    _this.SpecialCharacters = new specialCharacters();
                    _this.Numbers = new numbers();
                    if (obj != undefined && obj != null) {
                        _this = _super.call(this, obj.Edit) || this;
                        _this.construct(obj);
                    }
                    return _this;
                }
                characterTypes.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Casing = entities.base.BaseEntity.getConstructValue(data, 'Casing', new casing());
                    this.SpecialCharacters = entities.base.BaseEntity.getConstructValue(data, 'SpecialCharacters', new specialCharacters());
                    this.Numbers = entities.base.BaseEntity.getConstructValue(data, 'Numbers', new numbers());
                };
                characterTypes.prototype.clone = function () {
                    return new characterTypes(this);
                };
                return characterTypes;
            }(editable));
            entities.characterTypes = characterTypes;
            var email = (function (_super) {
                __extends(email, _super);
                function email(obj) {
                    var _this = _super.call(this, false) || this;
                    _this.Domain = '';
                    _this.Extension = '';
                    if (obj != undefined && obj != null) {
                        _this = _super.call(this, obj.Edit) || this;
                        _this.construct(obj);
                    }
                    return _this;
                }
                email.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Domain = entities.base.BaseEntity.getValue(data, 'Domain', '');
                    this.Extension = entities.base.BaseEntity.getValue(data, 'Extension', '');
                };
                email.prototype.clone = function () {
                    return new email(this);
                };
                return email;
            }(editable));
            entities.email = email;
            var webAddress = (function (_super) {
                __extends(webAddress, _super);
                function webAddress(obj) {
                    var _this = _super.call(this, false) || this;
                    _this.Includes = new Array();
                    _this.Protocols = new Array();
                    if (obj != undefined && obj != null) {
                        _this = _super.call(this, obj.Edit) || this;
                        _this.construct(obj);
                    }
                    return _this;
                }
                webAddress.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Includes = entities.base.BaseEntity.getValue(data, 'Includes', new Array());
                    this.Protocols = entities.base.BaseEntity.getValue(data, 'Protocols', new Array());
                };
                webAddress.prototype.clone = function () {
                    return new webAddress(this);
                };
                return webAddress;
            }(editable));
            entities.webAddress = webAddress;
            var typeValidation = (function (_super) {
                __extends(typeValidation, _super);
                function typeValidation(obj) {
                    var _this = _super.call(this, false) || this;
                    _this.Email = new email();
                    _this.WebAddress = new webAddress();
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                typeValidation.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Email = entities.base.BaseEntity.getConstructValue(data, 'Email', new email());
                    this.WebAddress = entities.base.BaseEntity.getConstructValue(data, 'WebAddress', new webAddress());
                };
                typeValidation.prototype.clone = function () {
                    return new typeValidation(this);
                };
                return typeValidation;
            }(editable));
            entities.typeValidation = typeValidation;
            var fieldValidation = (function () {
                function fieldValidation(obj) {
                    this.MinLength = new length();
                    this.MaxLength = new length();
                    this.CharacterTypes = new characterTypes();
                    this.TypeValidation = new typeValidation();
                    this.Regex = '';
                    this.Required = false;
                    this.Repeatable = false;
                    if (obj != undefined && obj != null) {
                        this.construct(obj);
                    }
                }
                fieldValidation.prototype.construct = function (data) {
                    this.MinLength = entities.base.BaseEntity.getConstructValue(data, 'MinLength', new length());
                    this.MaxLength = entities.base.BaseEntity.getConstructValue(data, 'MaxLength', new length());
                    this.CharacterTypes = entities.base.BaseEntity.getConstructValue(data, 'CharacterTypes', new characterTypes());
                    this.TypeValidation = entities.base.BaseEntity.getConstructValue(data, 'TypeValidation', new typeValidation());
                    this.Regex = entities.base.BaseEntity.getValue(data, 'Regex', '');
                    this.Required = entities.base.BaseEntity.getValue(data, 'Required', false);
                    this.Repeatable = entities.base.BaseEntity.getValue(data, 'Repeatable', false);
                };
                fieldValidation.prototype.clone = function () {
                    return new fieldValidation(this);
                };
                return fieldValidation;
            }());
            entities.fieldValidation = fieldValidation;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var contentTypeDefinitionField = (function (_super) {
                __extends(contentTypeDefinitionField, _super);
                function contentTypeDefinitionField(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.ContentTypeDefinitionId = 0;
                    if (obj !== undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                contentTypeDefinitionField.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ContentTypeDefinitionId = this.getValue(data, 'ContentTypeDefinitionId', 0);
                };
                contentTypeDefinitionField.prototype.clone = function () {
                    return new contentTypeDefinitionField(this);
                };
                return contentTypeDefinitionField;
            }(entities.genericContent.genericContentField));
            entities.contentTypeDefinitionField = contentTypeDefinitionField;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var contentTypeDataSourceController = (function (_super) {
                __extends(contentTypeDataSourceController, _super);
                function contentTypeDataSourceController() {
                    return _super.call(this, 'ContentTypeDefinitionDatasource/') || this;
                }
                contentTypeDataSourceController.prototype.getById = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetById', [id]);
                    options.responseData = new dataAccess.entities.contentTypeDataSource();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentTypeDataSourceController.prototype.getByContentTypeDefinitionId = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByContentTypeDefinitionId', [id]);
                    options.responseData = new dataAccess.entities.contentTypeDataSource();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentTypeDataSourceController.prototype.save = function (contentTypeDataSource, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.contentTypeDataSource();
                    options.requestData = contentTypeDataSource;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                contentTypeDataSourceController.prototype.del = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [id]);
                    options.responseData = new dataAccess.entities.contentTypeDataSource();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                contentTypeDataSourceController.prototype.getDataStructure = function (contentTypeDataSource, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = false;
                    options.address = this.getAddress('GetDataStructure');
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.requestData = contentTypeDataSource;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData.Value);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                contentTypeDataSourceController.prototype.getAllDatabaseTypes = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = false;
                    options.address = this.getAddress('GetAllDatabaseTypes');
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData.Value);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                return contentTypeDataSourceController;
            }(controllers.base.BaseController));
            controllers.contentTypeDataSourceController = contentTypeDataSourceController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var contentTypeDataSourceJoinController = (function (_super) {
                __extends(contentTypeDataSourceJoinController, _super);
                function contentTypeDataSourceJoinController() {
                    return _super.call(this, 'ContentTypeDefinitionDatasource/') || this;
                }
                contentTypeDataSourceJoinController.prototype.getById = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetById', [id]);
                    options.responseData = new dataAccess.entities.contentTypeDataSourceJoin();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentTypeDataSourceJoinController.prototype.save = function (contentTypeDataSourceJoin, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.contentTypeDataSourceJoin();
                    options.requestData = contentTypeDataSourceJoin;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                contentTypeDataSourceJoinController.prototype.del = function (contentTypeDataSourceJoin, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete');
                    options.responseData = new dataAccess.entities.contentTypeDataSourceJoin();
                    options.requestData = contentTypeDataSourceJoin;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                return contentTypeDataSourceJoinController;
            }(controllers.base.BaseController));
            controllers.contentTypeDataSourceJoinController = contentTypeDataSourceJoinController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var contentTypeDefinitionControllerGeneric = (function (_super) {
                __extends(contentTypeDefinitionControllerGeneric, _super);
                function contentTypeDefinitionControllerGeneric() {
                    return _super.call(this, 'ContentTypeDefinition/') || this;
                }
                contentTypeDefinitionControllerGeneric.prototype.getById = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetById', [id]);
                    options.responseData = new dataAccess.entities.contentTypeDefinition();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentTypeDefinitionControllerGeneric.prototype.getAll = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAll');
                    options.responseData = new dataAccess.entities.contentTypeDefinition();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentTypeDefinitionControllerGeneric.prototype.getByFolder = function (folderId, onSuccess, onError) {
                    folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByFolder', [folderId]);
                    options.responseData = new dataAccess.entities.contentTypeDefinition();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentTypeDefinitionControllerGeneric.prototype.contentTypeDefinitionsByFolder = function (folderId, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('ContentTypeDefinitionsByFolder', [folderId]);
                    options.responseData = new dataAccess.entities.contentTypeDefinition();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentTypeDefinitionControllerGeneric.prototype.save = function (contentTypeDefinition, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.contentTypeDefinition();
                    options.requestData = contentTypeDefinition;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                contentTypeDefinitionControllerGeneric.prototype.del = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [id]);
                    options.responseData = new dataAccess.entities.contentTypeDefinition();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                contentTypeDefinitionControllerGeneric.prototype.paginationGetAll = function (data, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('PaginationGetAll', data);
                    options.responseData = new dataAccess.entities.paginationEntity(dataAccess.entities.contentTypeDefinition);
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentTypeDefinitionControllerGeneric.prototype.getAllCount = function (countData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetAllCount', countData);
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData.Value);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                return contentTypeDefinitionControllerGeneric;
            }(controllers.base.BaseController));
            controllers.contentTypeDefinitionControllerGeneric = contentTypeDefinitionControllerGeneric;
            var contentTypeDefinitionController = (function (_super) {
                __extends(contentTypeDefinitionController, _super);
                function contentTypeDefinitionController() {
                    return _super !== null && _super.apply(this, arguments) || this;
                }
                return contentTypeDefinitionController;
            }(contentTypeDefinitionControllerGeneric));
            controllers.contentTypeDefinitionController = contentTypeDefinitionController;
            var contentTypeDefinitionControllerValue = (function (_super) {
                __extends(contentTypeDefinitionControllerValue, _super);
                function contentTypeDefinitionControllerValue() {
                    return _super !== null && _super.apply(this, arguments) || this;
                }
                return contentTypeDefinitionControllerValue;
            }(contentTypeDefinitionControllerGeneric));
            controllers.contentTypeDefinitionControllerValue = contentTypeDefinitionControllerValue;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var contentTypeDefinitionFieldController = (function (_super) {
                __extends(contentTypeDefinitionFieldController, _super);
                function contentTypeDefinitionFieldController() {
                    return _super.call(this, 'ContentTypeDefinitionField/') || this;
                }
                contentTypeDefinitionFieldController.prototype.getById = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetById', [id]);
                    options.responseData = new dataAccess.entities.contentTypeDefinitionField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentTypeDefinitionFieldController.prototype.getByContentTypeDefinition = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByContentTypeDefinition', [id]);
                    options.responseData = new dataAccess.entities.contentTypeDefinitionField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentTypeDefinitionFieldController.prototype.save = function (field, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.contentTypeDefinitionField();
                    options.requestData = field;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                contentTypeDefinitionFieldController.prototype.del = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [id]);
                    options.responseData = new dataAccess.entities.contentTypeDefinitionField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                return contentTypeDefinitionFieldController;
            }(controllers.base.BaseController));
            controllers.contentTypeDefinitionFieldController = contentTypeDefinitionFieldController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var contentTypeDefinitionFieldValueController = (function (_super) {
                __extends(contentTypeDefinitionFieldValueController, _super);
                function contentTypeDefinitionFieldValueController() {
                    return _super.call(this, 'ContentTypeDefinitionFieldValue/') || this;
                }
                contentTypeDefinitionFieldValueController.prototype.getByContent = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByContent', [id]);
                    options.responseData = new dataAccess.entities.contentTypeDefinitionFieldValue();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentTypeDefinitionFieldValueController.prototype.getByContentId = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByContentId', [id]);
                    options.responseData = new dataAccess.entities.contentTypeDefinitionFieldValue();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentTypeDefinitionFieldValueController.prototype.getByValue = function (value, contentTypeDefinitionId, contentTypeDefinitionFieldId, comparer, transform, onSuccess, onError) {
                    if (contentTypeDefinitionId === void 0) { contentTypeDefinitionId = 0; }
                    if (contentTypeDefinitionFieldId === void 0) { contentTypeDefinitionFieldId = 0; }
                    if (comparer === void 0) { comparer = mdBusinessLogic.helpers.data.comparerTypeEnum.equals; }
                    if (transform === void 0) { transform = mdBusinessLogic.helpers.data.dataTransformEnum.toString; }
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByValue', [
                        value,
                        contentTypeDefinitionId,
                        contentTypeDefinitionFieldId,
                        comparer,
                        transform
                    ]);
                    options.responseData = new dataAccess.entities.contentTypeDefinitionFieldValue();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentTypeDefinitionFieldValueController.prototype.save = function (fieldValue, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.contentTypeDefinitionFieldValue();
                    options.requestData = fieldValue;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                return contentTypeDefinitionFieldValueController;
            }(controllers.base.BaseController));
            controllers.contentTypeDefinitionFieldValueController = contentTypeDefinitionFieldValueController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var contentTypeDefinitionFolder = (function (_super) {
                __extends(contentTypeDefinitionFolder, _super);
                function contentTypeDefinitionFolder(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.FolderId = 0;
                    _this.ContentTypeDefinitionId = 0;
                    _this.Title = '';
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                contentTypeDefinitionFolder.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.FolderId = this.getValue(data, "FolderId", 0);
                    this.ContentTypeDefinitionId = this.getValue(data, "ContentTypeDefinitionId", 0);
                    this.Title = this.getValue(data, "Title", '');
                };
                contentTypeDefinitionFolder.prototype.clone = function () {
                    return new contentTypeDefinitionFolder(this);
                };
                return contentTypeDefinitionFolder;
            }(entities.base.BaseEntity));
            entities.contentTypeDefinitionFolder = contentTypeDefinitionFolder;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var contentTypeDefinitionFolderController = (function (_super) {
                __extends(contentTypeDefinitionFolderController, _super);
                function contentTypeDefinitionFolderController() {
                    return _super.call(this, 'ContentTypeDefinitionFolder/') || this;
                }
                contentTypeDefinitionFolderController.prototype.save = function (folder, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.contentTypeDefinitionFolder();
                    options.requestData = folder;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                contentTypeDefinitionFolderController.prototype.del = function (folder, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete');
                    options.responseData = new dataAccess.entities.contentTypeDefinitionFolder();
                    options.requestData = folder;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                contentTypeDefinitionFolderController.prototype.getByFolder = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('Delete', [id]);
                    options.responseData = new dataAccess.entities.contentTypeDefinitionFolder();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                return contentTypeDefinitionFolderController;
            }(controllers.base.BaseController));
            controllers.contentTypeDefinitionFolderController = contentTypeDefinitionFolderController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var contentTypeDefinitionFolderDataBoundConditionController = (function (_super) {
                __extends(contentTypeDefinitionFolderDataBoundConditionController, _super);
                function contentTypeDefinitionFolderDataBoundConditionController() {
                    return _super.call(this, 'ContentTypeDefinitionFolderDataBoundCondition/') || this;
                }
                contentTypeDefinitionFolderDataBoundConditionController.prototype.getByFolderAndContentTypeDefinitionId = function (folderId, contentTypeDefinitionId, onSuccess, onError) {
                    folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByFolderAndContentTypeDefinitionId', [folderId, contentTypeDefinitionId]);
                    options.responseData = new dataAccess.entities.contentTypeDefinitionFolderDataBoundCondition();
                    options.responseDataArray = new Array();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentTypeDefinitionFolderDataBoundConditionController.prototype.save = function (contentTypeDefinitionFolderDataBoundCondition, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.contentTypeDefinitionFolderDataBoundCondition();
                    options.requestData = contentTypeDefinitionFolderDataBoundCondition;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                contentTypeDefinitionFolderDataBoundConditionController.prototype.saveAll = function (contentTypeDefinitionFolderDataBoundConditions, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.contentType = new controllers.base.AjaxMethodHeader('Content-Type', 'application/json; charset=UTF-8');
                    options.address = this.getAddress('SaveAll');
                    options.responseDataArray = new Array();
                    options.requestData = contentTypeDefinitionFolderDataBoundConditions;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                contentTypeDefinitionFolderDataBoundConditionController.prototype.del = function (obj, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [obj.FolderId, obj.ContentTypeDefinitionId, obj.ContentTypeDefinitionFieldId]);
                    options.responseData = new dataAccess.entities.contentTypeDefinitionFolderDataBoundCondition();
                    options.onSuccess = function (options) {
                        onSuccess();
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                contentTypeDefinitionFolderDataBoundConditionController.prototype.deleteAll = function (folderId, contentTypeDefinitionId, onSuccess, onError) {
                    folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('DeleteAll', [folderId, contentTypeDefinitionId]);
                    options.responseData = new dataAccess.entities.contentTypeDefinitionFolderDataBoundCondition();
                    options.onSuccess = function (options) {
                        onSuccess();
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                return contentTypeDefinitionFolderDataBoundConditionController;
            }(controllers.base.BaseController));
            controllers.contentTypeDefinitionFolderDataBoundConditionController = contentTypeDefinitionFolderDataBoundConditionController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var contentTypeDefinitionFolderDataBoundSyncController = (function (_super) {
                __extends(contentTypeDefinitionFolderDataBoundSyncController, _super);
                function contentTypeDefinitionFolderDataBoundSyncController() {
                    return _super.call(this, 'contentTypeDefinitionFolderDataBoundSync/') || this;
                }
                contentTypeDefinitionFolderDataBoundSyncController.prototype.getByFolderAndContentTypeDefinitionId = function (folderId, contentTypeDefinitionId, onSuccess, onError) {
                    folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);
                    contentTypeDefinitionId = mdBusinessLogic.helpers.typeConversion.toInt(contentTypeDefinitionId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByFolderAndContentTypeDefinitionId', [folderId, contentTypeDefinitionId]);
                    options.responseData = new dataAccess.entities.contentTypeDefinitionFolderDataBoundSync();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                contentTypeDefinitionFolderDataBoundSyncController.prototype.save = function (contentTypeDefinitionFolderDataBoundSync, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.contentTypeDefinitionFolderDataBoundSync();
                    options.requestData = contentTypeDefinitionFolderDataBoundSync;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                contentTypeDefinitionFolderDataBoundSyncController.prototype.del = function (obj, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [obj.FolderId, obj.ContentTypeDefinitionId]);
                    options.responseData = new dataAccess.entities.contentTypeDefinitionFolderDataBoundSync();
                    options.onSuccess = function (options) {
                        onSuccess();
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                return contentTypeDefinitionFolderDataBoundSyncController;
            }(controllers.base.BaseController));
            controllers.contentTypeDefinitionFolderDataBoundSyncController = contentTypeDefinitionFolderDataBoundSyncController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var cultureController = (function (_super) {
                __extends(cultureController, _super);
                function cultureController() {
                    return _super.call(this, 'Culture/') || this;
                }
                cultureController.prototype.selectCulture = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('SelectCulture');
                    options.responseData = new dataAccess.entities.culture();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                cultureController.prototype.getByLCID = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByLCID', [id]);
                    options.responseData = new dataAccess.entities.culture();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                cultureController.prototype.getAll = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAll');
                    options.responseData = new dataAccess.entities.culture();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                cultureController.prototype.getApproved = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetApproved');
                    options.responseData = new dataAccess.entities.culture();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                cultureController.prototype.getAllForContentId = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAllForContentId', [id]);
                    options.responseData = new dataAccess.entities.culture();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                cultureController.prototype.save = function (culture, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.culture();
                    options.requestData = culture;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                cultureController.prototype.del = function (culture, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete');
                    options.responseData = new dataAccess.entities.culture();
                    options.requestData = culture;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                return cultureController;
            }(controllers.base.BaseController));
            controllers.cultureController = cultureController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var fileTypeEnum;
            (function (fileTypeEnum) {
                fileTypeEnum[fileTypeEnum["image"] = 1] = "image";
                fileTypeEnum[fileTypeEnum["video"] = 2] = "video";
                fileTypeEnum[fileTypeEnum["audio"] = 3] = "audio";
                fileTypeEnum[fileTypeEnum["application"] = 4] = "application";
                fileTypeEnum[fileTypeEnum["text"] = 5] = "text";
            })(fileTypeEnum = entities.fileTypeEnum || (entities.fileTypeEnum = {}));
            var file = (function (_super) {
                __extends(file, _super);
                function file(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.path = '';
                    _this.fileType = null;
                    _this.data = null;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                file.prototype.construct = function (data) {
                    this.path = this.getValue(data, "path", '');
                    this.fileType = this.getValue(data, "fileType", null);
                    this.data = this.getValue(data, "data", null);
                };
                file.prototype.clone = function () {
                    return new file(this);
                };
                file.prototype.getFileType = function () {
                    switch (this.fileType) {
                        case fileTypeEnum.video:
                            return 'video';
                        case fileTypeEnum.application:
                            return 'application';
                        case fileTypeEnum.audio:
                            return 'audio';
                        case fileTypeEnum.image:
                            return 'image';
                        default:
                            return 'text';
                    }
                };
                return file;
            }(entities.base.BaseEntity));
            entities.file = file;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var fileController = (function (_super) {
                __extends(fileController, _super);
                function fileController() {
                    return _super.call(this, 'Upload/') || this;
                }
                fileController.prototype.upload = function (file, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('PostFormData');
                    var formData = new FormData();
                    formData.append('file', file.data);
                    formData.append('path', file.path);
                    formData.append('fileType', file.fileType.toString());
                    options.isFormData = true;
                    options.requestData = formData;
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData.Value);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                return fileController;
            }(controllers.base.BaseController));
            controllers.fileController = fileController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var mediaContentMetaDataFeldValues = (function (_super) {
                __extends(mediaContentMetaDataFeldValues, _super);
                function mediaContentMetaDataFeldValues(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.MediaContentId = 0;
                    _this.DateCreated = new Date();
                    _this.Value = '';
                    _this.MetaDataFieldId = 0;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                mediaContentMetaDataFeldValues.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.MediaContentId = this.getValue(data, "MediaContentId", 0);
                    this.DateCreated = this.getValue(data, "DateCreated", new Date());
                    this.Value = this.getValue(data, "Value", '');
                    this.MetaDataFieldId = this.getValue(data, "MetaDataFieldId", 0);
                };
                mediaContentMetaDataFeldValues.prototype.clone = function () {
                    return new mediaContentMetaDataFeldValues(this);
                };
                return mediaContentMetaDataFeldValues;
            }(entities.metaDataField));
            entities.mediaContentMetaDataFeldValues = mediaContentMetaDataFeldValues;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var mediaContent = (function (_super) {
                __extends(mediaContent, _super);
                function mediaContent(obj) {
                    var _this = _super.call(this, obj) || this;
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
                    _this.UniqueId = "";
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
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
                    this.UniqueId = this.getValue(data, 'UniqueId', '');
                };
                mediaContent.prototype.clone = function () {
                    return new mediaContent(this);
                };
                return mediaContent;
            }(entities.base.BaseEntity));
            entities.mediaContent = mediaContent;
            var mediaContentInputType;
            (function (mediaContentInputType) {
                mediaContentInputType[mediaContentInputType["jpg"] = 1] = "jpg";
                mediaContentInputType[mediaContentInputType["txt"] = 2] = "txt";
                mediaContentInputType[mediaContentInputType["mp4"] = 3] = "mp4";
                mediaContentInputType[mediaContentInputType["JPG"] = 4] = "JPG";
                mediaContentInputType[mediaContentInputType["png"] = 5] = "png";
                mediaContentInputType[mediaContentInputType["PNG"] = 6] = "PNG";
                mediaContentInputType[mediaContentInputType["flv"] = 7] = "flv";
                mediaContentInputType[mediaContentInputType["mkv"] = 8] = "mkv";
                mediaContentInputType[mediaContentInputType["jpeg"] = 9] = "jpeg";
                mediaContentInputType[mediaContentInputType["JPEG"] = 10] = "JPEG";
                mediaContentInputType[mediaContentInputType["pdf"] = 11] = "pdf";
                mediaContentInputType[mediaContentInputType["docx"] = 12] = "docx";
                mediaContentInputType[mediaContentInputType["xls"] = 13] = "xls";
                mediaContentInputType[mediaContentInputType["xlsx"] = 14] = "xlsx";
            })(mediaContentInputType = entities.mediaContentInputType || (entities.mediaContentInputType = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var profileTypeFieldJsonField = (function () {
                function profileTypeFieldJsonField(obj) {
                    this.validation = new entities.fieldValidation();
                    this.helpText = '';
                    this.access = '';
                    this.cssClass = '';
                    this.toggle = '';
                    this.hidden = false;
                    this.enabled = true;
                    if (obj != undefined && obj != null) {
                        this.construct(obj);
                    }
                }
                profileTypeFieldJsonField.prototype.construct = function (data) {
                    this.validation = mdBusinessLogic.helpers.entityHelper.getConstructValue(data, 'validation', new entities.fieldValidation());
                    this.helpText = mdBusinessLogic.helpers.entityHelper.getValue(data, 'helpText', '');
                    this.access = mdBusinessLogic.helpers.entityHelper.getValue(data, 'access', '');
                    this.cssClass = mdBusinessLogic.helpers.entityHelper.getValue(data, 'cssClass', '');
                    this.toggle = mdBusinessLogic.helpers.entityHelper.getValue(data, 'toggle', '');
                    this.hidden = mdBusinessLogic.helpers.entityHelper.getValue(data, 'hidden', false);
                    this.enabled = mdBusinessLogic.helpers.entityHelper.getValue(data, 'enabled', true);
                };
                profileTypeFieldJsonField.prototype.clone = function () {
                    return new profileTypeFieldJsonField(this);
                };
                return profileTypeFieldJsonField;
            }());
            entities.profileTypeFieldJsonField = profileTypeFieldJsonField;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var profileTypeField = (function (_super) {
                __extends(profileTypeField, _super);
                function profileTypeField(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.ProfileTypeId = 0;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                profileTypeField.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ProfileTypeId = this.getValue(data, "ProfileTypeId", 0);
                };
                profileTypeField.prototype.clone = function () {
                    return new profileTypeField(this);
                };
                return profileTypeField;
            }(entities.genericContent.genericContentField));
            entities.profileTypeField = profileTypeField;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var profileTypeFieldValue = (function (_super) {
                __extends(profileTypeFieldValue, _super);
                function profileTypeFieldValue(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.ProfileTypeFieldId = 0;
                    _this.ProfileTypeId = 0;
                    _this.UserId = 0;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                profileTypeFieldValue.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ProfileTypeFieldId = this.getValue(data, "ProfileTypeFieldId", 0);
                    this.ProfileTypeId = this.getValue(data, "ProfileTypeId", 0);
                    this.UserId = this.getValue(data, "UserId", 0);
                };
                profileTypeFieldValue.prototype.clone = function () {
                    return new profileTypeFieldValue(this);
                };
                return profileTypeFieldValue;
            }(entities.genericContent.genericContentFieldValue));
            entities.profileTypeFieldValue = profileTypeFieldValue;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var rwdPermission = (function (_super) {
                __extends(rwdPermission, _super);
                function rwdPermission(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Read = false;
                    _this.Write = false;
                    _this.Delete = false;
                    _this.Target = rwdPermissionTargetEnum.None;
                    _this.TargetPrimaryKey = '';
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                rwdPermission.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Read = this.getValue(data, "Read", false);
                    this.Write = this.getValue(data, "Write", false);
                    this.Delete = this.getValue(data, "Delete", false);
                    this.Target = this.getValue(data, "Target", rwdPermissionTargetEnum.None);
                    this.TargetPrimaryKey = this.getValue(data, "TargetPrimaryKey", '');
                };
                rwdPermission.prototype.clone = function () {
                    return new rwdPermission(this);
                };
                return rwdPermission;
            }(entities.base.BaseEntity));
            entities.rwdPermission = rwdPermission;
            var rwdPermissionTargetEnum;
            (function (rwdPermissionTargetEnum) {
                rwdPermissionTargetEnum[rwdPermissionTargetEnum["None"] = 0] = "None";
                rwdPermissionTargetEnum[rwdPermissionTargetEnum["Folder"] = 1] = "Folder";
                rwdPermissionTargetEnum[rwdPermissionTargetEnum["Content"] = 2] = "Content";
                rwdPermissionTargetEnum[rwdPermissionTargetEnum["MediaContent"] = 3] = "MediaContent";
            })(rwdPermissionTargetEnum = entities.rwdPermissionTargetEnum || (entities.rwdPermissionTargetEnum = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var profileType = (function (_super) {
                __extends(profileType, _super);
                function profileType(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Name = '';
                    _this.Icon = '';
                    _this.Description = '';
                    _this.PermissionXmlText = '';
                    _this.Fields = new Array();
                    _this.IsAssigned = false;
                    _this.RWDPermissions = new Array();
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                profileType.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Name = this.getValue(data, "Name", '');
                    this.Icon = this.getValue(data, "Icon", '');
                    this.Description = this.getValue(data, "Description", '');
                    this.PermissionXmlText = this.getValue(data, "PermissionXmlText", '');
                    this.Fields = this.getArrayConstructEntityValue(data, "Fields", new Array(), new entities.profileTypeFieldValue());
                    this.IsAssigned = this.getValue(data, "IsAssigned", false);
                    this.RWDPermissions = this.getArrayConstructEntityValue(data, "RWDPermissions", new Array(), new entities.rwdPermission());
                };
                profileType.prototype.clone = function () {
                    return new profileType(this);
                };
                profileType.prototype.setFieldValue = function (value, fieldName) {
                    if (this.Fields != null) {
                        for (var i in this.Fields) {
                            if (this.Fields[i].Name == fieldName) {
                                this.Fields[i]['Value'] = value;
                                break;
                            }
                        }
                    }
                };
                profileType.prototype.getFieldValue = function (fieldName) {
                    if (this.Fields != null) {
                        for (var i in this.Fields) {
                            if (this.Fields[i].Name == fieldName && this.Fields[i]['Value'] !== undefined) {
                                return this.Fields[i]['Value'];
                            }
                        }
                    }
                    return null;
                };
                profileType.prototype.getField = function (fieldName) {
                    if (this.Fields != null) {
                        for (var i in this.Fields) {
                            if (this.Fields[i].Name == fieldName) {
                                return this.Fields[i];
                            }
                        }
                    }
                    return null;
                };
                return profileType;
            }(entities.base.BaseEntity));
            entities.profileType = profileType;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var folderMetaDataField = (function (_super) {
                __extends(folderMetaDataField, _super);
                function folderMetaDataField(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.FolderId = 0;
                    _this.MetaDataFieldId = 0;
                    _this.IsRequired = false;
                    _this.Checked = false;
                    _this.Name = '';
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                folderMetaDataField.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.FolderId = this.getValue(data, "FolderId", 0);
                    this.MetaDataFieldId = this.getValue(data, "MetaDataFieldId", 0);
                    this.IsRequired = this.getValue(data, "IsRequired", false);
                    this.Checked = this.getValue(data, "Checked", false);
                    this.Name = this.getValue(data, "Name", '');
                };
                folderMetaDataField.prototype.clone = function () {
                    return new folderMetaDataField(this);
                };
                return folderMetaDataField;
            }(entities.base.BaseEntity));
            entities.folderMetaDataField = folderMetaDataField;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var folder = (function (_super) {
                __extends(folder, _super);
                function folder(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.ParentId = 0;
                    _this.Name = '';
                    _this.Description = '';
                    _this.Parent = null;
                    _this.Children = new Array();
                    _this.Contents = new Array();
                    _this.FolderPath = '';
                    _this.MetaDataFields = new Array();
                    _this.MediaContent = new Array();
                    _this.ProfileTypePermissions = new Array();
                    _this.NotAuthorizedUsers = new Array();
                    _this.FolderMediaContentMetaDataField = new Array();
                    _this.ContentTypeDefinitionFolder = new Array();
                    _this.ContentTypeDefinitions = new Array();
                    _this.ContentTypeDefinitionId = 0;
                    _this.Templates = new Array();
                    _this.Inherit = true;
                    _this.IsNew = true;
                    _this.ParentArray = new Array();
                    _this.ChildrenTotalCount = 0;
                    _this.ContentsTotalCount = 0;
                    _this.MediaContentTotalCount = 0;
                    _this.IsHidden = false;
                    if (obj !== undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                folder.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ParentId = this.getValue(data, "ParentId", 0);
                    this.Name = this.getValue(data, "Name", '');
                    this.Description = this.getValue(data, "Description", '');
                    this.Parent = this.getConstructEntityValue(data, "Parent", new folder());
                    this.Children = this.getArrayConstructEntityValue(data, "Children", new Array(), new folder());
                    this.Contents = this.getArrayConstructEntityValue(data, "Contents", new Array(), new entities.content());
                    this.FolderPath = this.getValue(data, "FolderPath", '');
                    this.MetaDataFields = this.getArrayConstructEntityValue(data, "MetaDataFields", new Array(), new entities.folderMetaDataField());
                    this.MediaContent = this.getArrayConstructEntityValue(data, "MediaContent", new Array(), new entities.mediaContent());
                    this.ProfileTypePermissions = this.getArrayConstructEntityValue(data, "ProfileTypePermissions", new Array(), new entities.profileType());
                    this.NotAuthorizedUsers = this.getArrayConstructEntityValue(data, "NotAuthorizedUsers", new Array(), new entities.user());
                    this.FolderMediaContentMetaDataField = this.getArrayConstructEntityValue(data, "FolderMediaContentMetaDataField", new Array(), new entities.folderMediaContentMetaDataField());
                    this.ContentTypeDefinitionFolder = this.getArrayConstructEntityValue(data, "ContentTypeDefinitionFolder", new Array(), new entities.contentTypeDefinitionFolder());
                    this.ContentTypeDefinitions = this.getArrayConstructEntityValue(data, "ContentTypeDefinitions", new Array(), new entities.contentTypeDefinition());
                    this.ContentTypeDefinitionId = this.getValue(data, "ContentTypeDefinitionId", 0);
                    this.Templates = this.getArrayConstructEntityValue(data, "Templates", new Array(), new entities.template());
                    this.Inherit = this.getValue(data, "Inherit", true);
                    this.IsNew = this.getValue(data, "IsNew", true);
                    this.ParentArray = mdBusinessLogic.helpers.loadParentArray(this, 'Name', 'FolderPath');
                    this.ChildrenTotalCount = this.getValue(data, "ChildrenTotalCount", 0);
                    this.ContentsTotalCount = this.getValue(data, "ContentsTotalCount", 0);
                    this.MediaContentTotalCount = this.getValue(data, "MediaContentTotalCount", 0);
                    this.IsHidden = this.getValue(data, "IsHidden", false);
                };
                folder.prototype.clone = function () {
                    return new folder(this);
                };
                return folder;
            }(entities.base.BaseEntity));
            entities.folder = folder;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var folderController = (function (_super) {
                __extends(folderController, _super);
                function folderController() {
                    return _super.call(this, 'Folder/') || this;
                }
                folderController.prototype.get = function (opts, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('');
                    options.requestData = opts;
                    options.responseData = new dataAccess.entities.paginationEntity(dataAccess.entities.folder);
                    options.lcid = opts.Lcid;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                folderController.prototype.getByFolderPath = function (path, loadContents, onSuccess, onError) {
                    this.get({
                        Paths: [path],
                        FillContents: loadContents,
                        MaxNumberOfRows: 1
                    }, function (result) {
                        onSuccess(result.Items[0]);
                    }, function (error) {
                        onError(error);
                    });
                };
                folderController.prototype.search = function (searchTerm, parentId, recursive, onSuccess, onError) {
                    parentId = mdBusinessLogic.helpers.typeConversion.toInt(parentId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('Search', [searchTerm, parentId, recursive]);
                    options.responseData = new dataAccess.entities.folder();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                folderController.prototype.paginationGetByFolderPath = function (paginationData, onSuccess, onError) {
                    this.get({
                        Paths: [paginationData.path],
                        FillContents: paginationData.fillContents,
                        MaxNumberOfRows: 1,
                        CurrentPageIndex: 0,
                        SearchTerm: paginationData.searchTerm,
                        ContentRequestOptions: paginationData.fillContents ? ({
                            LoadAuthor: true,
                            MaxNumberOfRows: paginationData.pageSize,
                            CurrentPageIndex: paginationData.pageIndex,
                            FillFields: false,
                            FillMetaData: false
                        }) : null,
                        FillChildren: true,
                        ChildFolderRequestOptions: {
                            FillContents: false,
                            MaxNumberOfRows: paginationData.pageSize,
                            CurrentPageIndex: paginationData.pageIndex
                        }
                    }, function (result) {
                        onSuccess(result.Items[0]);
                    }, onError);
                };
                folderController.prototype.paginationGetByParentId = function (paginationData, onSuccess, onError) {
                    this.get({
                        ParentId: paginationData.parentId,
                        FillContents: paginationData.fillContents,
                        MaxNumberOfRows: paginationData.pageSize,
                        CurrentPageIndex: paginationData.pageIndex,
                        SearchTerm: paginationData.searchTerm,
                        ContentRequestOptions: paginationData.fillContents ? ({
                            LoadAuthor: true,
                            MaxNumberOfRows: paginationData.pageSize,
                            CurrentPageIndex: paginationData.pageIndex,
                            FillFields: false,
                            FillMetaData: false
                        }) : null,
                        FillChildren: true,
                        ChildFolderRequestOptions: {
                            FillContents: false,
                            MaxNumberOfRows: paginationData.pageSize,
                            CurrentPageIndex: paginationData.pageIndex
                        }
                    }, onSuccess, onError);
                };
                folderController.prototype.getByParentId = function (parentId, onSuccess, onError) {
                    parentId = mdBusinessLogic.helpers.typeConversion.toInt(parentId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByParentId', [parentId]);
                    options.responseData = new dataAccess.entities.folder();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                folderController.prototype.getHierarchyByParentId = function (parentId, depth, onSuccess, onError) {
                    parentId = mdBusinessLogic.helpers.typeConversion.toInt(parentId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    var params = new Array();
                    params.push(parentId);
                    if (typeof (depth) !== "boolean") {
                        params.push(depth);
                    }
                    options.address = this.getAddress('GetHierarchyByParentId', params);
                    options.responseData = new dataAccess.entities.folder();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                folderController.prototype.getByParentIdCount = function (countData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByParentIdCount', countData);
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData.Value);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                folderController.prototype.getById = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetById', [id]);
                    options.responseData = new dataAccess.entities.folder();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                folderController.prototype.save = function (folder, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.folder();
                    options.requestData = folder;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                folderController.prototype.del = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [id]);
                    options.responseData = new dataAccess.entities.folder();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                folderController.prototype.getByRequest = function (request, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByRequest');
                    options.requestData = request;
                    options.responseData = new dataAccess.entities.folder();
                    options.isJsonArray = true;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                return folderController;
            }(controllers.base.BaseController));
            controllers.folderController = folderController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var folderMediaContentMetaDataField = (function (_super) {
                __extends(folderMediaContentMetaDataField, _super);
                function folderMediaContentMetaDataField(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.FolderId = 0;
                    _this.MetaDataFieldId = 0;
                    _this.IsRequired = false;
                    _this.Checked = false;
                    _this.Name = '';
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                folderMediaContentMetaDataField.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.FolderId = this.getValue(data, "FolderId", 0);
                    this.MetaDataFieldId = this.getValue(data, "MetaDataFieldId", 0);
                    this.IsRequired = this.getValue(data, "IsRequired", false);
                    this.Checked = this.getValue(data, "Checked", false);
                    this.Name = this.getValue(data, "Name", '');
                };
                folderMediaContentMetaDataField.prototype.clone = function () {
                    return new folderMediaContentMetaDataField(this);
                };
                return folderMediaContentMetaDataField;
            }(entities.base.BaseEntity));
            entities.folderMediaContentMetaDataField = folderMediaContentMetaDataField;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var folderMediaContentMetaDataFieldController = (function (_super) {
                __extends(folderMediaContentMetaDataFieldController, _super);
                function folderMediaContentMetaDataFieldController() {
                    return _super.call(this, 'FolderMediaContentMetaDataField/') || this;
                }
                folderMediaContentMetaDataFieldController.prototype.getByIds = function (folderId, metaDataFieldId, onSuccess, onError) {
                    folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByIds', [folderId, metaDataFieldId]);
                    options.responseData = new dataAccess.entities.folderMediaContentMetaDataField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                folderMediaContentMetaDataFieldController.prototype.getUsed = function (folderId, onSuccess, onError) {
                    folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetUsedFolderMediaContentMetaDataField', [folderId]);
                    options.responseData = new dataAccess.entities.folderMediaContentMetaDataField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                folderMediaContentMetaDataFieldController.prototype.getAll = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAll');
                    options.responseData = new dataAccess.entities.folderMediaContentMetaDataField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                folderMediaContentMetaDataFieldController.prototype.getByFolderId = function (folderId, onSuccess, onError) {
                    folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByFolderId', [folderId]);
                    options.responseData = new dataAccess.entities.folderMediaContentMetaDataField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                folderMediaContentMetaDataFieldController.prototype.getMediaContentMetaDataFieldByFolder = function (folderId, onSuccess, onError) {
                    folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetMediaContentMetaDataFieldByFolder', [folderId]);
                    options.responseData = new dataAccess.entities.folderMediaContentMetaDataField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                return folderMediaContentMetaDataFieldController;
            }(controllers.base.BaseController));
            controllers.folderMediaContentMetaDataFieldController = folderMediaContentMetaDataFieldController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var folderMetaDataFieldController = (function (_super) {
                __extends(folderMetaDataFieldController, _super);
                function folderMetaDataFieldController() {
                    return _super.call(this, 'FolderMetaDataField/') || this;
                }
                folderMetaDataFieldController.prototype.getByIds = function (folderId, metaDataFieldId, onSuccess, onError) {
                    folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetById', [folderId, metaDataFieldId]);
                    options.responseData = new dataAccess.entities.folderMetaDataField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                folderMetaDataFieldController.prototype.getUsed = function (folderId, onSuccess, onError) {
                    folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetUsedFolderMetaDataField', [folderId]);
                    options.responseData = new dataAccess.entities.folderMetaDataField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                folderMetaDataFieldController.prototype.getAll = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAll');
                    options.responseData = new dataAccess.entities.folderMetaDataField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                return folderMetaDataFieldController;
            }(controllers.base.BaseController));
            controllers.folderMetaDataFieldController = folderMetaDataFieldController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var mediaContentController = (function (_super) {
                __extends(mediaContentController, _super);
                function mediaContentController() {
                    return _super.call(this, 'MediaContent/') || this;
                }
                mediaContentController.prototype.getById = function (id, lcid, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    lcid = mdBusinessLogic.helpers.typeConversion.toInt(lcid);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.lcid = lcid;
                    options.address = this.getAddress('GetById', [id]);
                    options.responseData = new dataAccess.entities.mediaContent();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                mediaContentController.prototype.getByIdWithMetaData = function (id, lcid, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    lcid = mdBusinessLogic.helpers.typeConversion.toInt(lcid);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.lcid = lcid;
                    options.address = this.getAddress('GetByIdWithMetaData', [id]);
                    options.responseData = new dataAccess.entities.mediaContent();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                mediaContentController.prototype.getByFolderId = function (id, lcid, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    lcid = mdBusinessLogic.helpers.typeConversion.toInt(lcid);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.lcid = lcid;
                    options.address = this.getAddress('GetByFolderId', [id, lcid]);
                    options.responseData = new dataAccess.entities.mediaContent();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                mediaContentController.prototype.getByFileType = function (id, lcid, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    lcid = mdBusinessLogic.helpers.typeConversion.toInt(lcid);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.lcid = lcid;
                    options.address = this.getAddress('GetByFileType', [id, lcid]);
                    options.responseData = new dataAccess.entities.mediaContent();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                mediaContentController.prototype.search = function (searchData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('Search', searchData);
                    options.responseData = new dataAccess.entities.mediaContent();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                mediaContentController.prototype.searchByFileType = function (searchText, fileType, lcid, onSuccess, onError) {
                    lcid = mdBusinessLogic.helpers.typeConversion.toInt(lcid);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('SearchByFileType', [searchText, fileType, lcid]);
                    options.responseData = new dataAccess.entities.mediaContent();
                    options.isJsonArray = true;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                mediaContentController.prototype.del = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [id]);
                    options.responseData = new dataAccess.entities.mediaContent();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                mediaContentController.prototype.getAll = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAll');
                    options.responseData = new dataAccess.entities.mediaContent();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                mediaContentController.prototype.paginationGetByFolderId = function (paginationData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetWithPaginationByFolderId', paginationData);
                    options.responseData = new dataAccess.entities.paginationEntity(dataAccess.entities.mediaContent);
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                mediaContentController.prototype.getByFolderIdCount = function (countData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByFolderIdCount', countData);
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData.Value);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                mediaContentController.prototype.SavePermissions = function (mediaContent, onSuccess, onError) {
                    this.save(mediaContent, onSuccess, onError);
                };
                mediaContentController.prototype.save = function (mediaContent, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.mediaContent();
                    options.requestData = mediaContent;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                return mediaContentController;
            }(controllers.base.BaseController));
            controllers.mediaContentController = mediaContentController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var mediaContentMetaDataFeldValuesController = (function (_super) {
                __extends(mediaContentMetaDataFeldValuesController, _super);
                function mediaContentMetaDataFeldValuesController() {
                    return _super.call(this, 'MediaContentMetaDataFieldValues/') || this;
                }
                mediaContentMetaDataFeldValuesController.prototype.getByMediaContentId = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByMediaContentId', [id]);
                    options.responseData = new dataAccess.entities.mediaContentMetaDataFeldValues();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                return mediaContentMetaDataFeldValuesController;
            }(controllers.base.BaseController));
            controllers.mediaContentMetaDataFeldValuesController = mediaContentMetaDataFeldValuesController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var menuContent = (function (_super) {
                __extends(menuContent, _super);
                function menuContent(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.LCID = 0;
                    _this.DateCreated = new Date();
                    _this.MenuId = 0;
                    _this.Title = '';
                    _this.MenuContentPath = '';
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                menuContent.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.LCID = this.getValue(data, "LCID", 0);
                    this.DateCreated = this.getValue(data, "DateCreated", new Date());
                    this.MenuId = this.getValue(data, "MenuId", 0);
                    this.Title = this.getValue(data, "Title", '');
                    this.MenuContentPath = this.getValue(data, "MenuContentPath", '');
                };
                menuContent.prototype.clone = function () {
                    return new menuContent(this);
                };
                return menuContent;
            }(entities.base.BaseEntity));
            entities.menuContent = menuContent;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var menuContentController = (function (_super) {
                __extends(menuContentController, _super);
                function menuContentController() {
                    return _super.call(this, 'MenuContent/') || this;
                }
                menuContentController.prototype.getById = function (id, lcid, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    lcid = mdBusinessLogic.helpers.typeConversion.toInt(lcid);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.lcid = lcid;
                    options.address = this.getAddress('GetByMenuId', [id]);
                    options.responseData = new dataAccess.entities.menuContent();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                menuContentController.prototype.del = function (data, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [data.Id, data.MenuId]);
                    options.responseData = new dataAccess.entities.menuContent();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                menuContentController.prototype.save = function (menuContent, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.menuContent();
                    options.requestData = menuContent;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                menuContentController.prototype.update = function (menu, orderStart, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Update', [orderStart]);
                    options.responseData = new dataAccess.entities.menuContent();
                    options.requestData = menu;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                menuContentController.prototype.deletemenu = function (menuContent, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete');
                    options.responseData = new dataAccess.entities.menuContent();
                    options.requestData = menuContent;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                menuContentController.prototype.getByMenuIdCount = function (countData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByMenuIdCount', countData);
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData.Value);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                menuContentController.prototype.paginationGetByMenuId = function (paginationData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('PaginationGetByMenuId', paginationData);
                    options.responseData = new dataAccess.entities.paginationEntity(dataAccess.entities.menuContent);
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                menuContentController.prototype.search = function (searchData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Search', searchData);
                    options.responseData = new dataAccess.entities.menuContent();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                return menuContentController;
            }(controllers.base.BaseController));
            controllers.menuContentController = menuContentController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var menu = (function (_super) {
                __extends(menu, _super);
                function menu(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.ParentId = 0;
                    _this.Name = '';
                    _this.Description = '';
                    _this.Parent = null;
                    _this.Children = [];
                    _this.Items = [];
                    _this.FreeTextField = '';
                    _this.Lcid = 0;
                    _this.FolderId = 0;
                    _this.MenuPath = '';
                    _this.Contents = [];
                    _this.Options = '';
                    _this.ParentArray = new Array();
                    _this.ChildrenTotalCount = 0;
                    _this.ContentsTotalCount = 0;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                menu.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ParentId = this.getValue(data, "ParentId", 0);
                    this.Name = this.getValue(data, "Name", '');
                    this.Description = this.getValue(data, "Description", '');
                    this.Parent = this.getConstructEntityValue(data, "Parent", new menu());
                    this.Children = this.getArrayConstructEntityValue(data, "Children", new Array(), new menu());
                    this.Items = this.getArrayConstructEntityValue(data, "Items", new Array(), new entities.menuContent());
                    this.Contents = this.getArrayConstructEntityValue(data, "Contents", new Array(), new entities.content());
                    this.FreeTextField = this.getValue(data, "FreeTextField", '');
                    this.Lcid = this.getValue(data, "Lcid", 0);
                    this.FolderId = this.getValue(data, "FolderId", 0);
                    this.MenuPath = this.getValue(data, "MenuPath", '');
                    this.Options = this.getValue(data, "Options", '');
                    this.ParentArray = mdBusinessLogic.helpers.loadParentArray(this, "Name", "MenuPath");
                    this.ChildrenTotalCount = this.getValue(this, "ChildrenTotalCount", 0);
                    this.ContentsTotalCount = this.getValue(this, "ContentsTotalCount", 0);
                };
                menu.prototype.clone = function () {
                    return new menu(this);
                };
                return menu;
            }(entities.base.BaseEntity));
            entities.menu = menu;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var menuController = (function (_super) {
                __extends(menuController, _super);
                function menuController() {
                    return _super.call(this, 'Menu/') || this;
                }
                menuController.prototype.getById = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetById', [id]);
                    options.responseData = new dataAccess.entities.menu();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                menuController.prototype.getByParentId = function (id, depth, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByParentId', [id]);
                    options.responseData = new dataAccess.entities.menu();
                    options.headers.push(new controllers.base.AjaxMethodHeader("depth", depth));
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                menuController.prototype.getByParentIdCount = function (countData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByParentIdCount', countData);
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData.Value);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                menuController.prototype.paginationGetMenuByPath = function (paginationData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('PaginationGetMenuByPath', paginationData);
                    options.responseData = new dataAccess.entities.menu();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                menuController.prototype.GetByParentIdWithPagination = function (paginationData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByParentIdWithPagination', paginationData);
                    options.responseData = new dataAccess.entities.paginationEntity(dataAccess.entities.menu);
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                menuController.prototype.getAll = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAll');
                    options.responseData = new dataAccess.entities.menu();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                menuController.prototype.getHierarchyByParentId = function (id, depth, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetHierarchyByParentId', [id]);
                    options.responseData = new dataAccess.entities.menu();
                    options.headers.push(new controllers.base.AjaxMethodHeader("depth", depth));
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                menuController.prototype.save = function (menu, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.menu();
                    options.requestData = menu;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                menuController.prototype.updateChildren = function (menu, orderStart, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('UpdateChildren', [orderStart]);
                    options.responseData = new dataAccess.entities.menu();
                    options.requestData = menu;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                menuController.prototype.del = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [id]);
                    options.responseData = new dataAccess.entities.menu();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                menuController.prototype.delContent = function (id, path, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress(this.getAddress('DeleteContent', [id], false), { ValueName: path });
                    options.responseData = new dataAccess.entities.menu();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                menuController.prototype.assignContentToMenu = function (menuId, contentId, onSuccess, onError) {
                    menuId = mdBusinessLogic.helpers.typeConversion.toInt(menuId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('AssignContentToMenu', [menuId, contentId]);
                    options.responseData = new dataAccess.entities.menu();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                menuController.prototype.getByMenuPath = function (path, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetMenuByPath');
                    options.requestData = { ValueName: path };
                    options.responseData = new dataAccess.entities.menu();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                menuController.prototype.search = function (searchData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('Search', searchData);
                    options.responseData = new dataAccess.entities.menu();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                return menuController;
            }(controllers.base.BaseController));
            controllers.menuController = menuController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var message = (function (_super) {
                __extends(message, _super);
                function message(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Subject = '';
                    _this.MessageContent = '';
                    _this.ParentId = 0;
                    _this.IsRead = false;
                    _this.MessageFolderId = 0;
                    _this.DateAdded = null;
                    _this.Type = 0;
                    _this.FromUserId = 0;
                    _this.ToUserId = 0;
                    _this.FromUser = new entities.user();
                    _this.ToUser = new entities.user();
                    _this.MainThread = 0;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                message.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Subject = this.getValue(data, "Subject", '');
                    this.MessageContent = this.getValue(data, "MessageContent", '');
                    this.ParentId = this.getValue(data, "ParentId", 0);
                    this.IsRead = this.getValue(data, "IsRead", false);
                    this.MessageFolderId = this.getValue(data, "MessageFolderId", 0);
                    this.DateAdded = this.getValue(data, "DateAdded", null);
                    this.Type = this.getValue(data, "Type", 0);
                    this.FromUserId = this.getValue(data, "FromUserId", 0);
                    this.ToUserId = this.getValue(data, "ToUserId", 0);
                    this.FromUser = this.getConstructEntityValue(data, "FromUser", new entities.user());
                    this.ToUser = this.getConstructEntityValue(data, "ToUser", new entities.user());
                    this.MainThread = this.getValue(data, "MainThread", 0);
                };
                message.prototype.clone = function () {
                    return new message(this);
                };
                return message;
            }(entities.base.BaseEntity));
            entities.message = message;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var messageController = (function (_super) {
                __extends(messageController, _super);
                function messageController() {
                    return _super.call(this, 'Message/') || this;
                }
                messageController.prototype.getByIdAndUserId = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByIdAndUserId', [id]);
                    options.responseData = new dataAccess.entities.message();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                messageController.prototype.getAll = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAll');
                    options.responseData = new dataAccess.entities.message();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                messageController.prototype.getByMessageFolder = function (messageFolderId, onSuccess, onError) {
                    messageFolderId = mdBusinessLogic.helpers.typeConversion.toInt(messageFolderId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByMessageFolder', [messageFolderId]);
                    options.responseData = new dataAccess.entities.message();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                messageController.prototype.getByMessageFolderAndUser = function (data, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByMessageFolderAndUserWithPagination', data);
                    options.responseData = new dataAccess.entities.paginationEntity(dataAccess.entities.message);
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                messageController.prototype.getByParent = function (parentId, onSuccess, onError) {
                    parentId = mdBusinessLogic.helpers.typeConversion.toInt(parentId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByParent', [parentId]);
                    options.responseData = new dataAccess.entities.message();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                messageController.prototype.getByUserId = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByUserId');
                    options.responseData = new dataAccess.entities.message();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                messageController.prototype.getByMainThread = function (mainThread, onSuccess, onError) {
                    mainThread = mdBusinessLogic.helpers.typeConversion.toInt(mainThread);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByMainThread', [mainThread]);
                    options.responseData = new dataAccess.entities.message();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                messageController.prototype.save = function (message, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.message();
                    options.requestData = message;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                messageController.prototype.messageRead = function (message, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('MessageRead');
                    options.responseData = new dataAccess.entities.message();
                    options.requestData = message;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                messageController.prototype.replace = function (message, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Replace');
                    options.responseData = new dataAccess.entities.message();
                    options.requestData = message;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                messageController.prototype['delete'] = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [id]);
                    options.responseData = new dataAccess.entities.message();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                messageController.prototype.deleteMultiple = function (messages, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('DeleteMultiple');
                    options.responseData = new dataAccess.entities.message();
                    options.requestData = messages;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                messageController.prototype.replaceMultiple = function (messages, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('ReplaceMultiple');
                    options.responseData = new dataAccess.entities.message();
                    options.requestData = messages;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                messageController.prototype.search = function (searchData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('Search', searchData);
                    options.responseData = new dataAccess.entities.message();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                messageController.prototype.getUnreadByUser = function (requestId, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions(requestId);
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetUnreadByUserSocket');
                    options.responseData = new dataAccess.entities.message();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray, options.socket);
                    };
                    options.onError = function (options) {
                        onError(options.exception, options.socket);
                    };
                    this._socket(options);
                    return options.getRequestId();
                };
                messageController.prototype.getAllChats = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetAllChats');
                    options.responseData = new dataAccess.entities.paginationEntity(dataAccess.entities.message);
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                return messageController;
            }(controllers.base.BaseController));
            controllers.messageController = messageController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var messageFolder = (function (_super) {
                __extends(messageFolder, _super);
                function messageFolder(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Name = '';
                    _this.Icon = '';
                    _this.Author = new entities.user();
                    _this.IsGlobal = false;
                    _this.Messages = new Array();
                    _this.MessagesCount = 0;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                messageFolder.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Name = this.getValue(data, "Name", '');
                    this.Icon = this.getValue(data, "Icon", '');
                    this.Author = this.getConstructEntityValue(data, "Author", new entities.user());
                    this.IsGlobal = this.getValue(data, "IsGlobal", false);
                    this.Messages = this.getArrayConstructEntityValue(data, "Messages", new Array(), new entities.message());
                    this.MessagesCount = this.getValue(data, "MessagesCount", 0);
                };
                messageFolder.prototype.clone = function () {
                    return new messageFolder(this);
                };
                return messageFolder;
            }(entities.base.BaseEntity));
            entities.messageFolder = messageFolder;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var messageFolderController = (function (_super) {
                __extends(messageFolderController, _super);
                function messageFolderController() {
                    return _super.call(this, 'MessageFolder/') || this;
                }
                messageFolderController.prototype.getById = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetById', [id]);
                    options.responseData = new dataAccess.entities.messageFolder();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                messageFolderController.prototype.getByIdAndAuthorId = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByIdAndAuthorId', [id]);
                    options.responseData = new dataAccess.entities.messageFolder();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                messageFolderController.prototype.getAll = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAll');
                    options.responseData = new dataAccess.entities.messageFolder();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                messageFolderController.prototype.getAllSystemFolders = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAllSystemFolders');
                    options.responseData = new dataAccess.entities.messageFolder();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                messageFolderController.prototype.getByAuthorId = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByAuthorId');
                    options.responseData = new dataAccess.entities.messageFolder();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                messageFolderController.prototype.save = function (messageFolder, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.messageFolder();
                    options.requestData = messageFolder;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                messageFolderController.prototype['delete'] = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('Delete', [id]);
                    options.responseData = new dataAccess.entities.messageFolder();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                return messageFolderController;
            }(controllers.base.BaseController));
            controllers.messageFolderController = messageFolderController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var metaDataFieldController = (function (_super) {
                __extends(metaDataFieldController, _super);
                function metaDataFieldController() {
                    return _super.call(this, 'MetaDataField/') || this;
                }
                metaDataFieldController.prototype.getAll = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAll');
                    options.responseData = new dataAccess.entities.metaDataField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                metaDataFieldController.prototype.getById = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetById', [id]);
                    options.responseData = new dataAccess.entities.metaDataField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                metaDataFieldController.prototype.paginationGetAll = function (paginationData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('PaginationGetAll', paginationData);
                    options.responseData = new dataAccess.entities.paginationEntity(dataAccess.entities.metaDataField);
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                metaDataFieldController.prototype.getAllCount = function (countData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetAllCount', countData);
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData.Value);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                metaDataFieldController.prototype.getByFolderId = function (folderId, onSuccess, onError) {
                    folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByFolderId', [folderId]);
                    options.responseData = new dataAccess.entities.metaDataField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                metaDataFieldController.prototype.metadatagetByFolder = function (folderId, onSuccess, onError) {
                    folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('MetaDataMediaContentGetByFolderId', [folderId]);
                    options.responseData = new dataAccess.entities.metaDataField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                metaDataFieldController.prototype.getByFolder = function (folderId, onSuccess, onError) {
                    folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByFolder', [folderId]);
                    options.responseData = new dataAccess.entities.metaDataField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                metaDataFieldController.prototype.save = function (metaDataField, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.metaDataField();
                    options.requestData = metaDataField;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                metaDataFieldController.prototype.del = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [id]);
                    options.responseData = new dataAccess.entities.metaDataField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                metaDataFieldController.prototype.assignMetaDataFieldToFolder = function (folderId, metaDataFieldId, onSuccess, onError) {
                    folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);
                    metaDataFieldId = mdBusinessLogic.helpers.typeConversion.toInt(metaDataFieldId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('AssignMetaDataFieldToFolder', [folderId, metaDataFieldId]);
                    options.responseData = new dataAccess.entities.metaDataField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                metaDataFieldController.prototype.search = function (searchData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Search', searchData);
                    options.responseData = new dataAccess.entities.metaDataField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                return metaDataFieldController;
            }(controllers.base.BaseController));
            controllers.metaDataFieldController = metaDataFieldController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var metaDataFieldValueController = (function (_super) {
                __extends(metaDataFieldValueController, _super);
                function metaDataFieldValueController() {
                    return _super.call(this, 'MetaDataFieldValue/') || this;
                }
                metaDataFieldValueController.prototype.getByContentId = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByContentId', [id]);
                    options.responseData = new dataAccess.entities.metaDataFieldValue();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                metaDataFieldValueController.prototype.getByContent = function (content, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByContent');
                    options.responseData = new dataAccess.entities.metaDataFieldValue();
                    options.requestData = content;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                return metaDataFieldValueController;
            }(controllers.base.BaseController));
            controllers.metaDataFieldValueController = metaDataFieldValueController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var permissionControllerProfileType = (function (_super) {
                __extends(permissionControllerProfileType, _super);
                function permissionControllerProfileType() {
                    return _super.call(this, 'Permissions/') || this;
                }
                permissionControllerProfileType.prototype.getProfileTypePermissionsByObject = function (object, objectId, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetProfileTypePermissionsByObject', [object, objectId]);
                    options.responseData = new dataAccess.entities.permissions.profileTypePermissions();
                    options.isJsonArray = true;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                permissionControllerProfileType.prototype.getProfileTypePermissionsByEntity = function (entity, entityId, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetProfileTypePermissionsByEntity', [entity, entityId]);
                    options.responseData = new dataAccess.entities.permissions.profileTypePermissions();
                    options.isJsonArray = true;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                permissionControllerProfileType.prototype.getProfileTypePermissionsByEntities = function (entity, entityIds, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetProfileTypePermissionsByEntities', [entity, entityIds.join('-')]);
                    options.responseData = new dataAccess.entities.permissions.profileTypePermissions();
                    options.isJsonArray = true;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                permissionControllerProfileType.prototype.savePermissions = function (permissions, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.contentType = new controllers.base.AjaxMethodHeader('Content-Type', 'application/json; charset=UTF-8');
                    options.address = this.getAddress('SaveProfileTypePermissionsByObject');
                    options.responseData = new dataAccess.entities.permissions.profileTypePermissions();
                    options.isJsonArray = true;
                    options.requestData = permissions;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                permissionControllerProfileType.prototype.getLoggedOnProfileTypePermissionsSocket = function (requestId, token, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions(requestId);
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('ProfileTypePermissionsSocket');
                    options.responseData = new dataAccess.entities.permissions.profileTypePermissions();
                    options.requestData = token;
                    options.isJsonArray = true;
                    options.onSuccess = function (response) {
                        onSuccess(response.responseDataArray, response.socket);
                    };
                    options.onError = function (response) {
                        onError(response.exception, response.socket);
                    };
                    this._socket(options);
                    return options.getRequestId();
                };
                permissionControllerProfileType.prototype.getLoggedOnProfileTypePermissions = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetAllPermissionsByProfileType');
                    options.responseData = new dataAccess.entities.permissions.profileTypePermissions();
                    options.isJsonArray = true;
                    options.onSuccess = function (response) {
                        onSuccess(response.responseDataArray);
                    };
                    options.onError = function (response) {
                        onError(response.exception);
                    };
                    this._get(options);
                };
                return permissionControllerProfileType;
            }(controllers.base.BaseController));
            controllers.permissionControllerProfileType = permissionControllerProfileType;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var permissionControllerUser = (function (_super) {
                __extends(permissionControllerUser, _super);
                function permissionControllerUser() {
                    return _super.call(this, 'Permissions/') || this;
                }
                permissionControllerUser.prototype.getUserPermissionsByObject = function (object, objectId, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetUserPermissionssByObject', [object, objectId]);
                    options.responseData = new dataAccess.entities.permissions.userPermissions();
                    options.isJsonArray = true;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                permissionControllerUser.prototype.getUserPermissionsByEntity = function (entity, entityId, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetUserPermissionssByEntity', [entity, entityId]);
                    options.responseData = new dataAccess.entities.permissions.userPermissions();
                    options.isJsonArray = true;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                permissionControllerUser.prototype.getUserPermissionsByEntities = function (entity, entityIds, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetUserPermissionsByEntities', [entity, entityIds.join('-')]);
                    options.responseData = new dataAccess.entities.permissions.userPermissions();
                    options.isJsonArray = true;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                permissionControllerUser.prototype.savePermissions = function (permissions, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.contentType = new controllers.base.AjaxMethodHeader('Content-Type', 'application/json; charset=UTF-8');
                    options.address = this.getAddress('SaveUserPermissionsByObject');
                    options.responseData = new dataAccess.entities.permissions.userPermissions();
                    options.isJsonArray = true;
                    options.requestData = permissions;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                permissionControllerUser.prototype.getLoggedOnUserPermissionsSocket = function (requestId, token, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions(requestId);
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('UserPermissionsSocket');
                    options.responseData = new dataAccess.entities.permissions.userPermissions();
                    options.requestData = token;
                    options.isJsonArray = true;
                    options.onSuccess = function (response) {
                        onSuccess(response.responseDataArray, response.socket);
                    };
                    options.onError = function (response) {
                        onError(response.exception, response.socket);
                    };
                    this._socket(options);
                    return options.getRequestId();
                };
                permissionControllerUser.prototype.getLoggedOnUserPermissions = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetAllPermissionsByUser');
                    options.responseData = new dataAccess.entities.permissions.userPermissions();
                    options.isJsonArray = true;
                    options.onSuccess = function (response) {
                        onSuccess(response.responseDataArray);
                    };
                    options.onError = function (response) {
                        onError(response.exception);
                    };
                    this._get(options);
                };
                return permissionControllerUser;
            }(controllers.base.BaseController));
            controllers.permissionControllerUser = permissionControllerUser;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var profileController = (function (_super) {
                __extends(profileController, _super);
                function profileController() {
                    return _super.call(this, 'Profile/') || this;
                }
                profileController.prototype.assignProfileTypeToUser = function (assignData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('AssignProfileTypeToUser', assignData);
                    options.responseData = new dataAccess.entities.profileType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                return profileController;
            }(controllers.base.BaseController));
            controllers.profileController = profileController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var profileTypeController = (function (_super) {
                __extends(profileTypeController, _super);
                function profileTypeController() {
                    return _super.call(this, 'ProfileType/') || this;
                }
                profileTypeController.prototype.getById = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetById', [id]);
                    options.responseData = new dataAccess.entities.profileType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                profileTypeController.prototype.getByIdAndTransformExpression = function (id, transform, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByIdAndTransformExpression', [id, transform]);
                    options.responseData = new dataAccess.entities.profileType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                profileTypeController.prototype.getAll = function (sort, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAll', [sort]);
                    options.responseData = new dataAccess.entities.profileType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                profileTypeController.prototype.getByUser = function (userId, onSuccess, onError) {
                    userId = mdBusinessLogic.helpers.typeConversion.toInt(userId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByUser', [userId]);
                    options.responseData = new dataAccess.entities.profileType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                profileTypeController.prototype.getAllWithPagination = function (paginationData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAllWitPagination', paginationData);
                    options.responseData = new dataAccess.entities.profileType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                profileTypeController.prototype.getAllCount = function (countData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetAllCount', countData);
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData.Value);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                profileTypeController.prototype.getNotBelonging = function (userId, onSuccess, onError) {
                    userId = mdBusinessLogic.helpers.typeConversion.toInt(userId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetNotBelonging', [userId]);
                    options.responseData = new dataAccess.entities.profileType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                profileTypeController.prototype.save = function (profileType, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.profileType();
                    options.requestData = profileType;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                profileTypeController.prototype.update = function (profileType, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('UpdateProfileTypePermissionsByFolder');
                    options.responseData = new dataAccess.entities.profileType();
                    options.requestData = profileType;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                profileTypeController.prototype.saveProfileTypeWithProfileTypeFieldValues = function (profileType, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('SaveProfileTypeWithProfileTypeFieldValues');
                    options.responseData = new dataAccess.entities.profileType();
                    options.requestData = profileType;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                profileTypeController.prototype.del = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [id]);
                    options.responseData = new dataAccess.entities.profileType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                profileTypeController.prototype.getAllProfileTypesWithPermissions = function (profileTypeData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAllProfileTypesWithPermissions', profileTypeData);
                    options.responseData = new dataAccess.entities.profileType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                profileTypeController.prototype.saveProfileTypePermissions = function (profileTypesData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('SaveProfileTypePermissions');
                    options.responseData = new dataAccess.entities.profileType();
                    options.requestData = profileTypesData;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                profileTypeController.prototype.search = function (searchData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('Search', searchData);
                    options.responseData = new dataAccess.entities.profileType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                return profileTypeController;
            }(controllers.base.BaseController));
            controllers.profileTypeController = profileTypeController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var profileTypeFieldController = (function (_super) {
                __extends(profileTypeFieldController, _super);
                function profileTypeFieldController() {
                    return _super.call(this, 'ProfileTypeField/') || this;
                }
                profileTypeFieldController.prototype.getById = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetById', [id]);
                    options.responseData = new dataAccess.entities.profileTypeField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                profileTypeFieldController.prototype.getByProfileType = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByProfileType', [id]);
                    options.responseData = new dataAccess.entities.profileTypeField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                profileTypeFieldController.prototype.save = function (profileTypeField, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.profileTypeField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                profileTypeFieldController.prototype.del = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [id]);
                    options.responseData = new dataAccess.entities.profileTypeField();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                return profileTypeFieldController;
            }(controllers.base.BaseController));
            controllers.profileTypeFieldController = profileTypeFieldController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var profileTypeFieldValueController = (function (_super) {
                __extends(profileTypeFieldValueController, _super);
                function profileTypeFieldValueController() {
                    return _super.call(this, 'ProfileTypeFieldValue/') || this;
                }
                profileTypeFieldValueController.prototype.getByUser = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByUser', [id]);
                    options.responseData = new dataAccess.entities.profileTypeFieldValue();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                profileTypeFieldValueController.prototype.save = function (profileTypeFieldValue, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.profileTypeFieldValue();
                    options.requestData = profileTypeFieldValue;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                return profileTypeFieldValueController;
            }(controllers.base.BaseController));
            controllers.profileTypeFieldValueController = profileTypeFieldValueController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var reportData = (function (_super) {
                __extends(reportData, _super);
                function reportData(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.DateCreated = new Date();
                    _this.Data = '';
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                reportData.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ReportId = this.getValue(data, "ReportId", 0);
                    this.DateCreated = this.getValue(data, "DateCreated", new Date());
                    this.Data = this.getValue(data, "Data", null);
                };
                reportData.prototype.clone = function () {
                    return new reportData(this);
                };
                return reportData;
            }(entities.base.BaseEntity));
            entities.reportData = reportData;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var reportDataController = (function (_super) {
                __extends(reportDataController, _super);
                function reportDataController() {
                    return _super.call(this, 'ReportData/') || this;
                }
                reportDataController.prototype.getAll = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAll');
                    options.responseData = new dataAccess.entities.reportData();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                reportDataController.prototype.getByReportSchedulerId = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByReportSchedulerId', [id]);
                    options.responseData = new dataAccess.entities.reportData();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                reportDataController.prototype.save = function (reportData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByReportSchedulerId');
                    options.responseData = new dataAccess.entities.reportData();
                    options.requestData = reportData;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                reportDataController.prototype.getReportData = function (data, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GenerateReportdata');
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.requestData = data;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData.Value);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                return reportDataController;
            }(controllers.base.BaseController));
            controllers.reportDataController = reportDataController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var innerReportDefinitionGridCoordinates = (function (_super) {
                __extends(innerReportDefinitionGridCoordinates, _super);
                function innerReportDefinitionGridCoordinates(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.x = 0;
                    _this.y = 0;
                    _this.width = 100;
                    _this.height = 50;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                innerReportDefinitionGridCoordinates.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.x = this.getValue(data, "x", 0);
                    this.y = this.getValue(data, "y", 0);
                    this.width = this.getValue(data, "width", 100);
                    this.height = this.getValue(data, "height", 50);
                };
                innerReportDefinitionGridCoordinates.prototype.clone = function () {
                    return new innerReportDefinitionGridCoordinates(this);
                };
                return innerReportDefinitionGridCoordinates;
            }(entities.base.BaseEntity));
            entities.innerReportDefinitionGridCoordinates = innerReportDefinitionGridCoordinates;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var innerReportDefinitionProperty = (function (_super) {
                __extends(innerReportDefinitionProperty, _super);
                function innerReportDefinitionProperty(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Type = 0;
                    _this.Name = '';
                    _this.Enabled = false;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                innerReportDefinitionProperty.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Type = this.getValue(data, "Type", 0);
                    this.Name = this.getValue(data, "Name", '');
                    this.Enabled = this.getValue(data, "Enabled", false);
                };
                innerReportDefinitionProperty.prototype.clone = function () {
                    return new innerReportDefinitionProperty(this);
                };
                return innerReportDefinitionProperty;
            }(entities.base.BaseEntity));
            entities.innerReportDefinitionProperty = innerReportDefinitionProperty;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var innerReportDefinitionEntity = (function (_super) {
                __extends(innerReportDefinitionEntity, _super);
                function innerReportDefinitionEntity(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Type = 0;
                    _this.Name = '';
                    _this.Icon = '';
                    _this.Coordinates = new entities.innerReportDefinitionGridCoordinates();
                    _this.UniqueId = '';
                    _this.Fields = new Array();
                    _this.BaseFields = new Array();
                    _this.ExtendedFields = new Array();
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                innerReportDefinitionEntity.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Type = this.getValue(data, "Type", 0);
                    this.Name = this.getValue(data, "Name", '');
                    this.Icon = this.getValue(data, "Icon", '');
                    this.Coordinates = this.getConstructEntityValue(data, "Coordinates", new entities.innerReportDefinitionGridCoordinates());
                    this.UniqueId = this.getValue(data, "UniqueId", '');
                    this.Fields = this.getArrayConstructEntityValue(data, "Fields", new Array(), new entities.innerReportDefinitionProperty());
                    this.BaseFields = this.getArrayConstructEntityValue(data, "BaseFields", new Array(), new entities.innerReportDefinitionProperty());
                    this.ExtendedFields = this.getArrayConstructEntityValue(data, "ExtendedFields", new Array(), new entities.innerReportDefinitionProperty());
                };
                innerReportDefinitionEntity.prototype.clone = function () {
                    return new innerReportDefinitionEntity(this);
                };
                innerReportDefinitionEntity.prototype.getTypeString = function () {
                    return innerReportDefinitionEntityTypes[this.Type];
                };
                return innerReportDefinitionEntity;
            }(entities.base.BaseEntity));
            entities.innerReportDefinitionEntity = innerReportDefinitionEntity;
            var innerReportDefinitionEntityTypes;
            (function (innerReportDefinitionEntityTypes) {
                innerReportDefinitionEntityTypes[innerReportDefinitionEntityTypes["Content"] = 1] = "Content";
                innerReportDefinitionEntityTypes[innerReportDefinitionEntityTypes["User"] = 2] = "User";
                innerReportDefinitionEntityTypes[innerReportDefinitionEntityTypes["Taxonomy"] = 3] = "Taxonomy";
                innerReportDefinitionEntityTypes[innerReportDefinitionEntityTypes["MediaContent"] = 4] = "MediaContent";
                innerReportDefinitionEntityTypes[innerReportDefinitionEntityTypes["Folder"] = 5] = "Folder";
            })(innerReportDefinitionEntityTypes = entities.innerReportDefinitionEntityTypes || (entities.innerReportDefinitionEntityTypes = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var innerReportDefinitionJoinInner = (function (_super) {
                __extends(innerReportDefinitionJoinInner, _super);
                function innerReportDefinitionJoinInner(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Entity = new entities.innerReportDefinitionEntity();
                    _this.Property = new entities.innerReportDefinitionProperty();
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                innerReportDefinitionJoinInner.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Entity = this.getConstructEntityValue(data, "Entity", new entities.innerReportDefinitionEntity());
                    this.Property = this.getConstructEntityValue(data, "Property", new entities.innerReportDefinitionProperty());
                };
                innerReportDefinitionJoinInner.prototype.clone = function () {
                    return new innerReportDefinitionJoinInner(this);
                };
                return innerReportDefinitionJoinInner;
            }(entities.base.BaseEntity));
            entities.innerReportDefinitionJoinInner = innerReportDefinitionJoinInner;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var innerReportDefinitionJoin = (function (_super) {
                __extends(innerReportDefinitionJoin, _super);
                function innerReportDefinitionJoin(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Left = new entities.innerReportDefinitionJoinInner();
                    _this.Right = new entities.innerReportDefinitionJoinInner();
                    _this.Type = 0;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                innerReportDefinitionJoin.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Left = this.getConstructEntityValue(data, "Left", new entities.innerReportDefinitionJoinInner());
                    this.Right = this.getConstructEntityValue(data, "Right", new entities.innerReportDefinitionJoinInner());
                    this.Type = this.getValue(data, "Type", 0);
                };
                innerReportDefinitionJoin.prototype.clone = function () {
                    return new innerReportDefinitionJoin(this);
                };
                return innerReportDefinitionJoin;
            }(entities.base.BaseEntity));
            entities.innerReportDefinitionJoin = innerReportDefinitionJoin;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var innerReportDefinitionUniqueProperty = (function (_super) {
                __extends(innerReportDefinitionUniqueProperty, _super);
                function innerReportDefinitionUniqueProperty(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.UniqueId = '';
                    _this.Property = new entities.innerReportDefinitionProperty();
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                innerReportDefinitionUniqueProperty.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.UniqueId = this.getValue(data, "UniqueId", '');
                    this.Property = this.getConstructEntityValue(data, "Property", new entities.innerReportDefinitionProperty());
                };
                innerReportDefinitionUniqueProperty.prototype.clone = function () {
                    return new innerReportDefinitionUniqueProperty(this);
                };
                return innerReportDefinitionUniqueProperty;
            }(entities.base.BaseEntity));
            entities.innerReportDefinitionUniqueProperty = innerReportDefinitionUniqueProperty;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var innerReportDefinitionColumn = (function (_super) {
                __extends(innerReportDefinitionColumn, _super);
                function innerReportDefinitionColumn(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Type = 0;
                    _this.Value = '';
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                innerReportDefinitionColumn.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Type = this.getValue(data, "Type", 0);
                    this.Value = this.getValue(data, "Value", '');
                };
                innerReportDefinitionColumn.prototype.clone = function () {
                    return new innerReportDefinitionColumn(this);
                };
                return innerReportDefinitionColumn;
            }(entities.innerReportDefinitionUniqueProperty));
            entities.innerReportDefinitionColumn = innerReportDefinitionColumn;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var innerReportDefinitionFilter = (function (_super) {
                __extends(innerReportDefinitionFilter, _super);
                function innerReportDefinitionFilter(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Type = 0;
                    _this.Value = '';
                    _this.Entity = new entities.innerReportDefinitionEntity();
                    _this.Property = new entities.innerReportDefinitionProperty();
                    _this.IsDynamic = false;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                innerReportDefinitionFilter.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Type = this.getValue(data, "Type", 0);
                    this.Entity = this.getConstructEntityValue(data, "Entity", new entities.innerReportDefinitionEntity());
                    this.Value = this.getValue(data, "Value", '');
                    this.Property = this.getConstructEntityValue(data, "Property", new entities.innerReportDefinitionProperty());
                    this.IsDynamic = this.getValue(data, "IsDynamic", false);
                };
                innerReportDefinitionFilter.prototype.clone = function () {
                    return new innerReportDefinitionFilter(this);
                };
                return innerReportDefinitionFilter;
            }(entities.innerReportDefinitionUniqueProperty));
            entities.innerReportDefinitionFilter = innerReportDefinitionFilter;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var innerReportDefinitionGroup = (function (_super) {
                __extends(innerReportDefinitionGroup, _super);
                function innerReportDefinitionGroup(obj) {
                    return _super.call(this, obj) || this;
                }
                innerReportDefinitionGroup.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                };
                innerReportDefinitionGroup.prototype.clone = function () {
                    return new innerReportDefinitionGroup(this);
                };
                return innerReportDefinitionGroup;
            }(entities.innerReportDefinitionUniqueProperty));
            entities.innerReportDefinitionGroup = innerReportDefinitionGroup;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var innerReportDefinitionLimit = (function (_super) {
                __extends(innerReportDefinitionLimit, _super);
                function innerReportDefinitionLimit(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.From = 0;
                    _this.To = 0;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                innerReportDefinitionLimit.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.From = this.getValue(data, "From", 0);
                    this.To = this.getValue(data, "To", 0);
                };
                innerReportDefinitionLimit.prototype.clone = function () {
                    return new innerReportDefinitionLimit(this);
                };
                return innerReportDefinitionLimit;
            }(entities.base.BaseEntity));
            entities.innerReportDefinitionLimit = innerReportDefinitionLimit;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var innerReportDefinition = (function (_super) {
                __extends(innerReportDefinition, _super);
                function innerReportDefinition(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Entities = new Array();
                    _this.Joins = new Array();
                    _this.Columns = new Array();
                    _this.Filters = new Array();
                    _this.Groupings = new Array();
                    _this.Limit = new entities.innerReportDefinitionLimit();
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                innerReportDefinition.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Entities = this.getArrayConstructEntityValue(data, "Entities", new Array(), new entities.innerReportDefinitionEntity());
                    this.Joins = this.getArrayConstructEntityValue(data, "Joins", new Array(), new entities.innerReportDefinitionJoin());
                    this.Columns = this.getArrayConstructEntityValue(data, "Columns", new Array(), new entities.innerReportDefinitionColumn());
                    this.Filters = this.getArrayConstructEntityValue(data, "Filters", new Array(), new entities.innerReportDefinitionFilter());
                    this.Groupings = this.getArrayConstructEntityValue(data, "Groupings", new Array(), new entities.innerReportDefinitionGroup());
                    this.Limit = this.getConstructEntityValue(data, "Limit", new entities.innerReportDefinitionLimit());
                };
                innerReportDefinition.prototype.clone = function () {
                    return new innerReportDefinition(this);
                };
                return innerReportDefinition;
            }(entities.base.BaseEntity));
            entities.innerReportDefinition = innerReportDefinition;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var reportDefinition = (function (_super) {
                __extends(reportDefinition, _super);
                function reportDefinition(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Name = '';
                    _this.Definition = new entities.innerReportDefinition();
                    _this.Sql = '';
                    _this.AuthorId = 0;
                    _this.Author = new entities.user();
                    _this.Json = '';
                    _this.DateCreated = new Date();
                    _this.DateModified = new Date();
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                reportDefinition.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Name = this.getValue(data, "Name", '');
                    this.Definition = this.getConstructEntityValue(data, "Definition", new entities.innerReportDefinition());
                    this.Sql = this.getValue(data, "Sql", '');
                    this.AuthorId = this.getValue(data, "AuthorId", 0);
                    this.Author = this.getConstructEntityValue(data, "Author", new entities.user());
                    this.Json = this.getValue(data, "Json", '');
                    this.DateCreated = this.getValue(data, "DateCreated", new Date());
                    this.DateModified = this.getValue(data, "DateModified", new Date());
                };
                reportDefinition.prototype.clone = function () {
                    return new reportDefinition(this);
                };
                return reportDefinition;
            }(entities.base.BaseEntity));
            entities.reportDefinition = reportDefinition;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var reportDefinitionController = (function (_super) {
                __extends(reportDefinitionController, _super);
                function reportDefinitionController() {
                    return _super.call(this, 'ReportDesigner/') || this;
                }
                reportDefinitionController.prototype.getEntities = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetEntities');
                    options.responseData = new dataAccess.entities.innerReportDefinitionEntity();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                reportDefinitionController.prototype.getReportPreview = function (data, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GenerateSampleReportdata');
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.requestData = data;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData.Value);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                reportDefinitionController.prototype.save = function (reportDefinition, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('SaveDefinition');
                    options.responseData = new dataAccess.entities.reportDefinition();
                    options.requestData = reportDefinition;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                reportDefinitionController.prototype.getReportColumns = function (reportDefinition, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetAllColumns');
                    options.responseData = new dataAccess.entities.reportDefinition();
                    options.requestData = reportDefinition;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                reportDefinitionController.prototype.getAll = function (sortData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAllDefinitions', [sortData.sort]);
                    options.responseData = new dataAccess.entities.reportDefinition();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                reportDefinitionController.prototype.getById = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetDefinitionById', [id]);
                    options.responseData = new dataAccess.entities.reportDefinition();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                reportDefinitionController.prototype.search = function (searchData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Search', searchData);
                    options.responseData = new dataAccess.entities.reportDefinition();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                reportDefinitionController.prototype.getAllWithPagination = function (paginationData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetAllWithPagination', paginationData);
                    options.responseData = new dataAccess.entities.paginationEntity(dataAccess.entities.reportDefinition);
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                reportDefinitionController.prototype.getAllCount = function (countData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetAllCount', countData);
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData.Value);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                reportDefinitionController.prototype.del = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [id]);
                    options.responseData = new dataAccess.entities.reportDefinition();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                return reportDefinitionController;
            }(controllers.base.BaseController));
            controllers.reportDefinitionController = reportDefinitionController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var reportDirectory = (function (_super) {
                __extends(reportDirectory, _super);
                function reportDirectory(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Path = '';
                    _this.Children = new Array();
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                reportDirectory.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Path = this.getValue(data, "Path", '');
                    this.Children = this.getArrayConstructEntityValue(data, "Children", new Array(), new reportDirectory());
                };
                reportDirectory.prototype.clone = function () {
                    return new reportDirectory(this);
                };
                return reportDirectory;
            }(entities.base.BaseEntity));
            entities.reportDirectory = reportDirectory;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var reportDirectoryController = (function (_super) {
                __extends(reportDirectoryController, _super);
                function reportDirectoryController() {
                    return _super.call(this, 'ReportDirectory/') || this;
                }
                reportDirectoryController.prototype.getReportDirectoryByPath = function (path, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetReportDirectoryByPath');
                    options.responseData = new dataAccess.entities.reportDirectory();
                    options.requestData = { ValueName: path };
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                return reportDirectoryController;
            }(controllers.base.BaseController));
            controllers.reportDirectoryController = reportDirectoryController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var reportSchedulerAction = (function (_super) {
                __extends(reportSchedulerAction, _super);
                function reportSchedulerAction(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.SchedulerId = 0;
                    _this.Name = '';
                    _this.AuthorId = 0;
                    _this.DateCreated = new Date();
                    _this.DateEdited = null;
                    _this.ActionType = 0;
                    _this.Options = '';
                    _this.IsActive = false;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                reportSchedulerAction.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.SchedulerId = this.getValue(data, "SchedulerId", 0);
                    this.Name = this.getValue(data, "Name", '');
                    this.AuthorId = this.getValue(data, "AuthorId", 0);
                    this.DateCreated = this.getValue(data, "DateCreated", new Date());
                    this.DateEdited = this.getValue(data, "DateEdited", null);
                    this.ActionType = this.getValue(data, "ActionType", 0);
                    this.Options = this.getValue(data, "Options", '');
                    this.IsActive = this.getValue(data, "IsActive", false);
                };
                reportSchedulerAction.prototype.clone = function () {
                    return new reportSchedulerAction(this);
                };
                return reportSchedulerAction;
            }(entities.base.BaseEntity));
            entities.reportSchedulerAction = reportSchedulerAction;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var reportScheduler = (function (_super) {
                __extends(reportScheduler, _super);
                function reportScheduler(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Name = '';
                    _this.AuthorId = 0;
                    _this.DateCreated = new Date();
                    _this.IsRecurring = false;
                    _this.Interval = 0;
                    _this.Start = null;
                    _this.End = null;
                    _this.ReportId = 0;
                    _this.IsActive = false;
                    _this.Actions = new Array();
                    _this.Author = new entities.user();
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                reportScheduler.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Name = this.getValue(data, "Name", '');
                    this.AuthorId = this.getValue(data, "AuthorId", 0);
                    this.DateCreated = this.getValue(data, "DateCreated", new Date());
                    this.IsRecurring = this.getValue(data, "IsRecurring", false);
                    this.Interval = this.getValue(data, "Interval", 0);
                    this.Start = this.getValue(data, "Start", null);
                    this.End = this.getValue(data, "End", null);
                    this.ReportId = this.getValue(data, "ReportId", 0);
                    this.IsActive = this.getValue(data, "IsActive", false);
                    this.Actions = this.getArrayConstructEntityValue(data, "Actions", new Array(), new entities.reportSchedulerAction());
                    this.Author = this.getConstructEntityValue(data, "Author", new entities.user());
                };
                reportScheduler.prototype.clone = function () {
                    return new reportScheduler(this);
                };
                return reportScheduler;
            }(entities.base.BaseEntity));
            entities.reportScheduler = reportScheduler;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var reportSchedulerController = (function (_super) {
                __extends(reportSchedulerController, _super);
                function reportSchedulerController() {
                    return _super.call(this, 'ReportScheduler/') || this;
                }
                reportSchedulerController.prototype.getById = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetById', [id]);
                    options.responseData = new dataAccess.entities.reportScheduler();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                reportSchedulerController.prototype.getByReportDefinitionId = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.address = this.getAddress('GetByReportDefinitionId', [id]);
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.responseData = new dataAccess.entities.reportScheduler();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                reportSchedulerController.prototype.getAll = function (searchTerm, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAll', [searchTerm]);
                    options.responseData = new dataAccess.entities.reportScheduler();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                reportSchedulerController.prototype.save = function (reportScheduler, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.reportScheduler();
                    options.requestData = reportScheduler;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                reportSchedulerController.prototype.del = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [id]);
                    options.responseData = new dataAccess.entities.reportScheduler();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                reportSchedulerController.prototype.getReportSchedulerActionTypes = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetReportSchedulerActionTypes');
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                reportSchedulerController.prototype.getAllWithPagination = function (paginationData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetAllWithPagination', paginationData);
                    options.responseData = new dataAccess.entities.paginationEntity(dataAccess.entities.reportScheduler);
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                reportSchedulerController.prototype.getAllCount = function (countData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetAllCount', countData);
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData.Value);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                return reportSchedulerController;
            }(controllers.base.BaseController));
            controllers.reportSchedulerController = reportSchedulerController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var searchController = (function (_super) {
                __extends(searchController, _super);
                function searchController() {
                    return _super.call(this, 'Search/') || this;
                }
                searchController.prototype.fullTextSearch = function (searchTerm, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('FullText', [searchTerm]);
                    options.responseData = new dataAccess.entities.search.searchResults();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                return searchController;
            }(controllers.base.BaseController));
            controllers.searchController = searchController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var hardwareInfoDrive = (function (_super) {
                __extends(hardwareInfoDrive, _super);
                function hardwareInfoDrive(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Label = '';
                    _this.TotalSizeMb = 0;
                    _this.AvaliableSizeMb = 0;
                    _this.UsedSizeMb = 0;
                    _this.TotalSizeGb = 0;
                    _this.AvaliableSizeGb = 0;
                    _this.UsedSizeGb = 0;
                    _this.Format = '';
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                hardwareInfoDrive.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Label = this.getValue(data, "Label", '');
                    this.TotalSizeMb = this.getValue(data, "TotalSizeMb", 0);
                    this.AvaliableSizeMb = this.getValue(data, "AvaliableSizeMb", 0);
                    this.UsedSizeMb = this.getValue(data, "UsedSizeMb", 0);
                    this.TotalSizeGb = this.getValue(data, "TotalSizeGb", 0);
                    this.AvaliableSizeGb = this.getValue(data, "AvaliableSizeGb", 0);
                    this.UsedSizeGb = this.getValue(data, "UsedSizeGb", 0);
                    this.Format = this.getValue(data, "Format", '');
                };
                hardwareInfoDrive.prototype.clone = function () {
                    return new hardwareInfoDrive(this);
                };
                return hardwareInfoDrive;
            }(entities.base.BaseEntity));
            entities.hardwareInfoDrive = hardwareInfoDrive;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var hardwareInfoNetworkInterface = (function (_super) {
                __extends(hardwareInfoNetworkInterface, _super);
                function hardwareInfoNetworkInterface(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Name = '';
                    _this.Description = '';
                    _this.SentMb = 0;
                    _this.ReceivedMb = 0;
                    _this.SentGb = 0;
                    _this.ReceivedGb = 0;
                    _this.NetworkUtilization = 0;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                hardwareInfoNetworkInterface.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Name = this.getValue(data, "Name", '');
                    this.Description = this.getValue(data, "Description", '');
                    this.SentMb = this.getValue(data, "SentMb", 0);
                    this.ReceivedMb = this.getValue(data, "ReceivedMb", 0);
                    this.SentGb = this.getValue(data, "SentGb", 0);
                    this.ReceivedGb = this.getValue(data, "ReceivedGb", 0);
                    this.NetworkUtilization = this.getValue(data, "NetworkUtilization", 0);
                };
                hardwareInfoNetworkInterface.prototype.clone = function () {
                    return new hardwareInfoNetworkInterface(this);
                };
                return hardwareInfoNetworkInterface;
            }(entities.base.BaseEntity));
            entities.hardwareInfoNetworkInterface = hardwareInfoNetworkInterface;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var hardwareInfoProcess = (function (_super) {
                __extends(hardwareInfoProcess, _super);
                function hardwareInfoProcess(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Name = '';
                    _this.User = '';
                    _this.ProcessorUsage = 0;
                    _this.MemoryUsageMb = 0;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                hardwareInfoProcess.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Name = this.getValue(data, "Name", '');
                    this.User = this.getValue(data, "User", '');
                    this.ProcessorUsage = this.getValue(data, "ProcessorUsage", 0);
                    this.MemoryUsageMb = this.getValue(data, "MemoryUsageMb", 0);
                };
                hardwareInfoProcess.prototype.clone = function () {
                    return new hardwareInfoProcess(this);
                };
                return hardwareInfoProcess;
            }(entities.base.BaseEntity));
            entities.hardwareInfoProcess = hardwareInfoProcess;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var hardwareInfoPerformance = (function (_super) {
                __extends(hardwareInfoPerformance, _super);
                function hardwareInfoPerformance(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.SampleDateTime = null;
                    _this.CpuUsage = 0;
                    _this.FreeMemoryMb = 0;
                    _this.TotalMemoryMb = 0;
                    _this.UsedMemoryMb = 0;
                    _this.Drives = new Array();
                    _this.NetworkInterfaces = new Array();
                    _this.Processes = new Array();
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                hardwareInfoPerformance.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.SampleDateTime = this.getValue(data, "SampleDateTime", new Date());
                    this.CpuUsage = this.getValue(data, "CpuUsage", 0);
                    this.FreeMemoryMb = this.getValue(data, "FreeMemoryMb", 0);
                    this.TotalMemoryMb = this.getValue(data, "TotalMemoryMb", 0);
                    this.UsedMemoryMb = this.TotalMemoryMb - this.FreeMemoryMb;
                    this.Drives = this.getArrayConstructEntityValue(data, "Drives", new Array(), new entities.hardwareInfoDrive());
                    this.NetworkInterfaces = this.getArrayConstructEntityValue(data, "NetworkInterfaces", new Array(), new entities.hardwareInfoNetworkInterface());
                    this.Processes = this.getArrayConstructEntityValue(data, "Processes", new Array(), new entities.hardwareInfoProcess());
                };
                hardwareInfoPerformance.prototype.clone = function () {
                    return new hardwareInfoPerformance(this);
                };
                return hardwareInfoPerformance;
            }(entities.base.BaseEntity));
            entities.hardwareInfoPerformance = hardwareInfoPerformance;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var pluginJob = (function (_super) {
                __extends(pluginJob, _super);
                function pluginJob(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.PluginName = '';
                    _this.Message = '';
                    _this.StartedOn = null;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                pluginJob.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.PluginName = this.getValue(data, "PluginName", '');
                    this.Message = this.getValue(data, "Message", '');
                    this.StartedOn = this.getValue(data, "StartedOn", null);
                };
                pluginJob.prototype.clone = function () {
                    return new pluginJob(this);
                };
                return pluginJob;
            }(entities.base.BaseEntity));
            entities.pluginJob = pluginJob;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var systemInfoController = (function (_super) {
                __extends(systemInfoController, _super);
                function systemInfoController() {
                    return _super.call(this, 'SystemInfo/') || this;
                }
                systemInfoController.processPreInit = function (callback) {
                    function deferer(callback) {
                        if (!systemInfoController._isInitialized) {
                            if (systemInfoController._initCallInProgress) {
                                setTimeout(function () {
                                    deferer(callback);
                                }, 500);
                            }
                            else {
                                systemInfoController._initCallInProgress = true;
                                (new systemInfoController()).getInit(function (data) {
                                    systemInfoController._initCallInProgress = false;
                                    systemInfoController._isInitialized = data.Initiated;
                                    callback();
                                }, function (error) {
                                    systemInfoController._initCallInProgress = false;
                                    throw new mdBusinessLogic.helpers.mdException("Failed to initialize the API, please look at the server logs or contact your administrator!");
                                });
                            }
                        }
                        else {
                            callback();
                        }
                    }
                    deferer(callback);
                };
                systemInfoController.getIsInitialized = function () {
                    return systemInfoController._isInitialized;
                };
                systemInfoController.prototype.getPerformance = function (requestId, delay, onSuccess, onError) {
                    delay = mdBusinessLogic.helpers.typeConversion.toInt(delay);
                    var options = new controllers.base.AjaxMethodOptions(requestId);
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Performance', [delay]);
                    options.responseData = new dataAccess.entities.hardwareInfoPerformance();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData, options.socket);
                    };
                    options.onError = function (options) {
                        onError(options.exception, options.socket);
                    };
                    this._socket(options);
                };
                systemInfoController.prototype.getPluginJobs = function (requestId, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions(requestId);
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAllJobs');
                    options.responseData = new dataAccess.entities.pluginJob();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray, options.socket);
                    };
                    options.onError = function (options) {
                        onError(options.exception, options.socket);
                    };
                    this._socket(options);
                    return options.getRequestId();
                };
                systemInfoController.prototype.getInit = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Init');
                    options.responseData = new dataAccess.entities.models.initModel();
                    options.isInitCall = true;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                systemInfoController._isInitialized = false;
                systemInfoController._initCallInProgress = false;
                return systemInfoController;
            }(controllers.base.BaseController));
            controllers.systemInfoController = systemInfoController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var taxonomyContentController = (function (_super) {
                __extends(taxonomyContentController, _super);
                function taxonomyContentController() {
                    return _super.call(this, 'TaxonomyContent/') || this;
                }
                taxonomyContentController.prototype.getByTaxonomyId = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByTaxonomyId', [id]);
                    options.responseData = new dataAccess.entities.taxonomyContent();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                taxonomyContentController.prototype.paginationGetByTaxonomyId = function (paginationData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('PaginationGetByTaxonomyId', paginationData);
                    options.responseData = new dataAccess.entities.paginationEntity(dataAccess.entities.taxonomyContent);
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                taxonomyContentController.prototype.getByTaxonomyIdCount = function (countData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByTaxonomyIdCount', countData);
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData.Value);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                taxonomyContentController.prototype.del = function (deleteData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [deleteData.Id, deleteData.TaxonomyId]);
                    options.responseData = new dataAccess.entities.taxonomyContent();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                taxonomyContentController.prototype.save = function (taxonomyContent, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.taxonomyContent();
                    options.requestData = taxonomyContent;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                taxonomyContentController.prototype.deletetaxonomy = function (content, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('DeleteTaxonomyContent');
                    options.responseData = new dataAccess.entities.taxonomyContent();
                    options.requestData = content;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                taxonomyContentController.prototype.savecontent = function (taxonomy, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('SaveTaxonomyContent');
                    options.responseData = new dataAccess.entities.taxonomyContent();
                    options.requestData = taxonomy;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                taxonomyContentController.prototype.update = function (taxonomy, pageIndex, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Update', [pageIndex]);
                    options.responseData = new dataAccess.entities.taxonomyContent();
                    options.requestData = taxonomy;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                taxonomyContentController.prototype.deletecontent = function (taxonomy, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('DeleteContent');
                    options.responseData = new dataAccess.entities.taxonomyContent();
                    options.requestData = taxonomy;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                taxonomyContentController.prototype.search = function (searchTerm, taxonomyId, lcid, onSuccess, onError) {
                    taxonomyId = mdBusinessLogic.helpers.typeConversion.toInt(taxonomyId);
                    lcid = mdBusinessLogic.helpers.typeConversion.toInt(lcid);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('Search', [searchTerm, taxonomyId, lcid]);
                    options.responseData = new dataAccess.entities.taxonomyContent();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                return taxonomyContentController;
            }(controllers.base.BaseController));
            controllers.taxonomyContentController = taxonomyContentController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var taxonomyController = (function (_super) {
                __extends(taxonomyController, _super);
                function taxonomyController() {
                    return _super.call(this, 'Taxonomy/') || this;
                }
                taxonomyController.prototype.getById = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetById', [id]);
                    options.responseData = new dataAccess.entities.taxonomy();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                taxonomyController.prototype.getByParentId = function (id, depth, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByParentId', [id]);
                    options.responseData = new dataAccess.entities.taxonomy();
                    options.headers.push(new controllers.base.AjaxMethodHeader("depth", depth));
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                taxonomyController.prototype.getByParentIdCount = function (countData, depth, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByParentIdCount', countData);
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                taxonomyController.prototype.paginationGetTaxonomyByPath = function (paginationData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetTaxonomyWithPaginationByPath', paginationData);
                    options.responseData = new dataAccess.entities.taxonomy();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                taxonomyController.prototype.GetByParentIdWithPagination = function (paginationData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByParentIdWithPagination', paginationData);
                    options.responseData = new dataAccess.entities.paginationEntity(dataAccess.entities.taxonomy);
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                taxonomyController.prototype.updateChildren = function (taxonomy, orderStart, onSuccess, onError) {
                    orderStart = mdBusinessLogic.helpers.typeConversion.toInt(orderStart);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('UpdateChildren', [orderStart]);
                    options.responseData = new dataAccess.entities.taxonomy();
                    options.requestData = taxonomy;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                taxonomyController.prototype.search = function (searchTerm, taxonomyId, recursive, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('Search', [searchTerm, taxonomyId, recursive]);
                    options.responseData = new dataAccess.entities.taxonomy();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                taxonomyController.prototype.getByContent = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByContent', [id]);
                    options.responseData = new dataAccess.entities.taxonomy();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                taxonomyController.prototype.taxonomyContentGetTaxonomyByContent = function (content, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('TaxonomyContentGetTaxonomyByContent');
                    options.responseData = new dataAccess.entities.taxonomy();
                    options.requestData = content;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                taxonomyController.prototype.getAll = function (lcid, onSuccess, onError) {
                    lcid = mdBusinessLogic.helpers.typeConversion.toInt(lcid);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.lcid = lcid;
                    options.address = this.getAddress('GetAll');
                    options.responseData = new dataAccess.entities.taxonomy();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                taxonomyController.prototype.getHierarchyByParentId = function (id, depth, onSuccess, onError) {
                    this.getHierarchyByParentIdWithContents(id, depth, false, onSuccess, onError);
                };
                taxonomyController.prototype.getHierarchyByParentIdWithContents = function (id, depth, loadContents, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetHierarchyByParentId', [id, loadContents]);
                    options.responseData = new dataAccess.entities.taxonomy();
                    options.headers.push(new controllers.base.AjaxMethodHeader("depth", depth));
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                taxonomyController.prototype.save = function (taxonomy, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.taxonomy();
                    options.requestData = taxonomy;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                taxonomyController.prototype.del = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [id]);
                    options.responseData = new dataAccess.entities.taxonomy();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                taxonomyController.prototype.assignContentToTaxonomy = function (id, contentId, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('AssignContentToTaxonomy', [id, contentId]);
                    options.responseData = new dataAccess.entities.taxonomy();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                taxonomyController.prototype.assignContentToTaxonomies = function (taxonomyIds, contentId, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('AssignContentToTaxonomies');
                    options.responseData = new dataAccess.entities.taxonomy();
                    options.isJsonArray = true;
                    options.requestData = {
                        contentId: contentId,
                        taxonomyIds: taxonomyIds
                    };
                    options.contentType = new controllers.base.AjaxMethodHeader('Content-Type', 'application/json');
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                taxonomyController.prototype.getByTaxonomyPath = function (path, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetTaxonomyByPath');
                    options.responseData = new dataAccess.entities.taxonomy();
                    options.requestData = {
                        ValueName: path
                    };
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                taxonomyController.prototype.delContent = function (id, path, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('DeleteContent', [id]);
                    options.responseData = new dataAccess.entities.taxonomy();
                    options.requestData = {
                        ValueName: path
                    };
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                return taxonomyController;
            }(controllers.base.BaseController));
            controllers.taxonomyController = taxonomyController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var templateController = (function (_super) {
                __extends(templateController, _super);
                function templateController() {
                    return _super.call(this, 'Template/') || this;
                }
                templateController.prototype.getByFolder = function (folderId, onSuccess, onError) {
                    folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetByFolder', [folderId]);
                    options.responseData = new dataAccess.entities.template();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                templateController.prototype.getAll = function (sort, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAll', [sort]);
                    options.responseData = new dataAccess.entities.template();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                templateController.prototype.getAllWithPagination = function (paginationData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetAllWithPagination', paginationData);
                    options.responseData = new dataAccess.entities.paginationEntity(dataAccess.entities.template);
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                templateController.prototype.getAllCount = function (countData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetAllCount', countData);
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData.Value);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                templateController.prototype.getById = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetById', [id]);
                    options.responseData = new dataAccess.entities.template();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                templateController.prototype.save = function (template, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.template();
                    options.requestData = template;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                templateController.prototype.del = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [id]);
                    options.responseData = new dataAccess.entities.template();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                templateController.prototype.search = function (searchData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('Search', searchData);
                    options.responseData = new dataAccess.entities.template();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                templateController.prototype.getScreenshot = function (templateScreenshot, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetTemplateScreenshot');
                    options.responseData = new dataAccess.entities.templateScreenshot();
                    options.requestData = templateScreenshot;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                return templateController;
            }(controllers.base.BaseController));
            controllers.templateController = templateController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var templateFile = (function (_super) {
                __extends(templateFile, _super);
                function templateFile(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Path = '';
                    _this.Name = '';
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                templateFile.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Path = this.getValue(data, "Path", '');
                    this.Name = this.getValue(data, "Name", '');
                };
                templateFile.prototype.clone = function () {
                    return new templateFile(this);
                };
                return templateFile;
            }(entities.base.BaseEntity));
            entities.templateFile = templateFile;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var templateDirectory = (function (_super) {
                __extends(templateDirectory, _super);
                function templateDirectory(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Path = '';
                    _this.Children = new Array();
                    _this.Files = new Array();
                    _this.Name = '';
                    _this.RootPath = '';
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                templateDirectory.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Path = this.getValue(data, "Path", '');
                    this.Children = this.getArrayConstructEntityValue(data, "Children", new Array(), new templateDirectory());
                    this.Files = this.getArrayConstructEntityValue(data, "Files", new Array(), new entities.templateFile());
                    this.Name = this.getValue(data, "Name", '');
                    this.RootPath = this.getValue(data, "RootPath", '');
                };
                templateDirectory.prototype.clone = function () {
                    return new templateDirectory(this);
                };
                return templateDirectory;
            }(entities.base.BaseEntity));
            entities.templateDirectory = templateDirectory;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var templateDirectoryController = (function (_super) {
                __extends(templateDirectoryController, _super);
                function templateDirectoryController() {
                    return _super.call(this, 'TemplateDirectory/') || this;
                }
                templateDirectoryController.prototype.getTemplateDirectoryByPath = function (template, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetTemplateDirectoryByPath');
                    options.responseData = new dataAccess.entities.templateDirectory();
                    options.requestData = template;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                return templateDirectoryController;
            }(controllers.base.BaseController));
            controllers.templateDirectoryController = templateDirectoryController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var userController = (function (_super) {
                __extends(userController, _super);
                function userController() {
                    return _super.call(this, 'User/') || this;
                }
                userController.prototype.getAuthData = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetAuthData', [id]);
                    options.responseData = new dataAccess.providers.authentication.authData();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                userController.prototype.getById = function (id, onSuccess, onError) {
                    id = mdBusinessLogic.helpers.typeConversion.toInt(id);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetById', [id]);
                    options.responseData = new dataAccess.entities.user();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                userController.prototype.getAllUserWithPermissions = function (usersData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAllUserWithPermissions', usersData);
                    options.responseData = new dataAccess.entities.user();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                userController.prototype.getOnlyNotAuthorizedUsers = function (usersData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetOnlyNotAuthorizedUsers', usersData);
                    options.responseData = new dataAccess.entities.user();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                userController.prototype.getAll = function (onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('GetAll');
                    options.responseData = new dataAccess.entities.user();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray.filter(function (user) { return user.Id != 0; }));
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                userController.prototype.paginationGetAll = function (paginationData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('PaginationGetAll', paginationData);
                    options.responseData = new dataAccess.entities.paginationEntity(dataAccess.entities.user);
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                userController.prototype.getAllCount = function (countData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetAllCount', countData);
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData.Value);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                userController.prototype.updateUserPermission = function (users, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('UpdateUserPermissionsByFolder');
                    options.responseData = new dataAccess.entities.user();
                    options.requestData = users;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                userController.prototype.save = function (user, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Save');
                    options.responseData = new dataAccess.entities.user();
                    options.requestData = user;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                userController.prototype.assignProfileTypeToUser = function (profileTypeId, userId, onSuccess, onError) {
                    profileTypeId = mdBusinessLogic.helpers.typeConversion.toInt(profileTypeId);
                    userId = mdBusinessLogic.helpers.typeConversion.toInt(userId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('AssignProfileTypeToUser', [profileTypeId, userId]);
                    options.responseData = new dataAccess.entities.user();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                userController.prototype.del = function (userId, onSuccess, onError) {
                    userId = mdBusinessLogic.helpers.typeConversion.toInt(userId);
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('Delete', [userId]);
                    options.responseData = new dataAccess.entities.user();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._delete(options);
                };
                userController.prototype.login = function (username, password, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = false;
                    options.address = this.getAddress('Login');
                    options.responseData = new dataAccess.entities.loggedOnUser();
                    options.requestData = new dataAccess.entities.user();
                    options.requestData.Username = username;
                    options.requestData.Password = password;
                    options.requestData.Token = mdBusinessLogic.helpers.Guid.create().toString();
                    options.onSuccess = function (options) {
                        mdBusinessLogic.globals.loggedOnUser = options.responseData;
                        mdBusinessLogic.globals.loggedOnUser.Token = mdBusinessLogic.helpers.encoder.base64.encode(mdBusinessLogic.globals.loggedOnUser.Username + ':' + mdBusinessLogic.globals.loggedOnUser.SessionId);
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                    return options.getRequestId();
                };
                userController.prototype.loginAuthData = function (data, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = false;
                    options.address = this.getAddress('LoginAuthData');
                    options.responseData = new dataAccess.entities.loggedOnUser();
                    options.requestData = data;
                    options.onSuccess = function (options) {
                        mdBusinessLogic.globals.loggedOnUser = options.responseData;
                        mdBusinessLogic.globals.loggedOnUser.Token = mdBusinessLogic.helpers.encoder.base64.encode(mdBusinessLogic.globals.loggedOnUser.Username + ':' + mdBusinessLogic.globals.loggedOnUser.SessionId);
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                    return options.getRequestId();
                };
                userController.prototype.logout = function (userLoggingOut, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = false;
                    options.address = this.getAddress('Logout');
                    options.responseData = new dataAccess.entities.loggedOnUser();
                    options.requestData = userLoggingOut;
                    options.onSuccess = function (options) {
                        if (onSuccess != undefined) {
                            onSuccess();
                        }
                        mdBusinessLogic.settings.ajax.connections.closeAll();
                    };
                    options.onError = function (options) {
                        if (onError != undefined) {
                            onError(options.exception);
                        }
                        mdBusinessLogic.settings.ajax.connections.closeAll();
                    };
                    this._post(options);
                };
                userController.prototype.getByToken = function (token, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('GetByToken', [encodeURIComponent(token)]);
                    options.responseData = new dataAccess.entities.user();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                userController.prototype.resetAccount = function (username, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('ResetAccount');
                    options.responseData = new dataAccess.entities.loggedOnUser();
                    options.requestData = new dataAccess.entities.user();
                    options.requestData.Username = username;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                userController.prototype.saveUserPermissions = function (permissionsData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.contentType = new controllers.base.AjaxMethodHeader('Content-Type', 'application/json; charset=UTF-8');
                    options.address = this.getAddress('SaveUserPermissions');
                    options.responseData = new dataAccess.entities.primitiveType();
                    options.requestData = permissionsData;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                userController.prototype.updateUser = function (user, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('UpdateUser');
                    options.responseData = new dataAccess.entities.user();
                    options.requestData = user;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                userController.prototype.updateAuthData = function (user, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('UpdateAuthData');
                    options.responseData = new dataAccess.entities.user();
                    options.requestData = user;
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._post(options);
                };
                userController.prototype.search = function (searchData, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = true;
                    options.isJsonArray = true;
                    options.address = this.getAddress('Search', searchData);
                    options.responseData = new dataAccess.entities.user();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseDataArray);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                userController.prototype.passwordReset = function (token, email, password, onSuccess, onError) {
                    var options = new controllers.base.AjaxMethodOptions();
                    options.includeAuthHeader = false;
                    options.address = this.getAddress('PasswordReset', [token, email, password]);
                    options.responseData = new dataAccess.entities.user();
                    options.onSuccess = function (options) {
                        onSuccess(options.responseData);
                    };
                    options.onError = function (options) {
                        onError(options.exception);
                    };
                    this._get(options);
                };
                userController.prototype.validateTokenSocket = function (requestId, token, onSuccess, onClose, onError) {
                    var options = new controllers.base.AjaxMethodOptions(requestId);
                    options.includeAuthHeader = true;
                    options.address = this.getAddress('ValidateTokenSocket');
                    options.responseData = new dataAccess.entities.user();
                    options.requestData = token;
                    options.onSuccess = function (response) {
                        onSuccess(response.responseData, response.socket);
                    };
                    options.onClose = function (response) {
                        onClose(response.socket);
                    };
                    options.onError = function (response) {
                        onError(response.exception, response.socket);
                    };
                    this._socket(options);
                    return options.getRequestId();
                };
                return userController;
            }(controllers.base.BaseController));
            controllers.userController = userController;
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var options;
            (function (options) {
                var v2;
                (function (v2) {
                    var enums;
                    (function (enums) {
                        var folderEnum;
                        (function (folderEnum) {
                            folderEnum[folderEnum["FolderId"] = 0] = "FolderId";
                            folderEnum[folderEnum["ParentId"] = 1] = "ParentId";
                            folderEnum[folderEnum["Name"] = 2] = "Name";
                            folderEnum[folderEnum["Description"] = 3] = "Description";
                            folderEnum[folderEnum["FolderPath"] = 4] = "FolderPath";
                        })(folderEnum = enums.folderEnum || (enums.folderEnum = {}));
                    })(enums = v2.enums || (v2.enums = {}));
                })(v2 = options.v2 || (options.v2 = {}));
            })(options = controllers.options || (controllers.options = {}));
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var options;
            (function (options) {
                var v2;
                (function (v2) {
                    var sortDirection;
                    (function (sortDirection) {
                        sortDirection[sortDirection["Ascending"] = 0] = "Ascending";
                        sortDirection[sortDirection["Descending"] = 1] = "Descending";
                    })(sortDirection = v2.sortDirection || (v2.sortDirection = {}));
                })(v2 = options.v2 || (options.v2 = {}));
            })(options = controllers.options || (controllers.options = {}));
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var actionSchedule = (function (_super) {
                __extends(actionSchedule, _super);
                function actionSchedule(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.ActionType = null;
                    _this.ExecutionType = executionScheduleType.Manual;
                    _this.ExecutionSecondsFrequency = 0;
                    _this.ExecutionStart = null;
                    _this.ExecutionEnd = null;
                    _this.Enabled = false;
                    if (obj !== undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                actionSchedule.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ActionType = this.getValue(data, "ActionType", null);
                    this.ExecutionType = this.getValue(data, "ExecutionType", 0);
                    this.ExecutionSecondsFrequency = this.getValue(data, "ExecutionSecondsFrequency", 0);
                    this.ExecutionStart = this.getValue(data, "ExecutionStart", null);
                    this.ExecutionEnd = this.getValue(data, "ExecutionEnd", null);
                    this.Enabled = this.getValue(data, "Enabled", false);
                };
                actionSchedule.prototype.clone = function () {
                    return new actionSchedule(this);
                };
                return actionSchedule;
            }(entities.base.BaseEntity));
            entities.actionSchedule = actionSchedule;
            var executionScheduleType;
            (function (executionScheduleType) {
                executionScheduleType[executionScheduleType["Manual"] = 0] = "Manual";
                executionScheduleType[executionScheduleType["Recurring"] = 1] = "Recurring";
            })(executionScheduleType = entities.executionScheduleType || (entities.executionScheduleType = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var aliasModel = (function () {
                function aliasModel(obj) {
                    this.Id = '';
                    this.Template = '';
                    this.Content = null;
                    this.Instance = new entities.content();
                    this.AliasType = aliasType.Content;
                    if (obj !== undefined && obj != null) {
                        this.construct(obj);
                    }
                }
                aliasModel.prototype.construct = function (data) {
                    this.Id = mdBusinessLogic.helpers.entityHelper.getValue(data, 'Id', '');
                    this.Template = mdBusinessLogic.helpers.entityHelper.getValue(data, 'Template', '');
                    this.AliasType = mdBusinessLogic.helpers.entityHelper.getValue(data, 'AliasType', aliasType.Content);
                    this.Content = mdBusinessLogic.helpers.entityHelper.getConstructEntityValue(data, 'Content', this.Instance);
                };
                aliasModel.prototype.clone = function () {
                    return new aliasModel(this);
                };
                return aliasModel;
            }());
            entities.aliasModel = aliasModel;
            var aliasType;
            (function (aliasType) {
                aliasType[aliasType["Content"] = 1] = "Content";
                aliasType[aliasType["Taxonomy"] = 2] = "Taxonomy";
                aliasType[aliasType["Folder"] = 3] = "Folder";
            })(aliasType = entities.aliasType || (entities.aliasType = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var attributeTypeEnum;
            (function (attributeTypeEnum) {
                attributeTypeEnum[attributeTypeEnum["input"] = 1] = "input";
                attributeTypeEnum[attributeTypeEnum["trueFalse"] = 2] = "trueFalse";
                attributeTypeEnum[attributeTypeEnum["textarea"] = 3] = "textarea";
                attributeTypeEnum[attributeTypeEnum["selectSingle"] = 4] = "selectSingle";
                attributeTypeEnum[attributeTypeEnum["selectMultiple"] = 5] = "selectMultiple";
                attributeTypeEnum[attributeTypeEnum["taxonomySelectorSingle"] = 6] = "taxonomySelectorSingle";
                attributeTypeEnum[attributeTypeEnum["taxonomySelectorMultiple"] = 7] = "taxonomySelectorMultiple";
                attributeTypeEnum[attributeTypeEnum["file"] = 8] = "file";
                attributeTypeEnum[attributeTypeEnum["date"] = 9] = "date";
                attributeTypeEnum[attributeTypeEnum["map"] = 10] = "map";
                attributeTypeEnum[attributeTypeEnum["contentSelectorSingle"] = 11] = "contentSelectorSingle";
                attributeTypeEnum[attributeTypeEnum["youtube"] = 12] = "youtube";
                attributeTypeEnum[attributeTypeEnum["section"] = 13] = "section";
                attributeTypeEnum[attributeTypeEnum["mediaContentSelectorSingle"] = 14] = "mediaContentSelectorSingle";
                attributeTypeEnum[attributeTypeEnum["userSelectorSingle"] = 15] = "userSelectorSingle";
                attributeTypeEnum[attributeTypeEnum["calculated"] = 16] = "calculated";
                attributeTypeEnum[attributeTypeEnum["tabbedSections"] = 17] = "tabbedSections";
            })(attributeTypeEnum = entities.attributeTypeEnum || (entities.attributeTypeEnum = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var contentTypeDataSourceJoin = (function (_super) {
                __extends(contentTypeDataSourceJoin, _super);
                function contentTypeDataSourceJoin(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.RightDataSourceId = 0;
                    _this.LeftRightDataSourceJoinType = '';
                    _this.LeftFieldId = 0;
                    _this.RightFieldId = 0;
                    if (obj !== undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                contentTypeDataSourceJoin.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.RightDataSourceId = mdBusinessLogic.helpers.entityHelper.getValue(data, 'RightDataSourceId', 0);
                    this.LeftRightDataSourceJoinType = mdBusinessLogic.helpers.entityHelper.getValue(data, 'LeftRightDataSourceJoinType', '');
                    this.LeftFieldId = mdBusinessLogic.helpers.entityHelper.getValue(data, 'LeftFieldId', 0);
                    this.RightFieldId = mdBusinessLogic.helpers.entityHelper.getValue(data, 'RightFieldId', 0);
                };
                contentTypeDataSourceJoin.prototype.clone = function () {
                    return new contentTypeDataSourceJoin(this);
                };
                return contentTypeDataSourceJoin;
            }(entities.base.BaseEntity));
            entities.contentTypeDataSourceJoin = contentTypeDataSourceJoin;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var contentTypeDataSource = (function (_super) {
                __extends(contentTypeDataSource, _super);
                function contentTypeDataSource(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.ConnectionString = '';
                    _this.ConnectionStringObject = null;
                    _this.DbType = '';
                    _this.ContentTypeDefinitionId = 0;
                    _this.CustomName = null;
                    if (obj !== undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                contentTypeDataSource.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ConnectionString = this.getValue(data, 'ConnectionString', '');
                    this.ConnectionStringObject = this.getValue(data, 'ConnectionStringObject', '');
                    this.DbType = this.getValue(data, 'DbType', '');
                    this.ContentTypeDefinitionId = this.getValue(data, 'ContentTypeDefinitionId', 0);
                };
                contentTypeDataSource.prototype.toString = function () {
                    if (this.CustomName !== undefined && this.CustomName != null && typeof this.CustomName) {
                        return this.CustomName;
                    }
                    return this.DbType + ' ' + this.ConnectionString;
                };
                contentTypeDataSource.prototype.clone = function () {
                    return new contentTypeDataSource(this);
                };
                return contentTypeDataSource;
            }(entities.base.BaseEntity));
            entities.contentTypeDataSource = contentTypeDataSource;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var contentTypeDefinitionFolderDataBoundCondition = (function () {
                function contentTypeDefinitionFolderDataBoundCondition(obj) {
                    this.ContentTypeDefinitionFieldId = 0;
                    this.Value = '';
                    this.ContentTypeDefinitionId = 0;
                    this.FolderId = 0;
                    this.Comparer = ComparerType.Equals;
                    if (obj != null) {
                        this.construct(obj);
                    }
                }
                contentTypeDefinitionFolderDataBoundCondition.prototype.construct = function (data) {
                    this.ContentTypeDefinitionFieldId = mdBusinessLogic.helpers.entityHelper.getValue(data, 'ContentTypeDefinitionFieldId', 0);
                    this.Value = mdBusinessLogic.helpers.entityHelper.getValue(data, 'Value', '');
                    this.ContentTypeDefinitionId = mdBusinessLogic.helpers.entityHelper.getValue(data, 'ContentTypeDefinitionId', 0);
                    this.FolderId = mdBusinessLogic.helpers.entityHelper.getValue(data, 'FolderId', 0);
                    this.Comparer = mdBusinessLogic.helpers.entityHelper.getValue(data, 'Comparer', ComparerType.Equals);
                };
                contentTypeDefinitionFolderDataBoundCondition.prototype.clone = function () {
                    return new contentTypeDefinitionFolderDataBoundCondition(this);
                };
                return contentTypeDefinitionFolderDataBoundCondition;
            }());
            entities.contentTypeDefinitionFolderDataBoundCondition = contentTypeDefinitionFolderDataBoundCondition;
            var ComparerType;
            (function (ComparerType) {
                ComparerType[ComparerType["Equals"] = 1] = "Equals";
                ComparerType[ComparerType["NotEquals"] = 2] = "NotEquals";
                ComparerType[ComparerType["Like"] = 3] = "Like";
                ComparerType[ComparerType["GreaterThan"] = 4] = "GreaterThan";
                ComparerType[ComparerType["GreaterThanOrEqualTo"] = 5] = "GreaterThanOrEqualTo";
                ComparerType[ComparerType["LessThan"] = 6] = "LessThan";
                ComparerType[ComparerType["LessThanOrEqualTo"] = 7] = "LessThanOrEqualTo";
            })(ComparerType = entities.ComparerType || (entities.ComparerType = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var contentTypeDefinitionFolderDataBoundSync = (function () {
                function contentTypeDefinitionFolderDataBoundSync(obj) {
                    this.FolderId = 0;
                    this.ContentTypeDefinitionId = 0;
                    this.StartTime = new Date();
                    this.EndTime = null;
                    this.Frequency = (60 * 60 * 12);
                    this.SyncType = contentTypeDefinitionFolderDataBoundSyncType.NoSync;
                    this.DeltaFieldId = null;
                    if (obj != null) {
                        this.construct(obj);
                    }
                }
                contentTypeDefinitionFolderDataBoundSync.prototype.construct = function (data) {
                    this.FolderId = mdBusinessLogic.helpers.entityHelper.getValue(data, 'FolderId', 0);
                    this.ContentTypeDefinitionId = mdBusinessLogic.helpers.entityHelper.getValue(data, 'ContentTypeDefinitionId', 0);
                    this.StartTime = mdBusinessLogic.helpers.entityHelper.getValue(data, 'StartTime', new Date());
                    this.EndTime = mdBusinessLogic.helpers.entityHelper.getValue(data, 'EndTime', null);
                    this.Frequency = mdBusinessLogic.helpers.entityHelper.getValue(data, 'Frequency', (60 * 60 * 12));
                    this.SyncType = mdBusinessLogic.helpers.entityHelper.getValue(data, 'SyncType', contentTypeDefinitionFolderDataBoundSyncType.NoSync);
                    this.DeltaFieldId = mdBusinessLogic.helpers.entityHelper.getValue(data, 'DeltaFieldId', null);
                };
                contentTypeDefinitionFolderDataBoundSync.prototype.clone = function () {
                    return new contentTypeDefinitionFolderDataBoundSync(this);
                };
                return contentTypeDefinitionFolderDataBoundSync;
            }());
            entities.contentTypeDefinitionFolderDataBoundSync = contentTypeDefinitionFolderDataBoundSync;
            var contentTypeDefinitionFolderDataBoundSyncType;
            (function (contentTypeDefinitionFolderDataBoundSyncType) {
                contentTypeDefinitionFolderDataBoundSyncType[contentTypeDefinitionFolderDataBoundSyncType["NoSync"] = 0] = "NoSync";
                contentTypeDefinitionFolderDataBoundSyncType[contentTypeDefinitionFolderDataBoundSyncType["RemoteToOmega"] = 1] = "RemoteToOmega";
                contentTypeDefinitionFolderDataBoundSyncType[contentTypeDefinitionFolderDataBoundSyncType["OmegaToRemote"] = 2] = "OmegaToRemote";
                contentTypeDefinitionFolderDataBoundSyncType[contentTypeDefinitionFolderDataBoundSyncType["Bidirectional"] = 3] = "Bidirectional";
            })(contentTypeDefinitionFolderDataBoundSyncType = entities.contentTypeDefinitionFolderDataBoundSyncType || (entities.contentTypeDefinitionFolderDataBoundSyncType = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var culture = (function (_super) {
                __extends(culture, _super);
                function culture(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.LCID = 0;
                    _this.Name = '';
                    _this.Code = '';
                    _this.IsoCode = '';
                    _this.IsApproved = false;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                culture.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.LCID = this.getValue(data, "LCID", 0);
                    this.Name = this.getValue(data, "Name", '');
                    this.Code = this.getValue(data, "Code", '');
                    this.IsoCode = this.getValue(data, "IsoCode", '');
                    this.IsApproved = this.getValue(data, "IsApproved", false);
                };
                culture.prototype.clone = function () {
                    return new culture(this);
                };
                return culture;
            }(entities.base.BaseEntity));
            entities.culture = culture;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var paginationEntity = (function () {
                function paginationEntity(type, obj) {
                    this.type = type;
                    this.Items = new Array();
                    this.TotalCount = 0;
                    if (obj !== undefined && obj != null) {
                        this.construct(obj);
                    }
                }
                paginationEntity.prototype.construct = function (data) {
                    this.Items = mdBusinessLogic.helpers.entityHelper.getArrayConstructEntityValue(data, "Items", new Array(), new this.type());
                    this.TotalCount = mdBusinessLogic.helpers.entityHelper.getValue(data, 'TotalCount', 0);
                };
                paginationEntity.prototype.clone = function () {
                    return new paginationEntity(this.type, this);
                };
                return paginationEntity;
            }());
            entities.paginationEntity = paginationEntity;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var primitiveType = (function (_super) {
                __extends(primitiveType, _super);
                function primitiveType(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.Value = null;
                    if (obj != undefined && obj != null) {
                        _this.Value = obj.Value;
                    }
                    return _this;
                }
                primitiveType.prototype.construct = function (value) {
                    if (value != undefined && value != null) {
                        if (!isNaN(parseInt(value.toString()))) {
                            this.Value = parseInt(value.toString());
                        }
                        else if (value.toString() === "true" || value.toString() === "false") {
                            this.Value = value.toString() === "true";
                        }
                        else {
                            try {
                                this.Value = JSON.parse(value.toString());
                            }
                            catch (e) {
                                this.Value = value;
                            }
                        }
                    }
                    else {
                        this.Value = value;
                    }
                };
                primitiveType.prototype.clone = function () {
                    return new primitiveType(this);
                };
                return primitiveType;
            }(entities.base.BaseEntity));
            entities.primitiveType = primitiveType;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var secureMessage = (function (_super) {
                __extends(secureMessage, _super);
                function secureMessage(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.EndPoint = '';
                    _this.Message = '';
                    _this.IsEncripted = false;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                secureMessage.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.EndPoint = this.getValue(data, "EndPoint", '');
                    this.Message = this.getValue(data, "Message", '');
                    this.IsEncripted = this.getValue(data, "IsEncripted", false);
                };
                secureMessage.prototype.clone = function () {
                    return new secureMessage(this);
                };
                return secureMessage;
            }(entities.base.BaseEntity));
            entities.secureMessage = secureMessage;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var templateScreenshot = (function (_super) {
                __extends(templateScreenshot, _super);
                function templateScreenshot(obj) {
                    var _this = _super.call(this, obj) || this;
                    _this.ScreenshotUrl = '';
                    _this.ScreenshotFile = '';
                    _this.ScreenshotWidth = 0;
                    _this.ScreenshotHeight = 0;
                    _this.Template = null;
                    if (obj != undefined && obj != null) {
                        _this.construct(obj);
                    }
                    return _this;
                }
                templateScreenshot.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ScreenshotUrl = this.getValue(data, 'ScreenshotUrl', '');
                    this.ScreenshotFile = this.getValue(data, 'ScreenshotFile', '');
                    this.ScreenshotWidth = this.getValue(data, 'ScreenshotWidth', 0);
                    this.ScreenshotHeight = this.getValue(data, 'ScreenshotHeight', 0);
                    this.Template = this.getConstructEntityValue(data, 'Template', null);
                };
                templateScreenshot.prototype.clone = function () {
                    return new templateScreenshot(this);
                };
                return templateScreenshot;
            }(entities.base.BaseEntity));
            entities.templateScreenshot = templateScreenshot;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var generic;
            (function (generic) {
                var extendedDateTime = (function () {
                    function extendedDateTime(data) {
                        this.maxDateTime = null;
                        this.minDateTime = null;
                        if (data) {
                            this.construct(data);
                        }
                    }
                    extendedDateTime.prototype.toDate = function () {
                        return mdBusinessLogic.helpers.entityHelper.parseDateValue(this.toString());
                    };
                    extendedDateTime.prototype.toString = function () {
                        return mdBusinessLogic.helpers.entityHelper.parseDateAndTimezoneToString(this.value, this.timezone);
                    };
                    extendedDateTime.prototype.construct = function (data) {
                        this.value = mdBusinessLogic.helpers.entityHelper.parseDateStringValue(data);
                        this.timezone = mdBusinessLogic.helpers.entityHelper.parseTimeZoneValue(data);
                    };
                    extendedDateTime.prototype.clone = function () {
                        return new extendedDateTime(this);
                    };
                    return extendedDateTime;
                }());
                generic.extendedDateTime = extendedDateTime;
            })(generic = entities.generic || (entities.generic = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var generic;
            (function (generic) {
                var genericKeyValuePair = (function () {
                    function genericKeyValuePair(obj) {
                        this.Key = '';
                        this.Value = null;
                        if (obj != undefined && obj != null) {
                            this.construct(obj);
                        }
                    }
                    genericKeyValuePair.prototype.construct = function (data) {
                        this.Key = mdBusinessLogic.helpers.entityHelper.getValue(data, "Key", '');
                        this.Value = mdBusinessLogic.helpers.entityHelper.getValue(data, "Value", null);
                    };
                    genericKeyValuePair.prototype.clone = function () {
                        return new genericKeyValuePair(this);
                    };
                    return genericKeyValuePair;
                }());
                generic.genericKeyValuePair = genericKeyValuePair;
            })(generic = entities.generic || (entities.generic = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var generic;
            (function (generic) {
                var genericCollection = (function () {
                    function genericCollection(obj) {
                        this.Collection = new Array();
                        if (obj && obj.Collection) {
                            this.Collection = obj.Collection;
                        }
                    }
                    genericCollection.prototype.getCollection = function () {
                        return this.Collection;
                    };
                    genericCollection.prototype.remove = function (key) {
                        for (var i = this.Collection.length - 1; i >= 0; i--) {
                            if (this.Collection[i].Key == key) {
                                this.Collection.splice(i, 1);
                                break;
                            }
                        }
                    };
                    genericCollection.prototype.add = function (key, value) {
                        var constraint = this.getKeyValuePair(key);
                        if (constraint) {
                            constraint.Value = value;
                        }
                        else {
                            this.Collection.push(new generic.genericKeyValuePair({
                                Key: key,
                                Value: value
                            }));
                        }
                    };
                    genericCollection.prototype.get = function (key) {
                        var constraint = this.getKeyValuePair(key);
                        if (constraint) {
                            return constraint.Value;
                        }
                        return null;
                    };
                    genericCollection.prototype.getKeyValuePair = function (key) {
                        var constraint = this.Collection.filter(function (constraint) { return constraint.Key == key; })[0];
                        if (constraint) {
                            return constraint;
                        }
                        return null;
                    };
                    genericCollection.prototype.construct = function (data) {
                        this.Collection = mdBusinessLogic.helpers.entityHelper.getValue(data, "Collection", new Array());
                    };
                    genericCollection.prototype.clone = function () {
                        return new genericCollection({
                            Collection: this.getCollection()
                        });
                    };
                    return genericCollection;
                }());
                generic.genericCollection = genericCollection;
            })(generic = entities.generic || (entities.generic = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var generic;
            (function (generic) {
                var keyValuePair = (function (_super) {
                    __extends(keyValuePair, _super);
                    function keyValuePair(obj) {
                        return _super.call(this, obj) || this;
                    }
                    keyValuePair.prototype.clone = function () {
                        return new keyValuePair(this);
                    };
                    return keyValuePair;
                }(generic.genericKeyValuePair));
                generic.keyValuePair = keyValuePair;
            })(generic = entities.generic || (entities.generic = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var grid;
            (function (grid) {
                var gridTileData = (function () {
                    function gridTileData(obj) {
                        this.width = 0;
                        this.width_medium = 0;
                        this.width_small = 0;
                        this.height = 0;
                        this.height_medium = 0;
                        this.height_small = 0;
                        this.minWidth = 10;
                        this.minHeight = 100;
                        this.id = mdBusinessLogic.helpers.Guid.create().toString();
                        this.parentId = undefined;
                        this.uniqueId = this.id;
                        this.index = 0;
                        this.layout = gridTileLayout.Row;
                        this.whiteframe = 4;
                        this.layoutPadding = true;
                        this.layoutMargin = true;
                        this.layoutWrap = true;
                        this.x = 0;
                        this.y = 0;
                        if (obj != undefined && obj != null) {
                            this.construct(obj);
                        }
                    }
                    gridTileData.prototype.construct = function (data) {
                        this.width = mdBusinessLogic.helpers.entityHelper.getValue(data, 'width', 0);
                        this.width_medium = mdBusinessLogic.helpers.entityHelper.getValue(data, 'width_medium', 0);
                        this.width_small = mdBusinessLogic.helpers.entityHelper.getValue(data, 'width_small', 0);
                        this.height = mdBusinessLogic.helpers.entityHelper.getValue(data, 'height', 0);
                        this.height_medium = mdBusinessLogic.helpers.entityHelper.getValue(data, 'height_medium', 0);
                        this.height_small = mdBusinessLogic.helpers.entityHelper.getValue(data, 'height_small', 0);
                        this.minWidth = mdBusinessLogic.helpers.entityHelper.getValue(data, 'minWidth', this.minWidth);
                        this.minHeight = mdBusinessLogic.helpers.entityHelper.getValue(data, 'minHeight', this.minHeight);
                        this.id = mdBusinessLogic.helpers.entityHelper.getValue(data, 'id', '');
                        if (this.id == '') {
                            this.id = mdBusinessLogic.helpers.Guid.create().toString();
                        }
                        this.uniqueId = this.id;
                        this.parentId = mdBusinessLogic.helpers.entityHelper.getValue(data, 'parentId', '');
                        if (this.parentId == '') {
                            this.parentId = undefined;
                        }
                        this.index = mdBusinessLogic.helpers.entityHelper.getValue(data, 'index', 0);
                        this.layout = mdBusinessLogic.helpers.entityHelper.getValue(data, 'layout', gridTileLayout.Row);
                        this.whiteframe = mdBusinessLogic.helpers.entityHelper.getValue(data, 'whiteframe', 0);
                        this.layoutPadding = mdBusinessLogic.helpers.entityHelper.getValue(data, 'layoutPadding', true);
                        this.layoutMargin = mdBusinessLogic.helpers.entityHelper.getValue(data, 'layoutMargin', true);
                        this.layoutWrap = mdBusinessLogic.helpers.entityHelper.getValue(data, 'layoutWrap', true);
                        this.x = mdBusinessLogic.helpers.entityHelper.getValue(data, 'x', 0);
                        this.y = mdBusinessLogic.helpers.entityHelper.getValue(data, 'y', 0);
                    };
                    gridTileData.prototype.clone = function () {
                        return new gridTileData(this);
                    };
                    gridTileData.prototype.setMinHeight = function (val) {
                        this.minHeight = val;
                    };
                    gridTileData.prototype.setMinWidth = function (val) {
                        this.minWidth = val;
                    };
                    gridTileData.prototype.getWidth = function (size) {
                        if (size === undefined || size == '') {
                            size = '';
                        }
                        else {
                            size = '_' + size;
                        }
                        var value = this['width' + size];
                        if (value <= 10) {
                            value = value * 10;
                        }
                        if (value > this.minWidth) {
                            return value;
                        }
                        return this.minWidth;
                    };
                    gridTileData.prototype.getHeight = function (size) {
                        if (size === undefined || size == '') {
                            size = '';
                        }
                        else {
                            size = '_' + size;
                        }
                        var value = this['height' + size];
                        if (value <= 10) {
                            value = value * 10;
                        }
                        if (value > this.minHeight) {
                            return value;
                        }
                        return this.minHeight;
                    };
                    gridTileData.prototype.setWidth = function (width, size) {
                        if (isNaN(width)) {
                            return;
                        }
                        if (size === undefined || size == '') {
                            size = '';
                        }
                        else {
                            size = '_' + size;
                        }
                        if (width < this.minWidth) {
                            this['width' + size] = this.minWidth;
                        }
                        else {
                            this['width' + size] = width;
                        }
                    };
                    gridTileData.prototype.setHeight = function (height, size) {
                        if (isNaN(height)) {
                            return;
                        }
                        if (size === undefined || size == '') {
                            size = '';
                        }
                        else {
                            size = '_' + size;
                        }
                        if (height < this.minHeight) {
                            this['height' + size] = this.minHeight;
                        }
                        else {
                            this['height' + size] = height;
                        }
                    };
                    return gridTileData;
                }());
                grid.gridTileData = gridTileData;
                var gridTileLayout;
                (function (gridTileLayout) {
                    gridTileLayout[gridTileLayout["Row"] = 1] = "Row";
                    gridTileLayout[gridTileLayout["Column"] = 2] = "Column";
                })(gridTileLayout = grid.gridTileLayout || (grid.gridTileLayout = {}));
            })(grid = entities.grid || (entities.grid = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var genericContent;
            (function (genericContent) {
                var genericContentFieldJsonField = (function () {
                    function genericContentFieldJsonField(obj) {
                        this.validation = new entities.fieldValidation();
                        this.helpText = '';
                        this.access = '';
                        this.cssClass = '';
                        this.toggle = '';
                        this.hidden = false;
                        this.enabled = true;
                        this.gridTileData = new mdBusinessLogic.dataAccess.entities.grid.gridTileData();
                        this.style = {};
                        this.constraints = new entities.generic.genericCollection();
                        this.metadata = new Array();
                        this.linkToTitle = false;
                        if (obj != undefined && obj != null) {
                            this.construct(obj);
                            if (this.style === undefined || this.style == null) {
                                this.style = {};
                            }
                            if (this.constraints === undefined || this.constraints == null) {
                                this.constraints = new entities.generic.genericCollection();
                            }
                        }
                    }
                    genericContentFieldJsonField.prototype.construct = function (data) {
                        this.validation = mdBusinessLogic.helpers.entityHelper.getConstructValue(data, 'validation', new entities.fieldValidation());
                        this.helpText = mdBusinessLogic.helpers.entityHelper.getValue(data, 'helpText', '');
                        this.access = mdBusinessLogic.helpers.entityHelper.getValue(data, 'access', '');
                        this.cssClass = mdBusinessLogic.helpers.entityHelper.getValue(data, 'cssClass', '');
                        this.toggle = mdBusinessLogic.helpers.entityHelper.getValue(data, 'toggle', '');
                        this.hidden = mdBusinessLogic.helpers.entityHelper.getValue(data, 'hidden', false);
                        this.enabled = mdBusinessLogic.helpers.entityHelper.getValue(data, 'enabled', true);
                        this.gridTileData = mdBusinessLogic.helpers.entityHelper.getConstructValue(data, 'gridTileData', new mdBusinessLogic.dataAccess.entities.grid.gridTileData());
                        this.style = mdBusinessLogic.helpers.entityHelper.getValue(data, 'style', {});
                        this.constraints = mdBusinessLogic.helpers.entityHelper.getConstructValue(data, 'constraints', new entities.generic.genericCollection());
                        this.metadata = mdBusinessLogic.helpers.entityHelper.getValue(data, 'metadata', new Array());
                        this.linkToTitle = mdBusinessLogic.helpers.entityHelper.getValue(data, 'linkToTitle', false);
                    };
                    genericContentFieldJsonField.prototype.clone = function () {
                        return new genericContentFieldJsonField(this);
                    };
                    genericContentFieldJsonField.prototype.getStyle = function (attributeType) {
                        return this.style[entities.attributeTypeEnum[attributeType]];
                    };
                    genericContentFieldJsonField.prototype.getConstraint = function (key) {
                        var constraint = this.constraints.get(key);
                        if (constraint) {
                            if (!constraint.contentIds) {
                                constraint.contentIds = [];
                            }
                            if (!constraint.contentTypeId) {
                                constraint.contentTypeId = '';
                            }
                            if (!constraint.folderPaths) {
                                constraint.folderPaths = [];
                            }
                            if (!constraint.menuPaths) {
                                constraint.menuPaths = [];
                            }
                            if (!constraint.profileId) {
                                constraint.profileId = '';
                            }
                            if (!constraint.taxonomyIds) {
                                constraint.taxonomyIds = [];
                            }
                            if (!constraint.userIds) {
                                constraint.userIds = [];
                            }
                            return constraint;
                        }
                        return null;
                    };
                    genericContentFieldJsonField.prototype.getDefaultConstraint = function () {
                        var constraint = this.getConstraint('default');
                        if (!constraint) {
                            constraint = {
                                contentIds: [],
                                contentTypeId: '',
                                folderPaths: [],
                                menuPaths: [],
                                profileId: '',
                                taxonomyIds: [],
                                userIds: []
                            };
                            this.constraints.add('default', constraint);
                        }
                        else {
                            constraint.contentIds = constraint.contentIds.filter(function (c) { return c.length; });
                            constraint.folderPaths = constraint.folderPaths.filter(function (c) { return c.length; });
                            constraint.menuPaths = constraint.menuPaths.filter(function (c) { return c.length; });
                            constraint.taxonomyIds = constraint.taxonomyIds.filter(function (c) { return c.length; });
                            constraint.userIds = constraint.userIds.filter(function (c) { return c.length; });
                        }
                        return constraint;
                    };
                    genericContentFieldJsonField.prototype.setDefaultConstraint = function (value) {
                        this.constraints.add('default', value);
                    };
                    genericContentFieldJsonField.prototype.getRelevantConstraint = function () {
                        var constraint = this.getDefaultConstraint();
                        return constraint;
                    };
                    return genericContentFieldJsonField;
                }());
                genericContent.genericContentFieldJsonField = genericContentFieldJsonField;
            })(genericContent = entities.genericContent || (entities.genericContent = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var models;
            (function (models) {
                var initModel = (function () {
                    function initModel(obj) {
                        this.Initiated = false;
                        if (obj !== undefined && obj != null) {
                            this.construct(obj);
                        }
                    }
                    initModel.prototype.construct = function (data) {
                        this.Initiated = mdBusinessLogic.helpers.entityHelper.getValue(data, "Initiated", false);
                    };
                    initModel.prototype.clone = function () {
                        return new initModel(this);
                    };
                    return initModel;
                }());
                models.initModel = initModel;
            })(models = entities.models || (entities.models = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var permissions;
            (function (permissions) {
                var permissionObjectEnum;
                (function (permissionObjectEnum) {
                    permissionObjectEnum[permissionObjectEnum["None"] = 0] = "None";
                    permissionObjectEnum[permissionObjectEnum["Folder"] = 1] = "Folder";
                    permissionObjectEnum[permissionObjectEnum["Content"] = 2] = "Content";
                    permissionObjectEnum[permissionObjectEnum["MediaContent"] = 3] = "MediaContent";
                })(permissionObjectEnum = permissions.permissionObjectEnum || (permissions.permissionObjectEnum = {}));
            })(permissions = entities.permissions || (entities.permissions = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var permissions;
            (function (permissions) {
                var permissionObjectTypeEnum;
                (function (permissionObjectTypeEnum) {
                    permissionObjectTypeEnum[permissionObjectTypeEnum["User"] = 0] = "User";
                    permissionObjectTypeEnum[permissionObjectTypeEnum["ProfileType"] = 1] = "ProfileType";
                })(permissionObjectTypeEnum = permissions.permissionObjectTypeEnum || (permissions.permissionObjectTypeEnum = {}));
            })(permissions = entities.permissions || (entities.permissions = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var permissions;
            (function (permissions) {
                var permissionTypeEnum;
                (function (permissionTypeEnum) {
                    permissionTypeEnum[permissionTypeEnum["Api"] = 1] = "Api";
                    permissionTypeEnum[permissionTypeEnum["Object"] = 2] = "Object";
                })(permissionTypeEnum = permissions.permissionTypeEnum || (permissions.permissionTypeEnum = {}));
            })(permissions = entities.permissions || (entities.permissions = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var search;
            (function (search) {
                var baseSearch = (function () {
                    function baseSearch(obj) {
                        this.Id = 0;
                        this.Name = '';
                        this.TableName = '';
                        if (obj !== undefined && obj != null) {
                            this.Id = obj.Id;
                            this.Name = obj.Name;
                            this.TableName = obj.TableName;
                        }
                    }
                    baseSearch.prototype.construct = function (data) {
                        this.Id = mdBusinessLogic.helpers.entityHelper.getValue(data, 'Id', 0);
                        this.Name = mdBusinessLogic.helpers.entityHelper.getValue(data, 'Name', '');
                        this.TableName = mdBusinessLogic.helpers.entityHelper.getValue(data, 'TableName', '');
                    };
                    return baseSearch;
                }());
                search.baseSearch = baseSearch;
            })(search = entities.search || (entities.search = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var search;
            (function (search) {
                var content = (function (_super) {
                    __extends(content, _super);
                    function content(obj) {
                        var _this = _super.call(this, obj) || this;
                        _this.Path = '';
                        _this.DateCreated = null;
                        _this.FolderId = 0;
                        if (obj !== undefined && obj != null) {
                            _this.Path = obj.Path;
                            _this.DateCreated = obj.DateCreated;
                            _this.FolderId = obj.FolderId;
                        }
                        return _this;
                    }
                    content.prototype.construct = function (data) {
                        _super.prototype.construct.call(this, data);
                        this.Path = mdBusinessLogic.helpers.entityHelper.getValue(data, 'Path', '');
                        this.DateCreated = mdBusinessLogic.helpers.entityHelper.getValue(data, 'DateCreated', null);
                        this.FolderId = mdBusinessLogic.helpers.entityHelper.getValue(data, 'FolderId', 0);
                    };
                    content.prototype.clone = function () {
                        return new content(this);
                    };
                    return content;
                }(search.baseSearch));
                search.content = content;
            })(search = entities.search || (entities.search = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var search;
            (function (search) {
                var contentType = (function (_super) {
                    __extends(contentType, _super);
                    function contentType(obj) {
                        return _super.call(this, obj) || this;
                    }
                    contentType.prototype.construct = function (data) {
                        _super.prototype.construct.call(this, data);
                    };
                    contentType.prototype.clone = function () {
                        return new contentType(this);
                    };
                    return contentType;
                }(search.baseSearch));
                search.contentType = contentType;
            })(search = entities.search || (entities.search = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var search;
            (function (search) {
                var folder = (function (_super) {
                    __extends(folder, _super);
                    function folder(obj) {
                        var _this = _super.call(this, obj) || this;
                        _this.Path = '';
                        if (obj !== undefined && obj != null) {
                            _this.Path = obj.Path;
                        }
                        return _this;
                    }
                    folder.prototype.construct = function (data) {
                        _super.prototype.construct.call(this, data);
                        this.Path = mdBusinessLogic.helpers.entityHelper.getValue(data, 'Path', '');
                    };
                    folder.prototype.clone = function () {
                        return new folder(this);
                    };
                    return folder;
                }(search.baseSearch));
                search.folder = folder;
            })(search = entities.search || (entities.search = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var search;
            (function (search) {
                var mediaContent = (function (_super) {
                    __extends(mediaContent, _super);
                    function mediaContent(obj) {
                        var _this = _super.call(this, obj) || this;
                        _this.Path = '';
                        _this.DateCreated = null;
                        _this.FolderId = 0;
                        _this.FileType = null;
                        _this.FileName = '';
                        if (obj !== undefined && obj != null) {
                            _this.Path = obj.Path;
                            _this.DateCreated = obj.DateCreated;
                            _this.FolderId = obj.FolderId;
                            _this.FileType = obj.FileType;
                            _this.FileName = obj.FileName;
                        }
                        return _this;
                    }
                    mediaContent.prototype.construct = function (data) {
                        _super.prototype.construct.call(this, data);
                        this.Path = mdBusinessLogic.helpers.entityHelper.getValue(data, 'Path', '');
                        this.DateCreated = mdBusinessLogic.helpers.entityHelper.getValue(data, 'DateCreated', null);
                        this.FolderId = mdBusinessLogic.helpers.entityHelper.getValue(data, 'FolderId', 0);
                        this.FileType = mdBusinessLogic.helpers.entityHelper.getValue(data, 'FileType', 0);
                        this.FileName = mdBusinessLogic.helpers.entityHelper.getValue(data, 'FileName', '');
                    };
                    mediaContent.prototype.clone = function () {
                        return new mediaContent(this);
                    };
                    return mediaContent;
                }(search.baseSearch));
                search.mediaContent = mediaContent;
            })(search = entities.search || (entities.search = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var search;
            (function (search) {
                var menu = (function (_super) {
                    __extends(menu, _super);
                    function menu(obj) {
                        var _this = _super.call(this, obj) || this;
                        _this.Path = '';
                        if (obj !== undefined && obj != null) {
                            _this.Path = obj.Path;
                        }
                        return _this;
                    }
                    menu.prototype.construct = function (data) {
                        _super.prototype.construct.call(this, data);
                        this.Path = mdBusinessLogic.helpers.entityHelper.getValue(data, 'Path', '');
                    };
                    menu.prototype.clone = function () {
                        return new menu(this);
                    };
                    return menu;
                }(search.baseSearch));
                search.menu = menu;
            })(search = entities.search || (entities.search = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var search;
            (function (search) {
                var profileType = (function (_super) {
                    __extends(profileType, _super);
                    function profileType(obj) {
                        return _super.call(this, obj) || this;
                    }
                    profileType.prototype.construct = function (data) {
                        _super.prototype.construct.call(this, data);
                    };
                    profileType.prototype.clone = function () {
                        return new profileType(this);
                    };
                    return profileType;
                }(search.baseSearch));
                search.profileType = profileType;
            })(search = entities.search || (entities.search = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var search;
            (function (search) {
                var searchResults = (function () {
                    function searchResults(obj) {
                        this.Folders = new Array();
                        this.Taxonomies = new Array();
                        this.Menus = new Array();
                        this.Contents = new Array();
                        this.ContentTypes = new Array();
                        this.ProfileTypes = new Array();
                        this.MediaContents = new Array();
                        if (obj !== undefined && obj != null) {
                            this.Folders = obj.Folders;
                            this.Taxonomies = obj.Taxonomies;
                            this.Menus = obj.Menus;
                            this.Contents = obj.Contents;
                            this.ContentTypes = obj.ContentTypes;
                            this.ProfileTypes = obj.ProfileTypes;
                            this.MediaContents = obj.MediaContents;
                        }
                    }
                    searchResults.prototype.construct = function (data) {
                        this.Folders = mdBusinessLogic.helpers.entityHelper.getArrayConstructValue(data, 'Folders', new Array(), new search.folder());
                        this.Taxonomies = mdBusinessLogic.helpers.entityHelper.getArrayConstructValue(data, 'Taxonomies', new Array(), new search.taxonomy());
                        this.Menus = mdBusinessLogic.helpers.entityHelper.getArrayConstructValue(data, 'Menus', new Array(), new search.menu());
                        this.Contents = mdBusinessLogic.helpers.entityHelper.getArrayConstructValue(data, 'Contents', new Array(), new search.content());
                        this.ContentTypes = mdBusinessLogic.helpers.entityHelper.getArrayConstructValue(data, 'ContentTypes', new Array(), new search.contentType());
                        this.ProfileTypes = mdBusinessLogic.helpers.entityHelper.getArrayConstructValue(data, 'ProfileTypes', new Array(), new search.profileType());
                        this.MediaContents = mdBusinessLogic.helpers.entityHelper.getArrayConstructValue(data, 'MediaContents', new Array(), new search.mediaContent());
                    };
                    searchResults.prototype.clone = function () {
                        return new searchResults(this);
                    };
                    return searchResults;
                }());
                search.searchResults = searchResults;
            })(search = entities.search || (entities.search = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var search;
            (function (search) {
                var taxonomy = (function (_super) {
                    __extends(taxonomy, _super);
                    function taxonomy(obj) {
                        var _this = _super.call(this, obj) || this;
                        _this.Path = '';
                        if (obj !== undefined && obj != null) {
                            _this.Path = obj.Path;
                        }
                        return _this;
                    }
                    taxonomy.prototype.construct = function (data) {
                        _super.prototype.construct.call(this, data);
                        this.Path = mdBusinessLogic.helpers.entityHelper.getValue(data, 'Path', '');
                    };
                    taxonomy.prototype.clone = function () {
                        return new taxonomy(this);
                    };
                    return taxonomy;
                }(search.baseSearch));
                search.taxonomy = taxonomy;
            })(search = entities.search || (entities.search = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var search;
            (function (search) {
                var user = (function (_super) {
                    __extends(user, _super);
                    function user(obj) {
                        return _super.call(this, obj) || this;
                    }
                    user.prototype.construct = function (data) {
                        _super.prototype.construct.call(this, data);
                    };
                    user.prototype.clone = function () {
                        return new user(this);
                    };
                    return user;
                }(search.baseSearch));
                search.user = user;
            })(search = entities.search || (entities.search = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var providers;
        (function (providers) {
            var authentication;
            (function (authentication) {
                var authData = (function () {
                    function authData(obj) {
                        this.Values = {};
                        this.AuthenticationProviderName = '';
                        if (obj !== undefined && obj != null) {
                            this.Values = obj.Values;
                            this.AuthenticationProviderName = obj.AuthenticationProviderName;
                        }
                    }
                    authData.prototype.construct = function (data) {
                        this.Values = mdBusinessLogic.helpers.entityHelper.getValue(data, 'Values', {});
                        this.AuthenticationProviderName = mdBusinessLogic.helpers.entityHelper.getValue(data, 'AuthenticationProviderName', '');
                    };
                    authData.prototype.clone = function () {
                        return new authData(this);
                    };
                    authData.prototype.GetData = function (key, defaultValue) {
                        if (defaultValue === void 0) { defaultValue = ''; }
                        return mdBusinessLogic.helpers.entityHelper.getValue(this.Values, key, defaultValue);
                    };
                    authData.prototype.SetData = function (key, value) {
                        this.Values[key] = value;
                    };
                    return authData;
                }());
                authentication.authData = authData;
            })(authentication = providers.authentication || (providers.authentication = {}));
        })(providers = dataAccess.providers || (dataAccess.providers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var providers;
        (function (providers) {
            var authentication;
            (function (authentication) {
                var authMode;
                (function (authMode) {
                    authMode[authMode["login"] = 0] = "login";
                    authMode[authMode["form"] = 1] = "form";
                })(authMode = authentication.authMode || (authentication.authMode = {}));
            })(authentication = providers.authentication || (providers.authentication = {}));
        })(providers = dataAccess.providers || (dataAccess.providers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var providers;
        (function (providers) {
            var authentication;
            (function (authentication) {
                var authenticationProviderRegistry = (function () {
                    function authenticationProviderRegistry() {
                    }
                    authenticationProviderRegistry.add = function (obj) {
                        if (obj.id === undefined || obj.id == null || obj.id.trim().length == 0) {
                            console.error('Attemptd to register illegal new registry authentication provider, missing id property!');
                            return false;
                        }
                        if (obj.name === undefined || obj.name == null || obj.name.trim().length == 0) {
                            console.error('Attemptd to register illegal new registry authentication provider, missing name property!');
                            return false;
                        }
                        if (obj.shortcode === undefined || obj.shortcode == null || obj.shortcode.trim().length == 0) {
                            console.error('Attemptd to register illegal new registry authentication provider, missing shortcode property!');
                            return false;
                        }
                        if (authenticationProviderRegistry._authenticationProviderRegistry.get(obj.name) != null) {
                            console.error('Attemptd to register illegal new registry authentication provider, provider exists!');
                            return false;
                        }
                        if (mdBusinessLogic.globals.enabledAuthenticationProviders.filter(function (enabledProvider) {
                            return enabledProvider === obj.id;
                        }).length == 0) {
                            return false;
                        }
                        if (obj.data === undefined || obj.data == null) {
                            obj.data = {};
                        }
                        authenticationProviderRegistry._authenticationProviderRegistry.add(obj.id, obj);
                        return true;
                    };
                    authenticationProviderRegistry.get = function (key) {
                        if (authenticationProviderRegistry._authenticationProviderRegistry.get(key) == null) {
                            console.error('Authentication provider dost not exists!');
                        }
                        if (mdBusinessLogic.globals.enabledAuthenticationProviders.filter(function (enabledProvider) {
                            return enabledProvider === key;
                        }).length == 0) {
                            console.error('Authentication provider is not enabled!');
                            return null;
                        }
                        return authenticationProviderRegistry._authenticationProviderRegistry.get(key);
                    };
                    authenticationProviderRegistry.getAll = function () {
                        return authenticationProviderRegistry._authenticationProviderRegistry.getCollection().filter(function (provider) {
                            return mdBusinessLogic.globals.enabledAuthenticationProviders.filter(function (enabledProvider) {
                                return enabledProvider === provider.Value.id;
                            }).length > 0;
                        });
                    };
                    authenticationProviderRegistry._authenticationProviderRegistry = new dataAccess.entities.generic.genericCollection();
                    return authenticationProviderRegistry;
                }());
                authentication.authenticationProviderRegistry = authenticationProviderRegistry;
            })(authentication = providers.authentication || (providers.authentication = {}));
        })(providers = dataAccess.providers || (dataAccess.providers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var providers;
        (function (providers) {
            var authentication;
            (function (authentication) {
                var builtIn = (function () {
                    function builtIn() {
                    }
                    builtIn.getAuthenticationProviderId = function () {
                        return 'BuiltInAuthenticationProvider';
                    };
                    return builtIn;
                }());
                authentication.builtIn = builtIn;
            })(authentication = providers.authentication || (providers.authentication = {}));
        })(providers = dataAccess.providers || (dataAccess.providers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var providers;
        (function (providers) {
            var dataaccess;
            (function (dataaccess) {
                var dataaccessObjectRegistry = (function () {
                    function dataaccessObjectRegistry() {
                    }
                    dataaccessObjectRegistry.add = function (obj) {
                        if (obj.getId() === undefined || obj.getId() == null || obj.getId().trim().length == 0) {
                            console.error('Attemptd to register illegal new registry dataaccess plugin, missing id property!');
                            return false;
                        }
                        if (mdBusinessLogic.globals.enabledAuthenticationProviders.filter(function (enabledProvider) {
                            return enabledProvider === obj.getId();
                        }).length == 0) {
                            return false;
                        }
                        dataaccessObjectRegistry._dataaccessObjectRegistry.add(obj.getId(), obj);
                        return true;
                    };
                    dataaccessObjectRegistry.get = function (key) {
                        if (dataaccessObjectRegistry._dataaccessObjectRegistry.get(key) == null) {
                            console.error('DataAccess plugin dost not exists!');
                        }
                        if (mdBusinessLogic.globals.enabledAuthenticationProviders.filter(function (enabledProvider) {
                            return enabledProvider === key;
                        }).length == 0) {
                            console.error('DataAccess plugin is not enabled!');
                            return null;
                        }
                        return dataaccessObjectRegistry._dataaccessObjectRegistry.get(key);
                    };
                    dataaccessObjectRegistry.getAll = function () {
                        return dataaccessObjectRegistry._dataaccessObjectRegistry.getCollection().filter(function (provider) {
                            return mdBusinessLogic.globals.enabledAuthenticationProviders.filter(function (enabledProvider) {
                                return enabledProvider === provider.Value.getId();
                            }).length > 0;
                        });
                    };
                    dataaccessObjectRegistry._dataaccessObjectRegistry = new dataAccess.entities.generic.genericCollection();
                    return dataaccessObjectRegistry;
                }());
                dataaccess.dataaccessObjectRegistry = dataaccessObjectRegistry;
            })(dataaccess = providers.dataaccess || (providers.dataaccess = {}));
        })(providers = dataAccess.providers || (dataAccess.providers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var providers;
        (function (providers) {
            var uiVisibility;
            (function (uiVisibility) {
                var iUiVisiblityType;
                (function (iUiVisiblityType) {
                    iUiVisiblityType[iUiVisiblityType["User"] = 0] = "User";
                    iUiVisiblityType[iUiVisiblityType["Profile"] = 1] = "Profile";
                })(iUiVisiblityType = uiVisibility.iUiVisiblityType || (uiVisibility.iUiVisiblityType = {}));
                var uiVisibilityProviderRegistry = (function () {
                    function uiVisibilityProviderRegistry() {
                    }
                    uiVisibilityProviderRegistry.getUniqueName = function (name, type, id) {
                        return name + '_' + type.valueOf() + '_' + id;
                    };
                    uiVisibilityProviderRegistry.add = function (obj) {
                        if (obj.id === undefined || obj.id == null || obj.id.trim().length == 0) {
                            console.error('Attemptd to register illegal new registry ui visibility setting, missing id property!');
                            return false;
                        }
                        if (obj.name === undefined || obj.name == null || obj.name.trim().length == 0) {
                            console.error('Attemptd to register illegal new registry ui visibility setting, missing name property!');
                            return false;
                        }
                        if (obj.type === undefined || obj.type == null) {
                            console.error('Attemptd to register illegal new registry ui visibility setting, missing type property!');
                            return false;
                        }
                        if (obj.visible === undefined || obj.visible == null) {
                            console.error('Attemptd to register illegal new registry ui visibility setting, missing visible property!');
                            return false;
                        }
                        uiVisibilityProviderRegistry._uiVisiblitySettingsRegistry.add(uiVisibilityProviderRegistry.getUniqueName(obj.name, obj.type, obj.id), obj);
                        return true;
                    };
                    uiVisibilityProviderRegistry.get = function (key) {
                        return uiVisibilityProviderRegistry._uiVisiblitySettingsRegistry.get(key);
                    };
                    uiVisibilityProviderRegistry.getAll = function () {
                        return uiVisibilityProviderRegistry._uiVisiblitySettingsRegistry.getCollection();
                    };
                    uiVisibilityProviderRegistry._uiVisiblitySettingsRegistry = new dataAccess.entities.generic.genericCollection();
                    return uiVisibilityProviderRegistry;
                }());
                uiVisibility.uiVisibilityProviderRegistry = uiVisibilityProviderRegistry;
            })(uiVisibility = providers.uiVisibility || (providers.uiVisibility = {}));
        })(providers = dataAccess.providers || (dataAccess.providers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var query;
        (function (query) {
            var queryExpressionGeneric = (function () {
                function queryExpressionGeneric(transform) {
                    this.transform = transform;
                }
                queryExpressionGeneric.prototype.compareGeneric = function (obj, comparer) {
                    obj.comparer = comparer;
                    return obj;
                };
                queryExpressionGeneric.prototype.withValueGeneric = function (obj, value) {
                    obj.value = value;
                    return obj;
                };
                queryExpressionGeneric.prototype.execute = function (onSuccess, onError) {
                    (new dataAccess.controllers.contentTypeDefinitionFieldValueController()).getByValue(this.value.toString(), this.contentType.Id, this.field.Id, this.comparer, this.transform, onSuccess, onError);
                };
                queryExpressionGeneric.prototype.executeAsContents = function (onSuccess, onError) {
                    (new dataAccess.controllers.contentTypeDefinitionFieldValueController()).getByValue(this.value.toString(), this.contentType.Id, this.field.Id, this.comparer, this.transform, function (data) {
                        var ids = data.map(function (cfv) { return cfv.ContentId; }).filter(function (value, index, self) { return self.indexOf(value) === index; });
                        (new dataAccess.controllers.contentController()).get({
                            ContentIds: ids,
                            Lcid: 0,
                            FillFields: true,
                            FillMetaData: true,
                            LoadAuthor: true
                        }, function (result) {
                            onSuccess(result.Items);
                        }, onError);
                    }, onError);
                };
                queryExpressionGeneric.queryGeneric = function (obj1, obj, fieldName) {
                    obj.contentType = obj1;
                    obj.field = obj1.getField(fieldName);
                    return obj;
                };
                return queryExpressionGeneric;
            }());
            query.queryExpressionGeneric = queryExpressionGeneric;
            var queryExpressionString = (function (_super) {
                __extends(queryExpressionString, _super);
                function queryExpressionString() {
                    return _super.call(this, mdBusinessLogic.helpers.data.dataTransformEnum.toString) || this;
                }
                queryExpressionString.prototype.compare = function (comparer) {
                    return _super.prototype.compareGeneric.call(this, this, comparer);
                };
                queryExpressionString.prototype.withValue = function (value) {
                    return _super.prototype.withValueGeneric.call(this, this, value);
                };
                queryExpressionString.query = function (obj1, fieldName) {
                    var obj = _super.queryGeneric.call(this, obj1, new queryExpressionString(), fieldName);
                    return obj;
                };
                return queryExpressionString;
            }(queryExpressionGeneric));
            query.queryExpressionString = queryExpressionString;
            var queryExpressionInteger = (function (_super) {
                __extends(queryExpressionInteger, _super);
                function queryExpressionInteger() {
                    return _super.call(this, mdBusinessLogic.helpers.data.dataTransformEnum.toInt) || this;
                }
                queryExpressionInteger.prototype.compare = function (comparer) {
                    return _super.prototype.compareGeneric.call(this, this, comparer);
                };
                queryExpressionInteger.prototype.withValue = function (value) {
                    return _super.prototype.withValueGeneric.call(this, this, value);
                };
                queryExpressionInteger.query = function (obj1, fieldName) {
                    var obj = _super.queryGeneric.call(this, obj1, new queryExpressionInteger(), fieldName);
                    return obj;
                };
                return queryExpressionInteger;
            }(queryExpressionGeneric));
            query.queryExpressionInteger = queryExpressionInteger;
            var queryExpressionDate = (function (_super) {
                __extends(queryExpressionDate, _super);
                function queryExpressionDate() {
                    return _super.call(this, mdBusinessLogic.helpers.data.dataTransformEnum.toDateTime) || this;
                }
                queryExpressionDate.prototype.compare = function (comparer) {
                    return _super.prototype.compareGeneric.call(this, this, comparer);
                };
                queryExpressionDate.prototype.withValue = function (value) {
                    return _super.prototype.withValueGeneric.call(this, this, value);
                };
                queryExpressionDate.query = function (obj1, fieldName) {
                    var obj = _super.queryGeneric.call(this, obj1, new queryExpressionDate(), fieldName);
                    return obj;
                };
                return queryExpressionDate;
            }(queryExpressionGeneric));
            query.queryExpressionDate = queryExpressionDate;
        })(query = dataAccess.query || (dataAccess.query = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var helpers;
    (function (helpers) {
        var array;
        (function (array) {
            Array.prototype['move'] = function (pos1, pos2) {
                var i, tmp;
                pos1 = parseInt(pos1, 10);
                pos2 = parseInt(pos2, 10);
                if (pos1 !== pos2 && 0 <= pos1 && pos1 <= this.length && 0 <= pos2 && pos2 <= this.length) {
                    tmp = this[pos1];
                    if (pos1 < pos2) {
                        for (i = pos1; i < pos2; i++) {
                            this[i] = this[i + 1];
                        }
                    }
                    else {
                        for (i = pos1; i > pos2; i--) {
                            this[i] = this[i - 1];
                        }
                    }
                    this[pos2] = tmp;
                }
            };
        })(array = helpers.array || (helpers.array = {}));
    })(helpers = mdBusinessLogic.helpers || (mdBusinessLogic.helpers = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var helpers;
    (function (helpers) {
        var checkType;
        (function (checkType) {
            function isFunction(functionToCheck) {
                var getType = {};
                return functionToCheck && getType.toString.call(functionToCheck) === '[object Function]';
            }
            checkType.isFunction = isFunction;
            function isArray(obj) {
                return (!!obj) && (obj.constructor === Array);
            }
            checkType.isArray = isArray;
            function isObject(obj) {
                return (!!obj) && (obj.constructor === Object);
            }
            checkType.isObject = isObject;
            function getTypeName(obj) {
                var funcNameRegex = /function (.{1,})\(/;
                var results = (funcNameRegex).exec((obj).constructor.toString());
                return (results && results.length > 1) ? results[1] : "";
            }
            checkType.getTypeName = getTypeName;
        })(checkType = helpers.checkType || (helpers.checkType = {}));
    })(helpers = mdBusinessLogic.helpers || (mdBusinessLogic.helpers = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var helpers;
    (function (helpers) {
        var crypto = (function () {
            function crypto() {
            }
            crypto.md5 = function (input) {
                return CryptoJS.MD5(input).toString();
            };
            crypto.aes = function (input) {
                return CryptoJS.AES.encrypt(input, '').toString();
            };
            crypto.sha256 = function (input) {
                return CryptoJS.SHA256(input).toString();
            };
            crypto.sha3 = function (input) {
                return CryptoJS.SHA3(input).toString();
            };
            return crypto;
        }());
        helpers.crypto = crypto;
    })(helpers = mdBusinessLogic.helpers || (mdBusinessLogic.helpers = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var helpers;
    (function (helpers) {
        var dialog = (function () {
            function dialog(dialog, state) {
                this.dialog = dialog;
                this.state = state;
            }
            dialog.prototype.showSimpleDialogO = function (_dialogInfo, _stateInfo) {
                var parentElement = angular.element(document.querySelector('.' + this.state.current.bodyClass));
                this.dialog.show(this.dialog.alert()
                    .parent(parentElement)
                    .clickOutsideToClose(true)
                    .parent(parentElement)
                    .title(_dialogInfo.title || '')
                    .textContent(_dialogInfo.text || '')
                    .ariaLabel(_dialogInfo.title || '')
                    .ok(_dialogInfo.okText || 'Got it!'));
                if (_stateInfo.changeState) {
                    this.state.go(_stateInfo.stateToGo, _stateInfo.stateParams, { reload: true });
                }
            };
            dialog.prototype.showCustomDialog = function (_onConfirm, _onDecline) {
                var show = this.dialog.show({
                    templateUrl: 'scripts/app/main/settings/configuration/template/dialogTemplates.html',
                    parent: angular.element(document.body),
                    clickOutsideToClose: true
                }).then(_onConfirm, _onDecline);
            };
            ;
            dialog.prototype.showSimpleDialog = function (_title, _text, _redirect, _state, _stateParams) {
                var parentElement = angular.element(document.querySelector('.' + this.state.current.bodyClass));
                this.dialog.show(this.dialog.alert()
                    .parent(parentElement)
                    .clickOutsideToClose(true)
                    .parent(parentElement)
                    .title(_title)
                    .textContent(_text)
                    .ariaLabel(_title)
                    .ok('Got it!'));
                if (_redirect) {
                    this.state.go(_state, _stateParams, { reload: true });
                }
            };
            dialog.prototype.showConfirmDialogO = function (_dialogInfo, _onConfirm, _onDecline) {
                var confirm = this.dialog.confirm()
                    .title(_dialogInfo.title || '')
                    .textContent(_dialogInfo.text || '')
                    .clickOutsideToClose(false)
                    .parent(angular.element(document.body))
                    .ok(_dialogInfo.ok || 'Ok')
                    .cancel(_dialogInfo.cancelText || 'Cancel');
                this.dialog.show(confirm).then(function () {
                    _onConfirm();
                }, function () {
                    _onDecline() || function () { };
                }, function () {
                });
            };
            dialog.prototype.showConfirmDialog = function (_title, _text, _ok, _cancel, _onConfirm, _onDecline) {
                var confirm = this.dialog.confirm()
                    .title(_title)
                    .textContent(_text)
                    .clickOutsideToClose(false)
                    .parent(angular.element(document.body))
                    .ok(_ok)
                    .cancel(_cancel);
                this.dialog.show(confirm).then(function () {
                    _onConfirm();
                }, function () {
                    _onDecline();
                });
            };
            dialog.prototype.redirect = function (_state, _stateParams) {
                this.state.go(_state, _stateParams, { reload: true });
            };
            return dialog;
        }());
        helpers.dialog = dialog;
    })(helpers = mdBusinessLogic.helpers || (mdBusinessLogic.helpers = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var helpers;
    (function (helpers) {
        var Guid = (function () {
            function Guid(guid) {
                if (!guid) {
                    throw new TypeError("Invalid argument; `value` has no value.");
                }
                this.value = Guid.EMPTY;
                if (guid && Guid.isGuid(guid)) {
                    this.value = guid;
                }
            }
            Guid.isGuid = function (guid) {
                var value = guid.toString();
                return guid && (guid instanceof Guid || Guid.validator.test(value));
            };
            Guid.create = function () {
                return new Guid([Guid.gen(2), Guid.gen(1), Guid.gen(1), Guid.gen(1), Guid.gen(3)].join("-"));
            };
            Guid.createEmpty = function () {
                return new Guid("emptyguid");
            };
            Guid.parse = function (guid) {
                return new Guid(guid);
            };
            Guid.raw = function () {
                return [Guid.gen(2), Guid.gen(1), Guid.gen(1), Guid.gen(1), Guid.gen(3)].join("-");
            };
            Guid.gen = function (count) {
                var out = "";
                for (var i = 0; i < count; i++) {
                    out += (((1 + mdBusinessLogic.helpers.math.random()) * 0x10000) | 0).toString(16).substring(1);
                }
                return out;
            };
            Guid.prototype.equals = function (other) {
                return Guid.isGuid(other) && this.value === other.toString();
            };
            Guid.prototype.isEmpty = function () {
                return this.value === Guid.EMPTY;
            };
            Guid.prototype.toString = function () {
                return this.value;
            };
            Guid.prototype.toJSON = function () {
                return {
                    value: this.value
                };
            };
            Guid.validator = new RegExp("^[a-z0-9]{8}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{12}$", "i");
            Guid.EMPTY = "00000000-0000-0000-0000-000000000000";
            return Guid;
        }());
        helpers.Guid = Guid;
    })(helpers = mdBusinessLogic.helpers || (mdBusinessLogic.helpers = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var helpers;
    (function (helpers) {
        var math = (function () {
            function math() {
            }
            math.random = function () {
                var crypto = window.crypto || window['msCrypto'];
                var array = new Uint32Array(1);
                return parseFloat('0.' + crypto.getRandomValues(array)[0].toString());
            };
            return math;
        }());
        helpers.math = math;
    })(helpers = mdBusinessLogic.helpers || (mdBusinessLogic.helpers = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var helpers;
    (function (helpers) {
        var touchScreenHelper;
        (function (touchScreenHelper) {
            function isTouchDevice() {
                return 'ontouchstart' in window
                    || navigator.maxTouchPoints;
            }
            touchScreenHelper.isTouchDevice = isTouchDevice;
        })(touchScreenHelper = helpers.touchScreenHelper || (helpers.touchScreenHelper = {}));
    })(helpers = mdBusinessLogic.helpers || (mdBusinessLogic.helpers = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
;
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var helpers;
    (function (helpers) {
        var typeConversion;
        (function (typeConversion) {
            function toInt(value, defaultValue, stripNonNumbers) {
                if (defaultValue === void 0) { defaultValue = 0; }
                if (stripNonNumbers === void 0) { stripNonNumbers = true; }
                var returnValue = defaultValue;
                try {
                    if (stripNonNumbers) {
                        value = value.toString().replace(/\D/g, '');
                    }
                    returnValue = parseInt(value.toString());
                }
                catch (_a) {
                    returnValue = defaultValue;
                }
                return returnValue;
            }
            typeConversion.toInt = toInt;
            function toFloat(value, defaultValue, stripNonNumbers) {
                if (defaultValue === void 0) { defaultValue = 0; }
                if (stripNonNumbers === void 0) { stripNonNumbers = true; }
                var returnValue = defaultValue;
                try {
                    if (stripNonNumbers) {
                        value = value.toString().replace(/\D/g, '');
                    }
                    returnValue = parseFloat(value.toString());
                }
                catch (_a) {
                    returnValue = defaultValue;
                }
                return returnValue;
            }
            typeConversion.toFloat = toFloat;
        })(typeConversion = helpers.typeConversion || (helpers.typeConversion = {}));
    })(helpers = mdBusinessLogic.helpers || (mdBusinessLogic.helpers = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var helpers;
    (function (helpers) {
        var data;
        (function (data) {
            var comparerTypeEnum;
            (function (comparerTypeEnum) {
                comparerTypeEnum[comparerTypeEnum["equals"] = 1] = "equals";
                comparerTypeEnum[comparerTypeEnum["notEquals"] = 2] = "notEquals";
                comparerTypeEnum[comparerTypeEnum["like"] = 3] = "like";
                comparerTypeEnum[comparerTypeEnum["greaterThan"] = 4] = "greaterThan";
                comparerTypeEnum[comparerTypeEnum["greaterThanOrEqualTo"] = 5] = "greaterThanOrEqualTo";
                comparerTypeEnum[comparerTypeEnum["lessThan"] = 6] = "lessThan";
                comparerTypeEnum[comparerTypeEnum["lessThanOrEqualTo"] = 7] = "lessThanOrEqualTo";
            })(comparerTypeEnum = data.comparerTypeEnum || (data.comparerTypeEnum = {}));
        })(data = helpers.data || (helpers.data = {}));
    })(helpers = mdBusinessLogic.helpers || (mdBusinessLogic.helpers = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var helpers;
    (function (helpers) {
        var data;
        (function (data) {
            var dataTransformEnum;
            (function (dataTransformEnum) {
                dataTransformEnum[dataTransformEnum["toString"] = 1] = "toString";
                dataTransformEnum[dataTransformEnum["toInt"] = 2] = "toInt";
                dataTransformEnum[dataTransformEnum["toLong"] = 3] = "toLong";
                dataTransformEnum[dataTransformEnum["toDateTime"] = 4] = "toDateTime";
                dataTransformEnum[dataTransformEnum["toFloat"] = 5] = "toFloat";
            })(dataTransformEnum = data.dataTransformEnum || (data.dataTransformEnum = {}));
        })(data = helpers.data || (helpers.data = {}));
    })(helpers = mdBusinessLogic.helpers || (mdBusinessLogic.helpers = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var settings;
    (function (settings) {
        var adminEventTypes;
        (function (adminEventTypes) {
            adminEventTypes[adminEventTypes["onTransitionBefore"] = 0] = "onTransitionBefore";
            adminEventTypes[adminEventTypes["onTransitionSuccess"] = 1] = "onTransitionSuccess";
            adminEventTypes[adminEventTypes["onTransitionError"] = 2] = "onTransitionError";
            adminEventTypes[adminEventTypes["ajaxOnComplete"] = 3] = "ajaxOnComplete";
            adminEventTypes[adminEventTypes["ajaxOnBeforeSend"] = 4] = "ajaxOnBeforeSend";
            adminEventTypes[adminEventTypes["ajaxOnUnauthorized"] = 5] = "ajaxOnUnauthorized";
            adminEventTypes[adminEventTypes["ajaxOnForbidden"] = 6] = "ajaxOnForbidden";
            adminEventTypes[adminEventTypes["ajaxOnJsonSerialize"] = 7] = "ajaxOnJsonSerialize";
            adminEventTypes[adminEventTypes["onLogin"] = 8] = "onLogin";
            adminEventTypes[adminEventTypes["onLogout"] = 9] = "onLogout";
            adminEventTypes[adminEventTypes["onLogedInAndPermissionsLoaded"] = 10] = "onLogedInAndPermissionsLoaded";
            adminEventTypes[adminEventTypes["onBeforeUnload"] = 11] = "onBeforeUnload";
        })(adminEventTypes = settings.adminEventTypes || (settings.adminEventTypes = {}));
        var adminEvent = (function () {
            function adminEvent(type, event) {
                this.type = type;
                this.event = event;
            }
            adminEvent.prototype.getType = function () {
                return this.type;
            };
            adminEvent.prototype.getPromise = function () {
                var args = [];
                for (var _i = 0; _i < arguments.length; _i++) {
                    args[_i] = arguments[_i];
                }
                return this.event.call(this, args);
            };
            return adminEvent;
        }());
        settings.adminEvent = adminEvent;
        var admin = (function () {
            function admin() {
            }
            admin.registerAdminEvent = function (adminEvent) {
                if (!adminEvent) {
                    throw new mdBusinessLogic.helpers.mdException('No admin event provided!');
                }
                switch (adminEvent.getType()) {
                    case adminEventTypes.ajaxOnJsonSerialize:
                        var sxistingEvent = admin.adminEvents.filter(function (event) {
                            return event.getType() === adminEvent.getType();
                        })[0];
                        if (sxistingEvent !== undefined) {
                            sxistingEvent = adminEvent;
                            break;
                        }
                    default:
                        this.adminEvents.push(adminEvent);
                }
            };
            admin.onEvent = function (type) {
                var args = [];
                for (var _i = 1; _i < arguments.length; _i++) {
                    args[_i - 1] = arguments[_i];
                }
                return Promise.all(admin.adminEvents.filter(function (event) {
                    return event.getType() === type;
                }).map(function (ev) {
                    return ev.getPromise(args);
                }).filter(function (ev) {
                    return ev !== undefined && ev != null;
                }).map(function (ev) {
                    return new Promise(function (resolve, reject) {
                        try {
                            ev.then(function (data) {
                                var responseData = undefined;
                                if (data !== undefined) {
                                    responseData = data[0];
                                }
                                if (responseData !== undefined) {
                                    responseData = responseData[0];
                                }
                                resolve(responseData);
                            })["catch"](function (data) {
                                reject(data);
                            });
                        }
                        catch (e) {
                            reject(e);
                        }
                    });
                }));
            };
            admin.adminEvents = new Array();
            return admin;
        }());
        settings.admin = admin;
    })(settings = mdBusinessLogic.settings || (mdBusinessLogic.settings = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var settings;
    (function (settings) {
        var ajax;
        (function (ajax) {
            var connections = (function () {
                function connections() {
                }
                connections.addSocket = function (socket) {
                    this._sockets.push(socket);
                };
                connections.addRequest = function (request) {
                    this._requests.push(request);
                };
                connections.getSocket = function (id) {
                    var returnObj = this._sockets.filter(function (sockObj) { return sockObj.id == id; })[0];
                    if (returnObj !== undefined) {
                        return returnObj.obj;
                    }
                    return null;
                };
                connections.getRequest = function (id) {
                    return this.getRequestObject(id).obj;
                };
                connections.getRequestObject = function (id) {
                    var returnObj = this._requests.filter(function (sockObj) { return sockObj.id == id; })[0];
                    if (returnObj !== undefined) {
                        return returnObj;
                    }
                    return null;
                };
                connections.removeSocket = function (id) {
                    for (var i = this._sockets.length - 1; i >= 0; i--) {
                        if (this._sockets[i].id == id) {
                            this._sockets[i].obj.close();
                            this._sockets.splice(i, 1);
                        }
                    }
                };
                connections.removeRequest = function (id) {
                    for (var i = this._requests.length - 1; i >= 0; i--) {
                        if (this._requests[i].id == id) {
                            this._requests[i].obj.abort();
                            this._requests.splice(i, 1);
                        }
                    }
                };
                connections.closeSockets = function () {
                    for (var i in this._sockets) {
                        if (this._sockets[i] !== undefined &&
                            this._sockets[i] != null &&
                            this._sockets[i].obj !== undefined &&
                            this._sockets[i].obj != null) {
                            this._sockets[i].obj.close();
                        }
                    }
                };
                connections.closeRequests = function () {
                    for (var i in this._requests) {
                        if (this._sockets[i] !== undefined &&
                            this._sockets[i] != null &&
                            this._sockets[i].obj !== undefined &&
                            this._sockets[i].obj != null) {
                            this._requests[i].obj.abort();
                        }
                    }
                };
                connections.closeAll = function () {
                    this.closeSockets();
                    this.closeRequests();
                };
                connections._sockets = new Array();
                connections._requests = new Array();
                return connections;
            }());
            ajax.connections = connections;
        })(ajax = settings.ajax || (settings.ajax = {}));
    })(settings = mdBusinessLogic.settings || (mdBusinessLogic.settings = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var settings;
    (function (settings) {
        var secureApi;
        (function (secureApi) {
            var forge;
            secureApi.enabled = false;
            secureApi.rsaKeys = new Object();
            secureApi.aesKey = '';
            secureApi.aesIV = '';
            secureApi.crypto = {
                rsa: (forge !== undefined ? forge.rsa : null),
                aes: null
            };
            secureApi.token = '';
        })(secureApi = settings.secureApi || (settings.secureApi = {}));
    })(settings = mdBusinessLogic.settings || (mdBusinessLogic.settings = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=businessLogic.compiled.js.map