using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;
using MD.Tools.Helpers.Core.Extensions.StringExt;
using MD.Tools.Helpers.Core;
using System.Xml;
using MD.CMS.BusinessLogic.Core.Properties;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public class LcidController : Singleton<LcidController>
    {
        public LCID Create(XmlNode node)
        {
            LCID obj = null;
            if (node != null)
            {
                obj = new LCID();
                obj.Id = node.SelectSingleNode("Id").InnerText.ToInt();
                obj.Name = node.SelectSingleNode("Name").InnerText;
            }
            return obj;
        }

        public List<LCID> GetAll()
        {
            List<LCID> lcidList = new List<LCID>();
            XmlNodeList list = Settings.Default.AvailableLcid.SelectNodes("/Lcids/Lcid");
            foreach (XmlNode lcid in list)
            {
                lcidList.Add(Create(lcid));
            }
            return lcidList;
        }

        public LCID GetById(int lcid = default(int))
        {
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }
            return GetAll().SingleOrDefault(l => l.Id.Equals(lcid));
        }
    }
}
