using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BridgeMVC.Models;
//using TodoListWebApp.DAL;
using System.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using static BridgeMVC.Models.AppModels;

namespace BridgeMVC.Controllers
{
    public class OnboardingController : Controller
    {
        //private TodoListWebAppContext db = new TodoListWebAppContext();

        // GET: /Onboarding/SignUp
        public ActionResult SignUp()
        {
            return View();
        }

        // POST: /Onboarding/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "ID,Name,AdminConsented")] Tenant tenant)
        {
            // generate a random value to identify the request
            string stateMarker = Guid.NewGuid().ToString();
            // store it in the temporary entry for the tenant, we'll use it later to assess if the request was originated from us
            // this is necessary if we want to prevent attackers from provisioning themselves to access our app without having gone through our onboarding process (e.g. payments, etc)
            tenant.IssValue = stateMarker;
            tenant.Created = DateTime.Now;
            //db.Tenants.Add(tenant);
            //db.SaveChanges();

            //create an OAuth2 request, using the web app as the client.
            //this will trigger a consent flow that will provision the app in the target tenant
            string authorizationRequest = String.Format(
                "https://login.microsoftonline.com/common/oauth2/authorize?response_type=code&client_id={0}&resource={1}&redirect_uri={2}&state={3}",
                 Uri.EscapeDataString(ConfigurationManager.AppSettings["ida:ClientID"]),
                 Uri.EscapeDataString("https://graph.windows.net"),
                 Uri.EscapeDataString(this.Request.Url.GetLeftPart(UriPartial.Authority).ToString() + "/Onboarding/ProcessCode"),
                 Uri.EscapeDataString(stateMarker)
                 );
            //if the prospect customer wants to provision the app for all users in his/her tenant, the request must be modified accordingly
            if (tenant.AdminConsented)
                authorizationRequest += String.Format("&prompt={0}", Uri.EscapeDataString("admin_consent"));
            // send the user to consent
            return new RedirectResult(authorizationRequest);
        }

        // GET: /TOnboarding/ProcessCode

    }
}