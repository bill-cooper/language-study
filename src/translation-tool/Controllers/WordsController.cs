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
        public async Task<Translation> Translation([FromBody] TextBody textBody)
        {
            var translation = new Translation();

            var service = new TranslateService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyBiPTuvLggID2YrmBshuHBZhij6HeFOxko",
                ApplicationName = "Project Name"
            });

            

            var tokenizer = new TextTokenizer();
            var blocks = tokenizer.GetBlocks(textBody.Input);


            var blockResponse = await service.Translations.List(blocks.Select(t => t.OriginalText).ToArray(), "en").ExecuteAsync();


            for (int i = 0; i < blockResponse.Translations.Count; i++)
            {
                blocks[i].TranslatedText = blockResponse.Translations[i].TranslatedText;



                var words = tokenizer.GetWords(blocks[i].OriginalText);


                var wordResponse = await service.Translations.List(words.Select(t => t.Value).ToArray(), "en").ExecuteAsync();


                for (int j = 0; j < wordResponse.Translations.Count; j++)
                {
                    words[j].Translation = wordResponse.Translations[j].TranslatedText;
                    blocks[i].Words.Add(words[j]);
                }

                
                translation.Blocks.Add(blocks[i]);
            }




            return translation;
        }


        // POST api/values
        [HttpPost]
        [Route("audios")]
        public async Task<IEnumerable<string>> Post([FromBody] TextBody textBody)
        {
            var tokenizer = new TextTokenizer();
            var tokens = tokenizer.GetWords(textBody.Input);
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
