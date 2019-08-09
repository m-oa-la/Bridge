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
    public class ReportController : Controller
    {
        [ActionName("Job_Index")]
        // GET: Report
        public async Task<ActionResult> Job_IndexAsync(string completed, string userSig)
        {
            if (string.IsNullOrEmpty(userSig))
            {
                userSig = (string)Session["UserSignature"];
            }
            ViewBag.userSig = userSig;

            string bm = (string)Session["BridgeModule"];
            IEnumerable<Job> myModel = null; 
            if (String.IsNullOrEmpty(completed) || completed == "Active")
            {
                ViewBag.jobType = "Active";
                myModel = await DocumentDBRepository.GetItemsAsync<Job>(d => d.Tag == "Job" && d.BridgeModule.ToUpper() == bm
             && d.TaskHandler.ToUpper() == userSig && d.IsComplete == false);
            }
            else if (completed == "Historic")
            {
                ViewBag.jobType = completed;
                myModel = await DocumentDBRepository.GetItemsAsync<Job>(d => d.Tag == "Job" && d.BridgeModule.ToUpper() == bm
             && d.TaskHandler.ToUpper() == userSig && d.IsComplete == true);
            }
            else if (completed == "All")
            {
                ViewBag.jobType = completed;
                myModel = await DocumentDBRepository.GetItemsAsync<Job>(d => d.Tag == "Job" && d.BridgeModule.ToUpper() == bm
             && d.TaskHandler.ToUpper() == userSig);
            }

            myModel = myModel.OrderBy(s => s.NpsJobId);
            return View(myModel);
        }
    }
}