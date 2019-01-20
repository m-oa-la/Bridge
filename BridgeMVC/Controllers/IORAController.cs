using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading.Tasks;
using BridgeMVC.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Dynamic;
using System.Reflection;
using MABridge.OpenXml;
using System.IO;
using System.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace BridgeMVC.Controllers
{
    [Authorize]
    public class IORAController : Controller
    {
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {

            var items = await DocumentDBRepository.GetItemsAsync<IORA>(d => d.Tag=="IORA");
            return View(items);
        }

        [ActionName("Create")]
        public ActionResult Create()
        {
            var i = new IORA
            {
                Tag = "IORA",
                BridgeModule = (string)Session["BridgeModule"],
                NpsJobID = (string)Session["NpsJobId"],
                DbJobId = (string)Session["DbJobId"],
                IORAFee = (string)Session["IORAFee"],
            };

            return View(i);

        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Tag,BridgeModule,NpsJobID," +
                "DnvUnitName501,DnvUnitNo501,DgIntUnVAT501,DnvIntCompAccnt501,DnvIntUnPrCeNo501,DpIntUnProjNo501," +
                "DnvUnitName502,DnvUnitNo502,DgIntUnVAT502,DnvContPersName502,DnvIntCompAccnt502,DnvIntUnPrCeNo502," +
                "DpIntUnProjNo502,DpProjName01,DpProjWorkLoc01,DpServiceName01,DpServiceCode01,DgCustName01," +
                "DgCustomerRef01,DpProjStartDate01,DpProjStartEnd01,DpSoW01,DpSupportingDocs01,IORAFee,DgSpecialConditions," +
                "Str_SpecialC,DnvIntUnPlace501,DnvIntUnDate501,DnvIntUnSigName501,DnvIntUnSigTitle501," +
            "SellingContactSig,BuyingContactSig,DgDNVDocNo01,DnvContPersName501,DpDeliverables01,IORASentTime,SignedIoraRcTime," +
            "NPSJNo,DbJobId" )] IORA item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.CreateItemAsync<IORA>(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Tag,BridgeModule,NpsJobID," +
                "DnvUnitName501,DnvUnitNo501,DgIntUnVAT501,DnvIntCompAccnt501,DnvIntUnPrCeNo501,DpIntUnProjNo501," +
                "DnvUnitName502,DnvUnitNo502,DgIntUnVAT502,DnvContPersName502,DnvIntCompAccnt502,DnvIntUnPrCeNo502," +
                "DpIntUnProjNo502,DpProjName01,DpProjWorkLoc01,DpServiceName01,DpServiceCode01,DgCustName01," +
                "DgCustomerRef01,DpProjStartDate01,DpProjStartEnd01,DpSoW01,DpSupportingDocs01,IORAFee,DgSpecialConditions," +
                "Str_SpecialC,DnvIntUnPlace501,DnvIntUnDate501,DnvIntUnSigName501,DnvIntUnSigTitle501," +
            "SellingContactSig,BuyingContactSig,DgDNVDocNo01,DnvContPersName501,DpDeliverables01,IORASentTime,SignedIoraRcTime," +
            "NPSJNo,DbJobId" )] IORA item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<IORA>(item.Id, item);
                IORA ii = item;
                Job j = await DocumentDBRepository.GetItemAsync<Job>(ii.DbJobId);
                ViewBag.Job = j;
                ViewBag.BIORA = await DocumentDBRepository.GetItemsAsync<BIORA>(d => d.Tag == "BIORA" && d.BridgeModule == ii.BridgeModule);
                ViewBag.Rules = await DocumentDBRepository.GetItemsAsync<Rule>(d => d.Tag == "Rule" && d.DbJobId == ii.DbJobId);
                var f = await DocumentDBRepository.GetItemsAsync<BFinancial>(d => d.Tag == "BFinancial" && d.BridgeModule == ii.BridgeModule && d.CertType == j.CertType);
                ViewBag.FinancialSet = f.FirstOrDefault();
                ViewBag.LUser = await DocumentDBRepository.GetItemsAsync<BUser>(d => d.Tag == "BUser" && (d.BridgesGranted).Contains(j.BridgeModule));
                 
                //return RedirectToAction("Index");

                return View(item);
            }
            return View(item);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IORA ii = await DocumentDBRepository.GetItemAsync<IORA>(id);
            Job j = await DocumentDBRepository.GetItemAsync<Job>(ii.DbJobId);
            ViewBag.Job = j;
            ViewBag.BIORA = await DocumentDBRepository.GetItemsAsync<BIORA>(d => d.Tag == "BIORA" && d.BridgeModule==ii.BridgeModule);
            ViewBag.Rules = await DocumentDBRepository.GetItemsAsync<Rule>(d => d.Tag == "Rule" && d.DbJobId == ii.DbJobId);
            var f = await DocumentDBRepository.GetItemsAsync<BFinancial>(d => d.Tag == "BFinancial" && d.BridgeModule == ii.BridgeModule && d.CertType == j.CertType );
            ViewBag.FinancialSet = f.FirstOrDefault();
            ViewBag.LUser = await DocumentDBRepository.GetItemsAsync<BUser>(d => d.Tag == "BUser" && (d.BridgesGranted).Contains(j.BridgeModule));

            //string clientId = ConfigurationManager.AppSettings["iAPI:ClientID"];
            //string clientSecret = ConfigurationManager.AppSettings["iAPI:Password"];
            //string aadAuthority = ConfigurationManager.AppSettings["iAPI:AADAuthority"];
            //string partnerIIAPISIT = ConfigurationManager.AppSettings["iAPI:PartnerIIAPISIT"];

            //var authContext = new AuthenticationContext(aadAuthority);
            ////App's credentials may be needed if access tokens need to be refreshed with a refresh token
            //var credential = new ClientCredential(clientId, clientSecret);

            //ViewBag.AADToken = credential;

            //var result = await authContext.AcquireTokenAsync(partnerIIAPISIT, credential);
            //ViewBag.PartnerToken = result.AccessToken;





            if (ii == null)
            {
                return HttpNotFound();
            }

            return View(ii);
        }

        public async Task<ActionResult> ExportToWordAsync(string ioraId)
        {
            
            IORA ii = await DocumentDBRepository.GetItemAsync<IORA>(ioraId);
            var blu = ii.BridgeModule;
            var npsjobid = ii.NpsJobID;
            string savePath = Server.MapPath("~/App_Data/" + blu + "/Projects/" + ii.NpsJobID + " IORA.docx");
            string templatePath = Server.MapPath("~/App_Data/" + blu + "/Templates/IORA Template.docx");

            Type t = ii.GetType();
            PropertyInfo[] props = t.GetProperties();

            var fieldValues = t.GetProperties()
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(ii, null) as string);

            if (System.IO.File.Exists(savePath))
            {
                System.IO.File.Delete(savePath);
            }

            var bytes = System.IO.File.ReadAllBytes(templatePath);
            using (var mem = new MemoryStream(bytes)) {
                OpenXmlWordHelper.FillForm(fieldValues, mem);
                using (var target = new FileStream(savePath, FileMode.CreateNew))
                {
                    mem.WriteTo(target);
                }
            }
            
            Session["dbJobId"] = ii.DbJobId;
            return RedirectToAction("ProjFolder", "Job");
        }

        [HttpGet]
        public async Task<string> ReadEmployeeInfo(string sig)
        {
            WebClient client = new WebClient
            {
                UseDefaultCredentials = true
            };

            string atoken = await GetAccessTokenAsync();
            string partnerIISubKey = ConfigurationManager.AppSettings["iAPI:PartnerIISubKey"];
            client.Headers.Add("Ocp-Apim-Subscription-Key", partnerIISubKey);
            client.Headers.Add("Authorization", "Bearer " + atoken);

            var uri = "https://api-internal.dnvgl.com/PartnerII/api/EmployeesExtendedBySignature?PersonSignature=" + sig;

            var response = client.DownloadString(uri);
            return response;
        }




        private async Task<string> GetAccessTokenAsync()
        {
            //throw new NotImplementedException("GetAccessTokenAsync not implemented");
            string clientId = ConfigurationManager.AppSettings["iAPI:ClientID"];
            string clientSecret = ConfigurationManager.AppSettings["iAPI:Password"];
            string aadAuthority = ConfigurationManager.AppSettings["iAPI:AADAuthority"];
            string partnerIIAPISIT = ConfigurationManager.AppSettings["iAPI:PartnerIIAPISIT"];

            var authContext = new Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext(aadAuthority);
            //App's credentials may be needed if access tokens need to be refreshed with a refresh token
            var credential = new ClientCredential(clientId, clientSecret);
            var result = await authContext.AcquireTokenAsync(partnerIIAPISIT, credential);
            return result.AccessToken;
        }

        [HttpPost]
        public async Task<string> ReadClientInfo(string sig)
        {
            var client = new HttpClient();
            string atoken = await GetAccessTokenAsync();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "4399b860b63d4f4689a06627fa2ab1ff");
            var uri = "https://testapi-internal.dnvgl.com/customersearch/CustomerSearch?" + sig;
            var response = await client.GetAsync(uri);
            return await response.Content.ReadAsStringAsync();
        }

    }
}