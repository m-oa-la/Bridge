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
        public async Task<ActionResult> CreateAsync()
        {
            var i = new IORA
            {
                Tag = "IORA",
                BridgeModule = (string)Session["BridgeModule"],
                NpsJobID = (string)Session["NpsJobId"],
                DbJobId = (string)Session["DbJobId"],
                IORAFee = (int)Session["IORAFee"],
            };

            await DocumentDBRepository.CreateItemAsync<IORA>(i);
            var ioras = await DocumentDBRepository.GetItemsAsync<IORA>(d => d.Tag == "IORA" && d.NpsJobID == i.NpsJobID);
            IORA iora1 = ioras.FirstOrDefault();

            return RedirectToAction("Edit/" + iora1.Id, "IORA");

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
            "NPSJNo,DbJobId,SendingFlag,IORASentBy" )] IORA item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.CreateItemAsync<IORA>(item);
                var ioras = await  DocumentDBRepository.GetItemsAsync<IORA>(d => d.Tag == "IORA" && d.NpsJobID == item.NpsJobID);
                IORA iora1 = ioras.FirstOrDefault();

                return RedirectToAction("Edit/" + iora1.Id, "IORA");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(IORA item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<IORA>(item.Id, item);
                IORA ii = item;
                Job j = await DocumentDBRepository.GetItemAsync<Job>(ii.DbJobId);
                if (!string.IsNullOrEmpty(item.IORASentBy))
                {
                    j.IoraSentTime = ii.IORASentTime;
                    j.Task2 = "Y";
                    await DocumentDBRepository.UpdateItemAsync<Job>(j.Id, j);
                }
                else
                {
                    j.IoraSentTime = null;
                    j.Task2 = "TASK";
                    await DocumentDBRepository.UpdateItemAsync<Job>(j.Id, j);
                }


                ViewBag.Job = j;
                ViewBag.BIORA = await DocumentDBRepository.GetItemsAsync<BIORA>(d => d.Tag == "BIORA" && d.BridgeModule == ii.BridgeModule);
                ViewBag.Rules = await DocumentDBRepository.GetItemsAsync<Rule>(d => d.Tag == "Rule" && d.DbJobId == ii.DbJobId);
                var f = await DocumentDBRepository.GetItemsAsync<BFinancial>(d => d.Tag == "BFinancial" && d.BridgeModule == ii.BridgeModule && d.CertType == j.CertType);
                ViewBag.FinancialSet = f.FirstOrDefault();
                ViewBag.LUser = await DocumentDBRepository.GetItemsAsync<BUser>(d => d.Tag == "BUser" && (d.BridgesGranted).Contains(j.BridgeModule));

                //await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);
                if (!string.IsNullOrEmpty(item.SendingFlag) && item.SendingFlag != "-")
                {
                    Session["SendingFlag"] = item.SendingFlag;
                    return Redirect(Url.Content("~/Job/SendJob/" + item.DbJobId));
                }
                else
                {
                    return View(item);
                }

            }
            else
            {
                return View(item);
            }
            
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
            ii.SendingFlag = "-";
            return View(ii);
        }

       

    }
}