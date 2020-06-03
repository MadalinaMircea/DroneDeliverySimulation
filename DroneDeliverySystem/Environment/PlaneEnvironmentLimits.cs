using DroneDeliverySystem.Utils;
using System;

namespace DroneDeliverySystem.Agents
{
    public class PlaneEnvironmentLimits : EnvironmentLimits
    {
        int MinX;
        int MaxX;
        int MinY;
        int MaxY;

        public PlaneEnvironmentLimits(int minX, int maxX, int minY, int maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }

        public override bool Contains(Position position)
        {
            return position.X >= MinX && position.X <= MaxX && position.Y >= MinY && position.Y <= MaxY;
        }

        public override Position GeneratePosition(Random rnd)
        {
            return new Position(rnd.Next(MinX, MaxX), rnd.Next(MinY, MaxY));
        }
    }
}
