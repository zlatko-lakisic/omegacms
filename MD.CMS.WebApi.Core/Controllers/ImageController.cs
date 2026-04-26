using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace MD.CMS.WebApi.Core.Controllers
{
    [TokenAuth]
    public class ImageController : ControllerBase
    {
        [HttpPost]
        [ActionName("Save")]
        public IActionResult Post([FromBody]byte file)
        {
            Guid g = Guid.NewGuid();


            string guid = string.Format(@"{0}{1}", g, file);//konvertujem u guid

            byte[] toBytes = Encoding.ASCII.GetBytes(guid);





            return null;
        }
    }
}