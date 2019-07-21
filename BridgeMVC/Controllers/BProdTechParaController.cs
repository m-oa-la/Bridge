using BridgeMVC.Extensions;
using BridgeMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BridgeMVC.Controllers
{
    [Authorize]
    public class BProdTechParaController : Controller
    {
        public Boolean IsAdmin()
        {
            //var user = User as ClaimsPrincipal;
            //string userEmail = user.Email().ToLower();
            //if (userEmail.ToLower() == "eric.song@dnvgl.com")
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }

        public async Task<String> SetViewBag()
        {
            string bm = (string)Session["BridgeModule"];
            var lmeditemno = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm && d.ListType == "MEDItemNo");
            lmeditemno = lmeditemno.OrderBy(d => d.ListItem);
            ViewBag.LMEDItemNo = lmeditemno;

            var lvalueresource = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm 
            && d.ListType.ToLower().Contains("blisttype") && (d.ListItem.ToLower().Contains("autocomplete") || d.ListItem.ToLower().Contains("dropdown") || d.ListItem.ToLower().Contains("free text")));

            lvalueresource = lvalueresource.OrderBy(d => d.ListItem);
            ViewBag.LValueResource = lvalueresource;

            return "";
        }

        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            if (IsAdmin())
            {
                var s = await DocumentDBRepository.GetItemsAsync<BProdTechPara>(d => d.Tag == "BProdTechPara");
                s = s.OrderBy(o => o.ViewSequence);
                return View(s);
            }
            else
            {
                return null;
            }
        }

        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            var S = new BProdTechPara();
            S.BridgeModule = (string)Session["BridgeModule"];
            await SetViewBag();
            return View(S);
        }


        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Tag,Id,BridgeModule,ProdName,TechParaName," +
            "Description,ValueSource,DefaultValue,ValueType,ViewSequence")] BProdTechPara item)
        {
            if (IsAdmin())
            {
               
                if (ModelState.IsValid)
                {
                    await DocumentDBRepository.CreateItemAsync<BProdTechPara>(item);
                    return RedirectToAction("Index");
                }
            }
            await SetViewBag();
            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Tag,Id,BridgeModule,ProdName,TechParaName," +
            "Description,ValueSource,DefaultValue,ValueType,ViewSequence")] BProdTechPara item)
        {
            if (IsAdmin())
            {
                if (ModelState.IsValid)
                {
                    await DocumentDBRepository.UpdateItemAsync<BProdTechPara>(item.Id, item);
                    return RedirectToAction("Index");
                }
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

            BProdTechPara item = await DocumentDBRepository.GetItemAsync<BProdTechPara>(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            await SetViewBag();
            return View(item);
        }

        [HttpPost]
        [ActionName("SaveViewSequence")]
        public async Task<ActionResult> SaveViewSequence (string id, string value)
        {
            BProdTechPara item = await DocumentDBRepository.GetItemAsync<BProdTechPara>(id);
            item.ViewSequence = value;
            await DocumentDBRepository.UpdateItemAsync<BProdTechPara>(item.Id, item);
            return null;
        }


    }
}