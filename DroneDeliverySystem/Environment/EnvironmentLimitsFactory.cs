namespace DroneDeliverySystem.Agents
{
    public class EnvironmentLimitsFactory
    {
        public EnvironmentLimits CreateEnvironmentLimits(int minX, int maxX, int minY, int maxY, int minZ, int maxZ)
        {
            if(minX == -1 || maxX == -1)
            {
                return null;
            }

            if(minY == -1 || maxY == -1)
            {
                return null;
            }

            if(minZ == -1)
            {
                if(maxZ != -1)
                {
                    return null;
                }

                return new PlaneEnvironmentLimits(minX, maxX, minY, maxY);
            }

            if(maxZ == -1)
            {
                return null;
            }

            return null;
        }
    }
}
