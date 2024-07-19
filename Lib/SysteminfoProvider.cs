using AssetManager.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace AssetManager.Lib
{
	internal class SysteminfoProvider : ISysteminfoProvider
	{
		JsonObject RootNode { get; set; }

		public SysteminfoProvider()
		{
			this.RootNode = new JsonObject();

		}

		public JsonObject Get(ISystemSourceObject source)
		{
			var _result = new JsonObject();

			return _result;
		}

		public JsonObject Get(IRegistrySourceObject source)
		{
			return RegistryInfoProvider.Get(source); ;
		}

		public JsonObject Get(IWmiSourceObject source)
		{
			var _result = new JsonObject();

			return _result;
		}

		private void PowershellScriptRun(string script)
		{

		}

		private void GetRegistryValue(IRegistrySourceObject _reg)
		{
			
		}
	}
}
