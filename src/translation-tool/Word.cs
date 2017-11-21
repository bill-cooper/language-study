using System.Collections.Generic;

namespace translation_tool
{

    public class Word {
        public Word() {
            Audios = new List<string>();
        }
        public string Value { get; set; }
        public string Translation { get; set; }
        public List<string> Audios { get; set; }
        public WordInfo Info { get; set; }
    }
}