using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

		/// <summary>
		/// Gets the filename, excluding the path
		/// </summary>
		public abstract string FileName
		{
			get;
		}

		/// <summary>
		/// Gets the file path of the file to read
		/// </summary>
		public virtual string FilePath
		{
			get
			{
				if (string.IsNullOrEmpty(FileName))
				{
					return null;
				}

				var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
				return Path.Combine(path, FileName);
			}
		}

		protected bool FileExists()
		{
			if (string.IsNullOrEmpty(FilePath))
			{
				return false;
			}

			return File.Exists(FilePath);
		}
	}
}
