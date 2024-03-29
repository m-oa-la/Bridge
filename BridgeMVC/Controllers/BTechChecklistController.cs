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
    [Authorize]
    public class BTechChecklistController : Controller
    {

        public string JobInstanceId = "891fb8cc-1ec0-499f-b2c7-d21f318c90f5";

        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync(string searchString)
        {
            string bm = (string)Session["BridgeModule"];

            ViewBag.LMainProdType = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm && d.ListType == "MainProdType");
            ViewBag.LSubProdType = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm && d.ListType == "SubProdType");
            ViewBag.Job = await DocumentDBRepository.GetItemAsync<Job>(JobInstanceId);

            var s = await DocumentDBRepository.GetItemsAsync<BTechChecklist>(d => d.Tag == "BTechChecklist");
           
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();

                if(searchString != "all") { 
           
                s = s.Where(x => (x.SubProdType + " " + x.Subject + " " + x.Condition).ToLower().Contains(searchString));
                }
            }


            return View(s);
        }

        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            string bm = (string)Session["BridgeModule"];
            ViewBag.LMainProdType = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm && d.ListType == "MainProdType");
            ViewBag.LSubProdType = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm && d.ListType == "SubProdType");
            ViewBag.Job = await DocumentDBRepository.GetItemAsync<Job>(JobInstanceId);
     
            var S = new BTechChecklist();

            S.BridgeModule = bm;
            S.MainProdType = "Life-Saving appliances";

            return View(S);
        }

      [ActionName("SaveBTCItem")]
      public async Task<string> SaveBTCItem(string itemId, string Subject, string SubProdType, string GudianceNote, string RuleRef, string Condition)
        {
            BTechChecklist btc = await DocumentDBRepository.GetItemAsync<BTechChecklist>(itemId);

            btc.Subject = Subject;
            btc.SubProdType = SubProdType;
            btc.GudianceNote = GudianceNote;
            btc.RuleRef = RuleRef;
            btc.Condition = Condition;

            await DocumentDBRepository.UpdateItemAsync<BTechChecklist>(btc.Id, btc);
            return "Saved";
        }



        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Tag,Id,BridgeModule,BookMarkName,Description,Formula,Condition,ItemNo," +
            "TemplateName,MainProdType,SubProdType,Uk,Subject,RuleRef,MEDItemNo,GudianceNote,Chapter")] BTechChecklist item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.CreateItemAsync<BTechChecklist>(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Tag,Id,BridgeModule,BookMarkName,Description,Formula,Condition,ItemNo," +
            "TemplateName,MainProdType,SubProdType,Uk,Subject,RuleRef,MEDItemNo,GudianceNote,Chapter")] BTechChecklist item)
        { 
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<BTechChecklist>(item.Id, item);
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

            BTechChecklist item = await DocumentDBRepository.GetItemAsync<BTechChecklist>(id);
            var j = await DocumentDBRepository.GetItemAsync<Job>(JobInstanceId);
            ViewBag.Job = j;

            string bm = (string)Session["BridgeModule"];
            ViewBag.LMainProdType = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm && d.ListType == "MainProdType");
            ViewBag.LSubProdType = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm && d.ListType == "SubProdType");


            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }


    }
}