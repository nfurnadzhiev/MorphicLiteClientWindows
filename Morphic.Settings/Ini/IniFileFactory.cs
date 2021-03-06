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

using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Morphic.Settings.Ini
{
    /// <summary>
    /// A factory that creates <code>IniFile</code> instances
    /// </summary>
    public class IniFileFactory: IIniFileFactory
    {

        public IIniFile Open(string path)
        {
            IniFile? file = null;
            while (file == null)
            {
                if (!iniFilesByPath.TryGetValue(path, out file))
                {
                    file = new IniFile(path);
                    if (!iniFilesByPath.TryAdd(path, file))
                    {
                        file = null;
                    }
                }
            }
            return file;
        }

        public Task Begin()
        {
            return Task.CompletedTask;
        }

        public async Task Commit()
        {
            foreach (var file in iniFilesByPath.Values)
            {
                if (file.NeedsWrite)
                {
                    await file.Write();
                }
            }
            iniFilesByPath.Clear();
        }

        private ConcurrentDictionary<string, IniFile> iniFilesByPath = new ConcurrentDictionary<string, IniFile>();
    }
}
