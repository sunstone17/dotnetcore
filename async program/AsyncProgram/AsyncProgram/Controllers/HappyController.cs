using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AsyncProgram.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HappyController : Controller
    {
        // GET: /<controller>/
        public ActionResult Index()
        {
            DateTime startTime = DateTime.Now;//进入DoSomething方法前的时间
            var startThreadId = Thread.CurrentThread.ManagedThreadId;//进入DoSomething方法前的线程ID

            DoSomething();//耗时操作

            DateTime endTime = DateTime.Now;//完成DoSomething方法的时间
            var endThreadId = Thread.CurrentThread.ManagedThreadId;//完成DoSomething方法后的线程ID
            return Content($"startTime:{ startTime.ToString("yyyy-MM-dd HH:mm:ss:fff") } startThreadId:{ startThreadId }<br />endTime:{ endTime.ToString("yyyy-MM-dd HH:mm:ss:fff") } endThreadId:{ endThreadId }<br /><br />");
        }

        /// <summary>
        /// 耗时操作
        /// </summary>
        /// <returns></returns>
        private void DoSomething()
        {
            Thread.Sleep(10000);
        }
    }
}
