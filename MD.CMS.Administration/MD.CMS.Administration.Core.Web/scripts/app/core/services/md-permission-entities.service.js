(function () {
    'use strict';

    angular
        .module('app.core')
        .provider('mdPermissionEntities', [mdPermissionEntitiesProvider])
        .factory('mdPermissionEntitiesService', ['mdPermissionEntities', mdPermissionEntitiesService]);

    /** @ngInject */
    function mdPermissionEntitiesService(mdPermissionEntities) {

        return {
            entities: mdPermissionEntities.entities,
            groups: mdPermissionEntities.groups
        }
    }

    function mdPermissionEntitiesProvider() {
        var entitiesEnum = mdBusinessLogic.dataAccess.entities.entitiesEnum;

        function entities() {
            var entities = [];
            for (var i in entitiesEnum) {
                if (isNaN(i)) {
                    var entity = {
                        'name': i.replace(/([a-z])([A-Z])/g, '$1 $2'),
                        'id': entitiesEnum[i],
                        'icon': '',
                        'display': true,
                        'description': 'This gives a user access to the ' + i + ' permission scope.'
                    };
                    switch (entitiesEnum[i]) {
                        case entitiesEnum.Content:
                            entity.icon = 'icon-file';
                            break;
                        case entitiesEnum.AttributeTypeDefinition:
                            entity.icon = 'icon-checkbox-multiple-marked-outline';
                            break;
                        case entitiesEnum.ContentTypeDefinition:
                            entity.icon = 'icon-document';
                            break;
                        case entitiesEnum.ContentTypeDefinitionField:
                            entity.icon = 'icon-file-document';
                            break;
                        case entitiesEnum.ContentTypeDefinitionFieldValue:
                            entity.icon = 'icon-file-xml';
                            break;
                        case entitiesEnum.ContentTypeDefinitionFolder:
                            entity.icon = 'icon-folder-multiple-outline';
                            break;
                        case entitiesEnum.Folder:
                            entity.icon = 'icon-folder';
                            break;
                        case entitiesEnum.FolderMediaContentMetaDataField:
                            entity.icon = 'icon-image-filter';
                            break;
                        case entitiesEnum.FolderMetaDataField:
                            entity.icon = 'icon-folder-plus';
                            break;
                        case entitiesEnum.MediaContentMetaDataFieldValues:
                            entity.icon = 'icon-file-xml';
                            break;
                        case entitiesEnum.MediaContent:
                            entity.icon = 'icon-file-image';
                            break;
                        case entitiesEnum.LCID:
                            entity.icon = 'icon-flag';
                            break;
                        case entitiesEnum.Culture:
                            entity.icon = 'icon-flag';
                            break;
                        case entitiesEnum.MenuContent:
                            entity.icon = 'icon-file-multiple';
                            break;
                        case entitiesEnum.ContentAlias:
                            entity.icon = 'icon-link-variant';
                            break;
                        case entitiesEnum.Menu:
                            entity.icon = 'icon-menu';
                            break;
                        case entitiesEnum.MetaDataField:
                            entity.icon = 'icon-xml';
                            break;
                        case entitiesEnum.MetaDataFieldValue:
                            entity.icon = 'icon-file-xml';
                            break;
                        case entitiesEnum.Permissions:
                            entity.icon = 'icon-lock-unlocked';
                            break;
                        case entitiesEnum.Profile:
                            entity.icon = 'icon-account-circle';
                            break;
                        case entitiesEnum.ProfileType:
                            entity.icon = 'icon-account-box';
                            break;
                        case entitiesEnum.ProfileTypeField:
                            entity.icon = 'icon-file-document';
                            break;
                        case entitiesEnum.ProfileTypeFieldValue:
                            entity.icon = 'icon-file-xml';
                            break;
                        case entitiesEnum.Session:
                            entity.icon = 'icon-brightness-2';
                            break;
                        case entitiesEnum.TaxonomyContent:
                            entity.icon = 'icon-file';
                            break;
                        case entitiesEnum.Taxonomy:
                            entity.icon = 'icon-fridge';
                            break;
                        case entitiesEnum.Template:
                            entity.icon = 'icon-xml';
                            break;
                        case entitiesEnum.User:
                            entity.icon = 'icon-account';
                            break;
                        case entitiesEnum.RWDPermission:
                            entity.name = 'RWD Permission';
                            entity.icon = 'icon-';
                            break;
                        case entitiesEnum.Report:
                            entity.icon = 'icon-file-document';
                            break;
                        case entitiesEnum.ReportDefinition:
                            entity.icon = 'icon-file-document-box';
                            break;
                        case entitiesEnum.ReportData:
                            entity.icon = 'icon-file-multiple';
                            break;
                        case entitiesEnum.ReportScheduler:
                            entity.icon = 'icon-calendar-clock';
                            break;
                        case entitiesEnum.ReportSchedulerAction:
                            entity.icon = 'icon-calendar-select';
                            break;
                        case entitiesEnum.ApprovalChain:
                            entity.icon = 'icon-account-network';
                            break;
                        case entitiesEnum.Step:
                            entity.icon = 'icon-debug-step-into';
                            break;
                        case entitiesEnum.StepAction:
                            entity.icon = 'icon-debug-step-over';
                            break;
                        case entitiesEnum.StepUser:
                            entity.icon = 'icon-account-switch';
                            break;
                        case entitiesEnum.MessageFolder:
                            entity.icon = 'icon-folder-plus';
                            break;
                        case entitiesEnum.Message:
                            entity.icon = 'icon-message-draw';
                            break;
                        case entitiesEnum.ApprovalChainApproval:
                            entity.icon = 'icon-link-variant';
                            break;
                        case entitiesEnum.ContentTypeDefinitionDataSource:
                            entity.icon = 'icon-data';
                            break;
                        case entitiesEnum.ContentTypeDefinitionDataSourceJoin:
                            entity.icon = 'icon-exit-to-app';
                            break;
                        case entitiesEnum.ContentTypeDefinitionFolderDataBoundCondition:
                            entity.icon = 'icon-filter';
                            break;
                        case entitiesEnum.ContentTypeDefinitionFolderDataBoundSync:
                            entity.icon = 'icon-sync';
                            break;
                    }
                    entities.push(entity);
                }
            }
            entities.sort(function (a, b) {
                if (a.name < b.name) { return -1; }
                if (a.name > b.name) { return 1; }
                return 0;
            });
            return entities;
        }

        function groups() {
            var groups = [{
                'name': 'Content',
                'icon': 'icon-file',
                'entities': getEntities([
                    entitiesEnum.ApprovalChain,
                    entitiesEnum.ApprovalChainApproval,
                    entitiesEnum.Content,
                    entitiesEnum.ContentAlias,
                    entitiesEnum.ContentTypeDefinitionFieldValue,
                    entitiesEnum.MetaDataFieldValue,
                    entitiesEnum.ContentAlias,
                    entitiesEnum.TaxonomyContent,
                    entitiesEnum.Template,
                    entitiesEnum.Permissions
                ])
            }, {
                'name': 'Taxonomy',
                'icon': 'icon-fridge',
                'entities': getEntities([
                    entitiesEnum.Taxonomy,
                    entitiesEnum.TaxonomyContent
                ])
            }, {
                'name': 'Menu',
                'icon': 'icon-menu',
                'entities': getEntities([
                    entitiesEnum.Menu,
                    entitiesEnum.MenuContent
                ])
            }, {
                'name': 'Media Content',
                'icon': 'icon-file-image',
                'entities': getEntities([
                    entitiesEnum.MediaContent,
                    entitiesEnum.MediaContentMetaDataFieldValues
                ])
            }, {
                'name': 'Creating Reports',
                'icon': 'icon-file-document-box',
                'entities': getEntities([
                    entitiesEnum.Report,
                    entitiesEnum.ReportDefinition,
                    entitiesEnum.ReportScheduler,
                    entitiesEnum.ReportSchedulerAction
                ])
            }, {
                'name': 'Generating Reports',
                'icon': 'icon-file-document-box',
                'entities': getEntities([
                    entitiesEnum.Report,
                    entitiesEnum.ReportData
                ])
            }, {
                'name': 'Configuration',
                'icon': 'icon-view-quilt',
                'entities': getEntities([
                    entitiesEnum.ContentTypeDefinition,
                    entitiesEnum.ContentTypeDefinitionDataSource,
                    entitiesEnum.ContentTypeDefinitionDataSourceJoin,
                    entitiesEnum.ContentTypeDefinitionField,
                    entitiesEnum.Culture,
                    entitiesEnum.MetaDataField,
                    entitiesEnum.Template,
                    entitiesEnum.Permissions,
                    entitiesEnum.User,
                    entitiesEnum.Profile,
                    entitiesEnum.ProfileType,
                    entitiesEnum.ProfileTypeField
                ])
            }];

            for (var i = 0; i < groups.length; i++) {
                groups[i].id = mdBusinessLogic.helpers.crypto.md5(i);
            }

            groups.sort(function (a, b) {
                if (a.name < b.name) { return -1; }
                if (a.name > b.name) { return 1; }
                return 0;
            });

            return groups;
        }

        function getEntities(_entities) {
            return entities().filter(function (entity) {
                return _entities.indexOf(entity.id) >= 0;
            });
        }

        this.entities = entities;
        this.groups = groups;

        this.$get = function () {
            return getEntities;
        };
    }
}());