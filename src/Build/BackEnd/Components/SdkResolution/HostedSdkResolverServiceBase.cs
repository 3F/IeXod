// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.BackEnd.Logging;
using net.r_eg.IeXod.Construction;
using net.r_eg.IeXod.Sdk;
using System;
using System.Threading;

namespace net.r_eg.IeXod.BackEnd.SdkResolution
{
    /// <summary>
    /// A base class for "hosted" ISdkResolverService implementations which are registered by an <see cref="IBuildComponentHost"/>.
    /// </summary>
    internal abstract class HostedSdkResolverServiceBase : IBuildComponent, INodePacketHandler, ISdkResolverService
    {
        /// <summary>
        /// An event to signal for waiting threads when the <see cref="IBuildComponent"/> is being shut down.
        /// </summary>
        protected readonly AutoResetEvent ShutdownEvent = new AutoResetEvent(initialState: false);

        /// <summary>
        /// The current <see cref="IBuildComponentHost"/> which is hosting this component.
        /// </summary>
        protected IBuildComponentHost Host;

        /// <inheritdoc cref="ISdkResolverService.SendPacket"/>
        public Action<INodePacket> SendPacket { get; set; }

        /// <inheritdoc cref="ISdkResolverService.ClearCache"/>
        public virtual void ClearCache(int submissionId)
        {
        }

        public virtual void ClearCaches()
        {
        }

        /// <inheritdoc cref="IBuildComponent.InitializeComponent"/>
        public virtual void InitializeComponent(IBuildComponentHost host)
        {
            Host = host;
        }

        /// <inheritdoc cref="INodePacketHandler.PacketReceived"/>
        ///
        public abstract void PacketReceived(int node, INodePacket packet);

        /// <inheritdoc cref="ISdkResolverService.ResolveSdk"/>
        public abstract SdkResult ResolveSdk(int submissionId, SdkReference sdk, LoggingContext loggingContext, ElementLocation sdkReferenceLocation, SdkEnv sdkEnv, bool interactive);

        /// <inheritdoc cref="IBuildComponent.ShutdownComponent"/>
        public virtual void ShutdownComponent()
        {
            ShutdownEvent.Set();
        }
    }
}
