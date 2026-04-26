using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using System.Globalization;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.Tools.Helpers.Core.Logging;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class CultureController : BaseController<CultureController>
    {
        private class DistinctCultureComparer : IEqualityComparer<Culture>
        {

            public bool Equals(Culture x, Culture y)
            {
                return x.LCID == y.LCID;
            }

            public int GetHashCode(Culture obj)
            {
                return obj.LCID.GetHashCode();
            }
        }

        public Culture Create(DataRow row)
        {
            Culture obj = null;
            if (row != null)
            {
                obj = new Culture();
                obj.LCID = row.GetValue<int>(CultureEnum.LCID.GetStringValue());
                obj.Name = row.GetValue<string>(CultureEnum.Name.GetStringValue());
                obj.Code = row.GetValue<string>(CultureEnum.Code.GetStringValue());
                obj.IsoCode = row.GetValue<string>(CultureEnum.IsoCode.GetStringValue());
                obj.IsApproved = true;

                if (obj.LCID != 127)
                {
                    try
                    {
                        RegionInfo ri = new RegionInfo(obj.LCID);
                        obj.IsoCode = ri.DisplayName;
                    }
                    catch (Exception error)
                    {
                        (typeof(CultureController)).Log(error);
                    }
                }
            }
            return obj;
        }

        public Culture CreateNew(CultureInfo row, bool isApproved)
        {
            Culture obj = null;
            if (row != null)
            {
                obj = new Culture();
                obj.LCID = row.LCID;
                obj.Name = row.EnglishName;
                obj.Code = row.Name;
                obj.IsoCode = row.TwoLetterISOLanguageName;
                obj.IsApproved = isApproved;

                if (obj.LCID != 127)
                {
                    try
                    {
                        RegionInfo ri = new RegionInfo(obj.LCID);
                        obj.IsoCode = ri.DisplayName;
                    }
                    catch (Exception error)
                    {
                        (typeof(CultureController)).LogVerbose(error.Message, error);
                    }
                }
            }
            return obj;
        }

        //Insert
        public async Task<Culture> SaveAsync(Culture obj)
        {
            await AuthenticateAndAuthorizeAsync();
            Culture culture = null;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Culture;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Methods.Insert.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Parameters.LCID.GetIntValue()) { Value = obj.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Parameters.Name.GetIntValue()) { Value = obj.Name });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Parameters.Code.GetIntValue()) { Value = obj.Code });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Parameters.IsoCode.GetIntValue()) { Value = obj.IsoCode });
                method.ClearCache = true;


                culture = Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return culture;
        }

        //Update
        public async Task UpdateAsync(Culture obj)
        {
            await AuthenticateAndAuthorizeAsync();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Culture;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Methods.Insert.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Parameters.LCID.GetIntValue()) { Value = obj.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Parameters.Name.GetIntValue()) { Value = obj.Name });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Parameters.Code.GetIntValue()) { Value = obj.Code });
                method.ClearCache = true;


                Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
                //method.WaitForOnAfterCompleted();
            }
        }
        //Delete
        public async Task<bool> DeleteAsync(Culture obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;

            using (Method method = new Method())
            {

                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Culture;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Parameters.LCID.GetIntValue()) { Value = obj.LCID });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);

                if (success)
                    obj = null;
                method.End();
                //method.WaitForOnAfterCompleted();
            }

            return success;
        }
        //SelectByLCID
        public async Task<Culture> GetByLCIDAsync(int lcid, bool selectFromAll = false)
        {
            await AuthenticateAndAuthorizeAsync();
            if (selectFromAll)
            {
                return (await GetAllAsync()).FirstOrDefault(c => c.LCID.Equals(lcid));
            }
            else
            {
                Method method = new Method();
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Culture;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Methods.GetByLCID.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Parameters.LCID.GetIntValue()) { Value = lcid });
                return Create(await ExecuteMethodRowAsync(method));

            }
        }
        //SelectByCode
        public async Task<Culture> GetByCodeAsync(string code, bool selectFromAll = false)
        {
            await AuthenticateAndAuthorizeAsync();
            if (selectFromAll)
            {
                return (await GetAllAsync()).FirstOrDefault(c => string.CompareOrdinal(c.Code, code).Equals(0));
            }
            else
            {

                Method method = new Method();
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Culture;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Methods.GetByLCID.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Parameters.Code.GetIntValue()) { Value = code });
                return Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
            }
        }
        //SelectAll
        public async Task<IEnumerable<Culture>> GetAllAsync()
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Culture;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Methods.GetAll.GetIntValue();
            method.Properties = new List<IMethodProperty>();
            DataTable results = await ExecuteMethodTableAsync(method);

            List<Culture> cultures = (from DataRow row in results.Rows select Create(row)).ToList();

            IEnumerable<Culture> allCultures = await GetClassCacheValueAsync<IEnumerable<Culture>>(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Culture, 999);
            if(allCultures == null)
            {
                allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures).Select(c => CreateNew(c, false));
                await AddClassCacheValueAsync(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Culture, 999, allCultures);
            }

            if (allCultures != null)
            {
                cultures.AddRange(allCultures);
            }

            return cultures.Distinct(new DistinctCultureComparer());
        }

        public async Task<IEnumerable<Culture>> GetAllAvailableForContentIdAsync(long contentId)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Culture;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Methods.GetAll.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Parameters.ContentId.GetIntValue()) { Value = contentId });
            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            return (from DataRow row in results.Rows select Create(row)).ToList();
        }

        //SelectAll
        public async Task<List<Culture>> GetApprovedAsync()
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Culture;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Methods.GetApproved.GetIntValue();

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            return (from DataRow row in results.Rows select Create(row)).ToList();
        }

        public async Task<DataSet> SearchCmsAsync(string searchTerm)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Culture;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Methods.SearchCms.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Parameters.LCID.GetIntValue()) { Value = DataAccessSettings.SelectedLcid });

            DataSet results = await ExecuteMethodDataSetAsync(method, this.UseDefaultPlugin);
            if (results.Tables.Count > 0)
            {
                for (int i = 0; i < results.Tables.Count; i++)
                {
                    if (results.Tables[i].Columns.Contains("TableName"))
                    {
                        if (results.Tables[i].Rows.Count > 0 && !String.IsNullOrEmpty(results.Tables[i].Rows[0].GetValue<string>("TableName")))
                        {
                            results.Tables[i].TableName = results.Tables[i].Rows[0].GetValue<string>("TableName");
                        }
                        else
                        {
                            results.Tables.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }

            return results;
        }
    }
}
