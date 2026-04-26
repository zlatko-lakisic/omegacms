(function () {
    'use strict';

    var attributeTypeEnum = mdBusinessLogic.dataAccess.entities.attributeTypeEnum;

    var mdGenerictypeDesignerElementConstatns = {
        controls: {
            formControls: {
                textBox: {
                    id: attributeTypeEnum.input
                },
                textArea: {
                    id: attributeTypeEnum.textarea
                },
                number: {
                    id: attributeTypeEnum.input
                },
                checkbox: {
                    id: attributeTypeEnum.selectMultiple
                },
                select: {
                    id: attributeTypeEnum.selectSingle
                },
                calculated: {
                    id: attributeTypeEnum.calculated
                }
            },
            cmsControls: {
                contentPicker: {
                    id: attributeTypeEnum.contentSelectorSingle
                },
                mediaContentPicker: {
                    id: attributeTypeEnum.mediaContentSelectorSingle
                },
                taxonomyPicker: {
                    id: attributeTypeEnum.taxonomySelectorSingle
                },
                userPicker: {
                    id: attributeTypeEnum.userSelectorSingle
                }
            },
            otherControls: {
                section: {
                    id: attributeTypeEnum.section
                },
                tabbedSections: {
                    id: attributeTypeEnum.tabbedSections
                },
                youtube: {
                    id: attributeTypeEnum.youtube
                },
                calendar: {
                    id: attributeTypeEnum.date
                },
                map: {
                    id: attributeTypeEnum.map
                },
                fileUpload: {
                    id: attributeTypeEnum.file
                }
            }
        },
        controlLabels: {
            formControls: {
                name: 'Form',
                icon: 'icon-window-restore'
            },
            cmsControls: {
                name: 'CMS',
                icon: 'icon-omega-logo'
            },
            otherControls: {
                name: 'Other',
                icon: 'icon-xml'
            }
        },
        fieldType: {},
        fieldSize: {},
        fieldClass: {},
        defaults: {
            defaultTileWidth: 30,
            defaultTileHeight: 150
        },
        getHeight: function (control) {
            var height = this.defaults.defaultTileHeight;
            if (control !== undefined && control.height !== undefined && !isNaN(control.height)) {
                height = control.height;
            }

            height = this.getMinHeight(control, height);

            return height;
        },
        getWidth: function (control) {
            var width = this.defaults.defaultTileWidth;
            if (control !== undefined && control.width !== undefined && !isNaN(control.width)) {
                height = control.width;
            }
            return width;
        },
        getControlByEnum: function (en) {
            for (var i in this.controls) {
                for (var control in this.controls[i]) {
                    if (control.id == en) {
                        return control;
                    }
                }
            }
            return undefined;
        },
        getMinHeight: function (control, defaultValue) {
            if (defaultValue === undefined) {
                defaultValue = 150;
            }

            if (mdGenerictypeDesignerElementConstatns.fieldSize[control.id] !== undefined &&
                mdGenerictypeDesignerElementConstatns.fieldSize[control.id].minHeight !== undefined &&
                !isNaN(mdGenerictypeDesignerElementConstatns.fieldSize[control.id].minHeight)) {
                if (mdGenerictypeDesignerElementConstatns.fieldSize[control.id].minHeight > defaultValue) {
                    defaultValue = mdGenerictypeDesignerElementConstatns.fieldSize[control.id].minHeight;
                }
            }

            return defaultValue;
        }
    };

    mdGenerictypeDesignerElementConstatns.fieldType[attributeTypeEnum.input] = "text-box";
    mdGenerictypeDesignerElementConstatns.fieldType[attributeTypeEnum.textarea] = "text-area";
    mdGenerictypeDesignerElementConstatns.fieldType[attributeTypeEnum.selectSingle] = "select";
    mdGenerictypeDesignerElementConstatns.fieldType[attributeTypeEnum.selectMultiple] = "checkbox";
    mdGenerictypeDesignerElementConstatns.fieldType[attributeTypeEnum.taxonomySelectorSingle] = "taxonomy-picker";
    mdGenerictypeDesignerElementConstatns.fieldType[attributeTypeEnum.file] = "file-upload";
    mdGenerictypeDesignerElementConstatns.fieldType[attributeTypeEnum.date] = "date-field";
    mdGenerictypeDesignerElementConstatns.fieldType[attributeTypeEnum.map] = "map";
    mdGenerictypeDesignerElementConstatns.fieldType[attributeTypeEnum.contentSelectorSingle] = "content-picker";
    mdGenerictypeDesignerElementConstatns.fieldType[attributeTypeEnum.mediaContentSelectorSingle] = "media-content-picker";
    mdGenerictypeDesignerElementConstatns.fieldType[attributeTypeEnum.youtube] = "youtube";
    mdGenerictypeDesignerElementConstatns.fieldType[attributeTypeEnum.userSelectorSingle] = "user-picker";
    mdGenerictypeDesignerElementConstatns.fieldType[attributeTypeEnum.calculated] = "calculated";

    mdGenerictypeDesignerElementConstatns.fieldClass[attributeTypeEnum.input] = "textbox";
    mdGenerictypeDesignerElementConstatns.fieldClass[attributeTypeEnum.textarea] = "textarea";
    mdGenerictypeDesignerElementConstatns.fieldClass[attributeTypeEnum.selectSingle] = "selectlist";
    mdGenerictypeDesignerElementConstatns.fieldClass[attributeTypeEnum.selectMultiple] = "checkbox";
    mdGenerictypeDesignerElementConstatns.fieldClass[attributeTypeEnum.taxonomySelectorSingle] = "taxonomypicker";
    mdGenerictypeDesignerElementConstatns.fieldClass[attributeTypeEnum.file] = "fileupload";
    mdGenerictypeDesignerElementConstatns.fieldClass[attributeTypeEnum.date] = "calendarfield";
    mdGenerictypeDesignerElementConstatns.fieldClass[attributeTypeEnum.map] = "googlemap";
    mdGenerictypeDesignerElementConstatns.fieldClass[attributeTypeEnum.contentSelectorSingle] = "contentpicker";
    mdGenerictypeDesignerElementConstatns.fieldClass[attributeTypeEnum.mediaContentSelectorSingle] = "contentmediapicker";
    mdGenerictypeDesignerElementConstatns.fieldClass[attributeTypeEnum.youtube] = "youtube";
    mdGenerictypeDesignerElementConstatns.fieldClass[attributeTypeEnum.userSelectorSingle] = "userpicker";
    mdGenerictypeDesignerElementConstatns.fieldClass[attributeTypeEnum.calculated] = "calculated";

    mdGenerictypeDesignerElementConstatns.fieldSize[attributeTypeEnum.file] = {
        minHeight: 300
    };

    angular
        .module('app.core')
        .constant('mdGenerictypeDesignerElementConstatns', mdGenerictypeDesignerElementConstatns);
})();
