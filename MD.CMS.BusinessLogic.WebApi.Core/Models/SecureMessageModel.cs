using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MD.CMS.BusinessLogic.WebApi.Core.Modeles
{
    public class SecureMessageModel
    {
        public class SecureMessageModel_Message
        {
            public class SecureMessageModel_MessageHeader
            {
                public string name { get; set; }
                public string value { get; set; }
            }
            public string method { get; set; }
            public List<SecureMessageModel_MessageHeader> headers { get; set; }
            public bool isJsonArray { get; set; }
            public string data { get; set; }
        }

        public string endpoint { get; set; }
        public SecureMessageModel_Message message { get; set; }
    }
}