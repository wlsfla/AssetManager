using AssetManager.Lib.Interface;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace AssetManager.Lib
{
	internal class RegistryInfoProvider : InfoProvider
	{
		public static JsonObject Get(IRegistrySourceObject _regobj)
		{
			var _result = new JsonObject();

			var dump = GetRegistryValue(_regobj);
			_result = JsonConverter.Parse(JsonConverter.Serialize(dump));

			

			var wrappedObject = new JsonObject
			{
				["Source"] = "Win_Registry",
				["Results"] = new JsonArray(_result)
			};

			return wrappedObject;
		}

		private static new Dictionary<string, object> GetRegistryValue(IRegistrySourceObject _regobj)
		{
			var _result = new Dictionary<string, object>();
			Console.WriteLine($"[*] INFO : Dump Registry Of {_regobj.ToString()}");

			try
			{
				using (var key = _regobj.RegistryKey.OpenSubKey(_regobj.SubKey))
				{
					if (key != null)
					{
						// search subkey value
						foreach (var valueName in key.GetValueNames())
						{
							var value = key.GetValue(valueName);
							if (value != null)
								_result[valueName] = value.ToString();
							else
								_result[valueName] = null;
						}

						// subkey searh
						//foreach (var valueName in key.GetValueNames())
						//{

						//}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error reading registry value: {ex.Message}");
			}

			return _result;
		}
	}
}
