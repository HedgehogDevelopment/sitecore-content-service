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
using HedgehogDevelopment.Scaas.Content.Fields;

namespace HedgehogDevelopment.Scaas.Content.Fields
{
    /// <summary>
    /// The FieldConverterService class
    /// </summary>
    internal class FieldConverterService
    {
        private Dictionary<Type, IFieldConverter> _converters;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldConverterService"/> class.
        /// </summary>
        public FieldConverterService()
        {
            _converters = new Dictionary<Type, IFieldConverter>()
            {
                // known converters
                { typeof(string), new StringFieldConverter()},
                { typeof(bool), new BoolFieldConverter()},
                { typeof(float), new FloatFieldConverter()},
                { typeof(int), new IntFieldConverter()},
                { typeof(DateTime), new DateTimeFieldConverter()},
                { typeof(ImageField), new ImageFieldConverter()},
                { typeof(LinkField), new LinkFieldConverter()},
                { typeof(ReferenceField), new ReferenceFieldConverter()},
                { typeof(MultiReferenceField), new MultiReferenceFieldConverter()},
            };
        }

        /// <summary>
        /// Converts the specified field to the Type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public object Convert(Type type, Sitecore.Data.Fields.Field field)
        {
            var converter = GetConverter(type);
            return converter.Convert(field);
        }

        /// <summary>
        /// Gets the converter for the Type.
        /// </summary>
        /// <param name="fieldType">Type of the field.</param>
        /// <returns></returns>
        private IFieldConverter GetConverter(Type fieldType)
        {
            if (!_converters.ContainsKey(fieldType))
            {
                throw new ApplicationException(string.Format("Converter for '{0}' type not found.", fieldType));
            }
            return (IFieldConverter)_converters[fieldType];
        }
    }
}
