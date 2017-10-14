using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace translation_tool.Document
{
    public interface IDocumentProvider
    {
        Task<IDocument> GetAsync(Uri uri);
    }
    public class DocumentProvider : IDocumentProvider
    {
        readonly IRequester _requester;
        public DocumentProvider(IRequester requester)
        {
            _requester = requester;
        }
        public async Task<IDocument> GetAsync(Uri uri)
        {
            var config = Configuration.Default.WithDefaultLoader(setup => setup.IsResourceLoadingEnabled = true, new[] { _requester });
            return await BrowsingContext.New(config).OpenAsync(uri.AbsoluteUri);
        }
    }
}
