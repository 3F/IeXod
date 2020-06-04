// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Shared;
using System.Resources;
using System.Resources.Extensions;

namespace net.r_eg.IeXod.Tasks.ResourceHandling
{
    internal class TypeConverterStringResource : IResource
    {
        public string Name { get; }
        public string TypeAssemblyQualifiedName { get; }
        public string OriginatingFile { get; }
        public string StringRepresentation { get; }

        public string TypeFullName => NameUtilities.FullNameFromAssemblyQualifiedName(TypeAssemblyQualifiedName);

        public TypeConverterStringResource(string name, string assemblyQualifiedTypeName, string stringRepresentation, string originatingFile)
        {
            Name = name;
            TypeAssemblyQualifiedName = assemblyQualifiedTypeName;
            StringRepresentation = stringRepresentation;
            OriginatingFile = originatingFile;
        }

        public void AddTo(IResourceWriter writer)
        {
            if (writer is PreserializedResourceWriter preserializedResourceWriter)
            {
                preserializedResourceWriter.AddResource(Name, StringRepresentation, TypeAssemblyQualifiedName);
            }
            else
            {
                throw new PreserializedResourceWriterRequiredException();
            }
        }
    }
}
