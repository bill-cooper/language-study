using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;

namespace translation_tool.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<IEnumerable<Token>> Post([FromBody] TextBody textBody)
        {

            var tokens = new List<Token>();
            var words = textBody.Input.Split(' ');
            foreach (var word in words) {
                var token = new Token { Display = word };
                var composition = new Composition { Return = new ContentSegment { Url = $"https://forvo.com/word/{word}/#en", Select="span.play"} };
                var elements = await composition.Return.DocumentElement();
                foreach (var element in elements) {
                    var onclick = element.GetAttribute("onclick");
                    var onclickParts = onclick.Split(',');
                    if(onclickParts.Count() >=5)
                        token.Audios.Add(Encoding.UTF8.GetString(Convert.FromBase64String(onclickParts[4].Trim(new[] { '\'','"'}))));

                 }
                tokens.Add(token);
            }
            return tokens;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
    public class TextBody {
        public string Input { get; set; }
    }

    public class Token {
        public Token() {
            Audios = new List<string>();
        }
        public string Display { get; set; }
        public List<string> Audios { get; set; }
    }
}
