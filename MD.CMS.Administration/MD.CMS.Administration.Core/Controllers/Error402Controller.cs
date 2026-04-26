using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MD.CMS.Administration.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace MD.CMS.Administration.Core.Controllers
{
    public class Error402Controller : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Response.StatusCode = 402;
            return View(new Layout(HttpContext));
        }
    }
}