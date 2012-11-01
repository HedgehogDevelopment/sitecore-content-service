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

namespace HedgehogDevelopment.Scaas.Content
{
    /// <summary>
    /// An interface to navigate a hierarchical CMS and get ContentItem(s).
    /// </summary>
    public interface IContentNavigator
    {
        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="TContentItem">The type of the content item.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        TContentItem GetItem<TContentItem>(object key) where TContentItem : ContentItem;

        /// <summary>
        /// Gets the parent of the item specified by the key.
        /// </summary>
        /// <typeparam name="TContentItem">The type of the content item.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        TContentItem GetParentItem<TContentItem>(object key) where TContentItem : ContentItem;

        /// <summary>
        /// Gets the ancestors of the item specified by the key.
        /// </summary>
        /// <typeparam name="TContentItem">The type of the content item.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        IEnumerable<TContentItem> GetAncestorItems<TContentItem>(object key) where TContentItem : ContentItem;
        
        /// <summary>
        /// Gets the children of the item specified by the key.
        /// </summary>
        /// <typeparam name="TContentItem">The type of the content item.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        IEnumerable<TContentItem> GetChildItems<TContentItem>(object key) where TContentItem : ContentItem;
        
        /// <summary>
        /// Gets the descendants of the item specified by the key.
        /// </summary>
        /// <typeparam name="TContentItem">The type of the content item.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        IEnumerable<TContentItem> GetDescendantItems<TContentItem>(object key) where TContentItem : ContentItem;
        
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <typeparam name="TContentItem">The type of the content item.</typeparam>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
        IEnumerable<TContentItem> GetItems<TContentItem>(IEnumerable<object> keys) where TContentItem : ContentItem;
                
        /// <summary>
        /// Gets the referrers of the item specified by the key..
        /// </summary>
        /// <typeparam name="TContentItem">The type of the content item.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        IEnumerable<TContentItem> GetReferringItems<TContentItem>(object key) where TContentItem : ContentItem;
    }
}
