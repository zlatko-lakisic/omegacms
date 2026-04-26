(function () {
    'use strict';

    angular
        .module('app.settings', [
            'app.settings.configuration.content-types',
            'app.settings.configuration.language_settings',        
            'app.settings.configuration.user_management.user',
            'app.settings.configuration.user_management.profile_type',
            'app.settings.configuration.meta-data-field',
            'app.settings.configuration.template',
            'app.settings.configuration.permissions',
            'app.settings.configuration.cache'
        ])
        .config(['msNavigationServiceProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider) {
        // Navigation
        msNavigationServiceProvider.saveItem('settings', {
            title: 'Menus.MainSettings',
            group: true,
            weight: 2
        });

        msNavigationServiceProvider.saveItem('settings.configuration', {
            title: 'Menus.MainSettingsConfiguration',
            icon: 'icon-cog'
        });

        msNavigationServiceProvider.saveItem('settings.configuration.content_types', {
            title: 'Menus.MainSettingsConfigurationContentTypes',
            state: 'app.content-types-list',
            icon: 'icon-document'
        });

        msNavigationServiceProvider.saveItem('settings.configuration.language_settings', {
            title: 'Menus.MainSettingsConfigurationLanguageSettings',
            state: 'app.settings_configuration_language_settings',
            icon: 'icon-flag'
        });      

        msNavigationServiceProvider.saveItem('settings.configuration.meta_data_field', {
            title: 'Menus.MainSettingsConfigurationMetaData',
            state: 'app.meta-data-field-list',
            icon: 'icon-format-list-bulleted'
        });

        msNavigationServiceProvider.saveItem('settings.configuration.template', {
            title: 'Menus.MainSettingsConfigurationTemplates',
            state: 'app.template-list',
            icon: 'icon-view-quilt'
        });

        msNavigationServiceProvider.saveItem('settings.configuration.user_management', {
            title: 'Menus.MainSettingsConfigurationUserManagement',
            icon: 'icon-account-multiple-outline',
            weight: 2          
        });

        msNavigationServiceProvider.saveItem('settings.configuration.user_management.user', {
            title: 'Menus.MainSettingsConfigurationUserManagementUsers',
            icon: 'icon-account',
            state: 'app.user_list',
            weight: 4
        });


        msNavigationServiceProvider.saveItem('settings.configuration.user_management.profile_type', {
            title: 'Menus.MainSettingsConfigurationUserManagementProfileTypes',
            icon: 'icon-account-box',
            state: 'app.profile-types-list',
            weight: 4
        });

        msNavigationServiceProvider.saveItem('settings.configuration.permissions', {
            title: 'Menus.MainSettingsConfigurationPermissions',
            state: 'app.settings_configuration_permissions',
            icon: 'icon-key-variant'
        });

        msNavigationServiceProvider.saveItem('settings.configuration.cache', {
            title: 'Menus.MainSettingsConfigurationCache',
            state: 'app.settings_configuration_cache',
            icon: 'icon-cube-outline'
        });    
        
    }
})();
