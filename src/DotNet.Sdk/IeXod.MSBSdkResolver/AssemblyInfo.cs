// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Resources;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("net.r_eg.IeXod.MSBSdkResolver.Tests, PublicKey=00240000048000009400000006020000002400005253413100040000010001004fa0cecf58927777918aff632fc8cef14b7c0c2dd733ed8b74b8327d2165655dc4fe69ff84cd54456e4007e47ce3d0f9aacdf4c60ee712025f5df0f28d5be4da5b4c231bba90d6f525a4fcad80580653b6aa2ec0c81dce7921a937e48c9b5a2b043eb53103d6b3404942dbd1de73075fe8b681ca5106ef61cb079ec1f3efc9ad")]

#if DEBUG && !RUNTIME_TYPE_NETCORE
    [assembly: NeutralResourcesLanguage("en", UltimateResourceFallbackLocation.Satellite)]
#else
    [assembly: NeutralResourcesLanguage("en")]
#endif
