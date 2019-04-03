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

        [JsonProperty(PropertyName = "uniqueKey")]
        public string Uk => $"{TCNo}{DbJobId}";


        [JsonProperty(PropertyName = "tcNo")]
        public int TCNo { get; set; }

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

        [JsonProperty(PropertyName = "tcNote")]
        public string TCNote { get; set; }
    }

  
}