using System;
using System.Collections.Generic;
using System.Text;
using RightMove.DataTypes;
using RightMove.Services;

namespace RightMove.Factory
{
	public class RightMovePropertyFactory
	{
		private IHttpService _httpService;
		
		public RightMovePropertyFactory(IHttpService httpService)
		{
			_httpService = httpService;	
		}
		
		public RightMoveProperty Create()
		{
			return new RightMoveProperty(_httpService);
		}
	}
}
