using BridgeMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BridgeMVC.Controllers
{
    [Authorize]
    public class BFinancialController : Controller
    {
        // GET: BFinancial
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            string bm = (string)Session["BridgeModule"];
            var s = await DocumentDBRepository.GetItemsAsync<BFinancial>(d => d.Tag == "BFinancial" && d.BridgeModule == bm);
            return View(s.OrderBy(s1 => s1.CertType));
        }

        [ActionName("Create")]
        public ActionResult Create()
        {
            string bm = (string)Session["BridgeModule"];
            var S = new BFinancial();
            S.BridgeModule = bm;
            return View(S);
        }


        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Tag,BridgeModule,CertType,MSA,TSA,AllocationFee,ServiceCode,Description," +
            "HourlyRate,Deliverable,CertAction1,CertAction2,CertAction3,CertAction4,ConvFactor,ColorCode")] BFinancial item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.CreateItemAsync<BFinancial>(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Tag,BridgeModule,CertType,MSA,TSA,AllocationFee,ServiceCode,Description," +
            "HourlyRate,Deliverable,CertAction1,CertAction2,CertAction3,CertAction4,ConvFactor,ColorCode")] BFinancial item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<BFinancial>(item.Id, item);
                return RedirectToAction("Index");
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

            BFinancial item = await DocumentDBRepository.GetItemAsync<BFinancial>(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BFinancial r = await DocumentDBRepository.GetItemAsync<BFinancial>(id);

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
            BFinancial r = await DocumentDBRepository.GetItemAsync<BFinancial>(id);
            await DocumentDBRepository.DeleteItemAsync(r.Id);

            return RedirectToAction("Index");
        }

    }
}