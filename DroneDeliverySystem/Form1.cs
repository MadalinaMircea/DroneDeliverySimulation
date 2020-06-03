using DroneDeliverySystem.Agents;
using DroneDeliverySystem.DisplayUtils;
using DroneDeliverySystem.Global;
using DroneDeliverySystem.Utils.Containers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DroneDeliverySystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            GlobalInformation.SetWinnerLabel(WinnerLabel);
            GlobalInformation.SetConsole(new DisplayConsole(console));
            GlobalInformation.CreateEnvironment();
            List<Producer> producers = GlobalInformation.CreateProducers(this);
            List<Drone> drones = GlobalInformation.CreateDrones();
            AddIcons(drones);
            AddTitles(drones);
            GlobalInformation.AddDroneObservables(drones, producers);

            AddDisplayLine("Console");
            AddDisplayLine("Hello world");

            StartButton.Enabled = true;

            Invalidate();

            //testPriorityQueue();
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
            StartButton.Text = "Start";
            GlobalInformation.StartAll();
        }

        private void testPriorityQueue()
        {
            IncreasingPriorityQueue<int> pq = new IncreasingPriorityQueue<int>();
            Debug.Assert(pq.Count() == 0);

            pq.Add(new PriorityPair<int>(1, 1));
            Debug.Assert(pq.Count() == 1);
            Debug.Assert(pq.Peek().Element == 1);
            Debug.Assert(pq.Peek().Priority == 1);

            pq.Add(new PriorityPair<int>(2, 5));
            Debug.Assert(pq.Count() == 2);
            Debug.Assert(pq.Peek().Element == 2);
            Debug.Assert(pq.Peek().Priority == 5);

            pq.Add(new PriorityPair<int>(3, 1));
            Debug.Assert(pq.Count() == 3);
            Debug.Assert(pq.Peek().Element == 2);
            Debug.Assert(pq.Peek().Priority == 5);

            pq.Add(new PriorityPair<int>(4, 1));
            Debug.Assert(pq.Count() == 4);
            Debug.Assert(pq.Peek().Element == 2);
            Debug.Assert(pq.Peek().Priority == 5);

            pq.Add(new PriorityPair<int>(5, 8));
            Debug.Assert(pq.Count() == 5);
            Debug.Assert(pq.Peek().Element == 5);
            Debug.Assert(pq.Peek().Priority == 8);

            pq.Add(new PriorityPair<int>(6, 2));
            Debug.Assert(pq.Count() == 6);
            Debug.Assert(pq.Peek().Element == 5);
            Debug.Assert(pq.Peek().Priority == 8);

            PriorityPair<int> p = pq.PopFront();
            Debug.Assert(p.Element == 5);
            Debug.Assert(p.Priority == 8);
            Debug.Assert(pq.Count() == 5);
            Debug.Assert(pq.Peek().Element == 2);
            Debug.Assert(pq.Peek().Priority == 6);

            pq.Remove(4);
            Debug.Assert(pq.Count() == 4);
            Debug.Assert(pq.Peek().Element == 2);
            Debug.Assert(pq.Peek().Priority == 6);

            pq.Remove(2);
            Debug.Assert(pq.Count() == 3);
            Debug.Assert(pq.Peek().Element == 1);
            Debug.Assert(pq.Peek().Priority == 3);

            p = pq.PopFront();
            Debug.Assert(p.Element == 1);
            Debug.Assert(p.Priority == 3);
            Debug.Assert(pq.Count() == 2);
            Debug.Assert(pq.Peek().Element == 3);
            Debug.Assert(pq.Peek().Priority == 2);

            pq.Remove(6);
            Debug.Assert(pq.Count() == 1);
            Debug.Assert(pq.Peek().Element == 3);
            Debug.Assert(pq.Peek().Priority == 2);

            p = pq.PopFront();
            Debug.Assert(p.Element == 3);
            Debug.Assert(p.Priority == 2);
            Debug.Assert(pq.Count() == 0);
        }
    }
}
