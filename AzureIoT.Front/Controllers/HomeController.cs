using AzureIOT.ConnectorService;
using AzureIOT.DAL.DataProvider;
using AzureIOT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureIoT.FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        ISubscriptionService subscriptionService;
        IDataService dataService;
        public HomeController()
        {
            subscriptionService = new AzureSubscriptionService();
            dataService = new DataProviderFile();
        }
        public ActionResult Index()
        {
            DashboardFacts facts = subscriptionService.GetDashboardFacts();
            facts.NumberOfRules = dataService.GetAllRules().Count();
            ViewBag.Facts = facts;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            ViewBag.LoginFailed = true;
            return View();
        }
    }
}