using MD.CMS.BusinessLogic.Core.DataAccess.Entities.ApprovalChain;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;
using System.Text;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MD.CMS.BusinessLogic.WebApi.Core.Models;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Options;

namespace MD.CMS.WebApi.Core.Controllers
{
    /// <summary>
    /// WebAPI for manipulating Approval chain, it's steps and actions
    /// </summary>
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "ApprovalChain")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Content")]
    public class ApprovalChainController : BaseLoggedOnWebApiController
    {
        /// <summary>
        /// Get approval chain by ID with it's steps. If not found return HTTP 404 status
        /// </summary>
        /// <param name="id">Approval chain ID</param>
        /// <returns>IActionResult with requested ApprovalChain</returns>
        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("GetById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ApprovalChain GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            ApprovalChain approvalChain = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain.ApprovalChainController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            if (approvalChain == null)
            {
                return NotFound();
            }
            return Ok(approvalChain);
        }

        /// <summary>
        /// Get approval chain for given folder.
        /// </summary>
        /// <param name="id">Folder id</param>
        /// <returns>IActionResult with approval chain<ApprovalChain></returns>
        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("GetByFolderId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ApprovalChain GetByFolderId")]
        public async Task<IActionResult> GetByFolderId(long id)
        {
            if (!await FolderExists(id))
            {
                return BadRequest("Folder does not exist!");
            }
            ApprovalChain approvalChain = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain.ApprovalChainController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderIdAsync(id);
            if (approvalChain == null)
            {
                return Ok();
            }
            return Ok(approvalChain);
        }

        /// <summary>
        /// Insert new or edit existing approval chain
        /// </summary>
        /// <param name="chain">ApprovalChain to edit or insert</param>
        /// <returns>IActionResult with ApprovalChain with new ID</returns>
        [HttpPost]
        [Route("[action]")]
        [ActionName("Save")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByFolderId")]
        [OmegaInvalidateCache("GetSepById")]
        [OmegaInvalidateCache("GetStepsByApprovalChainId")]
        [OmegaInvalidateCache("GetSepActionsById")]
        [OmegaInvalidateCache("GetSepActionsByStepId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ApprovalChain Save")]
        public async Task<IActionResult> Save([FromBody]ApprovalChain chain)
        {
            if (!await FolderExists(chain.FolderId))
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Folder does not exist");
            }
            bool exists = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain.ApprovalChainController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderIdAsync(chain.FolderId) != null;
            if(exists)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Folder already contains approval chain");
            }
            if (chain.Steps == null || chain.Steps.Count < 2)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Chain must contain more than 1 step");
            }
            int stepCount = chain.Steps.Count;
            foreach (Step step in chain.Steps)
            {
                if (step.Actions == null || step.Actions.Count < 2)
                {
                    throw new HttpException((int)HttpStatusCode.BadRequest, "Step " + step.Order + " must contain 2 actions: Rejected and Approved");
                }
                foreach(StepAction action in step.Actions)
                {
                    if(action.Type == StepActionType.Rejected)
                    {
                        if (!action.Id.Equals(default(long)) && action.Id > 0) // reject action has been changed
                        {
                            action.Type = StepActionType.Rejected;
                            action.Action = StepActionAction.Redirect;
                            if (action.RedirectTo >= step.Order)
                            {
                                // if trying to redirect to step above or same step on reject, redirect on step 
                                action.RedirectTo = 0;
                            }

                        }
                        continue;
                    }
                    if (step.Order < stepCount - 1) //skip last step, step order starts at 0
                    {
                        //make sure steps are linked in order when approving if actions are new or changed
                        if (!action.Id.Equals(default(long)) && step.Actions[0].Id > 0)
                        {
                            action.Type = StepActionType.Approved;
                            action.Action = StepActionAction.Redirect;
                            action.RedirectTo = step.Order + 1;
                        }
                    }
                    else //publish the content
                    {
                        action.Type = StepActionType.Approved;
                        action.Action = StepActionAction.Publish;
                    }
                    if (step.Order == 0)
                    {
                        continue; // step 0 does not require users or reject action fix
                    }
                }
                if (step.Order == 0)
                { 
                    continue; // step 0 does not require users or reject action fix
                }
                if (step.UserIds == null || step.UserIds.Count == 0)
                {
                    return BadRequest("Every step must contain at least 1 user. Check Step " + step.Order);
                }
            }

            ApprovalChain approvalChain = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain.ApprovalChainController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(chain);

            if (approvalChain == null)
            {
                throw new HttpException((int)HttpStatusCode.InternalServerError, "Failed to save approval chain!");
            }

            else
            {
                List<Step> savedSteps = new List<Step>();
                foreach (Step step in chain.Steps)
                {
                    step.ApprovalChainId = approvalChain.Id;
                    try
                    {
                        Step savedStep = await StepController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(step);
                        savedStep.Actions = step.Actions;
                        savedSteps.Add(savedStep);
                    }
                    catch (Exception e)
                    {
                        throw new HttpException((int)HttpStatusCode.InternalServerError, e.Message);
                    }
                }

                //When all the steps are saved check for action redirects and update the redirectTo to new ID set by the database
                //actions keep order number in redirectTo before being saved in database
                for (int i = 0; i < chain.Steps.Count; i++)
                {
                    Step step = chain.Steps[i];
                    foreach (StepAction action in step.Actions)
                    {
                        if (!action.Id.Equals(default(long)) && action.Id > 0) //skip step if not changed
                        {
                            continue;
                        }
                        if (action.Id < 0) //if updated id is set to id * -1 so it needs to be reverted
                        {
                            action.Id *= -1;
                        }
                        
                        action.StepId = savedSteps[i].Id; //if new action set stepId, if update, set to avoid possible bugs

                        if (action.Type == StepActionType.Approved)
                        {
                            if(action.Action == StepActionAction.Redirect) //last step is publish so it will skip this automatically
                            {
                                action.RedirectTo = savedSteps[i + 1].Id;
                            }
                            await StepActionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(action);
                            continue;
                        }
                        //if type of rejected and changed or new action
                        //redirect to is set to Order number instead of Step ID because of forntent drawing
                        Step redirectTo = savedSteps.Where(p => p.Order == action.RedirectTo).Select(p => p).First();
                        action.RedirectTo = redirectTo.Id; //revert it to step id
                        await StepActionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(action);
                    }
                }
            }
            approvalChain = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain.ApprovalChainController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(approvalChain.Id);
            return Ok(approvalChain);
        }

        /// <summary>
        /// Delete approval chain
        /// </summary>
        /// <param name="approvalChain">ApprovalChain to delete</param>
        /// <returns>IActionResult</returns>
        [HttpDelete]
        [Route("[action]")]
        [ActionName("Delete")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByFolderId")]
        [OmegaInvalidateCache("GetSepById")]
        [OmegaInvalidateCache("GetStepsByApprovalChainId")]
        [OmegaInvalidateCache("GetSepActionsById")]
        [OmegaInvalidateCache("GetSepActionsByStepId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ApprovalChain Delete")]
        public async Task<IActionResult> Delete(ApprovalChain approvalChain)
        {
            if (approvalChain.Steps != null)
            {
                foreach (Step step in approvalChain.Steps)
                {
                    DeleteStep(step);
                }
            }

            bool isDeleted = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain.ApprovalChainController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(approvalChain);

            if (!isDeleted)
            {
                throw new HttpException((int)HttpStatusCode.InternalServerError);
            }

            return Ok();
        }

        /// <summary>
        /// Get Step by given step ID
        /// </summary>
        /// <param name="id">long step id</param>
        /// <returns>IActionResult with Step</returns>
        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("GetStepById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ApprovalChain GetStepById")]
        public async Task<IActionResult> GetStepById(long id)
        {
            Step step = await StepController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            if (step == null)
            {
                return NotFound();
            }
            return Ok(step);
        }

        /// <summary>
        /// Get all steps for approval chain
        /// </summary>
        /// <param name="id">long id of approval chain</param>
        /// <returns>IActionResult with List of Step objects </returns>
        [HttpPost]
        [Route("[action]/{id?}")]
        [ActionName("GetStepsByApprovalChainId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ApprovalChain GetStepsByApprovalChainId")]
        public async Task<IActionResult> GetStepsByApprovalChainId(long id)
        {
            ApprovalChain chain = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain.ApprovalChainController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            if (chain == null)
            {
                return BadRequest("Approval chain does not exist");
            }
            return Ok(chain.Steps);
        }

        /// <summary>
        /// Add step to exitsing approval chain. If approval chain does not exist return HTTP 400 status.
        /// </summary>
        /// <param name="approvalStep">Step to add</param>
        /// <returns>IActionResult with Step</returns>
        [HttpPost]
        [Route("[action]")]
        [ActionName("AddStep")]
        [OmegaInvalidateCache("GetStepById")]
        [OmegaInvalidateCache("GetStepsByApprovalChainId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ApprovalChain AddStep")]
        public async Task<IActionResult> AddStep([FromBody]Step approvalStep)
        {
            if (approvalStep.UserIds.Count == 0)
            {
                return BadRequest("No users provided for step");
            }

            ApprovalChain approvalChain = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain.ApprovalChainController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(approvalStep.ApprovalChainId);
            if (approvalChain == null)
            {
                return BadRequest("Approval chain could not be found!");
            }
            Step step = await StepController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(approvalStep);
            if (step == null)
            {
                throw new HttpException((int)HttpStatusCode.InternalServerError);
            }
            return Ok(step);
        }

        /// <summary>
        /// Add multiple steps to existing approval chain
        /// </summary>
        /// <param name="approvalSteps">List of approval chain steps</param>
        /// <returns>IActionResult with ApprovalChain steps populated</returns>
        [HttpPost]
        [Route("[action]")]
        [ActionName("AddSteps")]
        [OmegaInvalidateCache("GetStepById")]
        [OmegaInvalidateCache("GetStepsByApprovalChainId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ApprovalChain AddSteps")]
        public async Task<IActionResult> AddSteps([FromBody]List<Step> approvalSteps)
        {
            if (approvalSteps == null || approvalSteps.Count == 0)
            {
                return BadRequest("List cannot be empty!");
            }

            ApprovalChain approvalChain = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain.ApprovalChainController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(approvalSteps[0].ApprovalChainId);
            if (approvalChain == null)
            {
                return BadRequest("Approval chain could not be found!");
            }

            foreach (Step step in approvalSteps)
            {
                Step stepSaved = await StepController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(step);
                if (stepSaved == null)
                {
                    throw new HttpException((int)HttpStatusCode.InternalServerError);
                }
            }
            //fetch approval chain again to populate steps list
            approvalChain = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain.ApprovalChainController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(approvalSteps[0].ApprovalChainId);
            return Ok(approvalChain);
        }

        /// <summary>
        /// Delete approval chain step
        /// </summary>
        /// <param name="step">Step to delete</param>
        /// <returns>IActionResult</returns>
        [HttpGet]
        [Route("[action]")]
        [ActionName("DeleteStep")]
        [OmegaInvalidateCache("GetSepById")]
        [OmegaInvalidateCache("GetStepsByApprovalChainId")]
        [OmegaInvalidateCache("GetSepActionsById")]
        [OmegaInvalidateCache("GetSepActionsByStepId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ApprovalChain DeleteStep")]
        public async Task<IActionResult> DeleteStep(Step step)
        {
            foreach (StepAction a in step.Actions)
            {
                DeleteStepAction(a);
            }
            bool isDeleted = await StepController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(step);

            if (!isDeleted)
            {
                throw new HttpException((int)HttpStatusCode.InternalServerError);
            }

            return Ok();
        }

        /// <summary>
        /// Get step action by provided id
        /// </summary>
        /// <param name="id">StepAction id</param>
        /// <returns>IActionResult with StepAction</returns>
        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("GetStepActionById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ApprovalChain GetStepActionById")]
        public async Task<IActionResult> GetStepActionById(long id)
        {
            StepAction action = await StepActionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            if (action == null)
            {
                return NotFound();
            }
            return Ok(action);
        }

        /// <summary>
        /// Insert or edit StepAction
        /// </summary>
        /// <param name="stepAction">StepAction to create or edit</param>
        /// <returns>IActionResult with StepAcion</returns>
        [HttpPost]
        [Route("[action]")]
        [ActionName("AddStepAction")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ApprovalChain AddStepAction")]
        public async Task<IActionResult> AddStepAction([FromBody]StepAction stepAction)
        {
            Step step = await StepController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(stepAction.StepId);
            if (step == null)
            {
                return BadRequest("Step not found!");
            }
            StepAction action = await StepActionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(stepAction);
            if (action == null)
            {
                throw new HttpException((int)HttpStatusCode.InternalServerError);
            }
            return Ok(action);
        }

        /// <summary>
        /// Get step actions list for provided step id
        /// </summary>
        /// <param name="id">Step id</param>
        /// <returns>IActionResult with List of StepAction objects</returns>
        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("GetStepActionsByStepId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ApprovalChain GetStepActionsByStepId")]
        public async Task<IActionResult> GetStepActionsByStepId(long id)
        {
            Step step = await StepController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            if (step == null)
            {
                return BadRequest("Step not found!");
            }
            return Ok(step.Actions);
        }

        /// <summary>
        /// Delete StepAction
        /// </summary>
        /// <param name="stepAction">StepAction to delete</param>
        /// <returns>IActionResult</returns>
        [HttpDelete]
        [Route("[action]")]
        [ActionName("DeleteStepAction")]
        [OmegaInvalidateCache("GetSepActionsById")]
        [OmegaInvalidateCache("GetSepActionsByStepId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ApprovalChain DeleteStepAction")]
        public async Task<IActionResult> DeleteStepAction(StepAction stepAction)
        {
            bool isDeleted = await StepActionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(stepAction);

            if (!isDeleted)
            {
                throw new HttpException((int)HttpStatusCode.InternalServerError);
            }

            return Ok();
        }

        /// <summary>
        /// Get approval by id
        /// </summary>
        /// <param name="approvalId">ApprovalChainApproval ID</param>
        /// <returns>ApprovalChainApprovals</returns>
        [HttpGet]
        [Route("[action]")]
        [ActionName("GetApprovalById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ApprovalChain GetApprovalById")]
        public async Task<IActionResult> GetApprovalById([FromQuery] long approvalId)
        {
            ApprovalChainApproval approval = await ApprovalChainApprovalController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(approvalId);
            if (approval == null)
            {
                return NotFound();
            }
            return Ok(approval);
        }

        /// <summary>
        /// Get user approvals and rejections for new content
        /// </summary>
        /// <param name="contentId">Content ID</param>
        /// <param name="contentDateCreated">Content date created</param>
        /// <param name="lcid">Content LCID</param>
        /// <returns>List of ApprovalChainApprovals</returns>
        [HttpGet]
        [Route("[action]")]
        [ActionName("GetContentApprovals")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ApprovalChain GetContentApprovals")]
        public async Task<IActionResult> GetContentApprovals([FromQuery] string contentId, [FromQuery] DateTime contentDateCreated, [FromQuery] int lcid)
        {
            List<ApprovalChainApproval> approvals = await ApprovalChainApprovalController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByContentAsync(contentId, lcid, contentDateCreated);
            if (approvals == null || approvals.Count == 0)
            {
                return NotFound();
            }
            return Ok(approvals);
        }

        /// <summary>
        /// Approve or reject new content
        /// </summary>
        /// <param name="approval">New ApprovalChainApproval for new content on one of the steps in approval chain</param>
        /// <returns>New ApprovalChainApproval object</returns>
        [HttpPost]
        [Route("[action]")]
        [ActionName("AddApproval")]
        [OmegaInvalidateCache("GetContentApprovals")]
        [OmegaInvalidateCache("GetApprovalById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ApprovalChain AddApproval")]
        public async Task<IActionResult> AddApproval([FromBody]ApprovalChainApproval approval)
        {
            DateTime contentDateCreated = DateTime.Parse(approval.Content.DateCreated);

            Content latestVersion = (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(new ContentOptions
            {
                ContentIds = new List<string> { approval.Content.Id },
                Lcid = approval.Content.LCID
            })).FirstOrDefault(); 
            
            if (latestVersion == null || !latestVersion.ApprovalPending)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Content approved or not existing! ");
            }
            if (!approval.Content.DateCreated.Equals(latestVersion.DateCreated))
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Bad content version! Cannot approve old content version!");
            }
            
            List<ApprovalChainApproval> contentApprovals = await ApprovalChainApprovalController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByContentAsync(approval.Content.Id, approval.Content.LCID, contentDateCreated);
            Step thisStep = await StepController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(approval.Step.Id);
            
            int countApprovalsMade = contentApprovals.Where(p => p.Step.Id == approval.Step.Id).Count(); // number of approvals on this step so far

            if( countApprovalsMade > 0) //is this the first action for this content ?
            {
                //how many times was content rejected on steps above
                int redirects = contentApprovals.Where(p => p.Step.Order > thisStep.Order && p.Step.Actions[1].RedirectTo == thisStep.Id).Count();

                if (thisStep.ComboOperator == StepComboOperator.OR)
                {
                    //if number of approvals with OR combination is greater than number of redirects + 1 (current approval being processed)
                    //it means user's trying to make approval on step which was already processed
                    if (countApprovalsMade > redirects + 1)
                    {
                        throw new HttpException((int)HttpStatusCode.BadRequest, "Content allready approved or rejected!");
                    }
                }
                else //if combination operator is AND we need to recalculate number of approvals 
                {
                    //how many times did user approve or reject content until now on this step
                    int userApprovals = contentApprovals.Where(p => p.Step.Id == approval.Step.Id && p.User.Id == approval.User.Id).Count();
                    //check if user already made action for this cycle
                    if (userApprovals > redirects + 1)
                    {
                        throw new HttpException((int)HttpStatusCode.BadRequest, "Content allready approved or rejected!");
                    }
                }                
            }

            ApprovalChainApproval savedApproval = await ApprovalChainApprovalController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(approval);
            if (savedApproval == null)
            {
                throw new HttpException((int)HttpStatusCode.InternalServerError);
            }
            contentApprovals.Add(savedApproval);

            //on approve or reject check what next to do in chain
            foreach (StepAction action in savedApproval.Step.Actions)
            {
                if (action.Type != savedApproval.ApprovalType)
                {
                    continue;
                }
                Step nextStep = null;
                StringBuilder contentParams = null;
                if(action.Action == StepActionAction.Redirect)
                {
                    nextStep = await StepController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(action.RedirectTo);
                    contentParams = new StringBuilder();
                    contentParams.Append("{ \"Id\": ");
                    contentParams.Append(savedApproval.Content.Id);
                    contentParams.Append(", \"LCID\" : ");
                    contentParams.Append(savedApproval.Content.LCID);
                    contentParams.Append(", \"DateCreated\": \"");
                    contentParams.Append(savedApproval.Content.DateCreated);
                    contentParams.Append("\", \"FolderId\" : ");
                    contentParams.Append(savedApproval.Content.FolderId);
                    contentParams.Append(", \"stepId\": ");
                    contentParams.Append(nextStep.Id);
                    if (savedApproval.ApprovalType == StepActionType.Rejected)
                    {
                        contentParams.Append(",\"Rejected\": 1");
                    }
                    if (!string.IsNullOrEmpty(savedApproval.Comment))
                    {
                        contentParams.Append(",\"Reason\": \"");
                        contentParams.Append(approval.Comment);
                        contentParams.Append("\"");
                    }
                    contentParams.Append("}");
                }

                if (savedApproval.ApprovalType == StepActionType.Rejected)
                {
                    //prevent users from getting approval request if anyone on that step rejects content
                    await removeMessages(thisStep.Id, thisStep.UserIds, savedApproval.Content.Id);

                    //then send messages to users on other steps
                    Message message = new Message();

                    //if redirect to step 0 it means content has benn rejected fully 
                    if (nextStep.Order == 0)
                    {
                        savedApproval.Content.ApprovalPending = false;
                        savedApproval.Content.IsPublished = false;
                        await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).ApproveRejectAsync(savedApproval.Content);
                        message.Subject = "Content rejected";
                        message.MessageContent = "Your content " + savedApproval.Content.Title + " has been rejected.<p>";
                        if (!string.IsNullOrEmpty(savedApproval.Comment))
                        {
                            message.MessageContent += "<p>Reason: " + savedApproval.Comment + "</p>";
                        }
                        message.ToUserId = savedApproval.Content.AuthorId;
                        return Ok(savedApproval);
                    }

                    message.Subject = "Approval pending for " + savedApproval.Content.Title;
                    message.MessageContent = contentParams.ToString();

                    foreach (string userId in nextStep.UserIds)
                    {
                        message.ToUserId = userId;
                        await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(message, SystemMessageFolder.Approvals);
                    }

                }
                else // if (savedApproval.ApprovalType == StepActionType.Approved)
                {
                    //first check if all users on this step have responded
                    bool proceed = true;
                    if (savedApproval.Step.ComboOperator == StepComboOperator.AND)
                    {
                        int count = 0;
                        foreach (string userid in savedApproval.Step.UserIds)
                        {
                            bool didApprove = contentApprovals.Where(p =>
                                p.Step.Id == savedApproval.Step.Id && p.User.Id == userid && p.ApprovalType == StepActionType.Approved).Any();
                            if (didApprove)
                            {
                                count++;
                            }
                        }
                        proceed = count == savedApproval.Step.UserIds.Count;
                    }
                    //if this is the last user to approve or step has combination operator "OR" continue, if not wait for others
                    if (proceed)
                    {
                        //if combination operator is AND messages will be deleted by frontend for each user
                        if (action.Action == StepActionAction.Publish) // only last step can have publish
                        {
                            savedApproval.Content.ApprovalPending = false;
                            savedApproval.Content.IsPublished = true;
                            await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).ApproveRejectAsync(savedApproval.Content);
                            Message message = new Message();
                            message.Subject = "Content approved";
                            message.MessageContent = "Your content " + savedApproval.Content.Title + " has been approved and published.<p>";
                            message.ToUserId = savedApproval.Content.AuthorId;
                            await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(message, SystemMessageFolder.Approvals);
                        }
                        else if (action.Action == StepActionAction.Redirect)
                        {
                            Message message = new Message();
                            message.Subject = "Approval pending for " + savedApproval.Content.Title;
                            if (message.Subject.Length > 45)
                            {
                                message.Subject = message.Subject.Substring(0, 40);
                                message.Subject += "...";
                            }
                            message.MessageContent = contentParams.ToString();
                            foreach (string userId in nextStep.UserIds)
                            {
                                message.ToUserId = userId;
                                await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(message, SystemMessageFolder.Approvals);
                            }
                        }

                        //remove left messages
                        await removeMessages(thisStep.Id, thisStep.UserIds, savedApproval.Content.Id);
                    }
                    //else Wait for other users on this to give the approval
                }
            }

            return Ok(savedApproval);
        }


        private async Task removeMessages(long stepId, List<string> userIds, string contentId) 
        {
            foreach (string userId in userIds)
            {
                List<Message> messages = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByMessageFolderIdAndUserIdAsync((int)SystemMessageFolder.Approvals, userId);
                foreach (Message m in messages)
                {
                    //check if message should be deleted
                    try
                    {
                        var obj = JObject.Parse(m.MessageContent);
                        if (obj.GetValue("stepId").ToObject<int>() == stepId && obj.GetValue("Id").ToObject<string>() == contentId)
                        {
                            await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteByMessageAndUserIdAsync(m, userId);
                        }
                    }
                    catch (JsonReaderException jex)
                    {
                        continue;
                    }

                }
            }
        }

        /// <summary>
        /// Delete existing approval
        /// </summary>
        /// <param name="approval">ApprovalChainApproval to remove</param>
        /// <returns>IActionResult</returns>
        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("DeleteApproval")]
        [OmegaInvalidateCache("GetContentApprovals")]
        [OmegaInvalidateCache("GetApprovalById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ApprovalChain DeleteApproval")]
        public async Task<IActionResult> DeleteApproval(long id)
        {
            bool removed = await ApprovalChainApprovalController.GetNewInstance().Caller(await GetLoggedOnUser()).DeleteAsync(id);
            if (!removed)
            {
                throw new HttpException((int)HttpStatusCode.InternalServerError);
            }
            return Ok();
        }

        private async Task<bool> FolderExists(long folderId)
        {
            Folder<Content> folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(folderId);
            if (folder == null)
                return false;

            return true;
        }
    }
}
