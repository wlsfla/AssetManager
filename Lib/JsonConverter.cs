using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;

namespace AssetManager.Lib
{
	internal class JsonConverter
	{
		public static string Serialize(object obj)
		{
			var _result = string.Empty;

			try
			{
				_result = JsonSerializer.Serialize(obj, new JsonSerializerOptions() { WriteIndented = true });
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error : JsonSerialize\n {ex.ToString()}");
			}

			return _result;
		}

		public static JsonObject Parse(object obj)
		{
			var _result = new JsonObject();

			try
			{
				_result = JsonNode.Parse(Serialize(obj)).AsObject();
			}
			catch (Exception ex)
			{
				_result = null;
				Console.WriteLine($"Error : JsonParse\n {ex.ToString()}");
			}

			return _result;
		}
	}
}
