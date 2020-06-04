﻿using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Shared.AssemblyFoldersFromConfig
{
    [DataContract(Name="AssemblyFolder", Namespace = "")]
    [DebuggerDisplay("{Name}: FrameworkVersion = {FrameworkVersion}, Platform = {Platform}, Path= {Path}")]
    internal class AssemblyFolderItem
    {
        [DataMember(IsRequired = false, Order = 1)]
        internal string Name { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        internal string FrameworkVersion { get; set; }

        [DataMember(IsRequired = true, Order = 3)]
        internal string Path { get; set; }

        [DataMember(IsRequired = false, Order = 4)]
        internal string Platform { get; set; }
    }
}