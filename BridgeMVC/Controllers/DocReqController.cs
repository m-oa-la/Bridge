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
    public class DocReqController : Controller
    {
        // GET: DocReq
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {

            var items = await DocumentDBRepository.GetItemsAsync<DocReq>(d => d.Tag == "DocReq" && d.DbJobId == (string)Session["DbJobId"]);
            //Session["BridgeModule"] = items.FirstOrDefault().BridgeModule;
            return View(items);
        }
        [ActionName("IndexReadOnly")]
        public async Task<ActionResult> IndexReadOnlyAsync()
        {

            var items = await DocumentDBRepository.GetItemsAsync<DocReq>(d => d.Tag == "DocReq" && d.DbJobId == (string)Session["DbJobId"]);
            //Session["BridgeModule"] = items.FirstOrDefault().BridgeModule;
            return View(items);
        }

        [ActionName("FullList")]
        public async Task<ActionResult> FullListAsync()
        {

            var items = await DocumentDBRepository.GetItemsAsync<DocReq>(d => d.Tag == "DocReq");
            //Session["BridgeModule"] = items.FirstOrDefault().BridgeModule;
            return View(items);
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DocReq r = await DocumentDBRepository.GetItemAsync<DocReq>(id);

            if (r == null)
            {
                return HttpNotFound();
            }

            return View(r);
        }

        [HttpPost, ActionName("Delete")]
       
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            DocReq r = await DocumentDBRepository.GetItemAsync<DocReq>(id);
            await DocumentDBRepository.DeleteItemAsync(r.Id);

            return RedirectToAction("Index");
        }









        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            var r = new DocReq
            {
                Tag = "DocReq",
                NpsJobId = (string)Session["NpsJobId"],
                DbJobId = (string)Session["DbJobId"],
                BridgeModule = (string)Session["BridgeModule"]

            };

            ViewBag.DocReqSelectList = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == r.BridgeModule && d.ListType == "DocReq");


            return View(r);

        }

        [HttpPost]
        [ActionName("Create")]
       
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Tag,BridgeModule,DocReqItem,NpsJobId,DbJobId")] DocReq item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.CreateItemAsync<DocReq>(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
       
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Tag,BridgeModule,DocReqItem,NpsJobId,DbJobId")] DocReq item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<DocReq>(item.Id, item);
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

            DocReq item = await DocumentDBRepository.GetItemAsync<DocReq>(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }





    }
}