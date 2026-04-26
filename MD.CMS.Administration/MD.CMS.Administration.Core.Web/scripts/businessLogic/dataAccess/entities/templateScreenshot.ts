/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
namespace mdBusinessLogic {
    export namespace dataAccess {
        export namespace entities {
            export class templateScreenshot extends base.BaseEntity implements base.IBaseEntity<templateScreenshot>{
                public ScreenshotUrl: string;
                public ScreenshotFile: string;
                public ScreenshotWidth: number;
                public ScreenshotHeight: number;
                public Template: template;

                constructor(obj?: templateScreenshot) {
                    super(obj);
                    this.ScreenshotUrl = '';
                    this.ScreenshotFile = '';
                    this.ScreenshotWidth = 0;
                    this.ScreenshotHeight = 0;
                    this.Template = null;
                    if (obj != undefined && obj != null) {
                        this.construct(obj);
                    }
                }

                public construct(data: any) {
                    super.construct(data);
                    this.ScreenshotUrl = this.getValue<string>(data, 'ScreenshotUrl', '');
                    this.ScreenshotFile = this.getValue<string>(data, 'ScreenshotFile', '');
                    this.ScreenshotWidth = this.getValue<number>(data, 'ScreenshotWidth', 0);
                    this.ScreenshotHeight = this.getValue<number>(data, 'ScreenshotHeight', 0);
                    this.Template = this.getConstructEntityValue<template>(data, 'Template', null);
                }

                public clone(): templateScreenshot {
                    return new templateScreenshot(this);
                }
            }
        }
    }
}