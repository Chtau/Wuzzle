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
        private System.Collections.Generic.Dictionary<Guid, DashTargetItem> dashTargets = new System.Collections.Generic.Dictionary<Guid, DashTargetItem>();
        private readonly Node _parent;
        private readonly KinematicBody2D _player;
        private CollisionShape2D playerCollision;

        private const int moveDashSpeed = 25000;
        private const float dashTimeout = 1f;
        private float dashTime = 0f;
        private int moveDashCount = 1000;
        private Guid currentDashTargetId = Guid.Empty;
        private Vector2 ToTarget = new Vector2();
        
        public DashTargetItem DashTarget { get; private set; }

        public Dash(Node parent, KinematicBody2D player, CollisionShape2D playerCollision)
        {
            _parent = parent;
            _player = player;
            this.playerCollision = playerCollision;
        }

        public void AddDashTarget(IDashTarget target)
        {
            dashTargets.Add(target.DashTargetId, new DashTargetItem
            {
                DashTarget = target,
                RayCast2D = GetDashRayCast2D(target, _parent, _player)
            });
        }

        public void RemoveDashTarget(IDashTarget target)
        {
            if (dashTargets.ContainsKey(target.DashTargetId))
            {
                _parent.RemoveChild(dashTargets[target.DashTargetId].RayCast2D); //DashArea2D
                dashTargets[target.DashTargetId].RayCast2D.QueueFree();
                dashTargets.Remove(target.DashTargetId);
            }
            RemoveDebugLine(target.DashTargetId);
        }

        public Player.PlayerPhysicsState ProcessPhysic(Player.PlayerPhysicsState previusState, float delta, ref Vector2 linear_vel, bool hitWall, bool hitFloor)
        {
            dashTime += delta;

            foreach (var item in dashTargets)
            {
                CheckDashTargets(item.Value);
            }

            bool moveLeft = false, moveRight = false, moveDown = false, moveUp = false;
            if (Input.IsActionPressed("move_left"))
            {
                moveLeft = true;
            }
            if (Input.IsActionPressed("move_right"))
            {
                moveRight = true;
            }
            if (Input.IsActionPressed("move_down"))
            {
                moveDown = true;
            }
            if (Input.IsActionPressed("move_up"))
            {
                moveUp = true;
            }

            if (currentDashTargetId != Guid.Empty)
            {
                var dif = ToTarget - _player.GlobalPosition;
                if (dif.x < 5 || hitFloor || hitWall)
                {
                    // reset
                    currentDashTargetId = Guid.Empty;
                    GD.Print("Target reached at!!!");
                    linear_vel.x = 0;
                    linear_vel.y = 0;

                    var cap = (CapsuleShape2D)playerCollision.Shape;
                    cap.Height = 44.4f;
                    cap.Radius = 10f;

                    if (previusState == Player.PlayerPhysicsState.Dash)
                        return Player.PlayerPhysicsState.Idle;
                    return previusState;
                }
            }

            if (AllowDashMove && Input.IsActionJustPressed("move_dash"))
            {
                if (currentDashTargetId == Guid.Empty)
                {
                    // perform a new Dash action
                    var target = OnDashTarget(moveLeft, moveRight, moveUp, moveDown);
                    if (target == null)
                    {
                        GD.Print("NO TARGET IN RANGE");
                        if (previusState == Player.PlayerPhysicsState.Dash)
                            return Player.PlayerPhysicsState.Idle;
                        return previusState;
                    }
                    DashTarget = target;
                    currentDashTargetId = target.DashTarget.DashTargetId;
                    GD.Print("Target:" + target.DashTarget.DashTargetId.ToString());
                    GD.Print(dashTime);
                    OnDashCountChange(-1);
                    dashTime = 0;

                    var target_speed_x = 0;
                    if (moveRight)
                        target_speed_x += 1;
                    else if (moveLeft)
                        target_speed_x += -1;
                    target_speed_x *= moveDashSpeed;

                    linear_vel.x = Mathf.Lerp(linear_vel.x, target_speed_x, 0.1f);

                    ToTarget = DashTarget.RayCast2D.CastTo - _player.GlobalPosition;
                    
                    float rotation = DashTarget.RayCast2D.CastTo.Angle();
                    linear_vel = linear_vel.Rotated(rotation);
                    DashTarget.RayCast2D.Enabled = false;
                    var cap = (CapsuleShape2D)playerCollision.Shape;
                    cap.Height = 1f;
                    cap.Radius = 1f;
                }
                return Player.PlayerPhysicsState.Dash;
            }
            return previusState;
        }

        public void AddDash()
        {
            OnDashCountChange(1);
        }

        private DashTargetItem OnDashTarget(bool left, bool right, bool up, bool down)
        {
            DashTargetItem target = null;
            if (left)
            {
                dashTargets.Where(x => x.Value.DashTargetDirection == DashTargetDirection.Left);
                GD.Print("NO");
                return null;
            } else if (right)
            {
                if (down)
                {
                    target = OnDashTargetItemFromDirection(dashTargets, DashTargetDirection.RightDown);
                    if (target != null)
                        return target;
                    // reverse check for down & right
                    target = OnDashTargetItemFromDirection(dashTargets, DashTargetDirection.DownRight);
                    if (target != null)
                        return target;
                }
                // for right, right and up or when no result from right and down
                target = OnDashTargetItemFromDirection(dashTargets, DashTargetDirection.RightUp);
                if (target != null)
                    return target;

                target = OnDashTargetItemFromDirection(dashTargets, DashTargetDirection.UpRight);
                return target;
            } else
            {
                return null;
            }
        }

        private DashTargetItem OnDashTargetItemFromDirection(IEnumerable<KeyValuePair<Guid, DashTargetItem>> items, DashTargetDirection direction)
        {
            var target = items.FirstOrDefault(x => x.Value.DashTargetDiagonalDirection == direction);
            if (target.Value != null)
                return target.Value;
            return null;
        }

        private void OnDashCountChange(int changeValue)
        {
            moveDashCount += changeValue;
            GD.Print("Current Burst:" + moveDashCount);
        }

        private bool AllowDashMove
        {
            get
            {
                if (moveDashCount > 0 && dashTime > dashTimeout)
                    return true;
                return false;
            }
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

        private void CheckDashTargets(DashTargetItem item)
        {
            item.RayCast2D.CastTo = item.DashTarget.Instance.GlobalPosition - _player.GlobalPosition;
            if (item.RayCast2D.IsColliding())
            {
                var coll = item.RayCast2D.GetCollider();
                if (coll is IDashTarget targetCollider && item.DashTarget.DashTargetId == targetCollider.DashTargetId)
                {
                    item.SetAngle(item.RayCast2D.Position.AngleToPoint(item.RayCast2D.CastTo));

                    if (item.DashTargetDirection == DashTargetDirection.None)
                        GD.Print("Collide angle:" + item.Angle + " Dir:" + item.DashTargetDirection);
                    //GD.Print("Collide angle:" + item.Angle + " Dir:" + item.DashTargetDirection + " Diag:" + item.DashTargetDiagonalDirection);
                    AddDebugLine(item.DashTarget.DashTargetId, item.RayCast2D.Position, item.RayCast2D.CastTo);
                }
                else
                {
                    RemoveDebugLine(item.DashTarget.DashTargetId);
                }
            }
        }

        #region Debug Lines for Targets
        System.Collections.Generic.Dictionary<Guid, Line2D> debugLines = new System.Collections.Generic.Dictionary<Guid, Line2D>();
        private void AddDebugLine(Guid id, Vector2 pos, Vector2 tar)
        {
            if (debugLines.ContainsKey(id))
            {
                for (int i = 0; i < debugLines[id].Points.Length; i++)
                {
                    debugLines[id].RemovePoint(i);
                }
                debugLines[id].AddPoint(pos);
                debugLines[id].AddPoint(tar);
            }
            else
            {
                var line = new Line2D
                {
                    DefaultColor = new Color(255, 0, 119.53f, 255),
                    Width = 10
                };
                line.AddPoint(pos);
                line.AddPoint(tar);
                _player.AddChild(line);
                debugLines.Add(id, line);
            }
        }

        private void RemoveDebugLine(Guid id)
        {
            if (debugLines.ContainsKey(id))
            {
                _player.RemoveChild(debugLines[id]);
                debugLines[id].QueueFree();
                debugLines.Remove(id);
            }
        }
        #endregion
    }
}
