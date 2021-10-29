﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Diagnostics;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Internal;
using net.r_eg.IeXod.Shared;
using System.Security;
#if FEATURE_SECURITY_PERMISSIONS
using System.Security.AccessControl;
#endif
using System.Security.Principal;

using BuildParameters = net.r_eg.IeXod.Execution.BuildParameters;

namespace net.r_eg.IeXod.BackEnd
{
    /// <summary>
    /// This is an implementation of INodeEndpoint for the out-of-proc nodes.  It acts only as a client.
    /// </summary>
    internal class NodeEndpointOutOfProc : NodeEndpointOutOfProcBase
    {
        #region Private Data

        /// <summary>
        /// The build component host
        /// </summary>
        private IBuildComponentHost _componentHost;

        private readonly bool _enableReuse;

        private readonly bool _lowPriority;

        #endregion

        #region Constructors and Factories

        /// <summary>
        /// Instantiates an endpoint to act as a client
        /// </summary>
        /// <param name="pipeName">The name of the pipe to which we should connect.</param>
        /// <param name="host">The component host.</param>
        /// <param name="enableReuse">Whether this node may be reused for a later build.</param>
        internal NodeEndpointOutOfProc(
            string pipeName, 
            IBuildComponentHost host,
            bool enableReuse,
            bool lowPriority)
        {
            ErrorUtilities.VerifyThrowArgumentNull(host, "host");
            _componentHost = host;
            _enableReuse = enableReuse;
            _lowPriority = lowPriority;

            InternalConstruct(pipeName);
        }

        #endregion

        /// <summary>
        /// Returns the host handshake for this node endpoint
        /// </summary>
        protected override long GetHostHandshake()
        {
            return NodeProviderOutOfProc.GetHostHandshake(_enableReuse, _lowPriority);
        }

        /// <summary>
        /// Returns the client handshake for this node endpoint
        /// </summary>
        protected override long GetClientHandshake()
        {
            return NodeProviderOutOfProc.GetClientHandshake();
        }

        #region Structs
        /// <summary>
        /// Used to return a matched pair of endpoints for in-proc nodes to use with the Build Manager.
        /// </summary>
        internal struct EndpointPair
        {
            /// <summary>
            /// The endpoint destined for use by a node.
            /// </summary>
            internal readonly NodeEndpointInProc NodeEndpoint;

            /// <summary>
            /// The endpoint destined for use by the Build Manager
            /// </summary>
            internal readonly NodeEndpointInProc ManagerEndpoint;

            /// <summary>
            /// Creates an endpoint pair
            /// </summary>
            /// <param name="node">The node-side endpoint.</param>
            /// <param name="manager">The manager-side endpoint.</param>
            internal EndpointPair(NodeEndpointInProc node, NodeEndpointInProc manager)
            {
                NodeEndpoint = node;
                ManagerEndpoint = manager;
            }
        }
        #endregion
    }
}
