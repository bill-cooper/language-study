using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace translation_tool
{
    public class TextTokenizer
    {
        public List<Word> GetWords(string text) {
            var tokens = new List<Word>();

            var items = text.Split(new[] { ' ',';',',','(',')','.' });

            foreach (var item in items.Select(i => i.Trim()).Where(i => i != string.Empty)) {
                tokens.Add(new Word {
                    Value = item
                });
            }

            return tokens;
        }

        public List<Block> GetBlocks(string text)
        {
            var blocks = new List<Block>();

            var items = text.Split(new[] { '!', '.', '?' });

            foreach (var item in items.Select(i => i.Trim()).Where(i => i != string.Empty))
            {
                blocks.Add(new Block
                {
                    OriginalText = item
                });
            }

            return blocks;
        }
    }
}
