﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace net.r_eg.IeXod.Tasks.ResourceHandling
{
    internal class MSBuildResXReader
    {
        public static IReadOnlyList<IResource> ReadResources(Stream s, string filename, bool pathsRelativeToBasePath)
        {
            var resources = new List<IResource>();
            var aliases = new Dictionary<string, string>();

            try
            {
                using (var xmlReader = new XmlTextReader(s))
                {
                    xmlReader.WhitespaceHandling = WhitespaceHandling.None;

                    XDocument doc = XDocument.Load(xmlReader, LoadOptions.PreserveWhitespace);
                    foreach (XElement elem in doc.Element("root").Elements())
                    {
                        switch (elem.Name.LocalName)
                        {
                            case "assembly":
                                ParseAssemblyAlias(aliases, elem);
                                break;
                            case "resheader":
                                break;
                            case "data":
                                ParseData(filename, pathsRelativeToBasePath, resources, aliases, elem);
                                break;
                        }
                    }
                }

                return resources;
            }
            catch (Exception e)
            {
                throw new MSBuildResXException("Error reading resx", e);
            }
        }

        private static void ParseAssemblyAlias(Dictionary<string,string> aliases, XElement elem)
        {
            string alias = elem.Attribute("alias")?.Value;
            string name = elem.Attribute("name").Value;

            if (string.IsNullOrEmpty(alias))
            {
                AssemblyName assemblyName = new AssemblyName(name);

                alias = assemblyName.Name;
            }

            // Match original last-alias-definition-wins behavior
            // https://github.com/dotnet/winforms/blob/33b9fe202f3dc1b8e7c4bf28492f8bd3252f1a20/src/System.Windows.Forms/src/System/Resources/ResXResourceReader.cs#L732-L738
            aliases[alias] = name;
        }

        // Consts from https://github.com/dotnet/winforms/blob/16b192389b377c647ab3d280130781ab1a9d3385/src/System.Windows.Forms/src/System/Resources/ResXResourceWriter.cs#L46-L63
        private const string Beta2CompatSerializedObjectMimeType = "text/microsoft-urt/psuedoml-serialized/base64";
        private const string CompatBinSerializedObjectMimeType = "text/microsoft-urt/binary-serialized/base64";
        private const string CompatSoapSerializedObjectMimeType = "text/microsoft-urt/soap-serialized/base64";
        private const string BinSerializedObjectMimeType = "application/x-microsoft.net.object.binary.base64";
        private const string SoapSerializedObjectMimeType = "application/x-microsoft.net.object.soap.base64";
        private const string DefaultSerializedObjectMimeType = BinSerializedObjectMimeType;
        private const string ByteArraySerializedObjectMimeType = "application/x-microsoft.net.object.bytearray.base64";
        private const string ResMimeType = "text/microsoft-resx";

        private const string StringTypeNamePrefix = "System.String, mscorlib,";
        private const string StringTypeName40 = "System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
        private const string MemoryStreamTypeNamePrefix = "System.IO.MemoryStream, mscorlib,";

        private static string GetFullTypeNameFromAlias(string aliasedTypeName, Dictionary<string, string> aliases)
        {
            if (aliasedTypeName == null)
            {
                return StringTypeName40;
            }

            int indexStart = aliasedTypeName.IndexOf(',');
            if (aliases.TryGetValue(aliasedTypeName.Substring(indexStart + 2), out string fullAssemblyIdentity))
            {
                return aliasedTypeName.Substring(0, indexStart + 2) + fullAssemblyIdentity;
            }

            // Allow "System.String" bare
            if (aliasedTypeName.Equals("System.String", StringComparison.Ordinal))
            {
                return StringTypeName40;
            }

            // No alias found. Hope it's sufficiently complete to be resolved at runtime
            return aliasedTypeName;
        }

        private static void ParseData(string resxFilename, bool pathsRelativeToBasePath, List<IResource> resources, Dictionary<string,string> aliases, XElement elem)
        {
            string name = elem.Attribute("name").Value;
            string value;

            XElement valueElement = elem.Element("value");
            if (valueElement is null)
            {
                if (elem.HasElements)
                {
                    throw new NotImplementedException("User-facing error for bad resx that has child elements but not `value`");
                }

                value = elem.Value;
            }
            else
            {
                value = valueElement.Value;
            }

            string typename = elem.Attribute("type")?.Value;
            string mimetype = elem.Attribute("mimetype")?.Value;

            typename = GetFullTypeNameFromAlias(typename, aliases);

            if (IsString(typename))
            {
                if (mimetype == null)
                {
                    // If nothing is specified, or String is explicitly specified
                    // with no mimetype: read the string from the resx and return it.
                    resources.Add(new StringResource(name, value, resxFilename));
                    return;
                }

                // It's a string, but it might be represented oddly.
                // Fall through to see if one of the serializers can handle it.
            }

            if (typename.StartsWith("System.Resources.ResXFileRef", StringComparison.Ordinal)) // TODO: is this too general? Should it be OrdinalIgnoreCase?
            {
                AddLinkedResource(resxFilename, pathsRelativeToBasePath, resources, name, value);
                return;
            }

            if (typename.StartsWith("System.Resources.ResXNullRef", StringComparison.Ordinal))
            {
                resources.Add(new LiveObjectResource(name, null));
                return;
            }

            // TODO: validate typename at this point somehow to make sure it's vaguely right?

            if (mimetype == null)
            {
                if (IsByteArray(typename))
                {
                    // Handle byte[]'s, which are stored as base-64 encoded strings.
                    byte[] byteArray = Convert.FromBase64String(value);

                    resources.Add(new LiveObjectResource(name, byteArray));
                    return;
                }

                resources.Add(new TypeConverterStringResource(name, typename, value, resxFilename));
                return;
            }
            else
            {
                switch (mimetype)
                {
                    case ByteArraySerializedObjectMimeType:
                        // TypeConverter from byte array
                        byte[] typeConverterBytes = Convert.FromBase64String(value);

                        resources.Add(new TypeConverterByteArrayResource(name, typename, typeConverterBytes, resxFilename));
                        return;
                    case BinSerializedObjectMimeType:
                    case Beta2CompatSerializedObjectMimeType:
                    case CompatBinSerializedObjectMimeType:
                        // BinaryFormatter from byte array
                        byte[] binaryFormatterBytes = Convert.FromBase64String(value);

                        resources.Add(new BinaryFormatterByteArrayResource(name, binaryFormatterBytes, resxFilename));
                        return;
                    default:
                        throw new NotSupportedException($"Resource \"{name}\" in \"{resxFilename}\"uses MIME type \"{mimetype}\", which is not supported by .NET Core MSBuild.");
                }
            }
        }

        private static void AddLinkedResource(string resxFilename, bool pathsRelativeToBasePath, List<IResource> resources, string name, string value)
        {
            string[] fileRefInfo = ParseResxFileRefString(value);

            string fileName = FileUtilities.FixFilePath(fileRefInfo[0]);
            string fileRefType = fileRefInfo[1];

            if (pathsRelativeToBasePath)
            {
                fileName = Path.Combine(
                    FileUtilities.GetDirectory(
                        FileUtilities.NormalizePath(resxFilename)),
                    fileName);
            }

            if (IsString(fileRefType))
            {
                string fileRefEncoding = null;
                if (fileRefInfo.Length == 3)
                {
                    fileRefEncoding = fileRefInfo[2];

#if RUNTIME_TYPE_NETCORE
                    // Ensure that all Windows codepages are available.
                    // Safe to call multiple times per https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding.registerprovider
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
                }

                // from https://github.com/dotnet/winforms/blob/a88c1a73fd7298b0a5c45251771f439262016826/src/System.Windows.Forms/src/System/Resources/ResXFileRef.cs#L231-L241
                Encoding textFileEncoding = fileRefEncoding != null
                    ? Encoding.GetEncoding(fileRefEncoding)
                    : Encoding.Default;
                using (StreamReader sr = new StreamReader(fileName, textFileEncoding))
                {
                    resources.Add(new StringResource(name, sr.ReadToEnd(), resxFilename));

                    return;
                }
            }
            else if (IsByteArray(fileRefType))
            {
                byte[] byteArray = File.ReadAllBytes(fileName);

                resources.Add(new LiveObjectResource(name, byteArray));
                return;
            }
            else if (IsMemoryStream(fileRefType))
            {
                // See special-case handling in ResXFileRef
                // https://github.com/dotnet/winforms/blob/689cd9c69e632997bc85bf421af221d79b12ddd4/src/System.Windows.Forms/src/System/Resources/ResXFileRef.cs#L293-L297
                byte[] byteArray = File.ReadAllBytes(fileName);

                resources.Add(new LiveObjectResource(name, new MemoryStream(byteArray)));
                return;
            }

            resources.Add(new FileStreamResource(name, fileRefType, fileName, resxFilename));
        }

        /// <summary>
        /// Does this assembly-qualified type name represent an array of bytes?
        /// </summary>
        /// <remarks>
        /// We can't hard-code byte[] type name due to version number
        /// updates & potential whitespace issues with ResX files.
        ///
        /// Comment and logic from https://github.com/dotnet/winforms/blob/16b192389b377c647ab3d280130781ab1a9d3385/src/System.Windows.Forms/src/System/Resources/ResXDataNode.cs#L411-L416
        /// </remarks>
        private static bool IsByteArray(string fileRefType)
        {
            return fileRefType.IndexOf("System.Byte[]") != -1 && fileRefType.IndexOf("mscorlib") != -1;
        }

        internal static bool IsString(string fileRefType)
        {
            return fileRefType.StartsWith(StringTypeNamePrefix, StringComparison.Ordinal);
        }

        internal static bool IsMemoryStream(string fileRefType)
        {
            return fileRefType.StartsWith(MemoryStreamTypeNamePrefix, StringComparison.Ordinal);
        }

        /// <summary>
        /// Extract <see cref="IResource"/>s from a given file on disk.
        /// </summary>
        public static IReadOnlyList<IResource> GetResourcesFromFile(string filename, bool pathsRelativeToBasePath)
        {
            using (var x = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return ReadResources(x, filename, pathsRelativeToBasePath);
            }
        }

        public static IReadOnlyList<IResource> GetResourcesFromString(string resxContent, string basePath = null, bool? useRelativePath = null)
        {
            using (var x = new MemoryStream(Encoding.UTF8.GetBytes(resxContent)))
            {
                return ReadResources(x, basePath, useRelativePath.GetValueOrDefault(basePath != null));
            }
        }

        // From https://github.com/dotnet/winforms/blob/a88c1a73fd7298b0a5c45251771f439262016826/src/System.Windows.Forms/src/System/Resources/ResXFileRef.cs#L187-L220
        internal static string[] ParseResxFileRefString(string stringValue)
        {
            string[] result = null;
            if (stringValue != null)
            {
                stringValue = stringValue.Trim();
                string fileName;
                string remainingString;
                if (stringValue.StartsWith("\""))
                {
                    int lastIndexOfQuote = stringValue.LastIndexOf("\"");
                    if (lastIndexOfQuote - 1 < 0)
                        throw new ArgumentException(nameof(stringValue));
                    fileName = stringValue.Substring(1, lastIndexOfQuote - 1); // remove the quotes in" ..... "
                    if (lastIndexOfQuote + 2 > stringValue.Length)
                        throw new ArgumentException(nameof(stringValue));
                    remainingString = stringValue.Substring(lastIndexOfQuote + 2);
                }
                else
                {
                    int nextSemiColumn = stringValue.IndexOf(";");
                    if (nextSemiColumn == -1)
                        throw new ArgumentException(nameof(stringValue));
                    fileName = stringValue.Substring(0, nextSemiColumn);
                    if (nextSemiColumn + 1 > stringValue.Length)
                        throw new ArgumentException(nameof(stringValue));
                    remainingString = stringValue.Substring(nextSemiColumn + 1);
                }
                string[] parts = remainingString.Split(';');
                if (parts.Length > 1)
                {
                    result = new string[] { fileName, parts[0], parts[1] };
                }
                else if (parts.Length > 0)
                {
                    result = new string[] { fileName, parts[0] };
                }
                else
                {
                    result = new string[] { fileName };
                }
            }
            return result;
        }

    }
}
