using DroneDeliverySystem.Agents;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DroneDeliverySystem.DisplayUtils
{
    class AgentIcon : MovingObject
    {
        public AgentIcon(PictureBox icon)
        {
            item = icon;
        }

        public override void Notify(Drone d, bool changeColor, bool toFront)
        {
            base.Notify(d, changeColor, toFront);
            if (changeColor)
            {
                if (d.IsAvailable)
                {
                    item.BackColor = Color.SpringGreen;
                }
                else
                {
                    item.BackColor = Color.Red;
                }
            }
        }
    }
}
