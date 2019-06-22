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
    public class BingEmailController : Controller
    {
        // GET: BingEmail
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var s = await DocumentDBRepository<BingEmail>.GetItemsAsync(d => d.Tag == "BingEmail");
            return View(s);
        }

        [ActionName("Create")]
        public ActionResult Create()
        {
            var S = new BingEmail();
            return View(S);
        }


        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Tag,BridgeModule,TemplateName,MailTo,MailCC,MailBOdy,MailTitle")] BingEmail item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BingEmail>.CreateItemAsync(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Tag,BridgeModule,TemplateName,MailTo,MailCC,MailBOdy,MailTitle")] BingEmail item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BingEmail>.UpdateItemAsync(item.Id, item);
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

            BingEmail item = await DocumentDBRepository<BingEmail>.GetItemAsync(id);
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

            BingEmail r = await DocumentDBRepository<BingEmail>.GetItemAsync(id);

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
            BingEmail r = await DocumentDBRepository<BingEmail>.GetItemAsync(id);
            await DocumentDBRepository<BingEmail>.DeleteItemAsync(r.Id);

            return RedirectToAction("Index");
        }
    }
}