using BridgeMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BridgeMVC.Controllers
{
    [Authorize]
    public class BIORAController : Controller
    {
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync(string searchString)
        {
            string bm = (string)Session["BridgeModule"];
            var bioras = await DocumentDBRepository.GetItemsAsync<BIORA>(d => d.Tag == "BIORA" && d.BridgeModule == bm);

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();

                bioras = bioras.Where(s => (s.BookMarkName+"x").ToLower().Contains(searchString) || (s.Chapter + "x").ToLower().Contains(searchString) || (s.Formula+"x").ToLower().Contains(searchString));
            }


            return View(bioras.OrderBy(s => s.Chapter));

        }

        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            
            var j = await DocumentDBRepository.GetItemAsync<Job>("baa3ba82-0829-4a27-bafc-d93c1a243699");
            ViewBag.Job = j;


            var f = await DocumentDBRepository.GetItemsAsync<BFinancial>(d => d.Tag == "BFinancial" && d.BridgeModule == j.BridgeModule && d.CertType == j.CertType);
            ViewBag.FinancialSet = f.FirstOrDefault();

            var S = new BIORA();
            S.BridgeModule = (string)Session["BridgeModule"];
            return View(S);
        }


        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Tag,Id,BridgeModule,Description,BookMarkName,Formula," +
            "Condition,Chapter")] BIORA item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.CreateItemAsync<BIORA>(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Tag,Id,BridgeModule,Description,BookMarkName,Formula," +
            "Condition,Chapter")] BIORA item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository.UpdateItemAsync<BIORA>(item.Id, item);
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

            BIORA item = await DocumentDBRepository.GetItemAsync<BIORA>(id);
            var j = await DocumentDBRepository.GetItemAsync<Job>("baa3ba82-0829-4a27-bafc-d93c1a243699");
            ViewBag.Job = j;
            //var i = await DocumentDBRepository.GetItemAsync<IORA>("38ea1592-88d9-4c53-a6d9-b571b1c37532");
            //ViewBag.IORA = i;
            var f = await DocumentDBRepository.GetItemsAsync<BFinancial>(d => d.Tag == "BFinancial" && d.BridgeModule == j.BridgeModule && d.CertType == j.CertType);
            ViewBag.FinancialSet = f.FirstOrDefault();

            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        

    }
}