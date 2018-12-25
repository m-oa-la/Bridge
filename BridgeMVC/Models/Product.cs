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

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "uniqueKey")]
        public string Uk => $"{Id}";

        [JsonProperty(PropertyName = "npsJobId")]
        public string NpsJobId { get; set; }

        [JsonProperty(PropertyName = "dbJobId")]
        public string DbJobId { get; set; }
    }
}