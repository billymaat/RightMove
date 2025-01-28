using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCollectionUtilities
{
	public interface IFactory<T>
	{
		T Create();
	}
}
