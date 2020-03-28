using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetCore.MiddleWares.Controllers
{
    [ApiController]
    public class BranchController : Controller
    {
        // GET: /<controller>/
        [HttpGet("branch1")]
        public string branch1()
        {
            return "branch1";
        }

        [HttpGet("branch2")]
        public string branch2()
        {
            return "branch2";
        }

        [HttpGet("branch3")]
        public string branch3()
        {
            return "branch3";
        }
    }
}
