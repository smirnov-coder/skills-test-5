using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SkillsTest.Controllers
{
    public class ErrorController : Controller
    {
        [Route("[controller]/403")]
        public IActionResult Error403() => View();

        [Route("[controller]/404")]
        public IActionResult Error404() => View();

        public IActionResult Index() => View();
    }
}
