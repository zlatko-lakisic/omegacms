using MD.CMS.BusinessLogic.WebApi.Core.Properties;
using System.Collections.Generic;
using MD.Tools.Helpers.Core.TypeConversion;
using MD.CMS.BusinessLogic.WebApi.Core.Session;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;

namespace MD.CMS.BusinessLogic.WebApi.Core.WebSockets
{
    public class WebSocketHelpers
    {
        private static string GetQueryStringValue(IDictionary<string, string> queryStrings, string queryStringName)
        {
            if (queryStrings != null)
            {
                return queryStrings.Where(kvp => string.Compare(kvp.Key, queryStringName, true).Equals(0)).Select(kvp => kvp.Value).FirstOrDefault();
            }
            return default;
        }

        public static bool GetIsAdministration(IDictionary<string, string> queryStrings)
        {
            string result = GetQueryStringValue(queryStrings, Settings.Default.IsAdministrationHeaderName);
            if (!string.IsNullOrEmpty(result))
            {
                return result.ToBoolean(false);
            }
            return false;
        }

        public static string GetAuthenticationHeader(IDictionary<string, string> queryStrings)
        {
            return GetQueryStringValue(queryStrings, Settings.Default.AuthenticateHeaderName);
        }

        public static string GetConnectionIdHeader(IDictionary<string, string> queryStrings)
        {
            return GetQueryStringValue(queryStrings, "connectionId");
        }

        public static async Task<MD.CMS.BusinessLogic.Core.DataAccess.Entities.User> GetLoggedOnUser(IDictionary<string, string> queryStrings)
        {
            return await GetLoggedOnUserAsync(queryStrings);
        }

        public static async Task<MD.CMS.BusinessLogic.Core.DataAccess.Entities.User> GetLoggedOnUserAsync(IDictionary<string, string> queryStrings)
        {
            return await GetLoggedOnUserAsync(GetIsAdministration(queryStrings), GetQueryStringValue(queryStrings, Settings.Default.AuthenticateHeaderName));
        }

        public static async Task<MD.CMS.BusinessLogic.Core.DataAccess.Entities.User> GetLoggedOnUserAsync(bool isAdministration, string tokenData)
        {
            if (!string.IsNullOrEmpty(tokenData))
            {
                return await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().Caller(BusinessLogic.Core.DataAccess.Entities.User.SystemUser()).DefaultPlugin(isAdministration).GetByIdAsync(await SessionTable.GetLoggedOnUserIdAsync(tokenData));
            }
            return null;
        }

        public static ConcurrentDictionary<string, string> QueryStringsToDictionary(IQueryCollection collection)
        {
            return new ConcurrentDictionary<string, string>(collection.Select(query => new KeyValuePair<string, string>(query.Key, query.Value.ToString())));
        }
    }
}
