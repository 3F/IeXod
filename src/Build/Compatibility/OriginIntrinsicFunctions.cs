// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Compatibility
{
    internal static class OriginIntrinsicFunctions
    {
        private const string RTYPE  = "Microsoft.Build.Evaluation.IntrinsicFunctions";
        private const string ASM    = "Microsoft.Build.dll";
        private const string EXE    = "MSBuild.exe";

        private static string _assemblyPath, _instancePath;

        internal static string AssemblyPath
            => _assemblyPath ??= Path.Combine(BuildEnvironmentHelper.Instance.CurrentMSBuildToolsDirectory, ASM);

        internal static string InstancePath
            => _instancePath ??= Path.Combine(BuildEnvironmentHelper.Instance.CurrentMSBuildToolsDirectory, EXE);

        internal static bool TryInvokeStatic(out object result, string name, params object[] args)
        {
            return TryInvokeStaticFrom(AssemblyPath, out result, name, args);
        }

        internal static bool TryUseInstance(out string result, string name, params object[] args)
        {
            return TryUseInstance(InstancePath, out result, name, args);
        }

        private static bool TryInvokeStaticFrom(string dll, out object result, string name, params object[] args)
        {
            MethodInfo mi = GetTypeFrom(dll)?.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static);
            if(mi == null)
            {
                result = null;
                return false;
            }

            ParameterInfo[] types = mi.GetParameters();
            object[] farg = new object[types.Length];

            for(int i = 0; i < farg.Length; ++i)
            {
                farg[i] = ArgTranslator.Translate(args[i], types[i].ParameterType);
            }

            try
            {
                result = mi.Invoke(null, farg);
                return true;
            }
            catch
            {
                // NOTE: Fail also is possible when using unsupported platforms, for example,
                //       IsOSPlatform() was based on RuntimeInformation.IsOSPlatform(OSPlatform)
                //       But OSPlatform struct is not provided by the System.Runtime.dll for .NET Core 2.1 runtime.
                //       Here we just delegate processing for other handlers in any cases(problems).
                result = null;
            }
            return false;
        }

        private static Type GetTypeFrom(string dll) => Use(dll)?.GetType(RTYPE);

        private static Assembly Use(string input)
        {
            try
            {
                return Assembly.LoadFrom(input);
            }
            catch
            {
                // TODO: log it
                return null; // just delegate processing for other handlers
            }
        }

        private static bool TryUseInstance(string fpath, out string result, string name, params object[] args)
        {
            string fargs;
            if(args?.Length > 0)
            {
                fargs = "'" + string.Join("','", args) + "'";
            }
            else
            {
                fargs = string.Empty;
            }

            string tfile = FileUtilities.GetTemporaryFile(".targets");

            using StreamWriter stream = FileUtilities.OpenWrite(tfile, false, Encoding.Unicode);

            stream.Write
            (@"
<Project DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Target Name=""Build"">
    <Message Text=""$([MSBuild]::" + name + $"({fargs})" + @")"" Importance=""High"" />
  </Target>
</Project>"
            );
            stream.Close();


            Process p = new()
            {
                StartInfo = new ProcessStartInfo(fpath, tfile + " /t:Build /v:m /m:1 /nologo")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                }
            };

            p.Start();
            p.WaitForExit();

            if(p.ExitCode != 0)
            {
                result = null;
                return false;
            }

            result = p.StandardOutput.ReadToEnd().Trim(BuildEnvironmentHelper.StreamTrimChars);

            if(result.IndexOf(tfile) != -1) return false; // MSBuild pushes some errors into stdout using its location, ie.: D:\path_to.targets(3,14): error MSB4186: ...
            return true;
        }
    }
}
