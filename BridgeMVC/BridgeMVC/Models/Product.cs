using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BridgeMVC.Models
{
    public class ProdTechPara
    {
        [JsonProperty(PropertyName = "techParaName")]
        public string TechParaName { get; set; }

        [JsonProperty(PropertyName = "techParaValue")]
        public string TechParaValue { get; set; }

    }

    public class Product
    {
        [Required]
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; } = "Product";
        [Required]
        [JsonProperty(PropertyName = "bridgeModule")]
        public string BridgeModule { get; set; }

        [JsonProperty(PropertyName = "mainProdType")]
        public string MainProdType { get; set; }

        [JsonProperty(PropertyName = "subProdType")]
        public string SubProdType { get; set; }

        [JsonProperty(PropertyName = "prodDescription")]
        public string ProdDescription { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
     
        [JsonProperty(PropertyName = "uniqueKey")]
        public string Uk => $"{Id}";

        [JsonProperty(PropertyName = "ncProdDbId")] //Nauticus certification product id
        public string NCProdDbId { get; set; }

        [JsonProperty(PropertyName = "dbJobId")]
        public string DbJobId { get; set; }

        [JsonProperty(PropertyName = "prodName")]
        public string ProdName { get; set; }

        [JsonProperty(PropertyName = "ncProdType")]
        public string NCProdType { get; set; }

        public List<ProdTechPara> PTPs { get; set; }
    }


}