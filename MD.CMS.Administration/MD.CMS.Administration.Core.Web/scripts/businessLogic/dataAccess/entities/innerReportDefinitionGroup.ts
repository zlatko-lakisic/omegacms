/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./innerReportDefinitionUniqueProperty.ts" />

namespace mdBusinessLogic.dataAccess.entities {
  export class innerReportDefinitionGroup extends innerReportDefinitionUniqueProperty implements base.IBaseEntity<innerReportDefinitionGroup> {

    constructor(obj?: innerReportDefinitionGroup) {
      super(obj);
    }

    construct(data: any): void {
      super.construct(data);
    }

    public clone(): innerReportDefinitionGroup {
      return new innerReportDefinitionGroup(this);
    }

  }
}
