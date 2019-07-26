using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BridgeMVC.Models
{
    public class BingFinancial
    {
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; } = "BingFinancial";

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "bridgeModule")]
        public string BridgeModule { get; set; }
        [Required]
        [StringLength(50)]

        [JsonProperty(PropertyName = "uniqueKey")]
        public string CertType { get; set; }


        [JsonProperty(PropertyName = "tsa")]
        public decimal TSA { get; set; }


        [JsonProperty(PropertyName = "msa")]
        public decimal MSA { get; set; }


        [JsonProperty(PropertyName = "allocationFee")]
        public decimal AllocationFee { get; set; }


        [JsonProperty(PropertyName = "hourlyRate")]
        public int HourlyRate { get; set; }


        [JsonProperty(PropertyName = "serviceCode")]
        public string ServiceCode { get; set; }

        [JsonProperty(PropertyName = "deliverable")]
        public string Deliverable { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "convFactor")]
        public string ConvFactor { get; set; }

        [JsonProperty(PropertyName = "CertAction1")]
        public string CertAction1 { get; set; }

        [JsonProperty(PropertyName = "CertAction2")]
        public string CertAction2 { get; set; }

        [JsonProperty(PropertyName = "CertAction3")]
        public string CertAction3 { get; set; }

        [JsonProperty(PropertyName = "CertAction4")]
        public string CertAction4 { get; set; }

    }
}