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
    public class BListController : Controller
    {
        // GET: BList
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var s = await DocumentDBRepository<BList>.GetItemsAsync(d => d.Tag == "BList");
            return View(s);
        }

        [ActionName("Create")]
        public ActionResult Create()
        {
            var bm = (string)Session["BridgeModule"];
            var S = new BList()
            {
                BridgeModule = bm,
            };
            return View(S);
        }


        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Tag,BridgeModule,ListType,ListItem,UpperLvl")] BList item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BList>.CreateItemAsync(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Tag,BridgeModule,ListType,ListItem,UpperLvl")] BList item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BList>.UpdateItemAsync(item.Id, item);
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

            BList item = await DocumentDBRepository<BList>.GetItemAsync(id);
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

            BList r = await DocumentDBRepository<BList>.GetItemAsync(id);

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
            BList r = await DocumentDBRepository<BList>.GetItemAsync(id);
            await DocumentDBRepository<BList>.DeleteItemAsync(r.Id);

            return RedirectToAction("Index");
        }
    }
}