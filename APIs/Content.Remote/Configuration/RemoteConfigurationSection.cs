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
using System.Configuration;

namespace HedgehogDevelopment.Scaas.Content.Remote.Configuration
{
    public class RemoteConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public static RemoteConfigurationSection Create()
        {
            return ConfigurationManager.GetSection("hedgehogdevelopment.scaas.content.remote") as RemoteConfigurationSection;
        }

        /// <summary>
        /// Gets or sets the xmlns.
        /// </summary>
        /// <value>The xmlns.</value>
        [ConfigurationProperty("xmlns")]
        private string Xmlns
        {
            get { return (string)base["xmlns"]; }
            set { base["xmlns"] = value; }
        }

        /// <summary>
        /// Gets or sets the base URI.
        /// </summary>
        /// <value>
        /// The base URI.
        /// </value>
        [ConfigurationProperty("baseUri", IsRequired = true)]
        public string BaseUri
        {
            get { return (string)base["baseUri"]; }
            set { base["baseUri"] = value;}
        }

        /// <summary>
        /// Gets or sets the service route prefix.
        /// </summary>
        /// <value>
        /// The service route prefix.
        /// </value>
        [ConfigurationProperty("serviceRoutePrefix", IsRequired = true)]
        public string ServiceRoutePrefix
        {
            get { return (string)base["serviceRoutePrefix"]; }
            set { base["serviceRoutePrefix"] = value; }
        }

        /// <summary>
        /// Gets the service endpoint.
        /// </summary>
        public Uri ServiceEndpoint
        {
            get
            {
                Uri baseUri = new Uri(BaseUri);
                Uri uri = new Uri(baseUri, ServiceRoutePrefix);
                return uri;
            }
        }

    }
}
