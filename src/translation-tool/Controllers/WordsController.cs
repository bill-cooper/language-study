using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Google.Apis.Translate.v2;
using Google.Apis.Services;

namespace translation_tool.Controllers
{
    [Route("api/[controller]")]
    public class WordsController : Controller
    {

        // POST api/values
        [HttpPost]
        [Route("translation")]
        public async Task<IEnumerable<Token>> Translation([FromBody] TextBody textBody)
        {

            var service = new TranslateService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyBiPTuvLggID2YrmBshuHBZhij6HeFOxko",
                ApplicationName = "Project Name"
            });

            

            var tokenizer = new TextTokenizer();
            var tokens = tokenizer.GetTokens(textBody.Input);
            //foreach (var token in tokens)
            //{
            //    var composition = new Composition { Return = new ContentSegment { Url = $"https://forvo.com/word/{token.Value}/#ru", Select = "span.play" } };
            //    var elements = await composition.Return.DocumentElement();
            //    foreach (var element in elements)
            //    {
            //        var onclick = element.GetAttribute("onclick");
            //        var onclickParts = onclick.Split(',');
            //        if (onclickParts.Count() >= 5)
            //            token.Audios.Add(Encoding.UTF8.GetString(Convert.FromBase64String(onclickParts[4].Trim(new[] { '\'', '"' }))));

            //        if (token.Audios.Count >= 5) break;
            //    }
            //}
            
            var response = await service.Translations.List(tokens.Select(t => t.Value).ToArray(), "en").ExecuteAsync();
            
            for (int i = 0; i < response.Translations.Count; i++)
            {
                tokens[i].Translation = response.Translations[i].TranslatedText;
            }

            return tokens;
        }


        // POST api/values
        [HttpPost]
        [Route("audios")]
        public async Task<IEnumerable<string>> Post([FromBody] TextBody textBody)
        {
            var tokenizer = new TextTokenizer();
            var tokens = tokenizer.GetTokens(textBody.Input);
            var audios = new List<string>();
            foreach (var token in tokens) {
                var composition = new Composition { Return = new ContentSegment { Url = $"https://forvo.com/word/{token.Value}/#ru", Select="span.play"} };
                var elements = await composition.Return.DocumentElement();
                foreach (var element in elements) {
                    var onclick = element.GetAttribute("onclick");
                    var onclickParts = onclick.Split(',');
                    if(onclickParts.Count() >=5)
                        audios.Add(Encoding.UTF8.GetString(Convert.FromBase64String(onclickParts[4].Trim(new[] { '\'','"'}))));

                    if (audios.Count >= 5) break;
                }
            }
            return audios;
        }

    }
    public class TextBody {
        public string Input { get; set; }
    }
}
