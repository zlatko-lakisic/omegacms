namespace mdBusinessLogic.dataAccess.providers.authentication {
    export class authenticationProviderRegistry {
        private static _authenticationProviderRegistry: entities.generic.genericCollection<iAuthenticationProvider> = new entities.generic.genericCollection<iAuthenticationProvider>();

        public static add(obj: iAuthenticationProvider): boolean {
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
        }

        public static get(key: string): iAuthenticationProvider {
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
        }

        public static getAll(): Array<entities.generic.genericKeyValuePair<iAuthenticationProvider>> {
            return authenticationProviderRegistry._authenticationProviderRegistry.getCollection().filter(function (provider: entities.generic.genericKeyValuePair<iAuthenticationProvider>) {
                return mdBusinessLogic.globals.enabledAuthenticationProviders.filter(function (enabledProvider) {
                    return enabledProvider === provider.Value.id;
                }).length > 0;
            });
        }
    }
}
