using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MABridge.OpenXml;
using System.IO;
using System.Collections.Generic;

namespace MABridge.OpenXml.Tester
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var bytes = File.ReadAllBytes("./Templates/IORA Template.docx");

            var fieldValues = new Dictionary<string, string> {
                { "DgDNVDocNo01", "2424-4424-2423-34" },
                { "DnvUnitName501", "First unit" },
                { "DnvUnitName502", "Second unit" },
                { "DnvUnitNo501", "unit no" },
                { "DgSpecialConditions", @"Certificate will be issued after examination is finished and prototype test report is received, reviewed and found to comply with applicable requirements. 
                                           90% of the total fee will be invoiced after design examination is finalized. The remaining 10% of the total fee will be invoiced after completion of job.
                                            If prototype test reports are not received past 6 month from finalized Type Examination for above mentioned product, the case will be closed. 
                                            If the verification/certification is rejected by DNV GL or cancelled by customer the spent hours will be invoiced at hourly rate of NOK 2354.-.
In addition to the original version of the documentation, evaluation of one possible revision is covered by the above fee provided the revision is necessary in order to incorporate DNV GL`s comments. "}
            };
            var newFile = "UnitTestStream.docx";
            if (File.Exists(newFile))
            {
                File.Delete(newFile);
            }
            using (var mem = new MemoryStream(bytes))
            {
                OpenXmlWordHelper.FillForm(fieldValues, mem);
                using (FileStream fileStream = new FileStream(newFile, FileMode.CreateNew))
                {
                    mem.WriteTo(fileStream);
                }
            }
            
        }

        
    }
}
