using System.Collections.Generic;

namespace translation_tool
{

    public class Translation {
        public Translation()
        {
            Blocks = new List<Block>();
        }
        public List<Block> Blocks { get; set; }
    }
}