/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
namespace mdBusinessLogic {
    export namespace dataAccess {
        export namespace entities {
            export class template extends base.BaseEntity implements base.IBaseEntity<template>{
                public Name: string;
                public Description: string;
                public TemplateUrl: string;

                constructor(obj?: template) {
                    super(obj);
                    this.Name = '';
                    this.Description = '';
                    this.TemplateUrl = '';
                    if (obj != undefined && obj != null) {
                        this.construct(obj);
                    }
                }

                public construct(data: any) {
                    super.construct(data);
                    this.Name = this.getValue<string>(data, 'Name', '');
                    this.Description = this.getValue<string>(data, 'Description', '');
                    this.TemplateUrl = this.getValue<string>(data, 'TemplateUrl', '');
                }

                public clone(): template {
                    return new template(this);
                }
            }
        }
    }
}