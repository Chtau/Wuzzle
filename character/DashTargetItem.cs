using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wuzzle.character.Interfaces;

namespace Wuzzle.character
{
    public class DashTargetItem
    {
        public IDashTarget DashTarget { get; set; }
        public RayCast2D RayCast2D { get; set; }
        public float? Angle { get; private set; }
        public DashTargetDirection DashTargetDirection { get; private set; }
        public DashTargetDirection DashTargetDiagonalDirection { get; private set; }

        public DashTargetItem()
        {
            DashTarget = null;
            RayCast2D = null;
            SetAngle(null);
        }

        public void SetAngle(float? angle)
        {
            Angle = angle;
            OnSetAngle();
        }

        private void OnSetAngle()
        {
            if (Angle.HasValue)
            {
                if (Angle.Value < 2 && Angle.Value > 1)
                {
                    DashTargetDirection = DashTargetDirection.Left;
                    if (Angle.Value < 1.5)
                        DashTargetDiagonalDirection = DashTargetDirection.LeftUp;
                    else
                        DashTargetDiagonalDirection = DashTargetDirection.LeftDown;
                }
                else if (Angle.Value > -2 && Angle.Value < -1)
                {
                    DashTargetDirection = DashTargetDirection.Right;
                    if (Angle.Value > 1.5 || Angle.Value > -1.5)
                        DashTargetDiagonalDirection = DashTargetDirection.RightUp;
                    else
                        DashTargetDiagonalDirection = DashTargetDirection.RightDown;
                }
                else if (Angle.Value > -1 && Angle.Value < 1)
                {
                    DashTargetDirection = DashTargetDirection.Up;
                    if (Angle.Value < 0)
                        DashTargetDiagonalDirection = DashTargetDirection.UpRight;
                    else
                        DashTargetDiagonalDirection = DashTargetDirection.UpLeft;
                }
                else if (Angle.Value > 2 || Angle.Value < -2)
                {
                    DashTargetDirection = DashTargetDirection.Down;
                    if (Angle.Value < -2)
                        DashTargetDiagonalDirection = DashTargetDirection.DownRight;
                    else
                        DashTargetDiagonalDirection = DashTargetDirection.DownLeft;
                }
            }
            else
            {
                DashTargetDiagonalDirection = DashTargetDirection.None;
                DashTargetDirection = DashTargetDirection.None;
            }
        }
    }

    public enum DashTargetDirection
    {
        None,
        Left,
        LeftUp,
        LeftDown,
        Up,
        UpRight,
        UpLeft,
        Right,
        RightDown,
        RightUp,
        Down,
        DownLeft,
        DownRight,
    }
}
