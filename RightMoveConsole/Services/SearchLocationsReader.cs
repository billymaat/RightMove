using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace RightMoveConsole.Services
{
	public interface ISearchLocationsReader : IFileReader
	{
		List<String> GetFilenames();
	}

	public class SearchLocationsReader : FileReaderService, ISearchLocationsReader
	{
		private Func<string> _filepath;

		public SearchLocationsReader(Func<string> filepath) : base()
		{
			_filepath = filepath;
		}

		public override string FileName => _filepath?.Invoke();

		public override string FilePath
		{
			get
			{
				var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
				return Path.Combine(path, FileName);
			}
		}
		public List<string> GetFilenames()
		{
			if (!FileExists())
			{
				Res = Result.FileDoesNotExist;
				return null;
			}

			List<string> filenames = new List<string>();

			using (System.IO.StreamReader reader = new System.IO.StreamReader(FilePath))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					line = line.Trim();
					if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
					{
						continue;					
					}

					filenames.Add(line);
				}
			}

			return filenames;
		}
	}
}
