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
    public class BListController : Controller
    {
        public async Task<String> SetViewBag()
        {
            string bm = (string)Session["BridgeModule"];
            var lblistType = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm && d.ListType == "BListType");
            lblistType = lblistType.OrderBy(d => d.ListItem);
            ViewBag.LBlistType = lblistType;
            return "";
        }

        // GET: BList
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync(string searchString)
        {
            string bm = (string)Session["BridgeModule"];
            var BLs = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm);
            //BLs.OrderBy(s => s.BridgeModule);
            BLs = BLs.OrderBy(s => s.ListItem);

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();

                BLs = BLs.Where(s => s.ListType.ToLower().Contains(searchString) || s.ListItem.ToLower().Contains(searchString));
            }

            return View(BLs.OrderBy(s => s.ListType));
        }
        [ActionName("GenericIndex")]
        public async Task<ActionResult> GenericIndex(string searchString)
        {
            string bm = (string)Session["BridgeModule"];
            var BLs = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm);
            BLs.OrderBy(s => s.BridgeModule);

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();

                BLs = BLs.Where(s => s.ListType.ToLower().Contains(searchString) || s.ListItem.ToLower().Contains(searchString));
            }

            return View(BLs.OrderBy(s => s.ListType));
        }

        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            var bm = (string)Session["BridgeModule"];
            var S = new BList()
            {
                BridgeModule = bm,
            };

            await SetViewBag();
            return View(S);
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Tag,BridgeModule,ListType,ListItem,UpperLvl,Note")] BList item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.CreateItemAsync<BList>(item);
                return RedirectToAction("Index");
            }
            await SetViewBag();
            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Tag,BridgeModule,ListType,ListItem,UpperLvl,Note")] BList item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<BList>(item.Id, item);
                return RedirectToAction("Index");
            }
            await SetViewBag();
            return View(item);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BList item = await DocumentDBRepository.GetItemAsync<BList>(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            await SetViewBag();
            return View(item);
        }

        [HttpPost]
        [ActionName("Saveblistvalue")]
        public async Task<string> Saveblistvalue(string id, string newval)
        {
            if (id == null)
            {
                return "bad request, id cannot be null";
            }

            BList item = await DocumentDBRepository.GetItemAsync<BList>(id);
            if (item == null)
            {
                return "bad request, invalid id";
            }
            item.ListItem = newval;
            var v = await DocumentDBRepository.UpdateItemAsync<BList>(item.Id, item);
            return "";
        }

        [HttpPost]
        [ActionName("CreateBlistItem")]
        public async Task<string> CreateBlistItem(string listType)
        {

            BList b = new BList()
            {
                BridgeModule = (string)Session["BridgeModule"],
                ListType = listType,
                ListItem = "xx",
            };
            var v = await DocumentDBRepository.CreateItemAsync<BList>(b);

            return GetIdFromDocument(v.ToString());
        }

        public string GetIdFromDocument(string s)
        {
            string str = s.Split(new string[] { "id\": \"" }, StringSplitOptions.None)[1];
            return str.Split(("\"")[0])[0];
        }

        [HttpPost]
        [ActionName("ChangeBlistTypeName")]
        public async Task<string> ChangeBlistTypeName(string oldval, string newval)
        {
            string bm = (string)Session["BridgeModule"];

            var BLs = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm && d.ListType == oldval);
            
            
            if (BLs == null)
            {
                return "bad request, invalid old value";
            }
            foreach (BList b in BLs)
            {
                b.ListType = newval;
                var v = await DocumentDBRepository.UpdateItemAsync<BList>(b.Id, b);
            }

            if (bm == "M3")
            {
                var BPTPara = await DocumentDBRepository.GetItemsAsync<BProdTechPara>(d => d.Tag == "BProdTechPara" && d.BridgeModule == bm && d.ValueSource == oldval);
                if (BPTPara != null)
                {
                    foreach (BProdTechPara b in BPTPara)
                    {
                        b.ValueSource = newval;
                        var v = await DocumentDBRepository.UpdateItemAsync<BProdTechPara>(b.Id, b);
                    }
                }
            }

            return "";
        }


    }
}