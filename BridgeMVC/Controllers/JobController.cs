﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading.Tasks;
using BridgeMVC.Models;
using System.Security.Claims;
using BridgeMVC.Extensions;
using Newtonsoft.Json;

namespace BridgeMVC.Controllers
{
    [Authorize]
    public class JobController : Controller
    {
        [ActionName("_Index")]
        public async Task<ActionResult> _Index()
        {
            string id = "";
            var user = User as ClaimsPrincipal;
            string userName =  user.Email().ToLower();

            //if (userName.Contains("dnvgl.com"))
            //{
                var users = await DocumentDBRepository.GetItemsAsync<BUser>(d => d.Tag == "BUser" && d.Email.ToLower() == userName);

            if (users != null)
            {
                BUser u = users.FirstOrDefault();
                id = u.Id;
                Session["BridgeModule"] = u.BridgeLastUsed;
                Session["UserSignature"] = u.Signature;
                Session["UserDbId"] = u.Id;
                if (u.BridgeLastUsed != null)
                {
                    return RedirectToAction(u.BridgeLastUsed + "_Index", "Job");
                }
                else
                {
                    return RedirectToAction("SignUp", "Onboarding");
                }
            }
            else
            {
                return RedirectToAction("SignUp", "Onboarding");
            }
         }

        [ActionName("Search")]
        public async Task<ActionResult> SearchAsync(string bm = "M1", string complete = "B")
        {
            string userSig = (string)Session["UserSignature"];

            var myModel = await DocumentDBRepository.GetItemsAsync<Job>(d => d.Tag == "Job" && d.BridgeModule.ToUpper() == bm);

            //if (complete == "B")
            //{
            //    myModel = await DocumentDBRepository.GetItemsAsync<Job>(d => d.Tag == "Job" && d.BridgeModule.ToUpper() == bm );

            //}
            //else if(complete == "Y")
            //{
            //    myModel = await DocumentDBRepository.GetItemsAsync<Job>(d => d.Tag == "Job" && d.BridgeModule.ToUpper() == bm && d.IsComplete == true);
  
            //}

           return View(myModel);
        }

        [ActionName("M1_Index")]
        public async Task<ActionResult> M1_IndexAsync(string searchString, string userSig)
        {
            if (string.IsNullOrEmpty(userSig))
            {
                userSig = (string)Session["UserSignature"];
            }
            ViewBag.userSig = userSig;
    
            string bm = (string)Session["BridgeModule"];
            var myModel = await DocumentDBRepository.GetItemsAsync<Job>(d => d.Tag == "Job" && d.BridgeModule.ToUpper() == bm && d.TaskHandler.ToUpper() == userSig && d.IsComplete == false);

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString.Replace(" ", "");
                searchString = searchString.ToLower();

                myModel = myModel.Where(s => s.NpsJobId.ToLower().Contains(searchString) || (s.CustomerName + "X").ToLower().Contains(searchString) || (s.ProdDescription + "X").ToLower().Contains(searchString) || (s.CertType + "X").ToLower().Contains(searchString));
            }
            await SetViewBags();
            return View(myModel.OrderBy(s => s.NpsJobId));
        }


