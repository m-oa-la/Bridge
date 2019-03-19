using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BridgeMVC.Models
{
    public class Product
    {
        [Required]
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; } = "Product";
        [Required]
        [JsonProperty(PropertyName = "bridgeModule")]
        public string BridgeModule { get; set; }
        [Required]
        [JsonProperty(PropertyName = "mainProdType")]
        public string MainProdType { get; set; }
        [Required]
        [JsonProperty(PropertyName = "subProdType")]
        public string SubProdType { get; set; }
        [Required]
        [JsonProperty(PropertyName = "prodDescription")]
        public string ProdDescription { get; set; }


        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
     
        [JsonProperty(PropertyName = "uniqueKey")]
        public string Uk => $"{NpsJobId} {SubProdType} {ProdDescription} {MainProdType} ";

        [JsonProperty(PropertyName = "npsJobId")]
        public string NpsJobId { get; set; }

        [JsonProperty(PropertyName = "dbJobId")]
        public string DbJobId { get; set; }

        [JsonProperty(PropertyName = "designPara1")]
        public string DesignPara1 { get; set; }

        [JsonProperty(PropertyName = "designPara2")]
        public string DesignPara2 { get; set; }

        [JsonProperty(PropertyName = "designPara3")]
        public string DesignPara3 { get; set; }


        [JsonProperty(PropertyName = "designPara4")]
        public string DesignPara4 { get; set; }


        [JsonProperty(PropertyName = "designPara5")]
        public string DesignPara5 { get; set; }

        [JsonProperty(PropertyName = "designPara6")]
        public string DesignPara6 { get; set; }


        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }

    }
}