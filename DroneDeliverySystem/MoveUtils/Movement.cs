using DroneDeliverySystem.Agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliverySystem.MoveUtils
{
    public abstract class Movement : IMovement
    {
        abstract public void Move(Drone drone);
    }
}
