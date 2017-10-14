using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace translation_tool.Util
{
    public static class Extensions
    {
        public static void ForEach(this IHtmlCollection<IElement> elements, Action<IElement> action)
        {
            foreach (var element in elements) action(element);
        }
    }
}
