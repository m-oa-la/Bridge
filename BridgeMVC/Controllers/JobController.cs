using System;
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
                    return RedirectToAction("CommonIndex", "Job");
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
        public async Task<ActionResult> SearchAsync(string SearchString, string terminateJobId = "NONE")
        {
            string bm = (string)Session["BridgeModule"];
            if (terminateJobId != "NONE")
            {
                Job j = await DocumentDBRepository.GetItemAsync<Job>(terminateJobId);
                if (j != null)
                {
                    j.IsComplete = true;
                    if (!j.NpsJobId.Contains("Terminated"))
                    {
                        j.NpsJobId = "Terminated" + j.NpsJobId;
                    }
                    j.CompletedBy = (string)Session["UserDbId"];
                    await DocumentDBRepository.UpdateItemAsync<Job>(j.Id, j);

                }


                var myModel = await DocumentDBRepository.GetItemsAsync<Job>(d=> d.Id == terminateJobId);
                   return View(myModel.Take(20));
            }


            if (!string.IsNullOrEmpty(SearchString))
            {
                SearchString = SearchString.ToLower().Replace(" ", "");
                var myModel = await DocumentDBRepository.GetItemsAsync<Job>(d => d.Tag == "Job" && d.BridgeModule == bm);
                myModel = myModel.Where(s => s.NpsJobId.ToLower().Contains(SearchString) || (s.CustomerName + "X").ToLower().Contains(SearchString) || (s.ProdDescription + "X").ToLower().Contains(SearchString) || (s.CertType + "X").ToLower().Contains(SearchString));
                return View(myModel.Take(20));
            }
            else
            {
                var myModel = await DocumentDBRepository.GetItemsAsync<Job>(d => d.Tag == "NotExisting" && d.BridgeModule.ToUpper() == bm);

                return View(myModel);
            }




        }

        [ActionName("CommonIndex")]
        public async Task<ActionResult> CommonIndex(string searchString, string userSig)
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
            myModel = myModel.OrderBy(s => s.NpsJobId);
           
            return View((string)Session["BridgeModule"] + "_Index", myModel);
        }

        [ActionName("CommonTask1")]
        public async Task<ActionResult> CommonTask1(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Job item = await DocumentDBRepository.GetItemAsync<Job>(id);

            item.StatusNote = "";
            item.SendingFlag = "-";
            Session["newHandler"] = "-";
            Session["newTask"] = "-";

            if (item == null)
            {
                return HttpNotFound();
            }
            //ViewBag.SelectList = await DocumentDBRepository<BRule>.GetItemsAsync(d => d.Tag == "BRule" && d.BridgeModule == item.BridgeModule);
            Session["DbJobId"] = item.Id;
            Session["NpsJobId"] = item.NpsJobId;


            await SetViewBags();
            return View((string)Session["BridgeModule"] + "_Task1", item);
        }

        [ActionName("CommonTask3")]
        public async Task<ActionResult> CommonTask3(string id)
        {
            Job item = await DocumentDBRepository.GetItemAsync<Job>(id);
            Session["DbJobId"] = item.Id;
            Session["NpsJobId"] = item.NpsJobId;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }



            if(item.BridgeModule == "M1" && item.MainProdType.ToLower() == "life-saving appliances" && item.CertType != "MED-F")
            {

                var tcs = await DocumentDBRepository.GetItemsAsync<TechChecklist>(d => d.Tag == "TechChecklist" && d.DbJobId == id);
                Session["TCNo"] = tcs.Count();
                return View("M1_Task3_LSA", item);
            }


            item.StatusNote = "";
            Session["newHandler"] = "-";
            Session["newTask"] = "-";

            if (item == null)
            {
                return HttpNotFound();
            }
            //ViewBag.SelectList = await DocumentDBRepository<BRule>.GetItemsAsync(d => d.Tag == "BRule" && d.BridgeModule == item.BridgeModule);



            await SetViewBags();
            return View((string)Session["BridgeModule"] + "_Task3", item);
        }
        [ActionName("CommonTask4")]
        public async Task<ActionResult> CommonTask4(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Job item = await DocumentDBRepository.GetItemAsync<Job>(id);

            item.StatusNote = "";
            Session["newHandler"] = "-";
            Session["newTask"] = "-";

            if (item == null)
            {
                return HttpNotFound();
            }
            //ViewBag.SelectList = await DocumentDBRepository<BRule>.GetItemsAsync(d => d.Tag == "BRule" && d.BridgeModule == item.BridgeModule);
            Session["DbJobId"] = item.Id;
            Session["NpsJobId"] = item.NpsJobId;


            await SetViewBags();
            return View((string)Session["BridgeModule"] + "_Task4", item);
        }


        public async Task<string> SetViewBags()
        {
            var bm = (string)Session["BridgeModule"];
            var lct = await DocumentDBRepository.GetItemsAsync<BFinancial>(d => d.Tag == "BFinancial" && d.BridgeModule == bm);
            lct = lct.OrderBy(d => d.CertType);
            ViewBag.LCertType = lct;

            var lca = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm && d.ListType == "CertAction");
            lca = lca.OrderBy(d => d.ListItem);
            ViewBag.LCertAction = lca;

            var lmp = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm && d.ListType == "MainProdType");
            lmp = lmp.OrderBy(d => d.ListItem);
            ViewBag.LMainProdType = lmp;

            var lsp = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm && d.ListType == "SubProdType");
            lsp = lsp.OrderBy(d => d.ListItem);
            ViewBag.LSubProdType = lsp;

            var lu = await DocumentDBRepository.GetItemsAsync<BUser>(d => d.Tag == "BUser" && (d.BridgesGranted).Contains(bm));
            lu = lu.OrderBy(d => d.Signature);
            ViewBag.LUser = lu;

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
            "DocReq,NoOfCert,FeeSet,VesselID,DocReqNote,NpsDbId,ExeDoneBy,ExeDoneTime,CompletedBy,IoraDbId,InternalFee,NpsJobName")] Job item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.CreateItemAsync<Job>(item);
            }
            await SetViewBags();
            return ("");
        }

        [HttpPost]
        [ActionName("CommonTask1")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CommonTask1Post(Job item, string NewTask, string NewHandler)
        {
            if (ModelState.IsValid)
            {
                string NewTaskNo = NewTask[0].ToString();

                if (!(NewTask + NewHandler).Contains("-"))
                {
                    Session["SendingFlag"] = NewTaskNo;
                    item.TaskHandler = NewHandler;
                    string targetTaskStatus = (string)item.GetType().GetProperty("Task" + NewTaskNo).GetValue(item,null);
                    if(targetTaskStatus != "Y") { 
                        item.GetType().GetProperty("Task" + NewTaskNo).SetValue(item, "TASK");
                    }
                    await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);

                    return Redirect(Url.Content("~/Job/SendJob/" + item.Id));
                }

                if (item.SendingFlag == "M2DraftIORA")
                {
                    item.Task1 = "Y";
                    item.Task2 = "TASK";
                    await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);
                    return Redirect(Url.Content("~/Job/IoraDraft/" + item.Id));
                }else if(item.SendingFlag == "TSAtoWB")
                {
                    item.RAE = "WHITEBOARD";
                    item.TaskHandler = "WHITEBOARD";
                    await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);
                    return Redirect(Url.Content("~/Job/_Index"));
                }




            }

            await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);
            await SetViewBags();
            return View((string)Session["BridgeModule"] + "_Task1", item);
        }

        [HttpPost]
        [ActionName("CommonTask3")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CommonTask3Post(Job item, string NewTask, string NewHandler, string submit="")
        {
            if (ModelState.IsValid)
            {
                
                switch (submit)
                {
                    case "Save":
                        await SetViewBags();
                        await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);
                        return View((string)Session["BridgeModule"] + "_Task3", item);
                    case "Send to WHITEBOARD":
                        item.RAE = "WHITEBOARD";
                        item.TaskHandler = "WHITEBOARD";
                        await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);
                        return Redirect(Url.Content("~/Job/_Index/"));
                    default:
                        break;
                }

                string NewTaskNo = NewTask[0].ToString();
                if (!(NewTask + NewHandler).Contains("-"))
                {
                    Session["SendingFlag"] = NewTaskNo;
                    item.TaskHandler = NewHandler;
                    item.GetType().GetProperty("Task" + NewTaskNo).SetValue(item, "TASK");
                    await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);
                    return Redirect(Url.Content("~/Job/SendJob/" + item.Id));
                }
            }

            await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);
            await SetViewBags();
            return View((string)Session["BridgeModule"] + "_Task3", item);
        }


        [HttpPost]
        [ActionName("CommonTask4")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CommonTask4Post(Job item, string NewTask, string NewHandler)
        {
            if (ModelState.IsValid)
            {
                string NewTaskNo = NewTask[0].ToString();

                if (!(NewTask + NewHandler).Contains("-"))
                {
                    Session["SendingFlag"] = NewTaskNo;
                    item.TaskHandler = NewHandler;
                    item.GetType().GetProperty("Task" + NewTaskNo).SetValue(item, "TASK");
                    await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);

                    return Redirect(Url.Content("~/Job/SendJob/" + item.Id));
                }
            }

            await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);
            await SetViewBags();
            return Redirect(Url.Content("~/Job/_Index/"));
        }



        [HttpGet]
        [ActionName("SetTaskSendingFlag")]
        public  string SetTaskSendingFlag(string newHandler, string newTask)
        {
            Session["newHandler"] = newHandler;
            Session["newTask"] = newTask;
            return "Done";
        }




        [HttpGet]
        [ActionName("M1_Task1_BudgetHourCalc")]
        public async Task<string> M1_TaskM1_Task1_BudgetHourCalc1(string bm_f, string ct_f)
        {
            var f = await DocumentDBRepository.GetItemsAsync<BFinancial>(d => d.Tag == "BFinancial" && d.BridgeModule == bm_f && d.CertType == ct_f);
            ViewBag.FinancialSet = f.FirstOrDefault();
            return JsonConvert.SerializeObject(f.FirstOrDefault());
        }



        [ActionName("IsIORAExisting")]
        public async Task<ActionResult> IsIORAExisting(string id)
        {
            Job j = await DocumentDBRepository.GetItemAsync<Job>(id);

            return null;
        }

        [ActionName("GetProdDesc")]
        public async Task<string> GetProdType(string id)
        {
            Job j = await DocumentDBRepository.GetItemAsync<Job>(id);
            if (!string.IsNullOrEmpty(j.ProdDescription))
            {
                return j.SubProdType + "-" + j.ProdDescription; 
            }
            else
            {
                var products = await DocumentDBRepository.GetItemsAsync<Product>(d => d.Tag == "Product" && d.DbJobId == id);
                if(products.Count() > 0)
                {
                    Product p = products.FirstOrDefault();
                    return p.SubProdType + "-" + p.ProdDescription ;
                }
            }
            return "";
        }


        [ActionName("ChangeJobRAE")]
        public async Task<string> ChangeJobRAE(string id, string newJobHandler)
        {
            Job j = await DocumentDBRepository.GetItemAsync<Job>(id);
            if (j != null)
            {
                j.RAE = newJobHandler;
                await DocumentDBRepository.UpdateItemAsync<Job>(j.Id, j);
            }
            return ("OK");
        }
        [ActionName("ChangeJobVerifier")]
        public async Task<string> ChangeJobVerifier(string id, string newJobHandler)
        {
            Job j = await DocumentDBRepository.GetItemAsync<Job>(id);
            if (j != null)
            {
                j.JobVerifier = newJobHandler;
                await DocumentDBRepository.UpdateItemAsync<Job>(j.Id, j);
            }
            return ("OK");
        }

   

        [ActionName("UpdateOnHoldNote")]
        public async Task<string> UpdateOnHoldNote(string id, string newNote)
        {
            Job j = await DocumentDBRepository.GetItemAsync<Job>(id);
            if (j != null)
            {
                if (newNote.Contains("OH_from"))
                {
                    j.IsHold = true;
                }
                else
                {
                    j.IsHold = false;
                }

                j.OnHoldNote = newNote + j.OnHoldNote;
                await DocumentDBRepository.UpdateItemAsync<Job>(j.Id, j);
            }

            if (string.IsNullOrEmpty(j.IoraDbId))
            {
                return "NoIORA";
            }
            else
            {
                return j.IoraDbId;
            }

        }


        [ActionName("IoraDraft")]
        public async Task<ActionResult> IoraDraftAsync(string id)
        {
            Job j = await DocumentDBRepository.GetItemAsync<Job>(id);

            ViewBag.Job = j;
            ViewBag.BIORA = await DocumentDBRepository.GetItemsAsync<BIORA>(d => d.Tag == "BIORA" && d.BridgeModule == j.BridgeModule);
            ViewBag.Rules = await DocumentDBRepository.GetItemsAsync<Rule>(d => d.Tag == "Rule" && d.DbJobId == j.Id);
            var f = await DocumentDBRepository.GetItemsAsync<BFinancial>(d => d.Tag == "BFinancial" && d.BridgeModule == j.BridgeModule && d.CertType == j.CertType);
            ViewBag.FinancialSet = f.FirstOrDefault();
            ViewBag.LUser = await DocumentDBRepository.GetItemsAsync<BUser>(d => d.Tag == "BUser" && (d.BridgesGranted).Contains(j.BridgeModule));
            ViewBag.Products = await DocumentDBRepository.GetItemsAsync<Product>(d => d.Tag == "Product" && d.DbJobId == j.Id);
            ViewBag.DocReqs = await DocumentDBRepository.GetItemsAsync<DocReq>(d => d.Tag == "DocReq" && d.DbJobId == j.Id);
            j.StatusNote = "";
            return View(j);
        }

        [HttpPost]
        [ActionName("IoraDraft")]
        public async Task<ActionResult> IoraDraftPost(Job item)
        {
    
                await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);
         
            return Redirect(Url.Content("~/Job/CreateIora/" + item.Id));
        }


        [ActionName("CreateIora")]
        public async Task<ActionResult> CreatIora(string id)
        {
            Job j = await DocumentDBRepository.GetItemAsync<Job>(id);
            j.StatusNote = "";
            return View(j);
        }



        [ActionName("SendJobAPI")]
        public async Task<ActionResult> SendJobAPIAsync(string id, string newHandler, string sendingFlag)
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
            string sf = sendingFlag;
            Session["DbJobId"] = id;
            Session["NpsJobId"] = item.NpsJobId;

            await SetViewBags();
            ViewBag.bEmail = await DocumentDBRepository.GetItemsAsync<BEmail>(d => d.Tag == "BEmail" && d.BridgeModule == item.BridgeModule && d.TemplateName == ("TASK" + sf));
            ViewBag.bUser = await DocumentDBRepository.GetItemsAsync<BUser>(d => d.Tag == "BUser" && d.Signature.ToLower() == item.TaskHandler.ToLower());
            ViewBag.iIORA = await DocumentDBRepository.GetItemsAsync<IORA>(d => d.Tag == "IORA" && d.DbJobId == item.Id);

            Session["SendingFlag"] = "";
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
        public async Task<ActionResult> EditNewAsync(Job item, string NewTask, string NewHandler)
        {
            if (ModelState.IsValid)
            {
                string  NewTaskNo = "1";

                if (!NewHandler.Contains("-") && !string.IsNullOrEmpty(NewHandler) && NewHandler.ToLower() != "null")
                {
                    Session["SendingFlag"] = NewTaskNo;
                    item.TaskHandler = NewHandler;
                    item.GetType().GetProperty("Task" + NewTaskNo).SetValue(item, "TASK");
                    await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);
                    return Redirect(Url.Content("~/Job/SendJob/" + item.Id));
                }
                else
                {
                   await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);
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




        [ActionName("Whiteboard")]
        public async Task<ActionResult> Whiteboard(string wbTab = "wb_dist")
        {
            var bm = (string)Session["BridgeModule"];

            if (!string.IsNullOrEmpty(wbTab) && wbTab.Contains(","))
            {
                List<string> ls = wbTab.Split(',').ToList<string>();
                Job j = await DocumentDBRepository.GetItemAsync<Job>(ls[0]);
                j.TaskHandler = ls[1];
                await DocumentDBRepository.UpdateItemAsync<Job>(j.Id, j);
            }

            var myModel = await DocumentDBRepository.GetItemsAsync<Job>(d => d.Tag == "Job" && d.BridgeModule == bm && d.IsComplete == false && (d.RAE + "x") != "x");

            switch (wbTab)
            {
                case "wb_dist":
                    myModel = myModel.Where(d => d.RAE == "WHITEBOARD");
                    break;
                case "wb_v":
                    myModel = myModel.Where(d => !string.IsNullOrEmpty(d.JobVerifier) && d.JobVerifier!= "WHITEBOARD");
                    break;
                case "wb_oh":
                    myModel = myModel.Where(d => d.IsHold == true);
                    break;
                case "wb_og":
                    myModel = myModel.Where(d => d.RAE !="WHITEBOARD" && (string.IsNullOrEmpty(d.JobVerifier)||d.JobVerifier == "WHITEBOARD") && d.IsHold == false);
                    //myModel = myModel.Where(d => d.IsHold == false && d.Task4 == "TASK");
                    break;
                case "wb_done":
                    myModel = myModel.Where(d => d.IsComplete == true && d.JobCompletedTime > DateTime.Today.AddDays(-7));
                    break;
                default:
                    myModel = myModel.Where(d => d.RAE == "WHITEBOARD");
                    break;
            }

            ViewBag.WBT = wbTab;
            
            await SetViewBags();
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

        public string CleanHtmlString (string s)
        {
            s = s.Replace("<br/>", "");
            s = s.Replace("<div>", "");
            s = s.Replace("</div>", "");
            return s;

        }

        [ActionName("M1_Task3_LSA")]
        public async Task<ActionResult> M1_Task3_LSA(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Job item = await DocumentDBRepository.GetItemAsync<Job>(id);

            item.StatusNote = "";
            Session["newHandler"] = "-";
            Session["newTask"] = "-";

            if (item == null)
            {
                return HttpNotFound();
            }
            //ViewBag.SelectList = await DocumentDBRepository<BRule>.GetItemsAsync(d => d.Tag == "BRule" && d.BridgeModule == item.BridgeModule);
            Session["DbJobId"] = item.Id;
            Session["NpsJobId"] = item.NpsJobId;


            await SetViewBags();
            return View("M1_Task3_LSA", item);
        }
        [HttpPost]
        [ActionName("M1_Task3_LSA")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> M1_Task3_LSAPost(Job item, string NewTask, string NewHandler, string tcID, string tcName, Boolean tcValue)
        {
            if (ModelState.IsValid)
            {
                string NewTaskNo = NewTask[0].ToString();

                if (!(NewTask + NewHandler).Contains("-"))
                {
                    Session["SendingFlag"] = NewTaskNo;
                    item.TaskHandler = NewHandler;
                    item.GetType().GetProperty("Task" + NewTaskNo).SetValue(item, "TASK");
                    await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);

                    return Redirect(Url.Content("~/Job/SendJob/" + item.Id));
                }



            }

            await DocumentDBRepository.UpdateItemAsync<Job>(item.Id, item);
            await SetViewBags();
            return View("M1_Task3_LSA", item);
        }

    }
}

