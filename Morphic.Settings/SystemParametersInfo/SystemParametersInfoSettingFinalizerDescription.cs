﻿// Copyright 2020 Raising the Floor - International
//
// Licensed under the New BSD license. You may not use this file except in
// compliance with this License.
//
// You may obtain a copy of the License at
// https://github.com/GPII/universal/blob/master/LICENSE.txt
//
// The R&D leading to these results received funding from the:
// * Rehabilitation Services Administration, US Dept. of Education under 
//   grant H421A150006 (APCP)
// * National Institute on Disability, Independent Living, and 
//   Rehabilitation Research (NIDILRR)
// * Administration for Independent Living & Dept. of Education under grants 
//   H133E080022 (RERC-IT) and H133E130028/90RE5003-01-00 (UIITA-RERC)
// * European Union's Seventh Framework Programme (FP7/2007-2013) grant 
//   agreement nos. 289016 (Cloud4all) and 610510 (Prosperity4All)
// * William and Flora Hewlett Foundation
// * Ontario Ministry of Research and Innovation
// * Canadian Foundation for Innovation
// * Adobe Foundation
// * Consumer Electronics Association Foundation

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Morphic.Core;
using Microsoft.Win32;

namespace Morphic.Settings.Spi
{

    /// <summary>
    /// An ini handler description that specifies which part of the ini file should be udpated
    /// </summary>
    public class SystemParametersInfoSettingFinalizerDescription : SettingFinalizerDescription
    {

        /// <summary>
        /// The filename of the ini file, possibly including environmental variables
        /// </summary>
        public SystemParametersInfo.Action Action;

        public int Parameter1 = 0;

        public object? Parameter2;

        /// <summary>
        /// Should a change notification be sent to the system
        /// </summary>
        public bool SendChange = false;

        /// <summary>
        /// Should the user's profile be updated
        /// </summary>
        public bool UpdateUserProfile = false;

        /// <summary>
        /// Create a new ini file handler
        /// </summary>
        /// <param name="settingId"></param>
        public SystemParametersInfoSettingFinalizerDescription(SystemParametersInfo.Action action) : base(FinalizerKind.SystemParametersInfo)
        {
            Action = action;
        }

        public SystemParametersInfoSettingFinalizerDescription(JsonElement element): base(FinalizerKind.SystemParametersInfo)
        {
            var actionString = element.GetProperty("action").GetString();
            Action = Enum.Parse<SystemParametersInfo.Action>(actionString, ignoreCase: true);
            try
            {
                SendChange = element.GetProperty("send_change").GetBoolean();
            }
            catch
            {
            }
            try
            {
                UpdateUserProfile = element.GetProperty("update_user_profile").GetBoolean();
            }
            catch
            {
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj is SystemParametersInfoSettingFinalizerDescription other)
            {
                return other.Action == Action && other.Parameter1 == Parameter1 && other.Parameter2 == Parameter2 && other.SendChange == SendChange && other.UpdateUserProfile == UpdateUserProfile;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Action.GetHashCode() ^ Parameter1 ^ (Parameter2?.GetHashCode() ?? 0) ^ SendChange.GetHashCode() ^ UpdateUserProfile.GetHashCode();
        }
    }
}
