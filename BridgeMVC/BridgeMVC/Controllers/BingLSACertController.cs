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
    public class BingLSACertController : Controller
    {

        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var s = await DocumentDBRepository<BingLSACert>.GetItemsAsync(d => d.Tag == "BingLSACert");
            return View(s);
        }

        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {

            var j = await DocumentDBRepository<Job>.GetItemAsync("a74571b7-2758-48ae-bd1a-d88efc437f26");
            ViewBag.Job = j;
   
            //var f = await DocumentDBRepository<BingFinancial>.GetItemsAsync(d => d.Tag == "BingFinancial" && d.BridgeModule == j.BridgeModule && d.CertType == j.CertType);
            //ViewBag.FinancialSet = f.FirstOrDefault();
            var S = new BingLSACert();
            return View(S);
        }


        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Tag,Id,BridgeModule,BookMarkName,Chapter,Formula,Description,Condition," +
            "Condition,Chapter")] BingLSACert item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BingLSACert>.CreateItemAsync(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Tag,Id,BridgeModule,BookMarkName,Chapter,Formula,Description,Condition," +
            "Condition,Chapter")] BingLSACert item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BingLSACert>.UpdateItemAsync(item.Id, item);
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

            BingLSACert item = await DocumentDBRepository<BingLSACert>.GetItemAsync(id);
            var j = await DocumentDBRepository<Job>.GetItemAsync("a74571b7-2758-48ae-bd1a-d88efc437f26");
            ViewBag.Job = j;



            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

    }
}