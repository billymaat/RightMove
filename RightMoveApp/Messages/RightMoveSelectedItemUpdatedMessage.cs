﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RightMove.DataTypes;

namespace RightMove.Desktop.Messages
{
    public sealed class RightMoveSelectedItemUpdatedMessage
    {
        public RightMoveProperty NewValue { get; set; }
    }
}
