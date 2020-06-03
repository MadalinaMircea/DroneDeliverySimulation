using DroneDeliverySystem.Utils;
using System;

namespace DroneDeliverySystem.Agents
{
    public abstract class EnvironmentLimits
    {
        public abstract bool Contains(Position position);
        public abstract Position GeneratePosition(Random rnd);
    }
}
