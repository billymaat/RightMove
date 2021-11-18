using System;
using System.Collections.Generic;
using System.Text;

namespace RightMove.Factory
{
	public class Factory<T> : IFactory<T>
	{
		private readonly Func<T> _initFunc;

		public Factory(Func<T> initFunc)
		{
			_initFunc = initFunc;
		}

		public T Create()
		{
			return _initFunc();
		}
	}
}
