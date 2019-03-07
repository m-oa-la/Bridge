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
    public class BListController : Controller
    {
        // GET: BList
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync(string searchString)
        {
            string bm = (string)Session["BridgeModule"];
            var BLs = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm);
            BLs.OrderBy(s => s.BridgeModule);

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();

                BLs = BLs.Where(s => s.ListType.ToLower().Contains(searchString) || s.ListItem.ToLower().Contains(searchString));
            }


            return View(BLs.OrderBy(s => s.ListType));
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
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Tag,BridgeModule,ListType,ListItem,UpperLvl,Note")] BList item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.CreateItemAsync<BList>(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Tag,BridgeModule,ListType,ListItem,UpperLvl,Note")] BList item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<BList>(item.Id, item);
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

            BList item = await DocumentDBRepository.GetItemAsync<BList>(id);
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

            BList r = await DocumentDBRepository.GetItemAsync<BList>(id);

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
            BList r = await DocumentDBRepository.GetItemAsync<BList>(id);
            await DocumentDBRepository.DeleteItemAsync(r.Id);

            return RedirectToAction("Index");
        }
    }
}