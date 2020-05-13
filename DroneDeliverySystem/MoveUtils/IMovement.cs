using DroneDeliverySystem.Agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliverySystem.MoveUtils
{
    interface IMovement
    {
        void Move(Drone drone);
    }
}
