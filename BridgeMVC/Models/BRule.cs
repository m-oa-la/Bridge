using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BridgeMVC.Models
{
    public class BRule
    {

        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; } = "BRule";

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "bridgeModule")]
        public string BridgeModule { get; set; }
        [Required]
        [StringLength(500)]

        [JsonProperty(PropertyName = "uniqueKey")]
        public string RuleName { get; set; }


    }
}