using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TimeMVC.Models;

namespace TimeMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public string getData(int test){
            string sqlText = "select currentIndex as CI,startTime,endTime,lastTime,createDate,note,type from mytime ";
            DataTable dt = SqlHelper.ExecuteDataTable(sqlText);
            return DataTableToJsonWithJsonNet(dt);
        }
        public string DataTableToJsonWithJsonNet(DataTable table)
        {
            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(table);
            return JsonString;
        }
        public IActionResult About(string name)
        {
            ViewData["Message"] =name;

            return View();
        }
        public HomeController(IOptions<ConfigurationModel> setting)
        {
            SqlHelper.Config = setting.Value;
            
        }
        public IActionResult Contact()
        {
            //ViewData["Message"] = "Your contact page.";

            //return View();

            return Content("test");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
