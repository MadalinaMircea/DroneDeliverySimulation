using DroneDeliverySystem.Agents;
using DroneDeliverySystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliverySystem.MoveUtils
{
    class ManhattanMovement : Movement
    {
        public override void Move(Drone drone)
        {
            int deltaX, deltaY;

            Position diff = drone.newPosition - drone.Position;

            while (drone.isMoving && Math.Abs(diff.X) >= 5)
            {
                deltaX = diff.X > 0 ? 1 : (diff.X == 0 ? 0 : -1);
                drone.Position.X += deltaX;

                if (drone.package != null)
                {
                    drone.package.Move(drone.Position);
                }

                drone.Changed();

                System.Threading.Thread.Sleep(30);
                diff = drone.newPosition - drone.Position;
            }

            while (drone.isMoving && Math.Abs(diff.Y) >= 5)
            {
                deltaY = diff.Y > 0 ? 1 : (diff.Y == 0 ? 0 : -1);
                drone.Position.Y += deltaY;

                if (drone.package != null)
                {
                    drone.package.Move(drone.Position);
                }

                drone.Changed();

                System.Threading.Thread.Sleep(30);
                diff = drone.newPosition - drone.Position;
            }
        }
    }
}