        [ActionName("M2_Index")]
        public async Task<ActionResult> M2_IndexAsync(string searchString, string userSig)
        {
            {
                if (string.IsNullOrEmpty(userSig))
                {
                    userSig = (string)Session["UserSignature"];
                }
                ViewBag.userSig = userSig;

                string bm = (string)Session["BridgeModule"];
                var myModel = await DocumentDBRepository.GetItemsAsync<Job>(d => d.Tag == "Job" && d.BridgeModule.ToUpper() == bm && d.TaskHandler.ToUpper() == userSig && d.IsComplete == false);

                if (!String.IsNullOrEmpty(searchString))
                {
                    searchString.Replace(" ", "");
                    searchString = searchString.ToLower();

                    myModel = myModel.Where(s => s.NpsJobId.ToLower().Contains(searchString) || (s.CustomerName + "X").ToLower().Contains(searchString) || (s.ProdDescription + "X").ToLower().Contains(searchString) || (s.CertType + "X").ToLower().Contains(searchString));
                }
                await SetViewBags();
                return View(myModel.OrderBy(s => s.NpsJobId));
            }
        }
        public async Task<string> SetViewBags()
        {
            var bm = (string)Session["BridgeModule"];
            ViewBag.LCertType = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm && d.ListType == "CertType");
            ViewBag.LCertAction = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm && d.ListType == "CertAction");
            ViewBag.LMainProdType = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm && d.ListType == "MainProdType");
            ViewBag.LSubProdType = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm && d.ListType == "SubProdType");
            ViewBag.LUser = await DocumentDBRepository.GetItemsAsync<BUser>(d => d.Tag == "BUser" && (d.BridgesGranted).Contains(bm));
 
           return ("");
        }



        [HttpPost]
        [ActionName("CreateNewJob")]
        [ValidateAntiForgeryToken]
        public async Task<string> CreateNewJob([Bind(Include = "Tag,BridgeModule,Id,NpsJobId,TaskHandler,Task1,Task2,Task3,Task4,CustomerName,ProdDescription,ApprNote," +
            "IsComplete,SalesOrderNo,SubOrderNo,CertType,CertAction,MainProdType, SubProdType,ReceivedTime,FeeSetTime,IoraSentTime,IoraReturnedTime,JobCompletedTime,CustomerName," +
            "CustomerId,Fee,FeeSetter,FeeVerifier,JobVerifier,RAE,MWL,ExistingCertNo,CertNo,SerialNo,MEDItemNo,DeliveryWeek,LocalUnit,ArchiveFolder," +
            "IsHold,StatusNote,VerifyLvl,SurveyDate,SurveyStation,TechPara1,TechPara2,TechPara3,TechPara4,MEDFactory,MEDFBNo,MEDFBDue,AnyDesignChange," +
            "ChecklistUsed,DesignFolder,IsDocQualityGood,IsDocSufficient,SetHoldTime,IORASpentTime,ModificationDesc,OnHoldNote,FeeVerifyTime,RegisterTime," +
            "DocReq,NoOfCert,FeeSet,VesselID,DocReqNote,NpsDbId,ExeDoneBy,ExeDoneTime,CompletedBy")] Job item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.CreateItemAsync<Job>(item);
            }
            await SetViewBags();
            return ("");
        }



        [HttpPost]
        [ActionName("M1_Task1")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> M1_Task1Async( Job item)
        {
            //await SaveJobChanges(item);
            if (ModelState.IsValid)
            {

                await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);
                if (!string.IsNullOrEmpty(item.SendingFlag) && item.SendingFlag != "-")
                {
                    Session["SendingFlag"] = item.SendingFlag;
                    return Redirect(Url.Content("~/Job/SendJob/" + item.Id));
                }
            }
            await SetViewBags();   
            return View(item);
        }

        [HttpGet]
        [ActionName("M1_Task1_BudgetHourCalc")]
        public async Task<string> M1_TaskM1_Task1_BudgetHourCalc1(string bm_f, string ct_f)
        {
            var f = await DocumentDBRepository.GetItemsAsync<BFinancial>(d => d.Tag == "BFinancial" && d.BridgeModule == bm_f && d.CertType == ct_f);
            ViewBag.FinancialSet = f.FirstOrDefault();
            return JsonConvert.SerializeObject(f.FirstOrDefault());
          
        }

        [HttpPost]
        [ActionName("M1_Task4")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> M1_Task4Async(Job item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);
                if (!string.IsNullOrEmpty(item.SendingFlag) && item.SendingFlag != "-")
                {
                    Session["SendingFlag"] = item.SendingFlag;
                    return Redirect(Url.Content("~/Job/SendJob/" + item.Id));
                }
            }
            await SetViewBags();
            return Redirect(Url.Content("~/Job/_Index/"));
            
        }

        [ActionName("M1_Task4")]
        public async Task<ActionResult> M1_Task4Async(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Job item = await DocumentDBRepository.GetItemAsync<Job>(id);

                item.StatusNote = "";
            
            item.IsComplete = true;
            if (item == null)
            {
                return HttpNotFound();
            }
            //ViewBag.SelectList = await DocumentDBRepository<BRule>.GetItemsAsync(d => d.Tag == "BRule" && d.BridgeModule == item.BridgeModule);
            Session["DbJobId"] = item.Id;
            Session["NpsJobId"] = item.NpsJobId;


            await SetViewBags();
            return View(item);
        }

        [ActionName("M1_Task1")]
        public async Task<ActionResult> M1_Task1Async(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Job item = await DocumentDBRepository.GetItemAsync<Job>(id);
            if (item == null)
            {
                return HttpNotFound();
            }


            Session["DbJobId"] = item.Id;
            Session["NpsJobId"] = item.NpsJobId;

            item.StatusNote = "";
            
            await SetViewBags();
            item.SendingFlag = "-";
            return View(item);
        }

        [ActionName("SendJob")]
        public async Task<ActionResult> SendJobAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Job item = await DocumentDBRepository.GetItemAsync<Job>(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            Session["DbJobId"] = item.Id;
            Session["NpsJobId"] = item.NpsJobId;
            string sf = (string)Session["SendingFlag"];
            
            await SetViewBags();
            ViewBag.bEmail = await DocumentDBRepository.GetItemsAsync<BEmail>(d => d.Tag == "BEmail" && d.BridgeModule == item.BridgeModule && d.TemplateName == ("TASK" + sf));
            ViewBag.bUser = await DocumentDBRepository.GetItemsAsync<BUser>(d => d.Tag == "BUser" && d.Signature == item.TaskHandler);
            ViewBag.iIORA = await DocumentDBRepository.GetItemsAsync<IORA>(d => d.Tag == "IORA" && d.DbJobId == item.Id);

            Session["SendingFlag"] = "";
            return View(item);
        }


        [HttpPost]
        [ActionName("EditNew")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditNewAsync(Job item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);
                if (!string.IsNullOrEmpty(item.SendingFlag) && item.SendingFlag != "-")
                {
                    Session["SendingFlag"] = item.SendingFlag;
                    return Redirect(Url.Content("~/Job/SendJob/" + item.Id));
                }
            }
            await SetViewBags();
            return View(item);
        }

        [ActionName("EditNew")]
        public async Task<ActionResult> EditNewAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Job item = await DocumentDBRepository.GetItemAsync<Job>(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            //ViewBag.SelectList = await DocumentDBRepository<BRule>.GetItemsAsync(d => d.Tag == "BRule" && d.BridgeModule == item.BridgeModule);
            Session["DbJobId"] = item.Id;
            Session["NpsJobId"] = item.NpsJobId;

            await SetViewBags();
            item.SendingFlag = "-";
            return View(item);
        }


        [ActionName("M1_LSACert")]
        public async Task<ActionResult> M1_LSACertAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Job item = await DocumentDBRepository.GetItemAsync<Job>(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            //ViewBag.SelectList = await DocumentDBRepository<BRule>.GetItemsAsync(d => d.Tag == "BRule" && d.BridgeModule == item.BridgeModule);
            Session["NpsJobId"] = item.NpsJobId;
            Session["DbJobId"] = item.Id;
            ViewBag.BLSACert = await DocumentDBRepository.GetItemsAsync<BLSACert>(d => d.Tag == "BLSACert");
            return View(item);
        }


        [ActionName("M1_Task2")]
        public async Task<ActionResult> M1_Task2Async(string id, string npsid, int fee)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ioras = await DocumentDBRepository.GetItemsAsync<IORA>(d => (d.NpsJobID==npsid) && (d.Tag=="IORA"));
             if (ioras.Count()==0 )
            {
                Session["NpsJobId"] = npsid;
                Session["DbJobId"] = id;
                Session["IORAFee"] = fee;
                return Redirect(Url.Content("~/IORA/Create/"));
            }
            else
            {
                return Redirect(Url.Content("~/IORA/Edit/" + ioras.FirstOrDefault().Id));
            }
        }

        [ActionName("M2_Task3")]
        public async Task<ActionResult> M2_Task3Async(string id, string npsid, int fee)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ioras = await DocumentDBRepository.GetItemsAsync<IORA>(d => (d.NpsJobID == npsid) && (d.Tag == "IORA"));
            if (ioras.Count() == 0)
            {
                Session["NpsJobId"] = npsid;
                Session["DbJobId"] = id;
                Session["IORAFee"] = fee;
                return Redirect(Url.Content("~/IORA/Create/"));
            }
            else
            {
                return Redirect(Url.Content("~/IORA/Edit/" + ioras.FirstOrDefault().Id));
            }
        }


        [ActionName("M1_Task3")]
        public async Task<ActionResult> M1_Task3Async(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Job item = await DocumentDBRepository.GetItemAsync<Job>(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            //ViewBag.SelectList = await DocumentDBRepository<BRule>.GetItemsAsync(d => d.Tag == "BRule" && d.BridgeModule == item.BridgeModule);
            Session["DbJobId"] = item.Id;
            Session["NpsJobId"] = item.NpsJobId;
           item.StatusNote = "";
            
            await SetViewBags();
            item.SendingFlag = "-";
            return View(item);
        }

        [HttpPost]
        [ActionName("M1_Task3")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> M1_Task3Post(Job item)
        {
            //await SaveJobChanges(item);
            if (ModelState.IsValid)
            {

                await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);
                if (!string.IsNullOrEmpty(item.SendingFlag) && item.SendingFlag != "-")
                {
                    Session["SendingFlag"] = item.SendingFlag;
                    return Redirect(Url.Content("~/Job/SendJob/" + item.Id));
                }
            }
            await SetViewBags();
            return View(item);
        }

        [ActionName("Whiteboard")]
        public async Task<ActionResult> Whiteboard()
        {
            var bm = (string)Session["BridgeModule"];
            await SetViewBags();
            var myModel = await DocumentDBRepository.GetItemsAsync<Job>(d => d.Tag == "Job" && d.BridgeModule == bm);
            return View(myModel);
        }

        [ActionName("SubViews/WB_SV1")]
        public async Task<ActionResult> WB_SV1()
        {
            Session["BridgeModule"] = "M2";
            var bm = (string)Session["BridgeModule"];
            var myModel = await DocumentDBRepository.GetItemsAsync<Job>(d => d.Tag == "Job" && d.BridgeModule == bm);

            return View(myModel);
        }

        [ActionName("Create")]
        public async Task<ActionResult> Create()
        {
            var j = new Job()
            {
                BridgeModule = (string)Session["BridgeModule"],
            };
            await SetViewBags();
            return View(j);
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Job item)
        {
            await CreateNewJob(item);
            return Redirect(Url.Content("~/Job/_Index/"));
        }


        [HttpPost]
        [ActionName("M2_Task1")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> M2_Task1Async(Job item)
        {
            //await SaveJobChanges(item);
            if (ModelState.IsValid)
            {

                await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);
                if (!string.IsNullOrEmpty(item.SendingFlag) && item.SendingFlag != "-")
                {
                    Session["SendingFlag"] = item.SendingFlag;
                    return Redirect(Url.Content("~/Job/SendJob/" + item.Id));
                }
            }
            await SetViewBags();
            return View(item);
        }

        [HttpPost]
        [ActionName("M2_Task2")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> M2_Task2Async(Job item)
        {
            //await SaveJobChanges(item);
            if (ModelState.IsValid)
            {

                await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);
                if (!string.IsNullOrEmpty(item.SendingFlag) && item.SendingFlag != "-")
                {
                    Session["SendingFlag"] = item.SendingFlag;
                    return Redirect(Url.Content("~/Job/SendJob/" + item.Id));
                }
            }
            await SetViewBags();
            return View(item);
        }

        [ActionName("M2_Task1")]
        public async Task<ActionResult> M2_Task1Async(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Job item = await DocumentDBRepository.GetItemAsync<Job>(id);
            if (item == null)
            {
                return HttpNotFound();
            }


            Session["DbJobId"] = item.Id;
            Session["NpsJobId"] = item.NpsJobId;

            item.StatusNote = "";

            await SetViewBags();
            item.SendingFlag = "-";
            return View(item);
        }


        [ActionName("M2_Task2")]
        public async Task<ActionResult> M2_Task2Async(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Job item = await DocumentDBRepository.GetItemAsync<Job>(id);
            if (item == null)
            {
                return HttpNotFound();
            }


            Session["DbJobId"] = item.Id;
            Session["NpsJobId"] = item.NpsJobId;

            item.StatusNote = "";

            await SetViewBags();
            item.SendingFlag = "-";
            return View(item);
        }



        public string CleanHtmlString (string s)
        {
            s = s.Replace("<br/>", "");
            s = s.Replace("<div>", "");
            s = s.Replace("</div>", "");
            return s;

        }

    }
}

