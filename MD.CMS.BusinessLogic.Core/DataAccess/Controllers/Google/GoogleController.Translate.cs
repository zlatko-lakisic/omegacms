using MD.Tools.BaseDataAccess.Core.Controllers;
using MD.Tools.BaseDataAccess.Core.Entities;
using MD.Tools.BaseDataAccess.Core.Entities.WebRequestProperties;
using MD.Tools.BaseDataAccess.Core.Enumerations;
using MD.Tools.BaseDataAccess.Core.Interfaces;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Google;
using MD.CMS.BusinessLogic.Core.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Google
{
    public partial class GoogleController : BaseGoogleController<GoogleController>
    {
        public GoogleTranslationEntity TranslateText(Culture sourceCulture, Culture targetCulture, params string[] stringsToTranslate)
        {
            BaseWebRequest request = new BaseWebRequest();
            request.Domain = "https://www.googleapis.com/";
            request.MethodPath = "language/";
            request.MethodName = "translate/v2";
            request.MethodType = WebRequestEnum.Post;
            request.Parameters = new List<IBaseWebRequestProperty>();
            request.Parameters.Add(new GenericChildlessProperty("key", Settings.Default.GoogleApiTranslationKey, true));
            request.Parameters.Add(new GenericChildlessProperty("source", sourceCulture.GoogleCode, true));
            request.Parameters.Add(new GenericChildlessProperty("target", targetCulture.GoogleCode, true));
            foreach (string stringToTranslate in stringsToTranslate)
            {
                request.Parameters.Add(new GenericChildlessProperty("q", stringToTranslate, true));
            }

           

            return JsonConvert.DeserializeObject<GoogleTranslationEntity>(string.Empty/*ExecuteWebRequest(request)*/);
        }
    }
}
