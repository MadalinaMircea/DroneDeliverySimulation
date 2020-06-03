using DroneDeliverySystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliverySystem.Agents
{
    public abstract class EnvironmentLimits
    {
        public abstract bool Contains(Position position);
        public abstract Position GeneratePosition(Random rnd);
    }
}
