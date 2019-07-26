using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BridgeMVC.Models
{
    public class BingProduct
    {
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; } = "BingProduct";

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "bridgeModule")]
        public string BridgeModule { get; set; }
        [Required]
        [StringLength(50)]
        [JsonProperty(PropertyName = "uniqueKey")]
        public string RuleName { get; set; }
    }
}