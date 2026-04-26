namespace mdBusinessLogic.dataAccess.providers.dataAccess {
    export class dataAccessPluginRegistry {
        private static _dataAccessPluginRegistry: entities.generic.genericCollection<iDataAccessPluginProvider> = new entities.generic.genericCollection<iDataAccessPluginProvider>();

        public static add(obj: iDataAccessPluginProvider): boolean {

            if (obj.id === undefined || obj.id == null || obj.id.trim().length == 0) {
                console.error('Attemptd to register illegal new registry data access plugin provider, missing id property!');
                return false;
            }

            if (obj.name === undefined || obj.name == null || obj.name.trim().length == 0) {
                console.error('Attemptd to register illegal new registry data access plugin provider, missing name property!');
                return false;
            }

            if (obj.setupDirective === undefined || obj.setupDirective == null || obj.setupDirective.trim().length == 0) {
                console.error('Attemptd to register illegal new registry data access plugin provider, missing setup directive property!');
                return false;
            }

            if (dataAccessPluginRegistry._dataAccessPluginRegistry.get(obj.name) != null) {
                console.error('Attemptd to register illegal new registry data access plugin provider, provider exists!');
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

            dataAccessPluginRegistry._dataAccessPluginRegistry.add(obj.id, obj);
            return true;
        }

        public static get(key: string): iDataAccessPluginProvider {
            if (dataAccessPluginRegistry._dataAccessPluginRegistry.get(key) == null) {
                console.error('Data access plugin dost not exists!');
            }

            if (mdBusinessLogic.globals.enabledAuthenticationProviders.filter(function (enabledProvider) {
                return enabledProvider === key;
            }).length == 0) {
                console.error('Data access plugin is not enabled!');
                return null;
            }

            return dataAccessPluginRegistry._dataAccessPluginRegistry.get(key);
        }

        public static getAll(): Array<entities.generic.genericKeyValuePair<iDataAccessPluginProvider>> {
            return dataAccessPluginRegistry._dataAccessPluginRegistry.getCollection().filter(function (provider: entities.generic.genericKeyValuePair<iDataAccessPluginProvider>) {
                return mdBusinessLogic.globals.enabledAuthenticationProviders.filter(function (enabledProvider) {
                    return enabledProvider === provider.Value.id;
                }).length > 0;
            });
        }
    }
}
