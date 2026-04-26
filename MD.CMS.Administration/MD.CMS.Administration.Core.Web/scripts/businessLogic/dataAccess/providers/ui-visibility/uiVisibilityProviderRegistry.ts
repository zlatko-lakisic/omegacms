namespace mdBusinessLogic.dataAccess.providers.uiVisibility {
    export enum iUiVisiblityType {
        User,
        Profile
    }

    export interface iUiVisiblitySetting {
        type: iUiVisiblityType,
        id: string,
        visible: boolean,
        name: string
    }

    export class uiVisibilityProviderRegistry {
        private static _uiVisiblitySettingsRegistry: entities.generic.genericCollection<iUiVisiblitySetting> = new entities.generic.genericCollection<iUiVisiblitySetting>();

        public static getUniqueName(name: string, type: iUiVisiblityType, id: string) {
            return name + '_' + type.valueOf() + '_' + id;
        }

        public static add(obj: iUiVisiblitySetting): boolean {
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
        }

        public static get(key: string): iUiVisiblitySetting {
            return uiVisibilityProviderRegistry._uiVisiblitySettingsRegistry.get(key);
        }

        public static getAll(): Array<entities.generic.genericKeyValuePair<iUiVisiblitySetting>> {
            return uiVisibilityProviderRegistry._uiVisiblitySettingsRegistry.getCollection();
        }
    }
}
