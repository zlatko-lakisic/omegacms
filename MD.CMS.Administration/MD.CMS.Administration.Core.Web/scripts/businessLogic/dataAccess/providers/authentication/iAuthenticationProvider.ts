namespace mdBusinessLogic.dataAccess.providers.authentication {
    export interface iAuthenticationProvider {
        id?: string,
        name?: string,
        shortcode?: string,
        data?: any,
        icon?: string,
    }
}
