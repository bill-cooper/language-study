using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace translation_tool
{
    public class TextTokenizer
    {
        public List<Token> GetTokens(string text) {
            var tokens = new List<Token>();

            var items = text.Split(new[] { ' ',';',',','(',')','.' });

            foreach (var item in items.Select(i => i.Trim()).Where(i => i != string.Empty)) {
                tokens.Add(new Token {
                    Value = item
                });
            }

            return tokens;

        }
    }
}
