﻿using BridgeMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BridgeMVC.Controllers
{
    public class BingIORAController : Controller
    {
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var s = await DocumentDBRepository<BingIORA>.GetItemsAsync(d => d.Tag == "BingIORA");
            return View(s);
        }

        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            
            var j = await DocumentDBRepository<Job>.GetItemAsync("a74571b7-2758-48ae-bd1a-d88efc437f26");
            ViewBag.Job = j;
            var i = await DocumentDBRepository<IORA>.GetItemAsync("290c5999-2076-46f1-b40f-443f42cea4f8");
            ViewBag.IORA = i;
            var f = await DocumentDBRepository<BingFinancial>.GetItemsAsync(d => d.Tag == "BingFinancial" && d.BridgeModule == j.BridgeModule && d.CertType == j.CertType);
            ViewBag.FinancialSet = f.FirstOrDefault();
            var S = new BingIORA();
            return View(S);
        }


        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Tag,Id,BridgeModule,Description,BookMarkName,Formula," +
            "Condition,Chapter")] BingIORA item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BingIORA>.CreateItemAsync(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Tag,Id,BridgeModule,Description,BookMarkName,Formula," +
            "Condition,Chapter")] BingIORA item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<BingIORA>.UpdateItemAsync(item.Id, item);
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

            BingIORA item = await DocumentDBRepository<BingIORA>.GetItemAsync(id);
            var j = await DocumentDBRepository<Job>.GetItemAsync("a74571b7-2758-48ae-bd1a-d88efc437f26");
            ViewBag.Job = j;
            var i = await DocumentDBRepository<IORA>.GetItemAsync("290c5999-2076-46f1-b40f-443f42cea4f8");
            ViewBag.IORA = i;
            var f = await DocumentDBRepository<BingFinancial>.GetItemsAsync(d => d.Tag == "BingFinancial" && d.BridgeModule == i.BridgeModule && d.CertType == j.CertType);
            ViewBag.FinancialSet = f.FirstOrDefault();

            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        

    }
}