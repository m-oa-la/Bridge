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
    public class BUserController : Controller
    {
        [ActionName("_Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var s = await DocumentDBRepository.GetItemsAsync<BUser>(d => d.Tag == "BUser");
            return View(s);
        }

        [ActionName("_Create")]
        public ActionResult Create()
        {
            var S = new BUser();
            return View(S);
        }


        [HttpPost]
        [ActionName("_Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Tag,Id,employeeID,email,signature,firstname,lastname,bridgesGranted,BridgeLastUsed")] BUser item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.CreateItemAsync<BUser>(item);
                return RedirectToAction("_Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("_Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Tag,Id,employeeID,email,signature,firstname,lastname,bridgesGranted,BridgeLastUsed")] BUser item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<BUser>(item.Id, item);
                return RedirectToAction("_Index");
            }

            return View(item);
        }

        [ActionName("_Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BUser item = await DocumentDBRepository.GetItemAsync<BUser>(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }




    }
}