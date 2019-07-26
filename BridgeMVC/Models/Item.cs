using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BridgeMVC.Models
{
    public class Item
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "uniqueKey")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "description2")]
        public string Description2 { get; set; }

        [JsonProperty(PropertyName = "isComplete")]
        public bool Completed { get; set; }

        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }



    }
}