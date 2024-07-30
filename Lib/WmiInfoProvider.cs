using AssetManager.Lib.Interface;
using System.Management.Automation;
using System.Text.Json.Nodes;

namespace AssetManager.Lib
{
	internal class WmiInfoProvider : InfoProvider
	{
		public static JsonObject Get(IWmiSourceObject _regobj)
		{
			var _result = new JsonObject();



			return _result;
		}

		private static string GetCIMScript(IWmiSourceObject source)
		{
			return $"Get-CimInstance -ClassName {source.ClassName} -Namespace {source.Namespace} | Select-Object -Property * -ExcludeProperty CimClass, CimInstanceProperties | ConvertTo-Json";
		}

		private static void RunPowerShellScript(string script, ref JsonObject obj)
		{
			using (PowerShell powerShell = PowerShell.Create())
			{
				powerShell.AddScript(script);
				var _result = powerShell.Invoke();

				// Test
				Console.WriteLine($"[*** info -> Result Count : {_result.Count}]");

				var jsonArr = new JsonArray();
				jsonArr.Add(JsonConverter.Parse(_result[0].ToString()));



			}
		}
	}
}
