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
    public class BIORAController : Controller
    {
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var s = await DocumentDBRepository.GetItemsAsync<BIORA>(d => d.Tag == "BIORA");
            return View(s);
        }

        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            
            var j = await DocumentDBRepository.GetItemAsync<Job>("a74571b7-2758-48ae-bd1a-d88efc437f26");
            ViewBag.Job = j;
            var i = await DocumentDBRepository.GetItemAsync<IORA>("290c5999-2076-46f1-b40f-443f42cea4f8");
            ViewBag.IORA = i;
            var f = await DocumentDBRepository.GetItemsAsync<BFinancial>(d => d.Tag == "BFinancial" && d.BridgeModule == j.BridgeModule && d.CertType == j.CertType);
            ViewBag.FinancialSet = f.FirstOrDefault();
            var S = new BIORA();
            return View(S);
        }


        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Tag,Id,BridgeModule,Description,BookMarkName,Formula," +
            "Condition,Chapter")] BIORA item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.CreateItemAsync<BIORA>(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Tag,Id,BridgeModule,Description,BookMarkName,Formula," +
            "Condition,Chapter")] BIORA item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<BIORA>(item.Id, item);
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

            BIORA item = await DocumentDBRepository.GetItemAsync<BIORA>(id);
            var j = await DocumentDBRepository.GetItemAsync<Job>("a74571b7-2758-48ae-bd1a-d88efc437f26");
            ViewBag.Job = j;
            var i = await DocumentDBRepository.GetItemAsync<IORA>("db2da043-82da-488f-a5b5-12116323f3a4");
            ViewBag.IORA = i;
            var f = await DocumentDBRepository.GetItemsAsync<BFinancial>(d => d.Tag == "BFinancial" && d.BridgeModule == i.BridgeModule && d.CertType == j.CertType);
            ViewBag.FinancialSet = f.FirstOrDefault();

            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        

    }
}