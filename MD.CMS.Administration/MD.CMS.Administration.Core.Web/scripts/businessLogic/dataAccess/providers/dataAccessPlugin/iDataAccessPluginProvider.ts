namespace mdBusinessLogic.dataAccess.providers.dataAccess {
    export interface iDataAccessPluginProvider {
        id?: string,
        name?: string,
        setupDirective?: string,
        data?: any,
        icon?: string,
        classType?: any,
        hasDynamicProperties?: boolean
        dynamicPropertiesDirective?: string
    }
}
