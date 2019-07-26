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
    public class BingFinancialController : Controller
    {
        // GET: BingFinancial
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var s = await DocumentDBRepository<BingFinancial>.GetItemsAsync(d => d.Tag == "BingFinancial");
            return View(s);
        }

        [ActionName("Create")]
        public ActionResult Create()
        {
            var S = new BingFinancial();
            return View(S);
        }


        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Tag,BridgeModule,CertType,MSA,TSA,AllocationFee,ServiceCode,Description," +
            "HourlyRate,Deliverable,CertAction1,CertAction2,CertAction3,CertAction4")] BingFinancial item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BingFinancial>.CreateItemAsync(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Tag,BridgeModule,CertType,MSA,TSA,AllocationFee,ServiceCode,Description," +
            "HourlyRate,Deliverable,CertAction1,CertAction2,CertAction3,CertAction4")] BingFinancial item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BingFinancial>.UpdateItemAsync(item.Id, item);
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

            BingFinancial item = await DocumentDBRepository<BingFinancial>.GetItemAsync(id);
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

            BingFinancial r = await DocumentDBRepository<BingFinancial>.GetItemAsync(id);

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
            BingFinancial r = await DocumentDBRepository<BingFinancial>.GetItemAsync(id);
            await DocumentDBRepository<BingFinancial>.DeleteItemAsync(r.Id);

            return RedirectToAction("Index");
        }

    }
}