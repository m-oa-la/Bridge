using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BridgeMVC.Models
{
    public class TechCheckLSA
    {
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; } = "TechCheckLSA";
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

        //[JsonProperty(PropertyName = "isFinalized")]
        //public Boolean IsFinalized { get; set; }

        //[JsonProperty(PropertyName = "isVerified")]
        //public Boolean IsVerified { get; set; }

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

        [JsonProperty(PropertyName = "ok1")]
        public Boolean OK1 { get; set; }
        [JsonProperty(PropertyName = "ok2")]
        public Boolean OK2 { get; set; }
        [JsonProperty(PropertyName = "ok3")]
        public Boolean OK3 { get; set; }
        [JsonProperty(PropertyName = "ok4")]
        public Boolean OK4 { get; set; }
        [JsonProperty(PropertyName = "ok5")]
        public Boolean OK5 { get; set; }
        [JsonProperty(PropertyName = "ok6")]
        public Boolean OK6 { get; set; }
        [JsonProperty(PropertyName = "ok7")]
        public Boolean OK7 { get; set; }
        [JsonProperty(PropertyName = "ok8")]
        public Boolean OK8 { get; set; }
        [JsonProperty(PropertyName = "ok9")]
        public Boolean OK9 { get; set; }
        [JsonProperty(PropertyName = "ok10")]
        public Boolean OK10 { get; set; }
        [JsonProperty(PropertyName = "ok11")]
        public Boolean OK11 { get; set; }
        [JsonProperty(PropertyName = "ok12")]
        public Boolean OK12 { get; set; }
        [JsonProperty(PropertyName = "ok13")]
        public Boolean OK13 { get; set; }
        [JsonProperty(PropertyName = "ok14")]
        public Boolean OK14 { get; set; }
        [JsonProperty(PropertyName = "ok15")]
        public Boolean OK15 { get; set; }
        [JsonProperty(PropertyName = "ok16")]
        public Boolean OK16 { get; set; }
        [JsonProperty(PropertyName = "ok17")]
        public Boolean OK17 { get; set; }
        [JsonProperty(PropertyName = "ok18")]
        public Boolean OK18 { get; set; }
        [JsonProperty(PropertyName = "ok19")]
        public Boolean OK19 { get; set; }
        [JsonProperty(PropertyName = "ok20")]
        public Boolean OK20 { get; set; }
        [JsonProperty(PropertyName = "na1")]
        public Boolean NA1 { get; set; }
        [JsonProperty(PropertyName = "na2")]
        public Boolean NA2 { get; set; }
        [JsonProperty(PropertyName = "na3")]
        public Boolean NA3 { get; set; }
        [JsonProperty(PropertyName = "na4")]
        public Boolean NA4 { get; set; }
        [JsonProperty(PropertyName = "na5")]
        public Boolean NA5 { get; set; }
        [JsonProperty(PropertyName = "na6")]
        public Boolean NA6 { get; set; }
        [JsonProperty(PropertyName = "na7")]
        public Boolean NA7 { get; set; }
        [JsonProperty(PropertyName = "na8")]
        public Boolean NA8 { get; set; }
        [JsonProperty(PropertyName = "na9")]
        public Boolean NA9 { get; set; }
        [JsonProperty(PropertyName = "na10")]
        public Boolean NA10 { get; set; }
        [JsonProperty(PropertyName = "na11")]
        public Boolean NA11 { get; set; }
        [JsonProperty(PropertyName = "na12")]
        public Boolean NA12 { get; set; }
        [JsonProperty(PropertyName = "na13")]
        public Boolean NA13 { get; set; }
        [JsonProperty(PropertyName = "na14")]
        public Boolean NA14 { get; set; }
        [JsonProperty(PropertyName = "na15")]
        public Boolean NA15 { get; set; }
        [JsonProperty(PropertyName = "na16")]
        public Boolean NA16 { get; set; }
        [JsonProperty(PropertyName = "na17")]
        public Boolean NA17 { get; set; }
        [JsonProperty(PropertyName = "na18")]
        public Boolean NA18 { get; set; }
        [JsonProperty(PropertyName = "na19")]
        public Boolean NA19 { get; set; }
        [JsonProperty(PropertyName = "na20")]
        public Boolean NA20 { get; set; }


    }
}