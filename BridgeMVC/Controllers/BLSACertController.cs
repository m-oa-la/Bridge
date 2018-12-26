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
    public class BLSACertController : Controller
    {

        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var s = await DocumentDBRepository.GetItemsAsync<BLSACert>(d => d.Tag == "BLSACert");
            return View(s);
        }

        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {

            var j = await DocumentDBRepository.GetItemAsync<Job>("a74571b7-2758-48ae-bd1a-d88efc437f26");
            ViewBag.Job = j;
   
            //var f = await DocumentDBRepository<BFinancial>.GetItemsAsync(d => d.Tag == "BFinancial" && d.BridgeModule == j.BridgeModule && d.CertType == j.CertType);
            //ViewBag.FinancialSet = f.FirstOrDefault();
            var S = new BLSACert();
            return View(S);
        }


        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Tag,Id,BridgeModule,BookMarkName,Chapter,Formula,Description,Condition," +
            "Condition,Chapter")] BLSACert item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.CreateItemAsync<BLSACert>(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Tag,Id,BridgeModule,BookMarkName,Chapter,Formula,Description,Condition," +
            "Condition,Chapter")] BLSACert item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<BLSACert>(item.Id, item);
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

            BLSACert item = await DocumentDBRepository.GetItemAsync<BLSACert>(id);
            var j = await DocumentDBRepository.GetItemAsync<Job>("a74571b7-2758-48ae-bd1a-d88efc437f26");
            ViewBag.Job = j;



            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

    }
}