using AssetManager.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManager.Lib
{
	internal class SystemSourceObject : ISystemSourceObject
	{
		public SystemSourceObject() { }

		public List<ISystemSourceObject> Sources {  get; set; }

		public void SetSource(ISystemSourceObject source)
		{
			this.Sources.Add(source);
		}
	}
}
