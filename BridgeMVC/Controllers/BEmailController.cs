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
    public class BEmailController : Controller
    {
        // GET: BEmail
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var s = await DocumentDBRepository.GetItemsAsync<BEmail>(d => d.Tag == "BEmail");
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
                await DocumentDBRepository.CreateItemAsync<BEmail>(item);
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
                await DocumentDBRepository.UpdateItemAsync<BEmail>(item.Id, item);
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

            BEmail item = await DocumentDBRepository.GetItemAsync<BEmail>(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var j = await DocumentDBRepository.GetItemAsync<Job>("93ffbdaa-72bd-4df8-a154-d140a73ef700");
            ViewBag.Job = j;
            var i = await DocumentDBRepository.GetItemAsync<IORA>("38ea1592-88d9-4c53-a6d9-b571b1c37532");
            ViewBag.IORA = i;
            var f = await DocumentDBRepository.GetItemsAsync<BFinancial>(d => d.Tag == "BFinancial" && d.BridgeModule == i.BridgeModule && d.CertType == j.CertType);
            ViewBag.FinancialSet = f.FirstOrDefault();
            var u = await DocumentDBRepository.GetItemsAsync<BUser>(d => d.Tag == "BUser" && d.Signature == "SXL");
            ViewBag.TargetUser = u.FirstOrDefault();

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

            BEmail r = await DocumentDBRepository.GetItemAsync<BEmail>(id);

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
            BEmail r = await DocumentDBRepository.GetItemAsync<BEmail>(id);
            await DocumentDBRepository.DeleteItemAsync(r.Id);

            return RedirectToAction("Index");
        }
    }
}