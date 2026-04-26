
namespace mdBusinessLogic.dataAccess.entities.genericContent {
    export interface iGenericContentFieldJsonFieldConstraint {
        folderPaths?: string[],
        contentIds?: string[],
        userIds?: string[],
        profileId?: string,
        contentTypeId?: string,
        taxonomyIds?: string[],
        menuPaths?: string[]
    }
}