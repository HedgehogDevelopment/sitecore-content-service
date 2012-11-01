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


namespace HedgehogDevelopment.Scaas.Content.Fields
{
    /// <summary>
    /// The LinkFieldConverter class
    /// </summary>
    internal class LinkFieldConverter : FieldConverter<LinkField>
    {
        /// <summary>
        /// Builds the field.
        /// </summary>
        /// <param name="scfield">The scfield.</param>
        /// <returns></returns>
        public override LinkField Convert(Sitecore.Data.Fields.Field scfield)
        {
            Sitecore.Data.Fields.LinkField linkField = scfield;
            LinkField field = new LinkField();
            field.Path = string.Empty;

            if (linkField != null)
            {
                field.Title = linkField.Text;

                if (linkField.IsInternal && linkField.TargetItem != null)
                {
                    field.LinkFieldType = LinkFieldType.Internal;
                    field.Path = linkField.TargetItem.Paths.FullPath;

                    if (string.IsNullOrEmpty(field.Title))
                    {
                        field.Title = linkField.TargetItem.Name;
                    }
                }
                else if (linkField.IsMediaLink && linkField.TargetItem != null)
                {
                    field.LinkFieldType = LinkFieldType.Media;
                    field.Path = linkField.TargetItem.Paths.Path;

                    if (string.IsNullOrEmpty(field.Title))
                    {
                        field.Title = linkField.TargetItem.Name;
                    }
                }
                else
                {
                    field.LinkFieldType = LinkFieldType.External;
                    field.Path = linkField.Url;
                }

                if (string.IsNullOrEmpty(field.Title))
                {
                    field.Title = scfield.Item.Name;
                }
            }

            if (string.IsNullOrEmpty(field.Path))
            {
                field.HasValue = false;
            }
            else
            {
                field.HasValue = true;
            }

            return field;
        }
    }
}
