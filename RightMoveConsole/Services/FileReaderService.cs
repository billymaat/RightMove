using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RightMoveConsole.Services
{
	public interface IFileReader
	{
		FileReaderService.Result Res
		{
			get;
		}
	}

	public abstract class FileReaderService : IFileReader
	{
		public FileReaderService()
		{
			Res = Result.None;
		}

		public enum Result
		{
			FileDoesNotExist,
			None
		}

		public Result Res
		{
			get;
			protected set;
		}

		public abstract string FileName
		{
			get;
		}

		protected bool FileExists()
		{
			return File.Exists(FileName);
		}
	}
}
