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
			//RootNode.Add("info_Edge", GetEdgeVersion);
			//RootNode.Add("info_SystemBuild", GetSystemBuilinfo);
			//RootNode.Add("info_hauri", GetHauriEngineVersion);

			//Console.WriteLine(RootNode.ToString());

			//RootNode.Add("info_monitor", new JsonObject()); // powershell test

			Test_WMI();


		}

		private static void Test_WMI()
		{
			var obj = new JsonObject();

			var classList = new Dictionary<string, string>();
			//classList.Add("MS_SystemInformation", @"root\wmi");
			////classList.Add("WmiMonitorID", @"root\wmi"); // 모니터정보는 별도 추출
			//classList.Add("Win32_Tpm", @"root\CIMV2\Security\MicrosoftTpm");
			//classList.Add("Win32_BIOS", @"root\cimv2");
			classList.Add("Win32_NetworkAdapter", @"root\cimv2");
			//classList.Add("Win32_NetworkAdapterConfiguration", @"root\cimv2");
			//classList.Add("Win32_ComputerSystem", @"root\cimv2");
			//classList.Add("Win32_ComputerSystemProduct", @"root\cimv2");
			//classList.Add("Win32_Processor", @"root\cimv2");
			//classList.Add("Win32_DiskDrive", @"root\cimv2");
			//classList.Add("Win32_OperatingSystem", @"root\cimv2");
			//classList.Add("Win32_Printer", @"root\cimv2");
			//classList.Add("Win32_PrinterConfiguration", @"root\cimv2");
			//classList.Add("Win32_PrinterDriver", @"root\cimv2");
			//classList.Add("Win32_PhysicalMemory", @"root\cimv2");
			//classList.Add("Win32_UserAccount", @"root\cimv2");



			//JsonSerializer.Serialize(regObject, new JsonSerializerOptions() { WriteIndented = true });

			using (PowerShell powerShell = PowerShell.Create())
			{
				foreach (var item in classList)
				{
					//string name = "Win32_BIOS";
					string script = GetCIMQuery(item.Key, item.Value);
					
					powerShell.AddScript(script);
					var results = powerShell.Invoke();

					foreach (var result in results)
					{
						var s = result.ToString();
						var jsonobj = new JsonObject();

						if (s[0] != '{') // check json list case
						{

							new JsonObject
							{
								[item.Key] = JsonNode.Parse(s).AsArray()
							};

							
						}
						else // normal case
						{
							jsonobj = JsonNode.Parse(s).AsObject();
							
						}
						
						Console.WriteLine(jsonobj);

						//Console.WriteLine(jsonobj.ToString());
						Console.WriteLine("######################################################################");
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
