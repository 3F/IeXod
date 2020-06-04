﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;
using System;

namespace net.r_eg.IeXod.Framework
{
    /// <summary> 
    /// Base class for build status events.  This class is meant
    /// to be extended.  
    /// </summary> 
    /// <remarks> 
    /// WARNING: marking a type [Serializable] without implementing 
    /// ISerializable imposes a serialization contract -- it is a 
    /// promise to never change the type's fields i.e. the type is 
    /// immutable; adding new fields in the next version of the type 
    /// without following certain special FX guidelines, can break both 
    /// forward and backward compatibility 
    /// </remarks> 
    [Serializable]
    public abstract class BuildStatusEventArgs : LazyFormattedBuildEventArgs
    {
        /// <summary> 
        /// Default constructor 
        /// </summary> 
        protected BuildStatusEventArgs()
            : base()
        {
            // do nothing 
        }

        /// <summary> 
        /// This constructor allows event data to be initialized.  
        /// </summary> 
        /// <param name="message">text message</param> 
        /// <param name="helpKeyword">help keyword </param> 
        /// <param name="senderName">name of event sender</param> 
        protected BuildStatusEventArgs
        (
            string message,
            string helpKeyword,
            string senderName
        )
            : this(message, helpKeyword, senderName, DateTime.UtcNow)
        {
            // do nothing 
        }


        /// <summary> 
        /// This constructor allows timestamp to be set
        /// </summary> 
        /// <param name="message">text message</param> 
        /// <param name="helpKeyword">help keyword </param> 
        /// <param name="senderName">name of event sender</param> 
        /// <param name="eventTimestamp">Timestamp when event was created</param>
        protected BuildStatusEventArgs
        (
            string message,
            string helpKeyword,
            string senderName,
            DateTime eventTimestamp
        )
            : this(message, helpKeyword, senderName, eventTimestamp, null)
        {
            // do nothing
        }

        /// <summary> 
        /// This constructor allows timestamp to be set
        /// </summary> 
        /// <param name="message">text message</param> 
        /// <param name="helpKeyword">help keyword </param> 
        /// <param name="senderName">name of event sender</param> 
        /// <param name="eventTimestamp">Timestamp when event was created</param>
        /// <param name="messageArgs">Optional arguments for formatting the message string.</param>
        protected BuildStatusEventArgs
        (
            string message,
            string helpKeyword,
            string senderName,
            DateTime eventTimestamp,
            params object[] messageArgs
        )
            : base(message, helpKeyword, senderName, eventTimestamp, messageArgs)
        {
            // do nothing 
        }
    }
}

