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
    public class BLSACert
    {
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; } = "BLSACert";

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "bridgeModule")]
        public string BridgeModule { get; set; }
        [Required]
        [StringLength(50)]
        [JsonProperty(PropertyName = "uniqueKey")]
        public string BookMarkName { get; set; }

        [JsonProperty(PropertyName = "chapter")]
        public string Chapter { get; set; }
        [DataType(DataType.MultilineText)]
        [JsonProperty(PropertyName = "formula")]
        public string Formula { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        [JsonProperty(PropertyName = "condition")]
        public string Condition { get; set; }

    }
}