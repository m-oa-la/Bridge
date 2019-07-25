using BridgeMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BridgeMVC.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }


        public async System.Threading.Tasks.Task<ActionResult> SaveCustomerAsync(string id, string contactPerson, string phoneNo, string email, string invoiceInfo)
        {
            Customer c = await DocumentDBRepository.GetItemAsync<Customer>(id);
            c.ContactPerson = contactPerson;
            c.PhoneNo = phoneNo;
            c.Email = email;
            c.InvoiceInfo = invoiceInfo;
            c.Email = email;
            await DocumentDBRepository.UpdateItemAsync<Customer>(c.Id, c);
            return View();
        }
    }
}