using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BridgeMVC.Models
{
    public class BEmail
    {
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; } = "BEmail";

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "bridgeModule")]
        public string BridgeModule { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("for TASK(?), eg. TASK1")]
        [JsonProperty(PropertyName = "uniqueKey")]
        public string TemplateName { get; set; }

        [JsonProperty(PropertyName = "mailTo")]
        public string MailTo { get; set; }


        [JsonProperty(PropertyName = "mailCC")]
        public string MailCC { get; set; }

        [JsonProperty(PropertyName = "mailTitle")]
        public string MailTitle { get; set; }

        [DataType(DataType.MultilineText)]
        [JsonProperty(PropertyName = "mailBody")]
        public string MailBody { get; set; }


        [JsonProperty(PropertyName = "mailAttach")]
        public string mailAttach { get; set; }
        
             
    }
}