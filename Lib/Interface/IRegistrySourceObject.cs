using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManager.Lib.Interface
{
	internal interface IRegistrySourceObject : ISystemSourceObject
	{
		RegistryKey RegistryKey { get; set; }
		string SubKey { get; set; }
		Dictionary<string, object> Dump { get; set; }

		string ToString();
	}
}
