using System;
using System.Collections.Generic;
using System.Text;

namespace RightMove.Db
{
	public interface IDbConfiguration
	{
		string DbFile
		{
			get;
		}
	}
}
