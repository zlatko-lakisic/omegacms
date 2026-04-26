/// <reference path="./dataAccess/entities/loggedOnUser.ts" />
/// <reference path="./dataAccess/entities/permissions/profileTypePermissions.ts" />
/// <reference path="./dataAccess/entities/permissions/userPermissions.ts" />
namespace mdBusinessLogic.globals {
    export var loggedOnUser: dataAccess.entities.loggedOnUser = null;
    export var loggedOnUserToken: string;
    export var selectedLanguage: string = '';
    export var systemName: string = '';
    export var systemVersion: string = '';
    export var numberAwsSocketRetries: number = 5;
    export var enabledAuthenticationProviders: Array<string> = new Array<string>();
    export var loggedOnProfileTypePermissions: Array<dataAccess.entities.permissions.profileTypePermissions> = new Array<dataAccess.entities.permissions.profileTypePermissions>();
    export var loggedOnUserPermissions: Array<dataAccess.entities.permissions.userPermissions> = new Array<dataAccess.entities.permissions.userPermissions>();
}
