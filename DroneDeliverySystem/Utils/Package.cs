using DroneDeliverySystem.DisplayUtils;
using System;
using System.Windows.Forms;

namespace DroneDeliverySystem.Utils
{
    public class Package : MovingObject, IDisposable
    {
        public Position Position { get; set; }
        public Position ReceiverPosition { get; set; }
        public TitleLabel Title { get; set; }
        public int ID { get; set; }
        public Package(int id, Position position, Position receiverPosition, PictureBox icon)
        {
            Position = position;
            ReceiverPosition = receiverPosition;
            item = icon;
            ID = id;
        }
        public void SetIcon(PictureBox icon)
        {
            item = icon;
        }

        public void Release()
        {
            Title.Dispose();
            Dispose();
        }

        public override void Move(Position newPosition)
        {
            Title.Move(newPosition);
            base.Move(newPosition);
        }
    }
}
