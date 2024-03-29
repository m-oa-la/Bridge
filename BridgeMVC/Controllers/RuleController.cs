﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading.Tasks;
using BridgeMVC.Models;
using System.Diagnostics;
using Newtonsoft.Json;

namespace BridgeMVC.Controllers
{
    [Authorize]
    public class RuleController : Controller
    {
        // GET: Rule
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {

            var items = await DocumentDBRepository.GetItemsAsync<Rule>(d => d.Tag=="Rule" && d.DbJobId== (string)Session["DbJobId"]);
            //Session["BridgeModule"] = items.FirstOrDefault().BridgeModule;
            return View(items);
        }
        [ActionName("IndexReadOnly")]
        public async Task<ActionResult> IndexReadOnlyAsync()
        {

            var items = await DocumentDBRepository.GetItemsAsync<Rule>(d => d.Tag == "Rule" && d.DbJobId == (string)Session["DbJobId"]);
            //Session["BridgeModule"] = items.FirstOrDefault().BridgeModule;
            return View(items);
        }

        [ActionName("FullList")]
        public async Task<ActionResult> FullListAsync()
        {

            var items = await DocumentDBRepository.GetItemsAsync<Rule>(d => d.Tag == "Rule");
            //Session["BridgeModule"] = items.FirstOrDefault().BridgeModule;
            return View(items);
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Rule r = await DocumentDBRepository.GetItemAsync<Rule>(id);

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
            Rule r = await DocumentDBRepository.GetItemAsync<Rule>(id);
            await DocumentDBRepository.DeleteItemAsync(r.Id);

            return RedirectToAction("Index");
        }









        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            var r = new Rule
            {
                Tag = "Rule",
                NpsJobId=(string)Session["NpsJobId"],
                DbJobId=(string)Session["DbJobId"],
                BridgeModule=(string)Session["BridgeModule"]
               
            };

            ViewBag.RuleSelectList = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == r.BridgeModule && d.ListType=="Rule");

           
            return View(r);
            
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Tag,BridgeModule,RuleName,NpsJobId,DbJobId")] Rule item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.CreateItemAsync<Rule>(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Tag,BridgeModule,RuleName,NpsJobId,DbJobId")] Rule item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<Rule>(item.Id, item);
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

            Rule item = await DocumentDBRepository.GetItemAsync<Rule>(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        



    }
}