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
    public class BEmailController : Controller
    {
        // GET: BEmail
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var s = await DocumentDBRepository<BEmail>.GetItemsAsync(d => d.Tag == "BEmail");
            return View(s);
        }

        [ActionName("Create")]
        public ActionResult Create()
        {
            var S = new BEmail();
            return View(S);
        }


        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Tag,BridgeModule,TemplateName,MailTo,MailCC,MailBOdy,MailTitle")] BEmail item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BEmail>.CreateItemAsync(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Tag,BridgeModule,TemplateName,MailTo,MailCC,MailBOdy,MailTitle")] BEmail item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BEmail>.UpdateItemAsync(item.Id, item);
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

            BEmail item = await DocumentDBRepository<BEmail>.GetItemAsync(id);
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

            BEmail r = await DocumentDBRepository<BEmail>.GetItemAsync(id);

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
            BEmail r = await DocumentDBRepository<BEmail>.GetItemAsync(id);
            await DocumentDBRepository<BEmail>.DeleteItemAsync(r.Id);

            return RedirectToAction("Index");
        }
    }
}