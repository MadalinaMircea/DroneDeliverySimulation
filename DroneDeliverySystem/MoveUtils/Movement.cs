using DroneDeliverySystem.Agents;

namespace DroneDeliverySystem.MoveUtils
{
    public abstract class Movement : IMovement
    {
        abstract public void Move(Drone drone);
    }
}
