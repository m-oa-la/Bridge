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

        [JsonProperty(PropertyName = "tc1")]
        public string TC1 { get; set; }
        [JsonProperty(PropertyName = "tc2")]
        public string TC2 { get; set; }
        [JsonProperty(PropertyName = "tc3")]
        public string TC3 { get; set; }
        [JsonProperty(PropertyName = "tc4")]
        public string TC4 { get; set; }
        [JsonProperty(PropertyName = "tc5")]
        public string TC5 { get; set; }
        [JsonProperty(PropertyName = "tc6")]
        public string TC6 { get; set; }
        [JsonProperty(PropertyName = "tc7")]
        public string TC7 { get; set; }
        [JsonProperty(PropertyName = "tc8")]
        public string TC8 { get; set; }
        [JsonProperty(PropertyName = "tc9")]
        public string TC9 { get; set; }
        [JsonProperty(PropertyName = "tc10")]
        public string TC10 { get; set; }
        [JsonProperty(PropertyName = "tc11")]
        public string TC11 { get; set; }
        [JsonProperty(PropertyName = "tc12")]
        public string TC12 { get; set; }
        [JsonProperty(PropertyName = "tc13")]
        public string TC13 { get; set; }
        [JsonProperty(PropertyName = "tc14")]
        public string TC14 { get; set; }
        [JsonProperty(PropertyName = "tc15")]
        public string TC15 { get; set; }
        [JsonProperty(PropertyName = "tc16")]
        public string TC16 { get; set; }
        [JsonProperty(PropertyName = "tc17")]
        public string TC17 { get; set; }
        [JsonProperty(PropertyName = "tc18")]
        public string TC18 { get; set; }
        [JsonProperty(PropertyName = "tc19")]
        public string TC19 { get; set; }
        [JsonProperty(PropertyName = "tc20")]
        public string TC20 { get; set; }
               

    }
}