using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace RightMoveConsole.Services
{
	public interface ISearchLocationsReader : IFileReader
	{
		List<string> GetLocations();
	}

	public class SearchLocationsReader : FileReaderService, ISearchLocationsReader
	{
		private readonly Func<string> _filepath;

		public SearchLocationsReader(Func<string> filepath) : base()
		{
			_filepath = filepath;
		}

		public override string FileName => _filepath?.Invoke();

		/// <summary>
		/// Get a list of locations
		/// </summary>
		/// <returns>A list of locations</returns>
		public List<string> GetLocations()
		{
			if (!FileExists())
			{
				Res = Result.FileDoesNotExist;
				return null;
			}

			List<string> locations = new List<string>();

			using (StreamReader reader = new StreamReader(FilePath))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					line = line.Trim();
					if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
					{
						continue;
					}

					locations.Add(line);
				}
			}

			return locations;
		}
	}
}
