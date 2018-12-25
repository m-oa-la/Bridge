using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading.Tasks;
using BridgeMVC.Models;
using System.Diagnostics;
using System.Net.Http;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Dynamic;
using System.Reflection;
using System;

namespace BridgeMVC.Controllers
{
    public class TechChecklistController : Controller
    {
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var items = await DocumentDBRepository<TechCheckLSA>.GetItemsAsync(d => d.Tag == "TechCheckLSA");
            return View(items);
        }

        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            var j = await DocumentDBRepository<Job>.GetItemAsync((string)Session["DbJobId"]);
            ViewBag.Job = j;

            var i = new TechCheckLSA
            {
                Tag = "TechCheckLSA",
                BridgeModule = j.BridgeModule,
                NpsJobId = j.NpsJobId,
                DbJobId = j.Id,
                MEDItemNo = j.MEDItemNo,
                MainProdType = j.MainProdType,
                SubProdType = j.SubProdType,
                CustomerName = j.CustomerName,
                Designation = j.ProdDescription,
                Verifier = j.JobVerifier,
                CertAction = j.CertAction,
                ApprNote = j.ApprNote,

            };
            return View(i);
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Tag,Id,BridgeModule,DbJobId,MainProdType,SubProdType,Uk," +
            "CustomerName,Designation,CustomerNo,IssuerSig,IssuerSection,NpsJobId,CertAction,DavitMWL,WinchMWL,MHL,WireDia,DavitComment," +
            "Verifier,VerificationLvl,SurveyDate,SurveyStation,FinalizeDate,VerifyDate,ApprNote,VerifyNote,MEDItemNo,MBL," +
            "OK1,OK2,OK3,OK4,OK5,OK6,OK7,OK8,OK9,OK10,OK11,OK12,OK13,OK14,OK15,OK16,OK17,OK18,OK19,OK20," +
            "NA1,NA2,NA3,NA4,NA5,NA6,NA7,NA8,NA9,NA10,NA11,NA12,NA13,NA14,NA15,NA16,NA17,NA18,NA19,NA20" )] TechCheckLSA item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<TechCheckLSA>.CreateItemAsync(item);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Tag,Id,BridgeModule,DbJobId,MainProdType,SubProdType,Uk," +
            "CustomerName,Designation,CustomerNo,IssuerSig,IssuerSection,NpsJobId,CertAction,DavitMWL,WinchMWL,MHL,WireDia,DavitComment," +
            "Verifier,VerificationLvl,SurveyDate,SurveyStation,FinalizeDate,VerifyDate,ApprNote,VerifyNote,MEDItemNo,MBL," +
            "OK1,OK2,OK3,OK4,OK5,OK6,OK7,OK8,OK9,OK10,OK11,OK12,OK13,OK14,OK15,OK16,OK17,OK18,OK19,OK20," +
            "NA1,NA2,NA3,NA4,NA5,NA6,NA7,NA8,NA9,NA10,NA11,NA12,NA13,NA14,NA15,NA16,NA17,NA18,NA19,NA20" )] TechCheckLSA item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<TechCheckLSA>.UpdateItemAsync(item.Id, item);
                TechCheckLSA ii = item;
                Job j = await DocumentDBRepository<Job>.GetItemAsync(ii.DbJobId);
                ViewBag.Job = j;
                ViewBag.BTechCheckLSA = await DocumentDBRepository<BTechChecklist>.GetItemsAsync(d => d.Tag == "BTechChecklist" && d.BridgeModule == ii.BridgeModule && d.TemplateName == "TechCheckLSA");

                ViewBag.LUser = await DocumentDBRepository<BUser>.GetItemsAsync(d => d.Tag == "BUser" && (d.BridgesGranted).Contains(ii.BridgeModule));

                return View(item);
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
            TechCheckLSA ii = await DocumentDBRepository<TechCheckLSA>.GetItemAsync(id);
            Job j = await DocumentDBRepository<Job>.GetItemAsync(ii.DbJobId);
            ViewBag.Job = j;
            ViewBag.BTechCheckLSA = await DocumentDBRepository<BTechChecklist>.GetItemsAsync(d => d.Tag == "BTechChecklist" && d.BridgeModule == ii.BridgeModule && d.TemplateName == "TechCheckLSA");
            ViewBag.LUser = await DocumentDBRepository<BUser>.GetItemsAsync(d => d.Tag == "BUser" && (d.BridgesGranted).Contains(ii.BridgeModule));


            if (ii == null)
            {
                return HttpNotFound();
            }

            return View(ii);
        }

        //public async Task<ActionResult> ExportToWordAsync(string TechCheckLSAId)
        //{
        //    ;
        //    //var bingTechCheckLSAs = await DocumentDBRepository<BingTechCheckLSA>.GetItemsAsync(d => (d.Tag == "BingTechCheckLSA") && (d.BridgeModule == blu));
        //    TechCheckLSA ii = await DocumentDBRepository<TechCheckLSA>.GetItemAsync(TechCheckLSAId);
        //    var blu = ii.BridgeModule;
        //    var npsjobid = ii.NpsJobId;
        //    string savePath = Server.MapPath("~/App_Data/" + blu + "/Projects/" + ii.NpsJobId + " TechCheckLSA.docx");
        //    string templatePath = Server.MapPath("~/App_Data/" + blu + "/Templates/TechCheckLSA Template.docx");

        //    Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
        //    Microsoft.Office.Interop.Word.Document doc = new Microsoft.Office.Interop.Word.Document();
        //    doc = app.Documents.Open(templatePath);
        //    doc.Activate();


        //    Type t = ii.GetType();
        //    PropertyInfo[] props = t.GetProperties();
        //    foreach (var prop in props)
        //    {
        //        string pname = prop.Name;
        //        if (doc.Bookmarks.Exists(pname))
        //        {
        //            string val = prop.GetValue(ii, null) as string;
        //            doc.Bookmarks[pname].Select();
        //            app.Selection.TypeText(val);

        //            string s = pname + "_1";
        //            if (doc.Bookmarks.Exists(s))
        //            {
        //                doc.Bookmarks[s].Select();
        //                app.Selection.TypeText(val);
        //            }
        //        }
        //    }

        //    doc.SaveAs2(savePath);
        //    app.Application.Quit();
        //    Session["dbJobId"] = ii.DbJobId;
        //    return RedirectToAction("ProjFolder", "Job");
        //    //Response.Write("Success");
        //}


        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TechCheckLSA r = await DocumentDBRepository<TechCheckLSA>.GetItemAsync(id);

            if (r == null)
            {
                return HttpNotFound();
            }

            return View(r);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            TechCheckLSA r = await DocumentDBRepository<TechCheckLSA>.GetItemAsync(id);
            await DocumentDBRepository<TechCheckLSA>.DeleteItemAsync(r.Id);

            return RedirectToAction("Index");
        }

    }
}