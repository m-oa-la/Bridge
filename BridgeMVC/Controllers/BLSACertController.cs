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
    public class BLSACertController : Controller
    {
        public async Task<String> SetViewBags()
        {
            string bm = (string)Session["BridgeModule"];
            ViewBag.LMEDItemNo = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm && d.ListType == "MEDItemNo");
            ViewBag.LPTPs = await DocumentDBRepository.GetItemsAsync<BProdTechPara>(d => d.Tag == "BProdTechPara" && d.BridgeModule == bm);
            if (bm == "M3")
            {
                Job J = await DocumentDBRepository.GetItemAsync<Job>("4e5a1ffe-df4a-4146-9d82-acc196b7a6ee");
                ViewBag.Job = J;
                var ps = await DocumentDBRepository.GetItemsAsync<Product>(d => d.Tag == "Product" && d.DbJobId == J.Id);
                ViewBag.Product = ps.FirstOrDefault();
            }
 
            ViewBag.LPTPs = await DocumentDBRepository.GetItemsAsync<BProdTechPara>(d => d.Tag == "BProdTechPara" && d.BridgeModule == bm);
            return "";
        }


        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync(string searchString)
        {
            var s = await DocumentDBRepository.GetItemsAsync<BLSACert>(d => d.Tag == "BLSACert" && d.BridgeModule == (string)Session["BridgeModule"]);

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                if (searchString != "all")
                {
                    s = s.Where(x => x.BookMarkName.ToLower().Contains(searchString.Replace(" ","")));
                }
            }
            s = s.OrderBy(o => o.BookMarkName).ThenBy(o => o.Description);
            await SetViewBags();
            return View(s);
        }

        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
   
            var S = new BLSACert();
            S.BridgeModule = (string)Session["BridgeModule"];
            S.Condition = "true";
            await SetViewBags();
            return View(S);
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Tag,Id,BridgeModule,BookMarkName,Chapter,Formula,Description,Condition," +
            "Condition,Chapter")] BLSACert item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.CreateItemAsync<BLSACert>(item);
                return RedirectToAction("Index");
            }
            await SetViewBags();
            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Tag,Id,BridgeModule,BookMarkName,Chapter,Formula,Description,Condition," +
            "Condition,Chapter")] BLSACert item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<BLSACert>(item.Id, item);
                return RedirectToAction("Index");
            }
            await SetViewBags();
            return View(item);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BLSACert item = await DocumentDBRepository.GetItemAsync<BLSACert>(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            await SetViewBags();
            return View(item);
        }

        [HttpPost]
        [ActionName("SaveViewSequence")]
        public async Task<ActionResult> SaveViewSequence(string id, string value)
        {
            BLSACert item = await DocumentDBRepository.GetItemAsync<BLSACert>(id);
            item.Description = value;
            await DocumentDBRepository.UpdateItemAsync<BLSACert>(item.Id, item);
            return null;
        }
    }
}