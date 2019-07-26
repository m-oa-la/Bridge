using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BridgeMVC.Models
{
    public class BUser
    {

        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; } = "BUser";

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "employeeID")]
        public string EmployeeID { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "uniqueKey")]
        public string Signature { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "bridgesGranted")]
        public string BridgesGranted { get; set; }

        [JsonProperty(PropertyName = "bridgeLastUsed")]
        public string BridgeLastUsed { get; set; }

    }

}