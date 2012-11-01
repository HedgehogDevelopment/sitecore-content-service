#region License
// Copyright © 2012 Hedgehog Development, LLC (www.hhogdev.com)
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using HedgehogDevelopment.Scaas.Content;
using HedgehogDevelopment.Scaas.Content.Remote.Configuration;

namespace HedgehogDevelopment.Scaas.Content.Remote
{
    public class RemoteSitecoreContentService : IContentNavigator
    {
        /// <summary>
        /// Supported methods of the content service api
        /// </summary>
        private enum Methods
        {
            item,
            parent,
            ancestors,
            children,
            descendants,
            referrers,
            items
        }

        RemoteConfigurationSection config = RemoteConfigurationSection.Create();

        public TContentItem GetItem<TContentItem>(object key) where TContentItem : ContentItem
        {
            return GetContentItemFromSitecore<TContentItem>(Methods.item, key);
        }

        public TContentItem GetParentItem<TContentItem>(object key) where TContentItem : ContentItem
        {
            return GetContentItemFromSitecore<TContentItem>(Methods.parent, key);
        }

        public IEnumerable<TContentItem> GetAncestorItems<TContentItem>(object key) where TContentItem : ContentItem
        {
            return GetContentItemsFromSitecore<TContentItem>(Methods.ancestors, key);
        }

        public IEnumerable<TContentItem> GetChildItems<TContentItem>(object key) where TContentItem : ContentItem
        {
            return GetContentItemsFromSitecore<TContentItem>(Methods.children, key);
        }

        public IEnumerable<TContentItem> GetDescendantItems<TContentItem>(object key) where TContentItem : ContentItem
        {
            return GetContentItemsFromSitecore<TContentItem>(Methods.descendants, key);
        }

        public IEnumerable<TContentItem> GetItems<TContentItem>(IEnumerable<object> keys) where TContentItem : ContentItem
        {
            return GetContentItemsFromSitecore<TContentItem>(Methods.items, string.Join("|", keys.Select(k => k.ToString()).ToArray()));
        }

        public IEnumerable<TContentItem> GetReferringItems<TContentItem>(object key) where TContentItem : ContentItem
        {
            return GetContentItemsFromSitecore<TContentItem>(Methods.referrers, key);
        }
                
        private TContentItem GetContentItemFromSitecore<TContentItem>(Methods method, object key) where TContentItem : ContentItem
        {
            string url = BuildServiceUrl(method, key.ToString());

            using (var httpClient = new HttpClient())
            {
                // call the JSON service and get the response
                HttpResponseMessage responseMessage = httpClient.GetAsync(url).Result;
                responseMessage.EnsureSuccessStatusCode();

                // Built in (to ASP.NET Web API) JSON deserialization
                TContentItem contentItem = responseMessage.Content.ReadAsAsync<TContentItem>().Result;
                return contentItem;
            }
        }

        private IEnumerable<TContentItem> GetContentItemsFromSitecore<TContentItem>(Methods method, object key) where TContentItem : ContentItem
        {
            string url = BuildServiceUrl(method, key.ToString());

            using (var httpClient = new HttpClient())
            {
                // call the JSON service and get the response
                var responseMessage = httpClient.GetAsync(url).Result;
                responseMessage.EnsureSuccessStatusCode();

                // Built in (to ASP.NET Web API) JSON deserialization
                var contentItems = responseMessage.Content.ReadAsAsync<IEnumerable<TContentItem>>().Result;
                return contentItems;
            }
        }

        /// <summary>
        /// Builds the service URL for the request.
        /// </summary>
        private string BuildServiceUrl(Methods method, string key)
        {
            // build up the url starting with the endpoint
            StringBuilder urlBuilder = new StringBuilder(config.ServiceEndpoint.ToString());

            // add a trailing slash to the url
            if (urlBuilder[urlBuilder.Length - 1] != '/')
            {
                urlBuilder.Append('/');
            }
            // add the method to the url
            urlBuilder.Append(method.ToString());

            // add the item key
            if (!key.StartsWith("/"))
            {
                urlBuilder.Append('/');
            }

            urlBuilder.Append(key.ToString());

            return urlBuilder.ToString();
        }
    }
}
