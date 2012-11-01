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

namespace HedgehogDevelopment.Scaas.Content.Fields
{
    /// <summary>
    /// The MultiReferenceFieldBuilder class
    /// </summary>
    internal class MultiReferenceFieldConverter : FieldConverter<MultiReferenceField>
    {
        /// <summary>
        /// Builds the field.
        /// </summary>
        /// <param name="scfield">The scfield.</param>
        /// <returns></returns>
        public override MultiReferenceField Convert(Sitecore.Data.Fields.Field scfield)
        {
            Sitecore.Data.Fields.MultilistField scField = scfield;
            MultiReferenceField field = null;

            if (scField != null)
            {
                field = new MultiReferenceField();
                field.TargetKeys = new List<string>(scField.TargetIDs.Select(t => t.ToString()));
            }
            else
            {
                field = new MultiReferenceField();
                field.TargetKeys = new string[0];
            }

            if (field.TargetKeys.Count() == 0)
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
