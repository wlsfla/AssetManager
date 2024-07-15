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
	internal class InfoObject
	{
		public List<object> ErrorMessage { get; set; }
		public List<object> Items { get; set; }

		public InfoObject()
		{
			ErrorMessage = new List<object>();
			Items = new List<object>();
		}
	}

	internal class RegistryObject
	{
		public RegistryKey RegistryKey { get; set; }
		public string RegistrySubKey { get; set; }
		public List<RegistryValueObject> RegistryValueObjects { get; set; }

		public RegistryObject()
		{
			RegistryValueObjects = new List<RegistryValueObject>();
		}

		public static JsonObject GetRegistry(RegistryKey key, string subkey, string[] names)
		{
			RegistryObject regobject = new RegistryObject();
			regobject.SetRegistrykey(key, subkey);
			regobject.GetRegistryValue(names);
			string jsonstring = JsonSerializer.Serialize(regobject, new JsonSerializerOptions() { WriteIndented = true });
			var obj = JsonNode.Parse(jsonstring).AsObject();

			return obj;
		}

		private void SetRegistrykey(RegistryKey key, string subkey)
		{
			this.RegistryKey = key;
			this.RegistrySubKey = subkey;
		}

		private void GetRegistryValue(string[] names)
		{
			var obj = new RegistryValueObject();
			obj.Success = false;

			try
			{
				using (var key = RegistryKey.OpenSubKey(RegistrySubKey))
				{
					if (key != null)
					{
						foreach (var name in names)
						{
							var value = key.GetValue(name);
							if (value != null)
							{
								obj.Success = true;
								obj.Value = value.ToString();
							}
							else
							{
								obj.Success = false;
								obj.Value = null;
								obj.ErrorMsg = "Value is Null";
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				obj.Success = false;
				obj.Value = null;
				obj.ErrorMsg = ex.Message;
				Console.WriteLine($"Error reading registry value: {ex.Message}");
			}

			RegistryValueObjects.Add(obj);
		}
	}

	internal class RegistryValueObject
	{
		public bool Success { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
		public string ErrorMsg { get; set; }
	}
}
