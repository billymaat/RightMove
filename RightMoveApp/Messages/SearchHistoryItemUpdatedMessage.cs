using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RightMove.DataTypes;

namespace RightMove.Desktop.Messages
{
    public class SearchHistoryItemsUpdatedMessage
    {
	    public List<SearchHistoryItem> NewValue { get; set; }
    }
}
