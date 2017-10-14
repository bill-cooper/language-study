using Microsoft.AspNetCore.Http;
using System;

namespace translation_tool.Util
{
    public class Route
    {
        public Route(Uri uri, string selectFilter)
        {
            Uri = uri;
            SelectFilter = selectFilter;
        }
        public Uri Uri { get; private set; }
        public string SelectFilter { get; private set; }
        public static Route Parse(string route, IQueryCollection query)
        {
            //get host route token and transform to a host
            //build uri without the filter portion
            var selectFilter = "";
            var queryString = "";
            foreach (var token in query)
            {
                if (token.Key.ToLower() == "$filter")
                {
                    selectFilter = token.Value;
                    continue;
                }
                if (queryString.Length > 0) queryString += "&";
                queryString += $"{token.Key}={token.Value}";
            }
            var cleanUrl = $"http://{route}{(queryString.Length > 0 ? "?" : "")}{queryString}";
            var uri = new Uri(cleanUrl);
            //resolve any redirects to get a clean url
            //create 
            return new Route(new Uri(cleanUrl), selectFilter);
        }
    }
}
