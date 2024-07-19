using AssetManager.Lib.Interface;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManager.Lib.DataModel
{
    internal class RegistrySourceObject : IRegistrySourceObject
    {
        public RegistryKey RegistryKey { get; set; }
        public string SubKey { get; set; }
        public Dictionary<string, object> Dump { get; set; }

        public RegistrySourceObject(RegistryKey registrykey, string subkey)
        {
            RegistryKey = registrykey;
            SubKey = subkey;
        }

        public override string ToString()
        {
            var _registryPath = $"{RegistryKey.Name}\\{SubKey}";

            return _registryPath;
        }
    }
}
