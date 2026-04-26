using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Google
{
    public class GoogleTranslationEntity
    {
        public class Translation
        {
            public string translatedText { get; set; }
        }

        public class Data
        {
            public List<Translation> translations { get; set; }
        }

        public Data data { get; set; }
    }
}
