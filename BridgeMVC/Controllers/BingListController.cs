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
    public class BingListController : Controller
    {
        // GET: BingList
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var s = await DocumentDBRepository<BingList>.GetItemsAsync(d => d.Tag == "BingList");
            return View(s);
        }

        [ActionName("Create")]
        public ActionResult Create()
        {
            var bm = (string)Session["BridgeModule"];
            var S = new BingList()
            {
                BridgeModule = bm,
            };
            return View(S);
        }


        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Tag,BridgeModule,ListType,ListItem,UpperLvl")] BingList item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BingList>.CreateItemAsync(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Tag,BridgeModule,ListType,ListItem,UpperLvl")] BingList item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BingList>.UpdateItemAsync(item.Id, item);
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

            BingList item = await DocumentDBRepository<BingList>.GetItemAsync(id);
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

            BingList r = await DocumentDBRepository<BingList>.GetItemAsync(id);

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
            BingList r = await DocumentDBRepository<BingList>.GetItemAsync(id);
            await DocumentDBRepository<BingList>.DeleteItemAsync(r.Id);

            return RedirectToAction("Index");
        }
    }
}