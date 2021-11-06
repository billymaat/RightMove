using System;
using System.Collections.Generic;
using System.Text;

namespace RightMove.Factory
{
	public interface IFactory<T>
	{
		T Create();
	}
}
