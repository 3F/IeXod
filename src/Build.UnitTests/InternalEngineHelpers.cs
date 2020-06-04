﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using net.r_eg.IeXod.BackEnd;
using net.r_eg.IeXod.BackEnd.SdkResolution;
using net.r_eg.IeXod.Definition;
using net.r_eg.IeXod.Evaluation.Context;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.Framework;
using SdkResolverContext = net.r_eg.IeXod.Framework.SdkResolverContext;
using SdkResult = net.r_eg.IeXod.BackEnd.SdkResolution.SdkResult;
using SdkResultFactory = net.r_eg.IeXod.Framework.SdkResultFactory;

namespace net.r_eg.IeXod.Unittest
{
    internal static class SdkUtilities
    {
        public static ProjectOptions CreateProjectOptionsWithResolver(SdkResolver resolver)
        {
            var context = EvaluationContext.Create(EvaluationContext.SharingPolicy.Isolated);
            var sdkService = (SdkResolverService)context.SdkResolverService;
            sdkService.InitializeForTests(null, new List<SdkResolver>() { resolver });

            return new ProjectOptions
            {
                EvaluationContext = context
            };
        }

        internal class ConfigurableMockSdkResolver : SdkResolver
        {
            private readonly Dictionary<string, SdkResult> _resultMap;
            private readonly Func<SdkReference, SdkResolverContext, SdkResultFactory, Framework.SdkResult> _resolveFunc;

            public ConcurrentDictionary<string, int> ResolvedCalls { get; } = new ConcurrentDictionary<string, int>();

            public ConfigurableMockSdkResolver(SdkResult result)
            {
                _resultMap = new Dictionary<string, SdkResult> { [result.SdkReference.Name] = result };
            }

            public ConfigurableMockSdkResolver(Dictionary<string, SdkResult> resultMap)
            {
                _resultMap = resultMap;
            }

            public ConfigurableMockSdkResolver(Func<SdkReference, SdkResolverContext, SdkResultFactory, Framework.SdkResult> resolveFunc)
            {
                _resolveFunc = resolveFunc;
            }

            public override string Name => nameof(ConfigurableMockSdkResolver);

            public override int Priority => int.MaxValue;

            public override Framework.SdkResult Resolve(SdkReference sdkReference, SdkResolverContext resolverContext, SdkResultFactory factory)
            {
                if (_resolveFunc != null)
                {
                    return _resolveFunc(sdkReference, resolverContext, factory);
                }

                ResolvedCalls.AddOrUpdate(sdkReference.Name, k => 1, (k, c) => c + 1);

                return _resultMap.TryGetValue(sdkReference.Name, out var result)
                    ? new SdkResult(sdkReference, result.Path, result.Version, null)
                    : null;
            }


        }

        internal class FileBasedMockSdkResolver : SdkResolver
        {
            private readonly Dictionary<string, string> _mapping;

            public FileBasedMockSdkResolver(Dictionary<string, string> mapping)
            {
                _mapping = mapping;
            }
            public override string Name => "FileBasedMockSdkResolver";
            public override int Priority => int.MinValue;

            public override Framework.SdkResult Resolve(SdkReference sdkReference, SdkResolverContext resolverContext, SdkResultFactory factory)
            {
                resolverContext.Logger.LogMessage($"{nameof(resolverContext.ProjectFilePath)} = {resolverContext.ProjectFilePath}", MessageImportance.High);
                resolverContext.Logger.LogMessage($"{nameof(resolverContext.SolutionFilePath)} = {resolverContext.SolutionFilePath}", MessageImportance.High);
                resolverContext.Logger.LogMessage($"{nameof(resolverContext.MSBuildVersion)} = {resolverContext.MSBuildVersion}", MessageImportance.High);

                return _mapping.ContainsKey(sdkReference.Name)
                    ? factory.IndicateSuccess(_mapping[sdkReference.Name], null)
                    : factory.IndicateFailure(new[] { $"Not in {nameof(_mapping)}" });
            }
        }
    }
}
