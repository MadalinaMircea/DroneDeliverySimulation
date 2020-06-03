using DroneDeliverySystem.DisplayUtils;
using DroneDeliverySystem.Global;
using DroneDeliverySystem.Messaging;
using DroneDeliverySystem.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DroneDeliverySystem.Agents
{
    public class Producer : Agent
    {
        delegate void SafeAddControlDelegate(Control ctrl);
        public Position position { get; set; }

        private Form1 form;

        private DateTime lastPackageCreated;
        private int minTimeBetweenPackages = 10;
        private int probabiltyOfPackage = 50;

        private Thread creatorThread = null;

        private List<Drone> observers = new List<Drone>();
        public void Changed(bool newPackage, Producer producer, Package package)
        {
            int newPack = newPackage ? 1 : 0;
            //string content = $"1-{newPack}-{producer.position.X},{producer.position.Y}-{producer.ID}-{package.ID}";
            string content = $"{producer.position.X},{producer.position.Y}-{producer.ID}-{package.ID}";
            foreach (Drone o in observers)
            {
                Send(ACLPerformative.REQUEST, ID, o.ID, content);
            }

            GlobalInformation.WriteToConsole("Producer " + ID + " " + content);
        }

        public void Add(Drone o)
        {
            observers.Add(o);
        }

        public Producer()
        {
            CreateThread();
        }

        public Producer(int id, string name, Position position)
        {
            this.position = position;
            //this.form = form;
            this.ID = id;
            lastPackageCreated = DateTime.Now;
            Name = name;
            CreateThread();
        }

        private double TimeSinceLastPackage()
        {
            return (DateTime.Now - lastPackageCreated).TotalSeconds;
        }

        private void CreateThread()
        {
            creatorThread = new Thread(new ThreadStart(CreatorThreadRun));
            //creatorThread.Start();
        }

        private void CreatorThreadRun()
        {
            while (true)
            {
                if (TimeSinceLastPackage() >= minTimeBetweenPackages)
                {
                    if (CurrentEnvironment.GetRandomInt(0, 100) <= probabiltyOfPackage)
                    {
                        CreatePackage();
                        Thread.Sleep(500);
                    }
                }
            }
        }

        private void SafeAddControl(Control ctrl)
        {
            if (form.InvokeRequired)
            {
                var d = new SafeAddControlDelegate(SafeAddControl);
                form.Invoke(d, new object[] { ctrl });
            }
            else
            {
                form.Controls.Add(ctrl);
                ctrl.BringToFront();
            }
        }

        public void SetForm(Form1 f)
        {
            form = f;
        }

        public Package CreatePackage()
        {
            int id = CurrentEnvironment.GetNextId();
            var picture = new PictureBox
            {
                Name = $"Package {id}",
                Size = new Size(45, 45),
                Location = new Point(position.X, position.Y),
                Image = Properties.Resources.parcel,

            };

            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            SafeAddControl(picture);

            Package package = new Package(id, position, CurrentEnvironment.GetRandomPosition(), picture);

            var title = new Label
            {
                Text = package.ID.ToString(),
                ForeColor = Color.White,
                Location = new Point(position.X, position.Y),
                Size = new Size(25, 15),
            };
            title.BackColor = System.Drawing.Color.Black;

            SafeAddControl(title);

            package.Title = new TitleLabel(title);

            GlobalInformation.AddPackage(this, package);

            Changed(true, this, package);

            lastPackageCreated = DateTime.Now;

            return package;
        }

        public bool HasNoObservers()
        {
            return observers.Count == 0;
        }

        public override void Stop()
        {
            StopThreads();
            base.Stop();
        }

        public override void Start()
        {
            StartThreads();
            base.Start();
        }

        private void StartThreads()
        {
            //isAlive = true;
            creatorThread.Start();
        }

        private void StopThreads()
        {
            //isAlive = false;
            creatorThread.Join();
        }

        //public override void Pause()
        //{
        //    StopThreads();
        //    CreateThread();
        //    base.Pause();
        //}

        //public override void Resume()
        //{
        //    StartThreads();
        //    base.Resume();
        //}
    }
}
