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
        // GET: Product
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            string dbjid = (string)Session["DbJobId"];
            //ProdName and Bridge Module filter to be addded!!!!!!
            Job j = await DocumentDBRepository.GetItemAsync<Job>(dbjid);
            var LProdTechPara = await DocumentDBRepository.GetItemsAsync<BProdTechPara>(d => d.Tag == "BProdTechPara" && d.BridgeModule == j.BridgeModule && d.ProdName == j.MEDItemNo);
            LProdTechPara = LProdTechPara.OrderBy(d => d.ViewSequence);
            ViewBag.LprodTech = LProdTechPara;
            var items = await DocumentDBRepository.GetItemsAsync<Product>(d => d.Tag == "Product" && d.DbJobId == dbjid);

            foreach (Product prod in items)
            {
                if(prod.PTPs.Count() == 0)
                {
                    foreach(BProdTechPara bptp in LProdTechPara)
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
            //The following line may be removed, to be checked
            items = await DocumentDBRepository.GetItemsAsync<Product>(d => d.Tag == "Product" && d.DbJobId == dbjid);
            return View(items);
        }
        [ActionName("IndexReadOnly")]
        public async Task<ActionResult> IndexReadOnlyAsync()
        {

            var items = await DocumentDBRepository.GetItemsAsync<Product>(d => d.Tag == "Product" && d.DbJobId == (string)Session["DbJobId"]);
            //Session["BridgeModule"] = items.FirstOrDefault().BridgeModule;
            return View(items);
        }

        [ActionName("FullList")]
        public async Task<ActionResult> FullListAsync()
        {

            var items = await DocumentDBRepository.GetItemsAsync<Product>(d => d.Tag == "Product");
            //Session["BridgeModule"] = items.FirstOrDefault().BridgeModule;
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

            return RedirectToAction("Index");
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
            await DocumentDBRepository.CreateItemAsync<Product>(p);
            return RedirectToAction("Index"); 

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
                return RedirectToAction("Index");
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
                return RedirectToAction("Index");
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
            return View(item);
        }





    }
}