using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using AssetManager.DataModel;

namespace AssetManager.Lib
{
	internal class LocalHWinfo
	{
		public static JsonObject RootNode = new JsonObject();

		public static void test()
		{
			RootNode.Add("info_Edge", GetEdgeVersion);
			RootNode.Add("info_SystemBuild", GetSystemBuilinfo);
			RootNode.Add("info_hauri", GetHauriEngineVersion);

			Console.WriteLine(GetBaseBoard);


			//RootNode.Add("info_monitor", new JsonObject()); // powershell test
			//RootNode.Add("info_PCProduct", GetPCProduct);
			//RootNode.Add("info_baseboard", GetBaseBoard);

			//var info_bios = new JsonObject();
			//RootNode.Add("info_bios", info_bios);

			//var info_printer = new JsonObject();
			//RootNode.Add("info_printer", info_printer);

			//var info_cpu = new JsonObject();
			//RootNode.Add("info_cpu", info_cpu);

			//var info_memory = new JsonObject();
			//RootNode.Add("info_memory", info_memory);

			//var info_hdd = new JsonObject();
			//RootNode.Add("info_hdd", info_hdd);

			//var info_nic = new JsonObject();
			//RootNode.Add("info_nic", info_nic);

			//var info_os = new JsonObject();
			//RootNode.Add("info_os", info_os);

			//var info_domain = new JsonObject();
			//RootNode.Add("info_domain", info_domain);

		}

		public static ManagementObjectCollection ExecuteWMIQuery(string query)
		{
			using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
			{
				return searcher.Get();
			}
		}

		public static JsonObject GetBaseBoard
		{
			get
			{
				var obj = new JsonObject();
				string query = "SELECT * FROM Win32_BaseBoard";

				using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
				{
					List<string> fields = new List<string>();
					var query_result = searcher.Get();
					foreach (ManagementObject item in query_result)
					{
						foreach (PropertyData property in item.Properties)
							fields.Add(property.Name);
						break;
					}

					foreach (ManagementObject item in query_result)
					{
						var line = new JsonObject();
						foreach (var field in fields)
							line.Add(field, item[field].ToString());

					}
				}

				return obj;
			}
		}

		public static JsonObject GetPCProduct
		{
			get
			{
				var obj = new JsonObject();
				string query = "SELECT Name, Vendor, IdentifyingNumber FROM Win32_ComputerSystemProduct";
				foreach (ManagementObject item in ExecuteWMIQuery(query))
				{
					var v = new JsonObject();
					obj.Add("Name", item["Name"].ToString());
					obj.Add("Vendor", item["Vendor"].ToString());
					obj.Add("IdentifyingNumber", item["IdentifyingNumber"].ToString());

				}

				return obj;
			}
		}

		public static JsonObject GetHauriEngineVersion
		{
			get
			{
				RegistryKey key = Registry.LocalMachine;
				string subkey = @"SOFTWARE\HAURI\ViRobot Security\Base";
				string[] names = ["EngineVersion"];
				var obj = RegistryObject.GetRegistry(key, subkey, names);

				return obj;
			}
		}

		public static JsonObject GetEdgeVersion
		{
			get
			{
				RegistryKey key = Registry.CurrentUser;
				string subkey = @"Software\Microsoft\Edge\BLBeacon";
				string[] names = ["version"];
				var obj = RegistryObject.GetRegistry(key, subkey, names);

				return obj;
			}
		}

		public static JsonObject GetSystemBuilinfo
		{
			get
			{
				RegistryKey key = Registry.LocalMachine;
				string subkey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
				string[] names = new string[] {
					"CompositionEditionID", "CurrentBuild", "CurrentBuildNumber", "DisplayVersion", "EditionID", "InstallDate", "InstallTime", "ProductId", "ProductName", "ReleaseId", "UBR" };
				var obj = RegistryObject.GetRegistry(key, subkey, names);

				return obj;
			}
		}
	}
}
