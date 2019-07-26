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
    public class BCertType
    {

        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; } = "BCertType";

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "bridgeModule")]
        public string BridgeModule { get; set; }
        [Required]
        [StringLength(50)]

        //[Remote("IsProductNameExist", "Product", AdditionalFields = "Id",
        //                ErrorMessage = "Product name already exists")]
        [JsonProperty(PropertyName = "uniqueKey")]
        public string CertTypeName { get; set; }



    }
}