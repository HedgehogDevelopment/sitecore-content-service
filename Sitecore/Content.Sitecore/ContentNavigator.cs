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

using System;
using System.Collections.Generic;
using System.Linq;
using HedgehogDevelopment.Scaas.Content.Items;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Exceptions;
using Sitecore.Links;

namespace HedgehogDevelopment.Scaas.Content
{
    /// <summary>
    /// The Content Navigator for Sitecore CMS
    /// </summary>
    public class ContentNavigator : IContentNavigator
    {
        private ConventionMapper _conventionMapper = new ConventionMapper();

        #region Properties
        /*
        * For a better approach, use dependency injection and more
        * interfaces/services for database resolution.
        */

        protected Sitecore.Data.Database Database
        {
            get
            {
                return Sitecore.Context.Database ?? Sitecore.Configuration.Factory.GetDatabase("web");
            }
        }

        protected Sitecore.Links.LinkDatabase LinkDatabase
        {
            get
            {
                return Sitecore.Globals.LinkDatabase;
            }
        }
        #endregion

        public TContentItem GetItem<TContentItem>(object key) where TContentItem : ContentItem
        {
            // Get the Sitecore item specified by the key
            Sitecore.Data.Items.Item item = GetItem(key);

            // Map the Sitecore item to a new TContentItem
            TContentItem contentItem = MapItem<TContentItem>(item);

            // return the Content Item
            return contentItem;
        }

        #region IContentNavigator Implementation
        public TContentItem GetParentItem<TContentItem>(object key) where TContentItem : ContentItem
        {
            Item item = GetItem(key);

            Item parentItem = item.Parent;
            if (parentItem == null)
            {
                throw new ItemNullException(string.Format("Parent of item '{0}' not found", item.Paths.FullPath));
            }

            return MapItem<TContentItem>(parentItem);
        }

        public IEnumerable<TContentItem> GetAncestorItems<TContentItem>(object key) where TContentItem : ContentItem
        {
            Item item = GetItem(key);

            IEnumerable<Item> ancestorItems = item.Axes.GetAncestors();

            return MapItems<TContentItem>(ancestorItems);
        }

        public IEnumerable<TContentItem> GetChildItems<TContentItem>(object key) where TContentItem : ContentItem
        {
            Item item = GetItem(key);

            Sitecore.Collections.ChildList childItems = item.Children;

            return MapItems<TContentItem>(childItems);
        }

        public IEnumerable<TContentItem> GetDescendantItems<TContentItem>(object key) where TContentItem : ContentItem
        {
            Item item = GetItem(key);

            IEnumerable<Item> descendantItems = item.Axes.GetDescendants();

            return MapItems<TContentItem>(descendantItems);
        }

        public IEnumerable<TContentItem> GetItems<TContentItem>(IEnumerable<object> keys) where TContentItem : ContentItem
        {
            var items = new List<Item>(keys.Count());
            foreach (var key in keys)
            {
                Item i = null;
                try
                {
                    i = GetItem(key);
                }
                catch (ItemNotFoundException) { } // eat not found exceptions

                if (i != null)
                {
                    items.Add(i);
                }
            }

            return MapItems<TContentItem>(items);
        }

        public IEnumerable<TContentItem> GetReferringItems<TContentItem>(object key) where TContentItem : ContentItem
        {
            Item item = GetItem(key);

            IEnumerable<ItemLink> links = LinkDatabase.GetReferrers(item);

            var referringItems = new List<Item>(links.Count());
            foreach (var link in links)
            {
                Item i = Database.GetItem(link.SourceItemID);

                if (i != null)
                {
                    referringItems.Add(i);
                }
            }

            return MapItems<TContentItem>(referringItems);
        }
        #endregion

        /// <summary>
        /// Gets the Sitecore item given the key or ID.
        /// </summary>
        /// <param name="key">The key, or ID, of the Sitecore item to get.</param>
        /// <returns>
        /// The Sitecore Item
        /// </returns>
        /// <exception cref="System.ArgumentNullException">key</exception>
        /// <exception cref="Sitecore.Exceptions.ItemNotFoundException"></exception>
        private Sitecore.Data.Items.Item GetItem(object key)
        {
            // Make sure we have a key.
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Sitecore.Data.Items.Item item = null;

            // Handle any complex key types here...
            //! * Not implemented for sample code!
            //  - Sitecore.Data.ID
            //  - Sitecore.Data.ShortID
            //  - ItemPointer
            //  - ItemUri
            //  - etc...

            // If we are still here, convert the key to a string.
            string sKey = key.ToString();

            // Check for weird strings that could be used to get an Item..
            //!  * Not implemented in sample code!
            //  - strings in Sitecore.Data.ShortID format
            //  - strings in Sitecore.Data.ItemUri format
            //  - etc...

            // If the key is a string that contains a '/' or starts with sitecore, 
            // but doesn't have a leading slash, then prefix the slash
            if ((sKey.Contains('/') || sKey.StartsWith("sitecore")) && !sKey.StartsWith("/"))
            {
                sKey = string.Concat('/', sKey);
            }

            // Get the item from the Sitecore Database
            item = Database.GetItem(sKey);

            if (item == null)
            {
                throw new ItemNotFoundException(string.Format("Could not find Sitecore item with key {0}", sKey));
            }

            return item;
        }
        
        /// <summary>
        /// Maps the item.
        /// </summary>
        /// <typeparam name="TContentItem">The type of the content item.</typeparam>
        /// <param name="item">The item.</param>
        /// <param name="allowNulls">if set to <c>true</c> [allow nulls].</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        private TContentItem MapItem<TContentItem>(Item item, bool allowNulls = false) where TContentItem : ContentItem
        {
            // Use our convention based mapper to turn 
            // a Sitecore item into a ContentItem
            ContentItem mappedItem = _conventionMapper.Map(item);

            // Ensure that the ContentItem type we want is assignable from the type we actualy got back
            if (!allowNulls && !typeof(TContentItem).IsAssignableFrom(mappedItem.GetType()))
            {
                throw new Exception(string.Format("Cannot map template '{0}' to type '{1}'.", item.TemplateName, mappedItem.GetType().ToString()));
            }

            return mappedItem as TContentItem;
        }

        #region Map multiple items...
        /// <summary>
        /// Maps the items.
        /// </summary>
        /// <typeparam name="TContentItem">The type of the content item.</typeparam>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        private IEnumerable<TContentItem> MapItems<TContentItem>(IEnumerable<Item> items) where TContentItem : ContentItem
        {
            foreach (Item item in items)
            {
                TContentItem mappedItem = MapItem<TContentItem>(item, true);

                if (mappedItem != null)
                {
                    yield return mappedItem;
                }
            }
        } 
        #endregion
    }
}
