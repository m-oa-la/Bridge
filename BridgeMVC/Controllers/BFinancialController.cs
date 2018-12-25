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
    public class BFinancialController : Controller
    {
        // GET: BFinancial
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var s = await DocumentDBRepository<BFinancial>.GetItemsAsync(d => d.Tag == "BFinancial");
            return View(s);
        }

        [ActionName("Create")]
        public ActionResult Create()
        {
            var S = new BFinancial();
            return View(S);
        }


        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Tag,BridgeModule,CertType,MSA,TSA,AllocationFee,ServiceCode,Description," +
            "HourlyRate,Deliverable,CertAction1,CertAction2,CertAction3,CertAction4")] BFinancial item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BFinancial>.CreateItemAsync(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Tag,BridgeModule,CertType,MSA,TSA,AllocationFee,ServiceCode,Description," +
            "HourlyRate,Deliverable,CertAction1,CertAction2,CertAction3,CertAction4")] BFinancial item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BFinancial>.UpdateItemAsync(item.Id, item);
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

            BFinancial item = await DocumentDBRepository<BFinancial>.GetItemAsync(id);
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

            BFinancial r = await DocumentDBRepository<BFinancial>.GetItemAsync(id);

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
            BFinancial r = await DocumentDBRepository<BFinancial>.GetItemAsync(id);
            await DocumentDBRepository<BFinancial>.DeleteItemAsync(r.Id);

            return RedirectToAction("Index");
        }

    }
}