using System;
using System.IO;

namespace RightMove.Desktop.Services
{
    internal static class RightMoveSearchHistoryConfig
    {
        public static readonly int MaxHistoryItems = 10;
        public static readonly string HistoryFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "RightMove",
            "search_history.json"
        );
    }
}
