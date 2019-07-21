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
    public class BingUserController : Controller
    {
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var s = await DocumentDBRepository<BingUser>.GetItemsAsync(d => d.Tag == "BingUser");
            return View(s);
        }

        [ActionName("Create")]
        public ActionResult Create()
        {
            var S = new BingUser();
            return View(S);
        }


        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Tag,Id,employeeID,email,signature,firstname,lastname,bridgesGranted,BridgeLastUsed")] BingUser item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BingUser>.CreateItemAsync(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Tag,Id,employeeID,email,signature,firstname,lastname,bridgesGranted,BridgeLastUsed")] BingUser item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BingUser>.UpdateItemAsync(item.Id, item);
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

            BingUser item = await DocumentDBRepository<BingUser>.GetItemAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }




    }
}