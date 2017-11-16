using System.Collections.Generic;

namespace translation_tool
{

    public class Token {
        public Token() {
            Audios = new List<string>();
        }
        public string Value { get; set; }
        public string Translation { get; set; }
        public List<string> Audios { get; set; }
    }
}