using BridgeMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using BridgeMVC.Extensions;

namespace BridgeMVC.Controllers
{
    public class HomeController : Controller

    {
        [Authorize]
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var user = User as ClaimsPrincipal;
            string id = "";
            string userName = user.Email().ToLower();
            if (userName.Contains("dnvgl.com"))
            {
                var users = await DocumentDBRepository.GetItemsAsync<BUser>(d => d.Tag == "BUser" && d.Email.ToLower() == userName);

                if (users != null)
                {
                    BUser u = users.FirstOrDefault();
                    id = u.Id;
                    Session["BridgeModule"] = u.BridgeLastUsed;
                    Session["UserSignature"] = u.Signature;
                }

            }
            else
            {
                
                return RedirectToAction("SignUp", "Onboarding");
            }

            if (id == null)
            {
                return View();
            }

            BUser item = await DocumentDBRepository.GetItemAsync<BUser>(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }


        [Authorize]
        [HttpPost]
        [ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Tag,Id,EmployeeID,Email,Signature,FirstName,Lastname,BridgesGranted,BridgeLastUsed,Signature")] BUser item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<BUser>(item.Id, item);
                
            }
             Session["BridgeModule"] = item.BridgeLastUsed;
            return RedirectToAction("_Index", "Job");
        }

        public ActionResult About()
        {
            return View();
        }

        // GET: BList
        [ActionName("CSSDemo")]
        public ActionResult CSSDemo()
        {

            return View();
        }

        [ActionName("Settings")]
        public ActionResult Settings()
        {

            return View();
        }


    }
}