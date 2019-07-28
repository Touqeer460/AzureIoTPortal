using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureIoT.FrontEnd.Controllers
{
    public class TablesController : Controller
    {
        //
        // GET: /Table/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Telemetries()
        {
            return View();
        }

        public ActionResult DeviceGroups()
        {
            return View();
        }

        public ActionResult AddDeviceGroup()
        {
            return View();
        }
    }
}