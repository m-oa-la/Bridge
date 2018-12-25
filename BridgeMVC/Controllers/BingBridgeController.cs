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
    public class BingBridgeController : Controller
    {
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var s = await DocumentDBRepository<BingBridge>.GetItemsAsync(d => d.Tag == "BingBridge");
            return View(s);
        }

        [ActionName("Create")]
        public ActionResult Create()
        {
            var S = new BingBridge();
            return View(S);
        }


        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Tag,Id,bridgeName,templatePath,archivePath")] BingBridge item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BingBridge>.CreateItemAsync(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Tag,Id,bridgeName,templatePath,archivePath")] BingBridge item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BingBridge>.UpdateItemAsync(item.Id, item);
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

            BingBridge item = await DocumentDBRepository<BingBridge>.GetItemAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }
                      
    }
}