using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace BridgeMVC.Models
{
    public class Customer
    {
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; } = "Customer";

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "customerId")]
        public string CustomerId { get; set; }

        [JsonProperty(PropertyName = "customerName")]
        public string CustomerName { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "contactPerson")]
        public string ContactPerson { get; set; }

        [JsonProperty(PropertyName = "phoneNo")]
        public string PhoneNo { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "invoiceInfo")]
        public string InvoiceInfo { get; set; }

        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }
    }

}