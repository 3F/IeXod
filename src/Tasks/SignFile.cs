﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Tasks.Deployment.ManifestUtilities;
using net.r_eg.IeXod.Utilities;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// This task signs the passed in file using the Authenticode cert
    /// provided and optionally uses a timestamp if a URL is provided.
    /// It can sign ClickOnce manifests as well as exe's.
    /// </summary>
    public sealed class SignFile : Task
    {
        public SignFile()
            : base(AssemblyResources.PrimaryResources, "MSBuild.")
        {
        }

        [Required]
        public string CertificateThumbprint { get; set; }

        [Required]
        public ITaskItem SigningTarget { get; set; }

        public String TargetFrameworkVersion { get; set; }

        public string TimestampUrl { get; set; }

        public override bool Execute()
        {
            try
            {
                SecurityUtilities.SignFile(CertificateThumbprint,
                TimestampUrl == null ? null : new Uri(TimestampUrl),
                SigningTarget.ItemSpec, TargetFrameworkVersion);
                return true;
            }
            catch (ArgumentException ex) when (ex.ParamName.Equals("certThumbprint"))
            {
                Log.LogErrorWithCodeFromResources("SignFile.CertNotInStore");
                return false;
            }
            catch (FileNotFoundException ex)
            {
                Log.LogErrorWithCodeFromResources("SignFile.TargetFileNotFound", ex.FileName);
                return false;
            }
            catch (ApplicationException ex)
            {
                Log.LogErrorWithCodeFromResources("SignFile.SignToolError", ex.Message.Trim());
                return false;
            }
            catch (WarningException ex)
            {
                Log.LogWarningWithCodeFromResources("SignFile.SignToolWarning", ex.Message.Trim());
                return true;
            }
            catch (CryptographicException ex)
            {
                Log.LogErrorWithCodeFromResources("SignFile.SignToolError", ex.Message.Trim());
                return false;
            }
            catch (Win32Exception ex)
            {
                Log.LogErrorWithCodeFromResources("SignFile.SignToolError", ex.Message.Trim());
                return false;
            }
            catch (UriFormatException ex)
            {
                Log.LogErrorWithCodeFromResources("SignFile.SignToolError", ex.Message.Trim());
                return false;
            }
        }
    }
}
