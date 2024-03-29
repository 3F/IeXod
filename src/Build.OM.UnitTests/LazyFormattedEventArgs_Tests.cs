// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Xml;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

using net.r_eg.IeXod.Collections;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Evaluation;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.BackEnd;
using net.r_eg.IeXod.Shared;
using System.Text;
using System.IO;
using net.r_eg.IeXod.Internal;
using Xunit;

namespace net.r_eg.IeXod.UnitTests.Framework
{
    /// <summary>
    /// Tests for LazyFormattedEventArgs
    /// </summary>
    public class LazyFormattedEventArgs_Tests
    {
#if FEATURE_CODETASKFACTORY
        /// <summary>
        /// Don't crash when task logs with too few format markers
        /// </summary>
        [Fact]
        public void DoNotCrashOnInvalidFormatExpression()
        {
            string content = @"
 <Project DefaultTargets=`t` ToolsVersion=`msbuilddefaulttoolsversion` xmlns=`http://schemas.microsoft.com/developer/msbuild/2003`>
   <UsingTask
     TaskName=`Crash`
     TaskFactory=`CodeTaskFactory`
     AssemblyFile=`$(IeXodBinPath)" + Path.DirectorySeparatorChar + @"IeXod.Tasks.dll` >
     <Task>
       <Code Type=`Fragment` Language=`cs`>
         this.Log.LogError(`Correct: {0}`, `[goodone]`);
         this.Log.LogError(`This is a message logged from a task {1} blah blah [crashing].`, `[crasher]`);

            try
            {
                this.Log.LogError(`Correct: {0}`, 4224);
                this.Log.LogError(`Malformed: {1}`, 42); // Line 13
                throw new InvalidOperationException();
            }
            catch (Exception e)
            {
                this.Log.LogError(`Catching: {0}`, e.GetType().Name);
            }
            finally
            {
                this.Log.LogError(`Finally`);
            }

            try
            {
                this.Log.LogError(`Correct: {0}`, 4224);
                throw new InvalidOperationException();
            }
            catch (Exception e)
            {
                this.Log.LogError(`Catching: {0}`, e.GetType().Name);
                this.Log.LogError(`Malformed: {1}`, 42); // Line 19
            }
            finally
            {
                this.Log.LogError(`Finally`);
            }

            try
            {
                this.Log.LogError(`Correct: {0}`, 4224);
                throw new InvalidOperationException();
            }
            catch (Exception e)
            {
                this.Log.LogError(`Catching: {0}`, e.GetType().Name);
            }
            finally
            {
                this.Log.LogError(`Finally`);
                this.Log.LogError(`Malformed: {1}`, 42); // Line 24
            }

       </Code>
     </Task>
   </UsingTask>

        <Target Name=`t`>
             <Crash />
        </Target>
</Project>
                ";

            MockLogger log = Helpers.BuildProjectWithNewOMExpectSuccess(content);

            log.AssertLogContains("[goodone]");
            log.AssertLogContains("[crashing]");
        }
#endif
    }
}
