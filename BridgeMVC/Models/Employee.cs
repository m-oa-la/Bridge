using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BridgeMVC.Models
{
    public class Employee
    {

        [JsonProperty(PropertyName = "personNumber")]
        public string PersonNumber { get; set; }

        [JsonProperty(PropertyName = "uniqueKey")]
        public string PersonSignature { get; set; }

        [JsonProperty(PropertyName = "preferredNameConcatenated")]
        public string PreferredNameConcatenated { get; set; }

        [JsonProperty(PropertyName = "departmentShortName")]
        public string DepartmentShortName { get; set; }

        [JsonProperty(PropertyName = "signature")]
        public string Signature { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "businessEmail")]
        public string BusinessEmail { get; set; }

        [JsonProperty(PropertyName = "legalEntityIdentifier")]
        public string LegalEntityIdentifier { get; set; }


        [JsonProperty(PropertyName = "addressLine1")]
        public string AddressLine1 { get; set; }

        [JsonProperty(PropertyName = "addressLine2")]
        public string AddressLine2 { get; set; }

        [JsonProperty(PropertyName = "addressLine3")]
        public string AddressLine3 { get; set; }

        [JsonProperty(PropertyName = "shortName")]
        public string ShortName { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "managerSignature")]
        public string ManagerSignature { get; set; }


    }
}