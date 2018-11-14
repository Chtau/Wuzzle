using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wuzzle.character.Interfaces;

namespace Wuzzle.character
{
    public class Dash
    {
        System.Collections.Generic.Dictionary<Guid, DashTargetItem> DashTargets = new System.Collections.Generic.Dictionary<Guid, DashTargetItem>();

        public void AddDashTarget(IDashTarget target, Node parent, params Node[] ignoreNode)
        {
            DashTargets.Add(target.DashTargetId, new DashTargetItem
            {
                DashTarget = target,
                RayCast2D = GetDashRayCast2D(target, parent, ignoreNode)
            });
        }

        public void RemoveDashTarget(IDashTarget target, Node parent)
        {
            if (DashTargets.ContainsKey(target.DashTargetId))
            {
                parent.RemoveChild(DashTargets[target.DashTargetId].RayCast2D); //DashArea2D
                DashTargets[target.DashTargetId].RayCast2D.QueueFree();
                DashTargets.Remove(target.DashTargetId);
            }
            //RemoveDebugLine(target.DashTargetId);
        }

        private RayCast2D GetDashRayCast2D(IDashTarget target, Node parent, params Node[] ignoreNode)
        {
            var ray = new RayCast2D();
            ray.SetCollisionMaskBit(0, true);
            //ray.SetCollisionMaskBit(4, true);
            foreach (var node in ignoreNode)
            {
                ray.AddException(node);
            }
            //ray.AddException(this);
            ray.ExcludeParent = true;
            ray.Enabled = true;

            parent.AddChild(ray); //DashArea2D
            return ray;
        }
    }
}
