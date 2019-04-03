using System;
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
    public class TechChecklistController : Controller
    {
        // GET: TechChecklist
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            string jid = (string)Session["DbJobId"];
            var items = await DocumentDBRepository.GetItemsAsync<TechChecklist>(d => d.Tag == "TechChecklist" && d.DbJobId == jid);

            if (items.Count() == 0)
            {
                Job j = await DocumentDBRepository.GetItemAsync<Job>(jid);
                var btcs = await DocumentDBRepository.GetItemsAsync<BTechChecklist>(d => d.Tag == "BTechChecklist" &&
                d.MainProdType == j.MainProdType && d.SubProdType == j.SubProdType);

                if(btcs.Count() > 0)
                {
                    foreach(BTechChecklist btc in btcs)
                    {
                        TechChecklist tc = new TechChecklist
                        {
                            Tag = "TechChecklist",
                            DbJobId = j.Id,
                            TCGuidance = btc.GudianceNote,
                            TCNo = btc.ItemNo,
                            TCRuleRef = btc.RuleRef,
                            TCSubject = btc.Subject,
                        };

                        await DocumentDBRepository.CreateItemAsync<TechChecklist>(tc);
                       

                    }
                    items = await DocumentDBRepository.GetItemsAsync<TechChecklist>(d => d.Tag == "TechChecklist" && d.DbJobId == jid);
                }

            }


             

            return View(items);
        }
        [ActionName("IndexReadOnly")]
        public async Task<ActionResult> IndexReadOnlyAsync()
        {

            var items = await DocumentDBRepository.GetItemsAsync<TechChecklist>(d => d.Tag == "TechChecklist" && d.DbJobId == (string)Session["DbJobId"]);
            //Session["BridgeModule"] = items.FirstOrDefault().BridgeModule;
            return View(items);
        }

        [ActionName("FullList")]
        public async Task<ActionResult> FullListAsync()
        {

            var items = await DocumentDBRepository.GetItemsAsync<TechChecklist>(d => d.Tag == "TechChecklist");
            //Session["BridgeModule"] = items.FirstOrDefault().BridgeModule;
            return View(items);
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TechChecklist r = await DocumentDBRepository.GetItemAsync<TechChecklist>(id);

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
            TechChecklist r = await DocumentDBRepository.GetItemAsync<TechChecklist>(id);
            await DocumentDBRepository.DeleteItemAsync(r.Id);

            return RedirectToAction("Index");
        }









        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            var r = new TechChecklist
            {
                Tag = "TechChecklist",
                DbJobId = (string)Session["DbJobId"],
                BridgeModule = (string)Session["BridgeModule"]

            };

            ViewBag.TechChecklistSelectList = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == r.BridgeModule && d.ListType == "TechChecklist");


            return View(r);

        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Tag,BridgeModule,DbJobId," +
            "UK,TCNo,TCSubject,TCOK,TCNA,TCRuleRef,TCGuidance,TCNote")] TechChecklist item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.CreateItemAsync<TechChecklist>(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Tag,BridgeModule,DbJobId," +
            "UK,TCNo,TCSubject,TCOK,TCNA,TCRuleRef,TCGuidance,TCNote")] TechChecklist item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<TechChecklist>(item.Id, item);
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

            TechChecklist item = await DocumentDBRepository.GetItemAsync<TechChecklist>(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }





    }
}