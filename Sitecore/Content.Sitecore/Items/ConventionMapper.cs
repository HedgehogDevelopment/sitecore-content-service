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
using System.ComponentModel;
using System.Linq;
using HedgehogDevelopment.Scaas.Content.Fields;
using Sitecore.Resources.Media;

namespace HedgehogDevelopment.Scaas.Content.Items
{
    internal class ConventionMapper
    {
        /// <summary>
        /// Gets the field service.
        /// </summary>
        protected FieldConverterService FieldConverters { get; private set; }

        public ConventionMapper()
        {
            FieldConverters = new Fields.FieldConverterService();
        }

        /// <summary>
        /// Maps the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public ContentItem Map(Sitecore.Data.Items.Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "Source Sitecore item cannot be null");
            }

            // create content item
            ContentItem contentItem = CreateContentItem(item);

            if (contentItem != null)
            {
                Initialize(item, contentItem);

                AutoMap(item, contentItem);
            }

            return contentItem;
        }

        /// <summary>
        /// Initializes the specified item with basic info.
        /// </summary>
        /// <param name="sitecoreItem">The Sitecore item.</param>
        /// <param name="contentItem">The content item.</param>
        protected virtual void Initialize(Sitecore.Data.Items.Item sitecoreItem, ContentItem contentItem)
        {
            // basic info common to all ContentItem
            contentItem.Key = sitecoreItem.Paths.FullPath; // This could be any unique identifier of a Sitecore item ID, ShortID, ItemUri, etc...
            contentItem.Name = sitecoreItem.Name;
            
            if (sitecoreItem.Paths.IsContentItem)
            {
                contentItem.Path = Sitecore.Links.LinkManager.GetItemUrl(sitecoreItem);
            }
            else if (sitecoreItem.Paths.IsMediaItem)
            {
                MediaUrlOptions options = new MediaUrlOptions();
                options.UseItemPath = false;
                contentItem.Path = Sitecore.Resources.Media.MediaManager.GetMediaUrl(sitecoreItem, options);
            }
            else // just to be safe, pass something out!
            {
                contentItem.Path = sitecoreItem.Paths.Path;
            }
        }
       
        /// <summary>
        /// Automatically maps fields on the Sitecore item to properties on the ContentItem
        /// </summary>
        /// <param name="sitecoreItem">The source.</param>
        /// <param name="contentItem">The target.</param>
        private void AutoMap(Sitecore.Data.Items.Item sitecoreItem, ContentItem contentItem)
        {
            if (sitecoreItem == null)
            {
                throw new ArgumentNullException("source", "Source Sitecore item cannot be null");
            }

            if (contentItem == null)
            {
                throw new ArgumentNullException("target", "Target ContentItem cannot be null");
            }

            // get the properties of the content item model
            PropertyDescriptorCollection contentItemProperties = GetProperties(contentItem);

            // get the real list of fields on an item
            IEnumerable<Sitecore.Data.Fields.Field> sitecoreItemFields = GetAllFields(sitecoreItem);

            // iterate over each Sitecore field
            foreach (Sitecore.Data.Fields.Field sitecoreField in sitecoreItemFields)
            {
                // see if we have a model property that matches a field name
                var targetProperty = contentItemProperties.Find(sitecoreField.Name, false);
                if (targetProperty == null)
                {
                    continue;
                }

                // target property type
                Type propertyType = targetProperty.PropertyType;

                // use our field service to convert the field to the property
                object propertyValue = FieldConverters.Convert(propertyType, sitecoreField);

                // set the value of the target content item
                targetProperty.SetValue(contentItem, propertyValue);
            }
        }

        /// <summary>
        /// Creates the content item mapped to the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private static ContentItem CreateContentItem(Sitecore.Data.Items.Item item)
        {
            ContentItem c = null;
            
            // Get the CLR object type we expect based on the item
            string expectedModelTypeName = GetExpectedClrTypeForItem(item);

            // Try to get the Type from the string representation
            Type t = Mappings.GetContentItemType(expectedModelTypeName);

            // if we have a specific ContentItem, then create that
            if (t != null)
            {
                c = Activator.CreateInstance(t) as ContentItem;
            }

            // return our custom type or a basic ContentItem if we don't have a custom type
            return c ?? new ContentItem();
        }

        /// <summary>
        /// Gets the expected CLR type for item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private static string GetExpectedClrTypeForItem(Sitecore.Data.Items.Item item)
        {
            // Convention based mapping dictates that the Content Item model should be named as:
            //  Sitecore Template: /sitecore/templates/HedgehogDevelopment/Scaas/Models/Article
            //  Class Type:                            HedgehogDevelopment.Scass.Models.ArticleItem

            // This convention is used with TDS and Code Generation. 
            // Look at the /templates/HedgehogDevelopment item in the TDS project for configuration

            // lets try to convert the path to a namespace
            // use template full name (Folder1/Folder2/Folder3/Template)
            string templateName = item.Template.FullName;
            string expectedNamespace = templateName.Replace('/', '.');
            expectedNamespace = expectedNamespace.Trim(".".ToCharArray());

            // append the 'Item' to the end of the template.
            // The ContentItem.tt file is reponsible for generating class names like this
            string expectedModelTypeName = string.Concat(expectedNamespace, "Item");
            return expectedModelTypeName;
        }

        private PropertyDescriptorCollection GetProperties(ContentItem contentItem)
        {
            return TypeDescriptor.GetProperties(contentItem.GetType());
        }

        private static IEnumerable<Sitecore.Data.Fields.Field> GetAllFields(Sitecore.Data.Items.Item item)
        {
            // get all non system fields from the templace definition since if there 
            // is no value then the field won't exist in item.Fields
            IEnumerable<Sitecore.Data.Items.TemplateFieldItem> allFields = item.Template.Fields.Where(f => !f.Name.StartsWith("__"));

            foreach (Sitecore.Data.Items.TemplateFieldItem field in allFields)
            {
                yield return item.Fields[field.ID];
            }
        }

    }
}
