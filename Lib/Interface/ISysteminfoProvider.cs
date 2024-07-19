using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace AssetManager.Lib.Interface
{
    internal interface ISysteminfoProvider
    {
		JsonObject Get(ISystemSourceObject source);
    }
}
