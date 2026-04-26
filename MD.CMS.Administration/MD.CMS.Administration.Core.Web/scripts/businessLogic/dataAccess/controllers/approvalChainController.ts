/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/approvalChain.ts" />
/// <reference path="../entities/approvalChainStep.ts" />
/// <reference path="../entities/approvalChainStepAction.ts" />
/// <reference path="../entities/approvalChainApproval.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class approvalChainController extends base.BaseController<approvalChainController, entities.approvalChain | entities.approvalChainStep | entities.approvalChainStepAction | entities.approvalChainApproval> {

        constructor() {
            super('ApprovalChain/');
        }

        public getById(id: number, onSuccess: (obj: entities.approvalChain) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain> = new base.AjaxMethodOptions<approvalChainController, entities.approvalChain>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetById', [id]);
            options.responseData = new entities.approvalChain();
            options.onSuccess = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByFolderId(folderId: number, onSuccess: (obj: entities.approvalChain) => void, onError: (error: helpers.mdException) => void): void {
            folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);

            let options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain> = new base.AjaxMethodOptions<approvalChainController, entities.approvalChain>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByFolderId', [folderId]);
            options.responseData = new entities.approvalChain();
            options.onSuccess = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(approvalChain: entities.approvalChain, onSuccess: (obj: entities.approvalChain) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain> = new base.AjaxMethodOptions<approvalChainController, entities.approvalChain>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.approvalChain();
            options.requestData = approvalChain;
            options.onSuccess = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public 'delete'(approvalChain: entities.approvalChain, onSuccess: (obj: entities.approvalChain) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain> = new base.AjaxMethodOptions<approvalChainController, entities.approvalChain>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete');
            options.responseData = new entities.approvalChain();
            options.requestData = approvalChain;
            options.onSuccess = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public getStepById(id: number, onSuccess: (obj: entities.approvalChain) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain> = new base.AjaxMethodOptions<approvalChainController, entities.approvalChain>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetStepById', [id]);
            options.responseData = new entities.approvalChain();
            options.onSuccess = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getStepsByApprovalChainId(id: number, onSuccess: (obj: entities.approvalChain) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain> = new base.AjaxMethodOptions<approvalChainController, entities.approvalChain>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetStepsByApprovalChainId', [id]);
            options.responseData = new entities.approvalChain();
            options.onSuccess = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public addStep(step: entities.approvalChainStep, onSuccess: (obj: entities.approvalChainStep) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<approvalChainController, entities.approvalChainStep> = new base.AjaxMethodOptions<approvalChainController, entities.approvalChainStep>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('AddStep');
            options.responseData = new entities.approvalChainStep();
            options.requestData = step;
            options.onSuccess = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChainStep>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChainStep>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public deleteStep(step: entities.approvalChainStep, onSuccess: (obj: entities.approvalChain) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain> = new base.AjaxMethodOptions<approvalChainController, entities.approvalChain>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('DeleteStep');
            options.responseData = new entities.approvalChain();
            options.requestData = step;
            options.onSuccess = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChain>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public getStepActionById(id: number, onSuccess: (obj: entities.approvalChainStepAction) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<approvalChainController, entities.approvalChainStepAction> = new base.AjaxMethodOptions<approvalChainController, entities.approvalChainStepAction>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetStepActionById', [id]);
            options.responseData = new entities.approvalChainStepAction();
            options.onSuccess = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChainStepAction>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChainStepAction>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getStepActionsByStepId(id: number, onSuccess: (obj: entities.approvalChainStepAction) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<approvalChainController, entities.approvalChainStepAction> = new base.AjaxMethodOptions<approvalChainController, entities.approvalChainStepAction>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetStepActionsByStepId', [id]);
            options.responseData = new entities.approvalChainStepAction();
            options.onSuccess = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChainStepAction>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChainStepAction>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public addStepAction(stepAction: entities.approvalChainStepAction, onSuccess: (obj: entities.approvalChainStepAction) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<approvalChainController, entities.approvalChainStepAction> = new base.AjaxMethodOptions<approvalChainController, entities.approvalChainStepAction>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('AddStepAction');
            options.responseData = new entities.approvalChainStepAction();
            options.requestData = stepAction;
            options.onSuccess = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChainStepAction>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChainStepAction>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public deleteStepAction(stepAction: entities.approvalChainStepAction, onSuccess: (obj: entities.approvalChainStepAction) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<approvalChainController, entities.approvalChainStepAction> = new base.AjaxMethodOptions<approvalChainController, entities.approvalChainStepAction>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('DeleteStepAction');
            options.responseData = new entities.approvalChainStepAction();
            options.requestData = stepAction;
            options.onSuccess = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChainStepAction>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChainStepAction>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public addApproval(approval: entities.approvalChainApproval, onSuccess: (obj: entities.approvalChainApproval) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<approvalChainController, entities.approvalChainApproval> = new base.AjaxMethodOptions<approvalChainController, entities.approvalChainApproval>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('AddApproval');
            options.responseData = new entities.approvalChainApproval();
            options.requestData = approval;
            options.onSuccess = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChainApproval>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<approvalChainController, entities.approvalChainApproval>): void => {
                onError(options.exception);
            }
            this._post(options);
        }
    }
}
