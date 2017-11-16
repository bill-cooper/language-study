using System.Collections.Generic;

namespace translation_tool
{

    public class Block {
        public Block() {
            Words = new List<Word>();
        }
        public string OriginalText { get; set; }
        public string TranslatedText { get; set; }
        public List<Word> Words { get; set; }
    }
}