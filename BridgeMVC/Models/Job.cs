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
        public Boolean IsComplete { get; set; }

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

        [JsonProperty(PropertyName = "customerId")]
        public string CustomerId { get; set; }

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

        [JsonProperty(PropertyName = "serialNo")]
        public string SerialNo { get; set; }

        [JsonProperty(PropertyName = "medItemNo")]
        public string MEDItemNo { get; set; }

        [JsonProperty(PropertyName = "deliveryWeek")]
        public int? DeliveryWeek { get; set; }

        [JsonProperty(PropertyName = "localUnit")]
        public string LocalUnit { get; set; }

        [JsonProperty(PropertyName = "isFinalized")]
        public Boolean IsFinalized { get; set; }

        [JsonProperty(PropertyName = "budgetHour")]
        public int BudgetHour { get; set; }

        [JsonProperty(PropertyName = "dueWeek")]
        public int DueWeek { get; set; }

        [JsonProperty(PropertyName = "sendingFlag")]
        public string SendingFlag { get; set; }

        [JsonProperty(PropertyName = "isHold")]
        public Boolean IsHold { get; set; }

        [DataType(DataType.MultilineText)]
        [JsonProperty(PropertyName = "statusNote")]
        public string StatusNote { get; set; }

        [JsonProperty(PropertyName = "verifyLvl")]
        public string VerifyLvl { get; set; }

        [JsonProperty(PropertyName = "surveyDate")]
        public DateTime? SurveyDate { get; set; }

        [JsonProperty(PropertyName = "surveyStation")]
        public string SurveyStation { get; set; }

        [JsonProperty(PropertyName = "techPara1")]
        public string TechPara1 { get; set; }

        [JsonProperty(PropertyName = "techPara2")]
        public string TechPara2 { get; set; }

        [JsonProperty(PropertyName = "techPara3")]
        public string TechPara3 { get; set; }

        [JsonProperty(PropertyName = "techPara4")]
        public string TechPara4 { get; set; }

        [JsonProperty(PropertyName = "medFactory")]
        public string MEDFactory { get; set; }

        [JsonProperty(PropertyName = "medFBNo")]
        public string MEDFBNo { get; set; }

        [JsonProperty(PropertyName = "medFBDue")]
        public string MEDFBDue { get; set; }

        [JsonProperty(PropertyName = "anyDesignChange")]
        public Boolean AnyDesignChange { get; set; }

        [JsonProperty(PropertyName = "checklistUsed")]
        public Boolean ChecklistUsed { get; set; }

        [JsonProperty(PropertyName = "designFolder")]
        public string DesignFolder { get; set; }


        [JsonProperty(PropertyName = "isDocQualityGood")]
        public Boolean IsDocQualityGood { get; set; }


        [JsonProperty(PropertyName = "isDocSufficient")]
        public Boolean IsDocSufficient { get; set; }

        [JsonProperty(PropertyName = "setHoldTime")]
        public DateTime? SetHoldTime { get; set; }

        [JsonProperty(PropertyName = "ioraSpentTime")]
        public int? IORASpentTime { get; set; }

        [JsonProperty(PropertyName = "modificationDesc")]
        public string ModificationDesc { get; set; }

        [DataType(DataType.MultilineText)]
        [JsonProperty(PropertyName = "onHoldNote")]
        public string OnHoldNote { get; set; }

        [JsonProperty(PropertyName = "feeVerifyTime")]
        public DateTime? FeeVerifyTime { get; set; }

        [JsonProperty(PropertyName = "registerTime")]
        public DateTime? RegisterTime { get; set; }

        [DataType(DataType.MultilineText)]
        [JsonProperty(PropertyName = "docReq")]
        public string DocReq { get; set; }

        [JsonProperty(PropertyName = "noOfCert")]
        public int? NoOfCert { get; set; }

        [JsonProperty(PropertyName = "feeSet")]
        public Boolean FeeSet { get; set; }

        [DataType(DataType.MultilineText)]
        [JsonProperty(PropertyName = "vesselID")]
        public string VesselID { get; set; }

        [DataType(DataType.MultilineText)]
        [JsonProperty(PropertyName = "docReqNote")]
        public string DocReqNote { get; set; }

        [JsonProperty(PropertyName = "npsDbId")]
        public string NpsDbId { get; set; }

        [JsonProperty(PropertyName = "certAmount")]
        public int? CertAmount { get; set; } = 1;

        [JsonProperty(PropertyName = "exeDoneBy")]
        public string ExeDoneBy { get; set; }

        [JsonProperty(PropertyName = "exeDoneTime")]
        public DateTime? ExeDoneTime { get; set; }

        [JsonProperty(PropertyName = "completedBy")]
        public string CompletedBy { get; set; }

        [JsonProperty(PropertyName = "ioraDbId")]
        public string IoraDbId { get; set; }

        [JsonProperty(PropertyName = "ioraProjName")]
        public string IoraProjName { get; set; }

        [DataType(DataType.MultilineText)]
        [JsonProperty(PropertyName = "ioraSoW")]
        public string IoraSoW { get; set; }

        [DataType(DataType.MultilineText)]
        [JsonProperty(PropertyName = "ioraConditions")]
        public string IoraConditions { get; set; }

        [DataType(DataType.MultilineText)]
        [JsonProperty(PropertyName = "ioraFeeCalc")]
        public string IoraFeeCalc { get; set; }

        [JsonProperty(PropertyName = "internalFee")]
        public int? InternalFee { get; set; }

        [JsonProperty(PropertyName = "npsJobName")]
        public string NpsJobName { get; set; }

    }
}
//ModificationDesc,OnHoldNote,FeeVerifyTime,RegisterTime,DocReq,NoOfCert,FeeSet,VesselID,DocReqNote