using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using AssetManager.DataModel;
using System.Web;
using System.Management.Automation;
using Microsoft.VisualBasic;
using System.Net;
using System.Net.Sockets;

namespace AssetManager.Lib
{
	internal class LocalSystemInfoProvider
	{
		public static JsonObject RootNode = new JsonObject();

		/// <summary>
		/// Exclude Meta Properties
		/// </summary>
		/// <param name="wmiClassName"></param>
		/// <returns></returns>
		private static string GetCIMQuery(string wmiClassName, string _namespace)
		{
			return $"Get-CimInstance -ClassName {wmiClassName} -Namespace {_namespace} | Select-Object -Property * -ExcludeProperty CimClass, CimInstanceProperties | ConvertTo-Json";
		}

		public static void test()
		{
			//GetRegistryDump();
			//GetWMIResource();


			//RootNode.Add("info_monitor", new JsonObject()); // powershell test

			RootNode.Add("Metainfo", GetMetainfo);

			Console.WriteLine(RootNode);


			string path = Path.Combine(
				Environment.GetEnvironmentVariable("USERPROFILE"),
				"Systeminfo.json"
				);
			//File.WriteAllText(path, RootNode.ToString());
		}

		private static JsonObject GetMetainfo
		{
			get
			{
				var result = new JsonObject();

				var ComputerName = Environment.GetEnvironmentVariable("ComputerName");
				var CurrentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				var CurrentTimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

				List<string> addrList = new List<string>();
				IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
				if (addresses != null && addresses.Length > 0)
					foreach (IPAddress address in addresses)
						if (address.AddressFamily == AddressFamily.InterNetwork)
							addrList.Add(address.ToString());
				
				string jsonstring = JsonSerializer.Serialize(addrList, new JsonSerializerOptions() { WriteIndented = true });

				result.Add("ComputerName", ComputerName);
				result.Add("CurrentDateTime", CurrentDateTime);
				result.Add("CurrentTimeStamp", CurrentTimeStamp);
				result.Add("IpAddresses", JsonArray.Parse(jsonstring));
				
				return result;
			}
		}

		private static void GetRegistryDump()
		{
			RootNode.Add("info_Edge", GetEdgeVersion);
			RootNode.Add("info_SystemBuild", GetSystemBuilinfo);
			RootNode.Add("info_hauri", GetHauriEngineVersion);
		}

		private static void GetWMIResource()
		{
			var classList = new Dictionary<string , string>();
			classList.Add("MS_SystemInformation", @"root\wmi");
			classList.Add("WmiMonitorID", @"root\wmi"); // 모니터정보는 별도 추출
			classList.Add("Win32_Tpm", @"root\cimv2\Security\MicrosoftTpm");
			classList.Add("Win32_BIOS", @"root\cimv2");
			classList.Add("Win32_NetworkAdapter", @"root\cimv2");
			classList.Add("Win32_NetworkAdapterConfiguration", @"root\cimv2");
			classList.Add("Win32_ComputerSystem", @"root\cimv2");
			classList.Add("Win32_ComputerSystemProduct", @"root\cimv2");
			classList.Add("Win32_Processor", @"root\cimv2");
			classList.Add("Win32_DiskDrive", @"root\cimv2");
			classList.Add("Win32_OperatingSystem", @"root\cimv2");
			classList.Add("Win32_Printer", @"root\cimv2");
			classList.Add("Win32_PrinterConfiguration", @"root\cimv2");
			classList.Add("Win32_PrinterDriver", @"root\cimv2");
			classList.Add("Win32_PhysicalMemory", @"root\cimv2");
			classList.Add("Win32_UserAccount", @"root\cimv2");
			classList.Add("MSFT_PhysicalDisk", @"root\Microsoft\Windows\Storage");

			using (PowerShell powerShell = PowerShell.Create())
			{
				string _resultPropertyString = @"Results";

				foreach (var item in classList)
				{
					Console.WriteLine($"[*] INFO : Query To {item.Key}");

					string script = GetCIMQuery(item.Key, item.Value);
					powerShell.AddScript(script);
					var results = powerShell.Invoke();

					if (results.Count > 0)
					{
						var jsonArr = new JsonArray();
						foreach (var result in results)
							jsonArr.Add(JsonNode.Parse(result.ToString()));

						var wrappedObject = new JsonObject
						{
							["Source"] = "Wmi",
							[_resultPropertyString] = jsonArr
						};

						RootNode.Add(item.Key, wrappedObject);
					}
					else
					{
						var wrappedObject = new JsonObject
						{
							["Source"] = "Wmi",
							[_resultPropertyString] = null
						};

						RootNode.Add(item.Key, wrappedObject);
					}
				}
			}
		}

		public static JsonObject GetHauriEngineVersion
		{
			get
			{
				RegistryKey key = Registry.LocalMachine;
				string subkey = @"SOFTWARE\HAURI\ViRobot Security\Base";
				var obj = RegistryObject.GetRegistry(key, subkey);

				return obj;
			}
		}

		public static JsonObject GetEdgeVersion
		{
			get
			{
				RegistryKey key = Registry.CurrentUser;
				string subkey = @"Software\Microsoft\Edge\BLBeacon";
				var obj = RegistryObject.GetRegistry(key, subkey);

				return obj;
			}
		}

		public static JsonObject GetSystemBuilinfo
		{
			get
			{
				RegistryKey key = Registry.LocalMachine;
				string subkey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
				// "CompositionEditionID", "CurrentBuild", "CurrentBuildNumber", "DisplayVersion", "EditionID", "InstallDate", "InstallTime", "ProductId", "ProductName", "ReleaseId", "UBR"
				var obj = RegistryObject.GetRegistry(key, subkey);

				return obj;
			}
		}
	}
}
