using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading.Tasks;
using BridgeMVC.Models;
using System.Diagnostics;

namespace BridgeMVC.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            
            var items = await DocumentDBRepository.GetItemsAsync<Item>(d => !d.Tag.Contains("ZZZZZ"));
            return View(items);
        }

        [ActionName("Create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Tag")] Item item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.CreateItemAsync<Item>(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Tag")] Item item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<Item>(item.Id, item);
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

            Item item = await DocumentDBRepository.GetItemAsync<Item>(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

    }
}