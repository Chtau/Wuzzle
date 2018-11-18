using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wuzzle.character.Interfaces
{
    public interface IDashTarget
    {
        Guid DashTargetId { get; }
        Node2D Instance { get; }
        void BeforeDashTo();
        bool Reached();
    }
}
