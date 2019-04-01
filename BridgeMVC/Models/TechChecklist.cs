using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BridgeMVC.Models
{
    public class TechChecklist
    {
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; } = "TechChecklist";
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "bridgeModule")]
        public string BridgeModule { get; set; }

        [JsonProperty(PropertyName = "dbJobId")]
        public string DbJobId { get; set; }

        [JsonProperty(PropertyName = "mainProdType")]
        public string MainProdType { get; set; }

        [JsonProperty(PropertyName = "subProdType")]
        public string SubProdType { get; set; }

        [JsonProperty(PropertyName = "medItemNo")]
        public string MEDItemNo { get; set; }

        [JsonProperty(PropertyName = "uniqueKey")]
        public string Uk => $"{MainProdType}{SubProdType}{DbJobId}";

        [JsonProperty(PropertyName = "customerName")]
        public string CustomerName { get; set; }

        [JsonProperty(PropertyName = "designation")]
        public string Designation { get; set; }

        [JsonProperty(PropertyName = "customerNo")]
        public string CustomerNo { get; set; }

        [JsonProperty(PropertyName = "issuerSig")]
        public string IssuerSig { get; set; }

        [JsonProperty(PropertyName = "issuerSection")]
        public string IssuerSection { get; set; }

        [JsonProperty(PropertyName = "npsJobId")]
        public string NpsJobId { get; set; }

        [JsonProperty(PropertyName = "certAction")]
        public string CertAction { get; set; }

        [JsonProperty(PropertyName = "davitMwl")]
        public string DavitMWL { get; set; }

        [JsonProperty(PropertyName = "winchMwl")]
        public string WinchMWL { get; set; }

        [JsonProperty(PropertyName = "mhl")]
        public string MHL { get; set; }

        [JsonProperty(PropertyName = "wireDia")]
        public string WireDia { get; set; }
        [DataType(DataType.MultilineText)]
        [JsonProperty(PropertyName = "davitComment")]
        public string DavitComment { get; set; }

        [JsonProperty(PropertyName = "verifier")]
        public string Verifier { get; set; }

        [JsonProperty(PropertyName = "verificationLvl")]
        public string VerificationLvl { get; set; }

        [JsonProperty(PropertyName = "surveyDate")]
        public string SurveyDate { get; set; }

        [JsonProperty(PropertyName = "surveyStation")]
        public string SurveyStation { get; set; }

        [JsonProperty(PropertyName = "finalizeDate")]
        public DateTime FinalizeDate { get; set; }

        [JsonProperty(PropertyName = "verifyDate")]
        public DateTime VerifyDate { get; set; }
        [DataType(DataType.MultilineText)]
        [JsonProperty(PropertyName = "apprNote")]
        public string ApprNote { get; set; }
        [DataType(DataType.MultilineText)]
        [JsonProperty(PropertyName = "verifyNote")]
        public string VerifyNote { get; set; }

        [JsonProperty(PropertyName = "mbl")]
        public string MBL { get; set; }

        [JsonProperty(PropertyName = "localUnit")]
        public string LocalUnit { get; set; }

        private List<TechCheckItem> TCItems = new List<TechCheckItem>();


    }

    public class TechCheckItem
    {
        [JsonProperty(PropertyName = "tcNo")]
        public string TCNo { get; set; }

        [JsonProperty(PropertyName = "tcSubject")]
        public string TCSubject { get; set; }

        [JsonProperty(PropertyName = "tcOK")]
        public Boolean TCOK { get; set; }

        [JsonProperty(PropertyName = "tcRuleRef")]
        public string TCRuleRef { get; set; }

        [JsonProperty(PropertyName = "tcNA")]
        public Boolean TCNA { get; set; }

        [JsonProperty(PropertyName = "tcGuidance")]
        public string TCGuidance { get; set; }
    }
}