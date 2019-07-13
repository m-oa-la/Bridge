using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BridgeMVC.Models
{
    public class ProdTechPara
    {
        [Required]
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; } = "BProdTechPara";

        [Required]
        [JsonProperty(PropertyName = "bridgeModule")]
        public string BridgeModule { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "uniqueKey")]
        public string TechParaName { get; set; }

        [JsonProperty(PropertyName = "productDbId")]
        public string ProductDbId { get; set; }

        [JsonProperty(PropertyName = "valueLong")]
        public long ValueLong { get; set; }

        [JsonProperty(PropertyName = "valueInteger")]
        public int ValueInteger { get; set; }

        [JsonProperty(PropertyName = "valueBoolean")]
        public Boolean ValueBoolean { get; set; }

        [JsonProperty(PropertyName = "valueString")]
        public string ValueString { get; set; }

    }
}