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
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Dynamic;
using System.Reflection;

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


            if (ii == null)
            {
                return HttpNotFound();
            }

            return View(ii);
        }

        public async Task<ActionResult> ExportToWordAsync(string ioraId)
        {
            ;
            //var BIORAs = await DocumentDBRepository<BIORA>.GetItemsAsync(d => (d.Tag == "BIORA") && (d.BridgeModule == blu));
            IORA ii = await DocumentDBRepository.GetItemAsync<IORA>(ioraId);
            var blu = ii.BridgeModule;
            var npsjobid = ii.NpsJobID;
            string savePath = Server.MapPath("~/App_Data/" + blu + "/Projects/" + ii.NpsJobID + " IORA.docx");
            string templatePath = Server.MapPath("~/App_Data/" + blu + "/Templates/IORA Template.docx");

             Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document doc = new Microsoft.Office.Interop.Word.Document();
            doc = app.Documents.Open(templatePath);
            doc.Activate();


            Type t = ii.GetType();
            PropertyInfo[] props = t.GetProperties();
            foreach (var prop in props)
            {
                string pname = prop.Name;
                if (doc.Bookmarks.Exists(pname))
                {
                    string val = prop.GetValue(ii, null) as string;
                    doc.Bookmarks[pname].Select();
                    app.Selection.TypeText(val);

                    string s = pname + "_1";
                    if (doc.Bookmarks.Exists(s))
                    {
                        doc.Bookmarks[s].Select();
                        app.Selection.TypeText(val);
                    }
                }
            }

            doc.SaveAs2(savePath);
            app.Application.Quit();
            Session["dbJobId"] = ii.DbJobId;
            return RedirectToAction("ProjFolder", "Job");
            //Response.Write("Success");
        }

        [HttpGet]
        public async Task<string> ReadEmployeeInfo(string sig)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            string atoken = await GetAccessTokenAsync();

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "057e18c4802a4534bf32977f3e4af5ba");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + atoken);

            var uri = "https://testapi-internal.dnvgl.com/PartnerII_Test/api/EmployeesExtendedBySignature?PersonSignature=" + sig;

            var response = await client.GetAsync(uri);
            return await response.Content.ReadAsStringAsync();
            //return atoken;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            string authority = "https://login.microsoftonline.com/adf10e2b-b6e9-41d6-be2f-c12bb566019c/";
            //var cache = _tokenCacheFactory.CreateForUser(User);


            var authContext = new Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext(authority);
            //App's credentials may be needed if access tokens need to be refreshed with a refresh token
            string clientId = "a0dccf3c-4333-49dd-b89d-00ee25ee1896";
            string clientSecret = "OCt9sJPOehC74smyUcWo7VyMDnyewszsu3V2zV3dMXg=";
            var credential = new ClientCredential(clientId, clientSecret);
            //var userId = User.GetObjectId();

            var result = await authContext.AcquireTokenAsync(
                "https://dnv.onmicrosoft.com/2da7430f-1437-4786-8f8b-0668159e9016", credential
                );
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