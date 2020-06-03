using DroneDeliverySystem.Agents;
using DroneDeliverySystem.Utils;
using System;

namespace DroneDeliverySystem.MoveUtils
{
    class MixedMovement : Movement
    {
        public override void Move(Drone drone)
        {
            int deltaX, deltaY;

            Position diff = drone.newPosition - drone.Position;

            while (drone.isMoving && (Math.Abs(diff.X) >= 5 || Math.Abs(diff.Y) >= 5))
            {
                deltaX = diff.X > 0 ? 1 : (diff.X == 0 ? 0 : -1);
                deltaY = diff.Y > 0 ? 1 : (diff.Y == 0 ? 0 : -1);
                drone.Position.X += deltaX;
                drone.Position.Y += deltaY;

                if (drone.package != null)
                {
                    drone.package.Move(drone.Position);
                }

                drone.Changed();

                System.Threading.Thread.Sleep(50);
                diff = drone.newPosition - drone.Position;
            }
        }
    }
}
