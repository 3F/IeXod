// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Framework;

namespace net.r_eg.IeXod.UnitTests.BackEnd
{
    internal class MockRemoteHostObject : ITaskHost, ITestRemoteHostObject
    {
        private int _state;

        public MockRemoteHostObject(int state)
        {
            _state = state;
        }

        public int GetState()
        {
            return _state;
        }
    }

    internal interface ITestRemoteHostObject
    {
        int GetState();
    }
}
