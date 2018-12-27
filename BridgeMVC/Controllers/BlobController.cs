using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;


namespace BridgeMVC.Controllers
{
    [Authorize]
    public class BlobController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadDoc()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("Storage connection string");
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("newcontainer"); //container name
            var source = new Uri("public file url");
            CloudBlockBlob target = container.GetBlockBlobReference("targe blob name");
            target.StartCopy(source);
            //or target.StartCopyAsync(source).Wait();
            return View();
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(HttpPostedFileBase uploadFile)
        {
            foreach (string file in Request.Files)
            {
                uploadFile = Request.Files[file];
            }
            // Container Name - picture
            BlobManager BlobManagerObj = new BlobManager("picture");
            string FileAbsoluteUri = BlobManagerObj.UploadFile(uploadFile);

            return View();
        }

        public ActionResult Get()
        {
            // Container Name - picture
            BlobManager BlobManagerObj = new BlobManager("picture");
            List<string> fileList = BlobManagerObj.BlobList();

            return View(fileList);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string uri)
        {
            // Container Name - picture
            BlobManager BlobManagerObj = new BlobManager("picture");
            BlobManagerObj.DeleteBlob(uri);

            return RedirectToAction("Get");
        }



    }
}