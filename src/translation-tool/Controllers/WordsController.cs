﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Google.Apis.Translate.v2;
using Google.Apis.Services;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

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

            using (var service = new TranslateService(new BaseClientService.Initializer
            {
                ApiKey = "AIzaSyBiPTuvLggID2YrmBshuHBZhij6HeFOxko",
                ApplicationName = "Project Name"
            }))
            {
                var input = textBody.Input;

                if (input.StartsWith("en:"))
                {
                    var russianResponse = await service.Translations.List(new string[] { input.Replace("en:","") }, "ru").ExecuteAsync();
                    input = russianResponse.Translations[0].TranslatedText;
                }


                var tokenizer = new TextTokenizer();
                var blocks = tokenizer.GetBlocks(input);

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
        }


        // POST api/values
        [HttpPost]
        [Route("audios")]
        public async Task<IEnumerable<string>> Audios([FromBody] TextBody textBody)
        {
            var tokenizer = new TextTokenizer();
            var tokens = tokenizer.GetWords(textBody.Input);
            var audios = new List<string>();
            foreach (var token in tokens)
            {
                var composition = new Composition { Return = new ContentSegment { Url = $"https://forvo.com/word/{token.Value.RemoveAccents()}/#ru", Select = "span.play" } };
                var elements = await composition.Return.DocumentElement();
                foreach (var element in elements)
                {
                    var onclick = element.GetAttribute("onclick");
                    var onclickParts = onclick.Split(',');
                    if (onclickParts.Count() >= 5)
                        audios.Add(Encoding.UTF8.GetString(Convert.FromBase64String(onclickParts[4].Trim(new[] { '\'', '"' }))));

                    if (audios.Count >= 5) break;
                }
            }
            return audios;
        }


        [HttpPost]
        [Route("detail")]
        public async Task<string> Detail([FromBody] TextBody textBody)
        {

            var composition = new Composition { Return = new ContentSegment { Url = $"https://en.openrussian.org/ru/{textBody.Input}", Select = "div.page" } };
            var elements = await composition.Return.DocumentElement();


            return elements.FirstOrDefault().OuterHtml;
        }

        [HttpPost]
        [Route("info")]
        public async Task<WordInfo> WordInfo([FromBody] TextBody textBody)
        {
            var info = new WordInfo();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"https://en.openrussian.org/suggestions?q={textBody.Input}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var term = JsonConvert.DeserializeObject<ORTerm>(json);

                if (term.Words.Length > 0 && textBody.Input.ToLower() == term.Words[0].Ru)
                {
                    var word = term.Words[0];
                    info.Word = word.Ru;
                    info.AccentedWord = word.RuAccented == string.Empty ? word.Ru : word.RuAccented;

                    var translationString = "";
                    if (word.Translations.Length > 0)
                        foreach (var translation in word.Translations[0])
                            translationString += $" {translation}";
                    info.Translation = translationString;
                }
                if (term.Derivates.Length > 0)
                {
                    var derivate = term.Derivates[0];
                    info.Derivate = derivate.BaseBare;
                    if(info.Translation == string.Empty)
                        info.Translation = derivate.Translation;
                }

                if (info.Word == string.Empty)
                    info.Word = textBody.Input;

                return info;

            }
        }

    }




    public class TextBody
    {
        public string Input { get; set; }
    }

}
