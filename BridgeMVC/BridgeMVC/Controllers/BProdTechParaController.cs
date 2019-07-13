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


        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            if (IsAdmin())
            {
                var s = await DocumentDBRepository.GetItemsAsync<BProdTechPara>(d => d.Tag == "BProdTechPara");
                return View(s);
            }
            else
            {
                return null;
            }
        }

        [ActionName("Create")]
        public ActionResult Create()
        {
            var S = new BProdTechPara();
            S.BridgeModule = (string)Session["BridgeModule"];
            return View(S);
        }


        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Tag,Id,BridgeModule,ProdName,TechParaName," +
            "Description,DropdownSource,AutoCompleteSource,DefaultValue,ValueType,ViewSequence")] BProdTechPara item)
        {
            if (IsAdmin())
            {
               
                if (ModelState.IsValid)
                {
                    await DocumentDBRepository.CreateItemAsync<BProdTechPara>(item);
                    return RedirectToAction("Index");
                }
                else
                {

                }
            }
            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Tag,Id,BridgeModule,ProdName,TechParaName," +
            "Description,DropdownSource,AutoCompleteSource,DefaultValue,ValueType,ViewSequence")] BProdTechPara item)
        {
            if (IsAdmin())
            {
                if (ModelState.IsValid)
                {
                    await DocumentDBRepository.UpdateItemAsync<BProdTechPara>(item.Id, item);
                    return RedirectToAction("Index");
                }
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

            BProdTechPara item = await DocumentDBRepository.GetItemAsync<BProdTechPara>(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

    }
}