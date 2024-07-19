﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManager.Lib.Interface
{
	internal interface IWmiSourceObject : ISystemSourceObject
	{
		string ClassName { get; }
		string Namespace { get; }
	}
}
