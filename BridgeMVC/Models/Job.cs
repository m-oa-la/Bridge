using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BridgeMVC.Models
{
    public class Job
    {
        [Required]
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; } = "Job";
        [Required]
        [JsonProperty(PropertyName = "bridgeModule")]
        public string BridgeModule { get; set; } 

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "uniqueKey")]
        public string NpsJobId { get; set; }

        [JsonProperty(PropertyName = "archiveFolder")]
        public string ArchiveFolder { get; set; }

        [JsonProperty(PropertyName = "task1")]
        public string Task1 { get; set; }

        [JsonProperty(PropertyName = "task2")]
        public string Task2 { get; set; }

        [JsonProperty(PropertyName = "task3")]
        public string Task3 { get; set; }

        [JsonProperty(PropertyName = "task4")]
        public string Task4 { get; set; }

        [JsonProperty(PropertyName = "task5")]
        public string Task5 { get; set; }

        [JsonProperty(PropertyName = "certType")]
        public string CertType { get; set; }

        [JsonProperty(PropertyName = "certAction")]
        public string CertAction { get; set; }

        [JsonProperty(PropertyName = "mainProdType")]
        public string MainProdType { get; set; }

        [JsonProperty(PropertyName = "subProdType")]
        public string SubProdType { get; set; }

        [JsonProperty(PropertyName = "taskHandler")]
        public string TaskHandler { get; set; }
        
        [JsonProperty(PropertyName = "isComplete")]
        public bool Completed { get; set; }

        [JsonProperty(PropertyName = "salesOrderNo")]
        public string SalesOrderNo { get; set; }

        [JsonProperty(PropertyName = "subOrderNo")]
        public string SubOrderNo { get; set; }

        [JsonProperty(PropertyName = "receivedTime")]
        public DateTime? ReceivedTime { get; set; }

        [JsonProperty(PropertyName = "fee")]
        public int? Fee { get; set; }

        [JsonProperty(PropertyName = "feeSetTime")]
        public DateTime? FeeSetTime { get; set; }

        [JsonProperty(PropertyName = "ioraSentTime")]
        public DateTime? IoraSentTime { get; set; }

        [JsonProperty(PropertyName = "ioraReturnedTime")]
        public DateTime? IoraReturnedTime { get; set; }

        [JsonProperty(PropertyName = "jobCompletedTime")]
        public DateTime? JobCompletedTime { get; set; }

        [JsonProperty(PropertyName = "customerName")]
        public string CustomerName { get; set; }

        [JsonProperty(PropertyName = "customerDbId")]
        public string CustomerDbId { get; set; }

        [JsonProperty(PropertyName = "prodDescription")]
        public string ProdDescription { get; set; }

        [DataType(DataType.MultilineText)]
        [JsonProperty(PropertyName = "apprNote")]
        public string ApprNote { get; set; }

        [JsonProperty(PropertyName = "feeSetter")]
        public string FeeSetter { get; set; }
        
        [JsonProperty(PropertyName = "feeVerifier")]
        public string FeeVerifier { get; set; }

        [JsonProperty(PropertyName = "jobVerifier")]
        public string JobVerifier { get; set; }
        
        [JsonProperty(PropertyName = "rae")]
        public string RAE { get; set; }

        [JsonProperty(PropertyName = "mwl")]
        public string MWL { get; set; }

        [JsonProperty(PropertyName = "existingCertNo")]
        public string ExistingCertNo { get; set; }

        [JsonProperty(PropertyName = "certNo")]
        public string CertNo { get; set; }

        [JsonProperty(PropertyName = "techPara1")]
        public string SerialNo { get; set; }

        [JsonProperty(PropertyName = "techPara2")]
        public string MEDItemNo { get; set; }

        [JsonProperty(PropertyName = "techPara3")]
        public string ExpireDate { get; set; }

        [JsonProperty(PropertyName = "deliveryWeek")]
        public string DeliveryWeek { get; set; }

        [JsonProperty(PropertyName = "localUnit")]
        public string LocalUnit { get; set; }

        [JsonProperty(PropertyName = "budgetHour")]
        public int BudgetHour { get; set; }

        [JsonProperty(PropertyName = "dueWeek")]
        public int DueWeek { get; set; }

        [JsonProperty(PropertyName = "sendingFlag")]
        public string SendingFlag { get; set; }

    }
}