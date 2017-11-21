using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace translation_tool
{
    public class ORTerm
    {
        [JsonProperty(PropertyName = "term")]
        public string Term { get; set; }
        [JsonProperty(PropertyName = "words")]
        public ORWord[] Words { get; set; }
        [JsonProperty(PropertyName = "derivates")]
        public ORDerivates[] Derivates { get; set; }
    }

    public class ORWord
    {
        [JsonProperty(PropertyName = "ru")]
        public string Ru { get; set; }
        [JsonProperty(PropertyName = "ruAccented")]
        public string RuAccented { get; set; }
        [JsonProperty(PropertyName = "tls")]
        public string[][] Translations { get; set; }
    }
    public class ORDerivates
    {
        [JsonProperty(PropertyName = "baseBare")]
        public string BaseBare { get; set; }
        [JsonProperty(PropertyName = "baseAccented")]
        public string BaseAccented { get; set; }
        [JsonProperty(PropertyName = "tl")]
        public string Translation { get; set; }
    }
    public class ORSentences
    {
        [JsonProperty(PropertyName = "ru")]
        public string Ru { get; set; }

        [JsonProperty(PropertyName = "tl")]
        public string Translation { get; set; }
    }
}
