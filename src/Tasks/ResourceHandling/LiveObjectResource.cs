// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Resources;

namespace net.r_eg.IeXod.Tasks.ResourceHandling
{
    /// <summary>
    /// Name value resource pair to go in resources list
    /// </summary>
    internal class LiveObjectResource : IResource
    {
        public LiveObjectResource(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public object Value { get; }

        public string TypeAssemblyQualifiedName => Value.GetType().AssemblyQualifiedName;

        public string TypeFullName => Value.GetType().FullName;

        public void AddTo(IResourceWriter writer)
        {
            writer.AddResource(Name, Value);
        }
    }
}
