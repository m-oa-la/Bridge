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
    public class BingList
    {
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; } = "BingList";

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "bridgeModule")]
        public string BridgeModule { get; set; }
        [Required]
        [StringLength(50)]

        [JsonProperty(PropertyName = "listType")] // CertType, Rule, EqtType, SubEqtType 
        public string ListType { get; set; } 

        [JsonProperty(PropertyName = "listItem")]
        public string ListItem { get; set; }

        [JsonProperty(PropertyName = "upperLvl")]
        public string UpperLvl { get; set; }


        [JsonProperty(PropertyName = "uniqueKey")]
        public string Uk => $"{ListType} {ListItem} {UpperLvl}";

    }
}