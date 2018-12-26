using BridgeMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Net;

namespace BridgeMVC.Controllers
{
    public class HomeController : Controller

    {
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            string id = "";
            string userName = User.Identity.Name.ToLower();
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
            return View(item);
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



    }
}