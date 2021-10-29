using net.r_eg.IeXod.BackEnd;
using net.r_eg.IeXod.BackEnd.Logging;
using net.r_eg.IeXod.BackEnd.SdkResolution;
using net.r_eg.IeXod.Construction;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Sdk;
using System;

namespace net.r_eg.IeXod.Engine.UnitTests.BackEnd
{
    internal class MockSdkResolverService : IBuildComponent, ISdkResolverService
    {
        public Action<INodePacket> SendPacket { get; }

        public void ClearCache(int submissionId)
        {
        }

        public void ClearCaches()
        {
        }

        public IeXod.BackEnd.SdkResolution.SdkResult ResolveSdk(int submissionId, SdkReference sdk, LoggingContext loggingContext, ElementLocation sdkReferenceLocation, SdkEnv sdkEnv, bool interactive)
        {
            return null;
        }

        public void InitializeComponent(IBuildComponentHost host)
        {
        }

        public void ShutdownComponent()
        {
        }
    }
}
