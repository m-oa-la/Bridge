using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
namespace BridgeMVC.Models
{
    public class BingTechChecklist
    {
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; } = "BingTechChecklist";
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "bridgeModule")]
        public string BridgeModule { get; set; }
        [Required]
        [StringLength(50)]
        [JsonProperty(PropertyName = "bookMarkName")]
        public string BookMarkName { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        [JsonProperty(PropertyName = "formula")]
        public string Formula { get; set; }

        [JsonProperty(PropertyName = "condition")]
        public string Condition { get; set; }
        [Required]
        [JsonProperty(PropertyName = "itemNo")]
        public int ItemNo { get; set; }

        [JsonProperty(PropertyName = "templateName")]
        public string TemplateName { get; set; }

        [JsonProperty(PropertyName = "mainProdType")]
        public string MainProdType { get; set; }

        [JsonProperty(PropertyName = "subProdType")]
        public string SubProdType { get; set; }
        
        [JsonProperty(PropertyName = "uniqueKey")]
        public string Uk => $"{MainProdType}{SubProdType}{TemplateName}{ItemNo}";

        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }

        [JsonProperty(PropertyName = "lsaRef")]
        public string LSARef { get; set; }

        [JsonProperty(PropertyName = "medItemNo")]
        public string MEDItemNo { get; set; }

        [JsonProperty(PropertyName = "gudianceNote")]
        public string GudianceNote { get; set; }
                     
    }
}