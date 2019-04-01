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
using System;

namespace BridgeMVC.Controllers
{
    [Authorize]
    public class TechChecklistController : Controller
    {
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var items = await DocumentDBRepository.GetItemsAsync<TechChecklist>(d => d.Tag == "TechChecklist");
            return View(items);
        }

        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            var j = await DocumentDBRepository.GetItemAsync<Job>((string)Session["DbJobId"]);
            ViewBag.Job = j;
            ViewBag.BTechChecklist = await DocumentDBRepository.GetItemsAsync<BTechChecklist>(d => d.Tag == "BTechChecklist" && d.BridgeModule == j.BridgeModule && d.MainProdType == j.MainProdType && d.SubProdType == j.SubProdType);

            ViewBag.LUser = await DocumentDBRepository.GetItemsAsync<BUser>(d => d.Tag == "BUser" && (d.BridgesGranted).Contains(j.BridgeModule));
            var i = new TechChecklist
            {
                Tag = "TechChecklist",
                BridgeModule = j.BridgeModule,
                NpsJobId = j.NpsJobId,
                DbJobId = j.Id,
                MEDItemNo = j.MEDItemNo,
                MainProdType = j.MainProdType,
                SubProdType = j.SubProdType,
                CustomerName = j.CustomerName,
                Designation = j.ProdDescription,
                Verifier = j.JobVerifier,
                CertAction = j.CertAction,
                ApprNote = j.ApprNote,

            };
            return View(i);
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Tag,Id,BridgeModule,DbJobId,MainProdType,SubProdType,Uk," +
            "CustomerName,Designation,CustomerNo,IssuerSig,IssuerSection,NpsJobId,CertAction,DavitMWL,WinchMWL,MHL,WireDia,DavitComment," +
            "Verifier,VerificationLvl,SurveyDate,SurveyStation,FinalizeDate,VerifyDate,ApprNote,VerifyNote,MEDItemNo,MBL,LocalUnit,IsFinalized" +
            "TCItems" )] TechChecklist item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.CreateItemAsync<TechChecklist>(item);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Tag,Id,BridgeModule,DbJobId,MainProdType,SubProdType,Uk," +
            "CustomerName,Designation,CustomerNo,IssuerSig,IssuerSection,NpsJobId,CertAction,DavitMWL,WinchMWL,MHL,WireDia,DavitComment," +
            "Verifier,VerificationLvl,SurveyDate,SurveyStation,FinalizeDate,VerifyDate,ApprNote,VerifyNote,MEDItemNo,MBL,LocalUnit,IsFinalized" +
            "TCItems" )] TechChecklist item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<TechChecklist>(item.Id, item);
                TechChecklist ii = item;
                Job j = await DocumentDBRepository.GetItemAsync<Job>(ii.DbJobId);
                ViewBag.Job = j;
                ViewBag.BTechChecklist = await DocumentDBRepository.GetItemsAsync<BTechChecklist>(d => d.Tag == "BTechChecklist" && d.BridgeModule == ii.BridgeModule && d.TemplateName == "TechChecklist");

                ViewBag.LUser = await DocumentDBRepository.GetItemsAsync<BUser>(d => d.Tag == "BUser" && (d.BridgesGranted).Contains(ii.BridgeModule));

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
            TechChecklist ii = await DocumentDBRepository.GetItemAsync<TechChecklist>(id);
            Job j = await DocumentDBRepository.GetItemAsync<Job>(ii.DbJobId);
            ViewBag.Job = j;
            ViewBag.BTechChecklist = await DocumentDBRepository.GetItemsAsync<BTechChecklist>(d => d.Tag == "BTechChecklist" && d.BridgeModule == ii.BridgeModule && d.TemplateName == "TechChecklist");
            ViewBag.LUser = await DocumentDBRepository.GetItemsAsync<BUser>(d => d.Tag == "BUser" && (d.BridgesGranted).Contains(ii.BridgeModule));


            if (ii == null)
            {
                return HttpNotFound();
            }

            return View(ii);
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TechChecklist r = await DocumentDBRepository.GetItemAsync<TechChecklist>(id);

            if (r == null)
            {
                return HttpNotFound();
            }

            return View(r);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            TechChecklist r = await DocumentDBRepository.GetItemAsync<TechChecklist>(id);
            await DocumentDBRepository.DeleteItemAsync(r.Id);

            return RedirectToAction("Index");
        }

    }
}