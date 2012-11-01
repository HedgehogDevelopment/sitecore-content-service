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

using Sitecore.Data.Items;

namespace HedgehogDevelopment.Scaas.Content.Fields
{
    /// <summary>
    /// The ImageFieldConverter class
    /// </summary>
    internal class ImageFieldConverter : FieldConverter<ImageField>
    {
        /// <summary>
        /// Builds the field.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        public override ImageField Convert(Sitecore.Data.Fields.Field scfield)
        {
            ImageField field = null;
            if (scfield != null)
            {
                var imageField = (Sitecore.Data.Fields.ImageField)scfield;
                var mediaItem = (Sitecore.Data.Items.MediaItem)imageField.MediaItem;

                field = BuildImageField(mediaItem);

                if (field.HasValue)
                {
                    // field specific values override the values on the media item
                    field.AlternateText = imageField.Alt;
                    field.Height = Parse(imageField.Height);
                    field.Width = Parse(imageField.Width);
                }
                else // if we don't have an item, then use the raw path
                {
                    field.Path = imageField.Value;
                }
            }
            return field;
        }

        internal static ImageField BuildImageField(MediaItem mediaItem)
        {
            if (mediaItem != null)
            {
                ImageField image = new ImageField();
                image.HasValue = true;
                image.Key = mediaItem.InnerItem.Paths.FullPath;
                image.Path = Sitecore.Resources.Media.MediaManager.GetMediaUrl(mediaItem, new Sitecore.Resources.Media.MediaUrlOptions() { AbsolutePath = false });
                image.AlternateText = mediaItem.InnerItem["Alt"];
                image.Width = Parse(mediaItem.InnerItem["Width"]);
                image.Height = Parse(mediaItem.InnerItem["Height"]);
                image.Title = mediaItem.Title;
                image.Extension = mediaItem.Extension;
                image.FileSize = mediaItem.Size;
                return image;
            }
            return new ImageField
            {
                HasValue = false
            };
        }

        /// <summary>
        /// Parses the specified string to an int.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        private static int? Parse(string s)
        {
            int i = 0;
            if (int.TryParse(s, out i))
                return i;
            else
                return null;
        }
    }
}
