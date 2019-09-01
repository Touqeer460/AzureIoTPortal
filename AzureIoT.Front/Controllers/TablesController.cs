using AzureIOT.ConnectorService;
using AzureIOT.DAL.DataProvider;
using AzureIOT.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AzureIoT.FrontEnd.Controllers
{
    public class TablesController : Controller
    {
        private IDataService dataService;
        private ISubscriptionService subscriptionService;

        public TablesController()
        {
            //if (!AzureAuthorization.AuthToken.Authorized)
            //{
            //    throw new AuthenticationException();
            //}
            dataService = new DataProviderFile();
            subscriptionService = new AzureSubscriptionService();
        }

        //
        // GET: /Table/
        public async Task<ActionResult> Index()
        {
            List<DeviceGroup> groups = dataService.GetAllGroups();
            ViewBag.Groups = groups;
            return View(await dataService.GetAllDevices());
        }

        public ActionResult Telemetries()
        {
            return View(dataService.GetTelemetries());
        }

        public ActionResult DeviceGroups()
        {
            List<Telemetries> telemetries = dataService.GetTelemetries();
            ViewBag.Telemetries = telemetries;
            return View(dataService.GetAllGroups());
        }

        public bool AddDeviceGroup(DeviceGroup group)
        {
            if (group != null && group.Telemetries != null && group.Telemetries.Length == 1)
            {
                group.Telemetries = group.Telemetries[0].Split(',');
            }
            return dataService.InsertGroup(group);
        }

        public ActionResult RuleDetail(string Id)
        {
            List<Rules> rules = dataService.GetAllRules();
            return View(rules.Find(x => x.Id == Id));
        }

        public bool AddDevice(Device device)
        {
            return dataService.InsertDevice(device);
        }

        public bool AddTelemetry(Telemetries telemetry)
        {
            return dataService.InsertTelemetry(telemetry);
        }

        public bool AddRule(Rules rule)
        {
            return dataService.InsertRule(rule);
        }

        public ActionResult Rules()
        {
            ViewBag.Groups = dataService.GetAllGroups();
            ViewBag.Telemetries = dataService.GetTelemetries();
            ViewBag.Operators = AzureIOT.Models.Rules.AllowedOperators();
            return View(dataService.GetAllRules());
        }

        public ActionResult DeviceGroupDetail(string Id)
        {
            List<DeviceGroup> group = dataService.GetAllGroups();
            return View(group.Find(x => x.Id == Id));
        }

        public bool RemoveRule(string ruleId)
        {
            List<Rules> rules = dataService.GetAllRules();
            return dataService.RemoveRule(rules.Find(x => x.Id == ruleId));
        }

        public bool RemoveTelemetry(string telemetryId)
        {
            List<Telemetries> telemetries = dataService.GetTelemetries();
            return dataService.RemoveTelemetry(telemetries.Find(x => x.telemeteryId == telemetryId));
        }

        public bool RemoveGroup(string groupId)
        {
            List<DeviceGroup> groups = dataService.GetAllGroups();
            return dataService.RemoveGroup(groups.Find(x => x.Id == groupId));
        }

        public bool RemoveDevice(string deviceId)
        {
            List<Device> devices = dataService.GetAllDevices().Result;
            return dataService.RemoveDevice(devices.Find(x => x.Id == deviceId));
        }

        public ActionResult DeviceDetail(string Id)
        {
            return View(subscriptionService.DeviceDetail(Id));
        }
    }
}