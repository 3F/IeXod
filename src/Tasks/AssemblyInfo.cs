// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

// This is the assembly-level GUID, and the GUID for the TypeLib associated with
// this assembly.  We should specify this explicitly, as opposed to letting 
// tlbexp just pick whatever it wants.  
[assembly: GuidAttribute("E3D4D3B9-944C-407b-A82E-B19719EA7FB3")]

[assembly: InternalsVisibleTo("net.r_eg.IeXod.Tasks.UnitTests, PublicKey=00240000048000009400000006020000002400005253413100040000010001004fa0cecf58927777918aff632fc8cef14b7c0c2dd733ed8b74b8327d2165655dc4fe69ff84cd54456e4007e47ce3d0f9aacdf4c60ee712025f5df0f28d5be4da5b4c231bba90d6f525a4fcad80580653b6aa2ec0c81dce7921a937e48c9b5a2b043eb53103d6b3404942dbd1de73075fe8b681ca5106ef61cb079ec1f3efc9ad")]
[assembly: InternalsVisibleTo("net.r_eg.IeXod.Tasks.Whidbey.Unittest, PublicKey=00240000048000009400000006020000002400005253413100040000010001004fa0cecf58927777918aff632fc8cef14b7c0c2dd733ed8b74b8327d2165655dc4fe69ff84cd54456e4007e47ce3d0f9aacdf4c60ee712025f5df0f28d5be4da5b4c231bba90d6f525a4fcad80580653b6aa2ec0c81dce7921a937e48c9b5a2b043eb53103d6b3404942dbd1de73075fe8b681ca5106ef61cb079ec1f3efc9ad")]
[assembly: InternalsVisibleTo("net.r_eg.IeXod.Utilities.UnitTests, PublicKey=00240000048000009400000006020000002400005253413100040000010001004fa0cecf58927777918aff632fc8cef14b7c0c2dd733ed8b74b8327d2165655dc4fe69ff84cd54456e4007e47ce3d0f9aacdf4c60ee712025f5df0f28d5be4da5b4c231bba90d6f525a4fcad80580653b6aa2ec0c81dce7921a937e48c9b5a2b043eb53103d6b3404942dbd1de73075fe8b681ca5106ef61cb079ec1f3efc9ad")]

// This will enable passing the SafeDirectories flag to any P/Invoke calls/implementations within the assembly, 
// so that we don't run into known security issues with loading libraries from unsafe locations 
[assembly: DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]

// Needed for the "hub-and-spoke model to locate and retrieve localized resources": https://msdn.microsoft.com/en-us/library/21a15yht(v=vs.110).aspx
// We want "en" to require a satellite assembly for debug builds in order to flush out localization
// issues, but we want release builds to work without it. Also, .net core does not have resource fallbacks
#if (DEBUG && !RUNTIME_TYPE_NETCORE)
[assembly: NeutralResourcesLanguage("en", UltimateResourceFallbackLocation.Satellite)]
#else
[assembly: NeutralResourcesLanguage("en")]
#endif

[assembly: ComVisible(false)]

[assembly: CLSCompliant(true)]
