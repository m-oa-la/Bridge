using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BridgeMVC.Models
{
    public class BBridge
    {

        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; } = "BBridge";

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "uniqueKey")]
        public string BridgeName { get; set; }

        [JsonProperty(PropertyName = "templatePath")]
        public string TemplatePath { get; set; }

        [JsonProperty(PropertyName = "archivePath")]
        public string ArchivePath { get; set; }

    }

}