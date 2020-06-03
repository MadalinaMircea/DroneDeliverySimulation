using System.Windows.Forms;

namespace DroneDeliverySystem.DisplayUtils
{
    class ChangingLabel
    {
        Label label;
        delegate void SafeSetNameCallDelegate(string name);

        public ChangingLabel(Label l)
        {
            label = l;
        }

        private void SetNameSafe(string name)
        {
            if (label.InvokeRequired)
            {
                var d = new SafeSetNameCallDelegate(SetNameSafe);
                label.Invoke(d, new object[] { name });
            }
            else
            {
                label.Text = name;
            }
        }

        public void SetName(string name)
        {
            SetNameSafe(name);
        }
    }
}
