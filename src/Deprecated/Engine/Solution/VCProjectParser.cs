// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace net.r_eg.IeXod.BuildEngine
{
	static internal class VCProjectParser
	{
        /// <summary>
        /// For a given VC project, retrieves the projects it references 
        /// </summary>
        /// <param name="projectPath"></param>
        /// <returns></returns>
        /// <owner>LukaszG</owner>
        static internal List<string> GetReferencedProjectGuids(XmlDocument project)
        {
            List<string> referencedProjectGuids = new List<string>();

            XmlNodeList referenceElements = project.DocumentElement.GetElementsByTagName("References");

            if (referenceElements.Count > 0)
            {
                foreach (XmlElement referenceElement in ((XmlElement)referenceElements[0]).GetElementsByTagName("ProjectReference"))
                {
                    string referencedProjectGuid = referenceElement.GetAttribute("ReferencedProjectIdentifier");

                    if (!string.IsNullOrEmpty(referencedProjectGuid))
                    {
                        referencedProjectGuids.Add(referencedProjectGuid);
                    }
                }
            }

            return referencedProjectGuids;
        }

        /// <summary>
        /// Is the project built as a static library for the given configuration?
        /// </summary>
        internal static bool IsStaticLibrary(XmlDocument project, string configurationName)
        {
            XmlNodeList configurationsElements = project.DocumentElement.GetElementsByTagName("Configurations");
            XmlElement configurationElement = null;

            bool isStaticLibrary = false;

            // There should be only one configurations element
            if (configurationsElements.Count > 0)
            {
                foreach (XmlNode configurationNode in configurationsElements[0].ChildNodes)
                {
                    if (configurationNode.NodeType == XmlNodeType.Element)
                    {
                        XmlElement element = (XmlElement)configurationNode;

                        // Look for configuration that matches our name
                        if ((string.Compare(element.Name, "Configuration", StringComparison.OrdinalIgnoreCase) == 0) &&
                            (string.Compare(element.GetAttribute("Name"), configurationName, StringComparison.OrdinalIgnoreCase) == 0))
                        {
                            configurationElement = element;

                            string configurationType = configurationElement.GetAttribute("ConfigurationType");
                            isStaticLibrary = (configurationType == "4");

                            // we found our configuration, nothing more to do here
                            break;
                        }
                    }
                }
            }

            return isStaticLibrary;
        }
    }
}
