using DroneDeliverySystem.Agents;
using DroneDeliverySystem.DisplayUtils;
using DroneDeliverySystem.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DroneDeliverySystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            GlobalInformation.SetConsole(new DisplayConsole(console));
            GlobalInformation.CreateEnvironment();
            List<Producer> producers = GlobalInformation.CreateProducers(this);
            //GlobalInformation.StartAgents(producers);
            List<Drone> drones = GlobalInformation.CreateDrones();
            AddIcons(drones);
            AddTitles(drones);
            GlobalInformation.AddDroneObservables(drones, producers);
            //GlobalInformation.StartAgents(drones);

            AddDisplayLine("Console");
            AddDisplayLine("Hello world");

            PauseButton.Enabled = false;
            StopButton.Enabled = false;
            StartButton.Enabled = true;

            Invalidate();
        }

        private void AddIcons(List<Drone> drones)
        {
            foreach (Drone d in drones)
            {
                d.AddObserver(new AgentIcon(CreateDroneIcon(d)));
            }
        }

        private void AddTitles(List<Drone> drones)
        {
            foreach (Drone d in drones)
            {
                d.AddObserver(new TitleLabel(CreateDroneLabel(d)));
            }
        }

        private PictureBox CreateDroneIcon(Drone d)
        {
            var picture = new PictureBox
            {
                Name = d.ToString(),
                Size = new Size(40, 40),
                Location = new Point(d.Position.X, d.Position.Y),
                Image = Properties.Resources.drone,

            };

            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            picture.BackColor = Color.SpringGreen;
            Controls.Add(picture);
            picture.BringToFront();

            return picture;
        }

        private Label CreateDroneLabel(Drone d)
        {
            var title = new Label
            {
                Text = d.ID.ToString(),
                ForeColor = Color.White,
                Location = new Point(d.Position.X, d.Position.Y),
                Size = new Size(25, 15),
            };
            title.BackColor = System.Drawing.Color.Black;


            Controls.Add(title);
            title.BringToFront();

            return title;
        }

        void AddDisplayLine(string line)
        {
            GlobalInformation.WriteToConsole(line);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //private void background_Click(object sender, EventArgs e)
        //{
        //    GlobalInformation.CreateRandomPackage();
        //}

        private void console_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            console.SelectionStart = console.Text.Length;
            // scroll it automatically
            console.ScrollToCaret();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            StartButton.Enabled = false;
            PauseButton.Enabled = true;
            StopButton.Enabled = true;
            StartButton.Text = "Start";
            PauseButton.Text = "Pause";
            GlobalInformation.StartAll();
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            StartButton.Text = "Resume";
            StartButton.Enabled = true;
            PauseButton.Enabled = false;
            StopButton.Enabled = false;
            GlobalInformation.PauseAll();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            StartButton.Enabled = false;
            StopButton.Enabled = false;
            PauseButton.Enabled = false;
            GlobalInformation.StopAll();
        }
    }
}
