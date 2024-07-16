using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;

namespace AssetManager.DataModel
{
	internal class RegistryObject
	{
		public string Registrykey { get; set; }
		public Dictionary<string, object> RegistryDumps { get; set; }

		public RegistryObject()
		{
			RegistryDumps = new Dictionary<string, object>();
		}

		public static JsonObject GetRegistry(RegistryKey key, string subkey)
		{
			// Dumped Registry
			RegistryObject regObject = new RegistryObject();
			regObject.RegistryDumps = regObject.GetRegistryValue(key, subkey);

			// Serialized Registry
			string jsonstring = JsonSerializer.Serialize(regObject, new JsonSerializerOptions() { WriteIndented = true });
			var obj = JsonNode.Parse(jsonstring).AsObject();

			var wrappedObject = new JsonObject
			{
				["Source"] = "Win_Registry",
				["Results"] = new JsonArray(obj)
			};

			return wrappedObject;
		}

		private new Dictionary<string, object> GetRegistryValue(RegistryKey regkey, string subkey)
		{
			var result = new Dictionary<string, object>();
			this.Registrykey = $"{regkey.Name}\\{subkey}";
			Console.WriteLine($"[*] INFO : Dump Registry Of {Registrykey}");

			try
			{
				using (var key = regkey.OpenSubKey(subkey))
				{
					if (key != null)
					{
						// key value search
						foreach (var valueName in key.GetValueNames())
						{
							var value = key.GetValue(valueName);
							if (value != null)
								result[valueName] = value.ToString();
							else
								result[valueName] = null;
						}

						// subkey searh
						foreach (var valueName in key.GetValueNames())
						{

						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error reading registry value: {ex.Message}");
			}

			return result;
		}
	}
}
