﻿#region License
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

using HedgehogDevelopment.Scaas.Content.Fields;
using Sitecore.Data.Items;

namespace HedgehogDevelopment.Scaas.Content.Fields
{
    /// <summary>
    /// The ReferenceFieldBuilder class
    /// </summary>
    internal class ReferenceFieldConverter : FieldConverter<ReferenceField>
    {
        /// <summary>
        /// Builds the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public override ReferenceField Convert(Sitecore.Data.Fields.Field field)
        {
            Sitecore.Data.Fields.ReferenceField referenceField = field;
            ReferenceField targetField = new ReferenceField();

            if (referenceField != null && referenceField.TargetID != Sitecore.Data.ID.Null)
            {
                targetField.TargetKey = referenceField.TargetID.ToString();
            }

            if (targetField.TargetKey == null || string.IsNullOrEmpty(referenceField.Path))
            {
                targetField.HasValue = false;
            }
            else
            {
                targetField.HasValue = true;
            }

            return targetField;
        }
    }
}
