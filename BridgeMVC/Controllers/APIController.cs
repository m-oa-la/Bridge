using BridgeMVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BridgeMVC.Controllers
{
    [Authorize]
    public class APIController : Controller
    {
        // GET: API
        [HttpGet]
        public async Task<string> ReadBEmail(string templateName, string bridgeModule)
        {
            var be = await DocumentDBRepository.GetItemsAsync<BEmail>(d =>
            d.TemplateName.ToLower() == templateName.ToLower() &&
            d.BridgeModule == bridgeModule && d.Tag == "BEmail");

            if (be == null)
            {
                //throw a meaningful exception or give some useful feedback to the user!
                return "User not found";
            }
            //ViewBag.BEmailInfo = be.FirstOrDefault();

            return JsonConvert.SerializeObject(be.FirstOrDefault());
        }


        [HttpGet]
        public async Task<string> ReadBUser(string sig)
        {
            var users = await DocumentDBRepository.GetItemsAsync<BUser>(d => d.Signature.ToLower() == sig.ToLower());
            if (users == null)
            {
                //throw a meaningful exception or give some useful feedback to the user!
                return "User not found";
            }
            BUser bu = users.FirstOrDefault();

            return JsonConvert.SerializeObject(bu);

        }





    }
}