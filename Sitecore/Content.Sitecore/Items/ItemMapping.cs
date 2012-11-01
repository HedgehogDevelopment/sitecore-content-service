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
using System.IO;
using System.Reflection;

namespace HedgehogDevelopment.Scaas.Content.Items
{
    /// <summary>
    /// A cached copy of a model's TypeName to Type
    /// </summary>
    internal static class Mappings
    {
        private static string _binDirectory;
        private static Dictionary<string, Type> ItemModels;

        static Mappings()
        {
            ItemModels = new Dictionary<string, Type>();

            // Get the bin directory of the web application
            _binDirectory = AppDomain.CurrentDomain.RelativeSearchPath;

            // load each DLL and cache the types
            foreach (string assemblyName in GetWatchedAssemblies())
            {
                Assembly assembly = null;

                // There should be a very good logging mechanism here...
                // It is always hard to figure out why the "magic handshake" isn't working.

                try
                {
                    assembly = GetAssembly(assemblyName);
                }
                catch (FileNotFoundException) // assembly not found... eat it for now.
                {
                    
                }

                if (assembly != null)
                {
                    MapAssembly(assembly);
                }
            }
        }

        /// <summary>
        /// Gets the type of the content item.
        /// </summary>
        /// <param name="modelName">Name of the model.</param>
        /// <returns></returns>
        internal static Type GetContentItemType(string modelName)
        {
            if (ItemModels.ContainsKey(modelName))
            {
                return ItemModels[modelName];
            }
            return null;
        }

        /// <summary>
        /// Gets the watched assemblies from the configuration.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<string> GetWatchedAssemblies()
        {
            /*
             * For a better approach, move these assembly names to some configurable area.
             */
            yield return "HedgehogDevelopment.Scaas.Models.dll";
        }

        /// <summary>
        /// Loads the named assembly from the bin directory.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static Assembly GetAssembly(string name)
        {
            return Assembly.LoadFrom(Path.Combine(_binDirectory, name));
        }

        /// <summary>
        /// Maps the Content Items within the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        private static void MapAssembly(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(ContentItem).IsAssignableFrom(type) &&
                    type.IsAbstract == false &&
                    type.IsGenericTypeDefinition == false &&
                    type.IsInterface == false &&
                    type.Name.EndsWith("Item"))
                {
                    // Add the model mapping
                    ItemModels.Add(type.FullName, type);
                }
            }
        }
    }
}
