using System;
using System.Collections.Generic;
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

		public List<string> GetFilenames()
		{
			if (!FileExists())
			{
				Res = Result.FileDoesNotExist;
				return null;
			}

			List<string> filenames = new List<string>();

			using (System.IO.StreamReader reader = new System.IO.StreamReader(FileName))
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
