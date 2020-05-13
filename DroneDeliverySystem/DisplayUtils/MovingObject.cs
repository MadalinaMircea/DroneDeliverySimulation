using DroneDeliverySystem.Agents;
using DroneDeliverySystem.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DroneDeliverySystem.DisplayUtils
{
    public class MovingObject
    {
        public Control item;

        delegate void SafeBringToFrontDelegate();
        delegate void SafePositionCallDelegate(Position position);
        delegate void SafeDisposeCallDelegate();

        virtual public void Move(Position newPosition)
        {
            ChangePositionSafe(newPosition);
        }

        public void ChangePositionSafe(Position position)
        {
            if (item.InvokeRequired)
            {
                var d = new SafePositionCallDelegate(ChangePositionSafe);
                item.Invoke(d, new object[] { position });
            }
            else
            {
                item.Location = new Point(position.X, position.Y);
            }
        }

        public void DisposeSafe()
        {
            if (item.InvokeRequired)
            {
                var d = new SafeDisposeCallDelegate(DisposeSafe);
                item.Invoke(d, new object[] { });
            }
            else
            {
                item.Dispose();
            }
        }

        public void Dispose()
        {
            DisposeSafe();
        }

        private void SafeBringToFront()
        {
            if (item.InvokeRequired)
            {
                var d = new SafeBringToFrontDelegate(SafeBringToFront);
                item.Invoke(d, new object[] { });
            }
            else
            {
                item.BringToFront();
            }
        }

        public virtual void Notify(Drone d, bool changeColor, bool toFront)
        {
            ChangePositionSafe(d.Position);
            if (toFront)
            {
                SafeBringToFront();
            }
        }
    }
}
