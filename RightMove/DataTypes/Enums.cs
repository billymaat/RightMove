using System;
using System.Collections.Generic;
using System.Text;

namespace RightMove.DataTypes
{
	[Flags]
	public enum PropertyTypeEnum
	{
		None = 0,
		Bungalow = 1,
		Flat = 2,
		Land = 4,
		SemiDetached = 8,
		Detached = 16,
		Terraced = 32,
		ParkHome = 64
	}
}
