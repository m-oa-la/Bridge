using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BridgeMVC.Models
{
    public class IORA
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Required]
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; } = "IORA";

        [Required]
        [JsonProperty(PropertyName = "bridgeModule")]
        public string BridgeModule { get; set; }

        [Required]
        [Display(Name = "NPS Job ID")]
        [JsonProperty(PropertyName = "uniqueKey")]
        public string NpsJobID { get; set; }

        [Required]
        [JsonProperty(PropertyName = "dbJobId")]
        public string DbJobId { get; set; }

        [JsonProperty(PropertyName = "NPSJNo")]
        public string NPSJNo { get; set; }

        [Display(Name = "Selling Unit Contact Signature")]
        [JsonProperty(PropertyName = "SellingContactSig")]
        public string SellingContactSig { get; set; }

        [Display(Name = "Buying Unit Contact Signature")]
        [JsonProperty(PropertyName = "BuyingContactSig")]
        public string BuyingContactSig { get; set; }
        //Selling Unit
        [JsonProperty(PropertyName = "DnvUnitName501")]
        public string DnvUnitName501 { get; set; }
        [JsonProperty(PropertyName = "DnvUnitNo501")]
        public string DnvUnitNo501 { get; set; }
        [JsonProperty(PropertyName = "DgIntUnVAT501")]
        public string DgIntUnVAT501 { get; set; }
        [JsonProperty(PropertyName = "DnvIntCompAccnt501")]
        public string DnvIntCompAccnt501 { get; set; }
        [JsonProperty(PropertyName = "DnvIntUnPrCeNo501")]
        public string DnvIntUnPrCeNo501 { get; set; }
        [JsonProperty(PropertyName = "DpIntUnProjNo501")]
        public string DpIntUnProjNo501 { get; set; }

        //Buying Unit
        [JsonProperty(PropertyName = "DnvUnitName502")]
        public string DnvUnitName502 { get; set; }
        [JsonProperty(PropertyName = "DnvUnitNo502")]
        public string DnvUnitNo502 { get; set; }
        [JsonProperty(PropertyName = "DgIntUnVAT502")]
        public string DgIntUnVAT502 { get; set; }
        [JsonProperty(PropertyName = "DnvContPersName502")]
        public string DnvContPersName502 { get; set; }
        [JsonProperty(PropertyName = "DnvIntCompAccnt502")]
        public string DnvIntCompAccnt502 { get; set; }
        [JsonProperty(PropertyName = "DnvIntUnPrCeNo502")]
        public string DnvIntUnPrCeNo502 { get; set; }
        [JsonProperty(PropertyName = "DpIntUnProjNo502")]
        public string DpIntUnProjNo502 { get; set; }
 
        //Project Details
        [JsonProperty(PropertyName = "DpProjName01")]
        public string DpProjName01 { get; set; }
        [JsonProperty(PropertyName = "DpProjWorkLoc01")]
        public string DpProjWorkLoc01 { get; set; }
        [JsonProperty(PropertyName = "DpServiceName01")]
        public string DpServiceName01 { get; set; }
        [JsonProperty(PropertyName = "DpServiceCode01")]
        public string DpServiceCode01 { get; set; }
        [JsonProperty(PropertyName = "DgCustName01")]
        public string DgCustName01 { get; set; }
        [JsonProperty(PropertyName = "DgCustomerRef01")]
        public string DgCustomerRef01 { get; set; }
        [JsonProperty(PropertyName = "DpProjStartDate01")]
        public string DpProjStartDate01 { get; set; }
        [JsonProperty(PropertyName = "DpProjStartEnd01")]
        public string DpProjStartEnd01 { get; set; }
        [DataType(DataType.MultilineText)]
        [JsonProperty(PropertyName = "DpSoW01")]
        public string DpSoW01 { get; set; }
        [JsonProperty(PropertyName = "DpSupportingDocs01")]
        public string DpSupportingDocs01 { get; set; }
        [JsonProperty(PropertyName = "IORAFee")]
        public int? IORAFee { get; set; }

        [DataType(DataType.MultilineText)]
        [JsonProperty(PropertyName = "DgSpecialConditions")]
        public string DgSpecialConditions { get; set; }
        [DataType(DataType.MultilineText)]
        [JsonProperty(PropertyName = "Str_SpecialC")]
        public string Str_SpecialC { get; set; }
        [JsonProperty(PropertyName = "DnvIntUnPlace501")]
        public string DnvIntUnPlace501 { get; set; }
        [JsonProperty(PropertyName = "DnvIntUnDate501")]
        public string DnvIntUnDate501 { get; set; }
        [JsonProperty(PropertyName = "DnvIntUnSigName501")]
        public string DnvIntUnSigName501 { get; set; }
        [JsonProperty(PropertyName = "DnvIntUnSigTitle501")]
        public string DnvIntUnSigTitle501 { get; set; }
  
        [JsonProperty(PropertyName = "DgDNVDocNo01")]
        public string DgDNVDocNo01 { get; set; }

        //DnvContPersName501

        [JsonProperty(PropertyName = "DnvContPersName501")]
        public string DnvContPersName501 { get; set; }
        [JsonProperty(PropertyName = "DpDeliverables01")]
        public string DpDeliverables01 { get; set; }

        [JsonProperty(PropertyName = "ioraSentDate")]
        public DateTime IORASentTime { get; set; }

        [JsonProperty(PropertyName = "signedIoraRcTime")]
        public DateTime SignedIoraRcTime { get; set; }

        [JsonProperty(PropertyName = "sendingFlag")]
        public string SendingFlag { get; set; }

        [JsonProperty(PropertyName = "ioraSentBy")]
        public string IORASentBy { get; set; }
    }
}