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

using Microsoft.Win32;
using System;

namespace Morphic.ThirdPartyApps
{
    /* CRITICAL NOTE: this class is under development, does not yet have a stable API contract, is not QA'd, and should not yet be used  */

    internal class RegistryHelpers
    {
        // throws RegistryKeyNotFoundException if the path does not exist
        public static T? GetRegistryEntry_NullDefault<T>(RegistryKey baseKey, String path, String name) where T : struct
        {
            try
            {
                return RegistryHelpers.GetRegistryValueData<T>(baseKey, path, name);
            }
            catch (RegistryKeyNotFoundException)
            {
                return null;
            }
        }

        // TODO: verify that struct will accept primary types like Boolean, UInt32, etc.
        // throws RegistryKeyNotFoundException if the path does not exist
        public static T? GetRegistryValueData<T>(RegistryKey baseKey, String path, String name) where T: struct
        {
            var registryKey = baseKey.OpenSubKey(path);
            if (registryKey == null)
            {
                throw new RegistryKeyNotFoundException();
            }

            // get the value (data)
            var valueAsObject = registryKey.GetValue(name);
            if (valueAsObject == null)
            {
                // if the key does not exist, return null
                return null;
            }

            return (T)valueAsObject;
        }

        public static void SetRegistryValueData<T>(RegistryKey baseKey, String path, String name, T value)
        {
            var registryKey = baseKey.OpenSubKey(path, true);
            if (registryKey == null)
            {
                registryKey = baseKey.CreateSubKey(path, true);
            }

            // TODO: capture the type of the generic type "T" and specify the registryValueKind accordingly (as an extra level of safety)

            // set the value (data)
            registryKey.SetValue(name, value);

            //Microsoft.Win32.RegistryValueKind registryValueKind;
            //registryKey.SetValue(name, value, registryValueKind);
        }
    }
}
