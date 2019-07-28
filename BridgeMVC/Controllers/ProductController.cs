using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading.Tasks;
using BridgeMVC.Models;
using System.Diagnostics;
using Newtonsoft.Json;

namespace BridgeMVC.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        public async Task<string> SetViewBags()
        {
            var bm = (string)Session["BridgeModule"];
            var medItemNo = (string)Session["MEDItemNo"];
            ViewBag.LProdTech = await DocumentDBRepository.GetItemsAsync<BProdTechPara>(d => d.Tag == "BProdTechPara" && d.BridgeModule == bm && d.ProdName == medItemNo);

            return ("");
        }

        public async Task<string> SetMainSubProdViewBags()
        {
            var bm = (string)Session["BridgeModule"];
            var lmp = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm && d.ListType == "MainProdType");
            lmp = lmp.OrderBy(d => d.ListItem);
            ViewBag.LMainProdType = lmp;

            var lsp = await DocumentDBRepository.GetItemsAsync<BList>(d => d.Tag == "BList" && d.BridgeModule == bm && d.ListType == "SubProdType");
            lsp = lsp.OrderBy(d => d.ListItem);
            ViewBag.LSubProdType = lsp;
            return "";
        }

        // GET: Product
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            string dbjid = (string)Session["DbJobId"];
            await AddPTPifEmpty(dbjid);
            var items = await DocumentDBRepository.GetItemsAsync<Product>(d => d.Tag == "Product" && d.DbJobId == dbjid);
            return View(items);
        }

        [ActionName("IndexNonePED")]
        public async Task<ActionResult> IndexNonePED()
        {

            var items = await DocumentDBRepository.GetItemsAsync<Product>(d => d.Tag == "Product" && d.DbJobId == (string)Session["DbJobId"]);
            //Session["BridgeModule"] = items.FirstOrDefault().BridgeModule;
            return View(items);
        }
        [ActionName("IndexReadOnly")]
        public async Task<ActionResult> IndexReadOnlyAsync()
        {
            var items = await DocumentDBRepository.GetItemsAsync<Product>(d => d.Tag == "Product" && d.DbJobId == (string)Session["DbJobId"]);
            return View(items);
        }
        [ActionName("RefreshPTP")]
        public async Task<ActionResult> RefreshPTP(string bridgeJobId)
        {
            string dbjid = (string)Session["DbJobId"];
            string redirectUrl = "~/Job/CommonTask1/" + bridgeJobId;
            await RefreshPTPAsync(dbjid);
            return Redirect(Url.Content(redirectUrl));
        }
        [ActionName("FullList")]
        public async Task<ActionResult> FullListAsync()
        {
            var items = await DocumentDBRepository.GetItemsAsync<Product>(d => d.Tag == "Product");
            return View(items);
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product r = await DocumentDBRepository.GetItemAsync<Product>(id);

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
            Product r = await DocumentDBRepository.GetItemAsync<Product>(id);
            await DocumentDBRepository.DeleteItemAsync(r.Id);

            return RedirectToAction("IndexNonePED");
        }
        [HttpPost]
        [ActionName("SavePTP")]
        public async Task<ActionResult> SavePTP(string ProdId, int PTPId, string PTPVal)
        {
            Product p = await DocumentDBRepository.GetItemAsync<Product>(ProdId);
            p.PTPs[PTPId].TechParaValue = PTPVal;
            await DocumentDBRepository.UpdateItemAsync<Product>(p.Id, p);
  
            return null;
        }

        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {

            var p = new Product
            {
                Tag = "Product",
                DbJobId = (string)Session["DbJobId"],
                BridgeModule = (string)Session["BridgeModule"]

            };
            //await DocumentDBRepository.CreateItemAsync<Product>(p);
            await SetMainSubProdViewBags();
            return View(p); 

        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Tag,BridgeModule,MainProdType,SubProdType,ProdDescription,DbJobId," +
            "Uk,NCProductDbId,DbJobId,ProdName,NCProdType,PTPs")] Product item)
        {
            if (ModelState.IsValid)
            {
                //await SetViewBags();
                await DocumentDBRepository.CreateItemAsync<Product>(item);
                return RedirectToAction("IndexNonePED");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Tag,BridgeModule,MainProdType,SubProdType,ProdDescription,DbJobId," +
            "Uk,NCProductDbId,DbJobId,ProdName,NCProdType,PTPs")] Product item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<Product>(item.Id, item);
                return RedirectToAction("IndexNonePED");
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

            Product item = await DocumentDBRepository.GetItemAsync<Product>(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            await SetViewBags();
            await SetMainSubProdViewBags();
            return View(item);
        }

        public async Task AddPTPifEmpty (string dbjid)
        {
            Job j = await DocumentDBRepository.GetItemAsync<Job>(dbjid);
            var LProdTechPara = await DocumentDBRepository.GetItemsAsync<BProdTechPara>(d => d.Tag == "BProdTechPara" && d.BridgeModule == j.BridgeModule && d.ProdName == j.MEDItemNo);
                LProdTechPara = LProdTechPara.OrderBy(d => d.ViewSequence);
            var items = await DocumentDBRepository.GetItemsAsync<Product>(d => d.Tag == "Product" && d.DbJobId == dbjid);

            foreach (Product prod in items)
            {
                if (prod.PTPs.Count() == 0)
                {
                    foreach (BProdTechPara bptp in LProdTechPara)
                    {
                        ProdTechPara newptp = new ProdTechPara()
                        {
                            TechParaName = bptp.TechParaName,
                            TechParaValue = bptp.DefaultValue,
                        };
                        prod.PTPs.Add(newptp);
                    }
                }
            }
        }
        public async Task RefreshPTPAsync(string dbjid)
        {
            Job j = await DocumentDBRepository.GetItemAsync<Job>(dbjid);
            var LProdTechPara = await DocumentDBRepository.GetItemsAsync<BProdTechPara>(d => d.Tag == "BProdTechPara" && d.BridgeModule == j.BridgeModule && d.ProdName == j.MEDItemNo);
                LProdTechPara = LProdTechPara.OrderBy(d => d.ViewSequence);
            var items = await DocumentDBRepository.GetItemsAsync<Product>(d => d.Tag == "Product" && d.DbJobId == dbjid);

            foreach (Product prod in items)
            {
                if (prod.PTPs.Count() != 0)
                {
                    prod.PTPs.Clear();
                }

                foreach (BProdTechPara bptp in LProdTechPara)
                {
                    ProdTechPara newptp = new ProdTechPara()
                    {
                        TechParaName = bptp.TechParaName,
                        TechParaValue = bptp.DefaultValue,
                    };
                    prod.PTPs.Add(newptp);
                }
                await DocumentDBRepository.UpdateItemAsync<Product>(prod.Id, prod);
            }
        }


    }
}