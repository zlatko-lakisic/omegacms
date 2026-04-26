/// <reference path="../entities/contentTypeDefinition.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />
/// <reference path="../controllers/options/iContentRequestOptions.ts" />

namespace mdBusinessLogic.dataAccess.query {
    export class queryExpressionGeneric<T, K extends queryExpressionGeneric<T, K>> {
        private field: entities.contentTypeDefinitionField;
        private contentType: entities.contentTypeDefinition<entities.contentTypeDefinitionField>;
        private comparer: helpers.data.comparerTypeEnum;
        private transform: helpers.data.dataTransformEnum;
        private value: T;

        constructor(transform: helpers.data.dataTransformEnum) {
            this.transform = transform;
        }

        protected compareGeneric(obj: K, comparer: helpers.data.comparerTypeEnum): K {
            obj.comparer = comparer;
            return obj;
        }

        protected withValueGeneric(obj: K, value: T): K {
            obj.value = value;
            return obj;
        }

        public execute(onSuccess: (data: Array<entities.contentTypeDefinitionFieldValue>) => void, onError: (error: helpers.mdException) => void): void {
            (new controllers.contentTypeDefinitionFieldValueController()).getByValue(this.value.toString(), this.contentType.Id, this.field.Id, this.comparer, this.transform, onSuccess, onError);
        }

        public executeAsContents(onSuccess: (data: Array<entities.content>) => void, onError: (error: helpers.mdException) => void): void {
            (new controllers.contentTypeDefinitionFieldValueController()).getByValue(this.value.toString(), this.contentType.Id, this.field.Id, this.comparer, this.transform, (data) => {
                let ids: Array<string> = data.map((cfv) => { return cfv.ContentId; }).filter((value, index, self) => { return self.indexOf(value) === index; });
                (new controllers.contentController()).get({
                    ContentIds: ids,
                    Lcid: 0,
                    FillFields: true,
                    FillMetaData: true,
                    LoadAuthor: true
                }, (result) => {
                    onSuccess(result.Items);
                }, onError);
            }, onError);
        }

        protected static queryGeneric<T, K extends queryExpressionGeneric<T, K>>(obj1: entities.contentTypeDefinition<entities.contentTypeDefinitionField>, obj: K, fieldName: string): K {
            obj.contentType = obj1;
            obj.field = obj1.getField(fieldName);
            return obj;
        }
    }

    export class queryExpressionString extends queryExpressionGeneric<string, queryExpressionString> {
        constructor() {
            super(helpers.data.dataTransformEnum.toString);
        }

        public compare(comparer: helpers.data.comparerTypeEnum): queryExpressionString {
            return super.compareGeneric(this, comparer);
        }

        public withValue(value: string): queryExpressionString {
            return super.withValueGeneric(this, value);
        }

        public static query(obj1: entities.contentTypeDefinition<entities.contentTypeDefinitionField>, fieldName: string): queryExpressionString {
            let obj = super.queryGeneric(obj1, new queryExpressionString(), fieldName);
            return obj;
        }
    }

    export class queryExpressionInteger extends queryExpressionGeneric<number, queryExpressionInteger> {
        constructor() {
            super(helpers.data.dataTransformEnum.toInt);
        }

        public compare(comparer: helpers.data.comparerTypeEnum): queryExpressionInteger {
            return super.compareGeneric(this, comparer);
        }

        public withValue(value: number): queryExpressionInteger {
            return super.withValueGeneric(this, value);
        }

        public static query(obj1: entities.contentTypeDefinition<entities.contentTypeDefinitionField>, fieldName: string): queryExpressionInteger {
            let obj = super.queryGeneric(obj1, new queryExpressionInteger(), fieldName);
            return obj;
        }
    }

    export class queryExpressionDate extends queryExpressionGeneric<Date, queryExpressionDate> {
        constructor() {
            super(helpers.data.dataTransformEnum.toDateTime);
        }

        public compare(comparer: helpers.data.comparerTypeEnum): queryExpressionDate {
            return super.compareGeneric(this, comparer);
        }

        public withValue(value: Date): queryExpressionDate {
            return super.withValueGeneric(this, value);
        }

        public static query(obj1: entities.contentTypeDefinition<entities.contentTypeDefinitionField>, fieldName: string): queryExpressionDate {
            let obj = super.queryGeneric(obj1, new queryExpressionDate(), fieldName);
            return obj;
        }
    }
}