using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wuzzle.Pickups.Interfaces;

namespace Wuzzle.character
{
    public class DashTargetItem
    {
        public IDashTarget DashTarget { get; set; }
        public RayCast2D RayCast2D { get; set; }
        /// <summary>
        /// Dot Product Angle (-1 = 180° & 0 = 0°)
        /// </summary>
        public float? Angle { get; set; }
        public DashTargetDirection DashTargetDirection
        {
            get
            {
                if (Angle.HasValue)
                {
                    if (Angle.Value < 2 && Angle.Value > 1)
                        return DashTargetDirection.Left;
                    else if (Angle.Value > -2 && Angle.Value < -1)
                        return DashTargetDirection.Right;
                    else if (Angle.Value > -1 && Angle.Value < 1)
                        return DashTargetDirection.Down;
                    else if (Angle.Value > 2 || Angle.Value < -2)
                        return DashTargetDirection.Down;
                }
                return DashTargetDirection.None;
            }
        }

        public DashTargetItem()
        {
            DashTarget = null;
            RayCast2D = null;
            Angle = null;
        }
    }

    public enum DashTargetDirection
    {
        None,
        Left,
        LeftUp,
        Up,
        UpRight,
        Right,
        RightDown,
        Down,
        DownLeft
    }
}
