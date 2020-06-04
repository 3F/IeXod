// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace net.r_eg.IeXod.UnitTests
{
    public class MockFaultInjectionHelper<FailurePointEnum>
        where FailurePointEnum : IComparable
    {
        private FailurePointEnum _failureToInject;
        private Exception _exceptionToThrow;

        public MockFaultInjectionHelper()
        {
        }

        public void InjectFailure(FailurePointEnum failureToInject, Exception exceptionToThrow)
        {
            _failureToInject = failureToInject;
            _exceptionToThrow = exceptionToThrow;
        }

        public void FailurePointThrow(FailurePointEnum failurePointId)
        {
            if (_failureToInject.CompareTo(failurePointId) == 0)
            {
                throw _exceptionToThrow;
            }
        }
    }
}
