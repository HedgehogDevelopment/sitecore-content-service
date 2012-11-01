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
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HedgehogDevelopment.Scaas.Content;
using Sitecore.Exceptions;

namespace HedgehogDevelopment.Scaas.Web.Framework.Controllers
{
    /// <summary>
    /// Our ASP.NET WebAPI Controller.
    /// System.Web.Http.ApiController is defined in System.Web.Http.dll 
    /// System.Web.Http.dll is provided via NuGet package
    /// Microsoft.AspNet.WebApi.Core.4.0.20710.0
    /// </summary>
    public class ContentApiController : System.Web.Http.ApiController
    {
        private IContentNavigator _contentNavigator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentApiController"/> class.
        /// </summary>
        public ContentApiController()
        {
            /*
             * Calling new ContentService() in the controller is not the best design, 
             * because it ties this controller to HedgehogDevelopment.Scaas.Content.Items.ContentNavigator. 
             * For a better approach, use a Dependency Resolver (Injection) for our IContentNavigator inteface.
             */
            _contentNavigator = new ContentNavigator();
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("item")]
        public ContentItem GetItem(string key)
        {
            ContentItem contentItem = null;
            
            try
            {
                // use the IContentNavigator to get a ContentItem
                contentItem = _contentNavigator.GetItem<ContentItem>(key);
            }
            catch (ItemNotFoundException e)
            {
                // If the Sitecore item doesn't exist, send a...
                //  - 404 response
                //  - nice JSON object with the error message
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, e.Message));
            }
            catch
            {
            }

            // Validate the item
            if (contentItem == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            return contentItem;
        }

        /// <summary>
        /// Gets the parent of the item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("parent")]
        public ContentItem GetParent(string key)
        {
            ContentItem contentItem = null;
            try
            {
                contentItem = _contentNavigator.GetParentItem<ContentItem>(key);
            }
            catch (ItemNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            catch (ItemNullException)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format("Parent for item '{0}' not found.", key))
                    }
                );
            }
            catch { }

            // Validate the item
            if (contentItem == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            return contentItem;
        }

        /// <summary>
        /// Gets the ancestors of the item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ancestors")]
        public IEnumerable<ContentItem> GetAncestors(string key)
        {
            try
            {
                return _contentNavigator.GetAncestorItems<ContentItem>(key).ToList();
            }
            catch (ItemNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Gets the children of the item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("children")]
        public IEnumerable<ContentItem> GetChildren(string key)
        {
            return _contentNavigator.GetChildItems<ContentItem>(key).ToList();
        }

        /// <summary>
        /// Gets the descendants of the item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("descendants")]
        public IEnumerable<ContentItem> GetDescendants(string key)
        {
            return _contentNavigator.GetDescendantItems<ContentItem>(key).ToList();
        }

        /// <summary>
        /// Gets the referrers.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("referrers")]
        public IEnumerable<ContentItem> GetReferrers(string key)
        {
            return _contentNavigator.GetReferringItems<ContentItem>(key).ToList();
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("items")]
        public IEnumerable<ContentItem> GetItems(string key)
        {
            return _contentNavigator.GetItems<ContentItem>(key.Split(",".ToCharArray())).ToList();
        }
    }
}