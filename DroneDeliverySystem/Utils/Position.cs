using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliverySystem.Utils
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position(string message)
        {

            string[] pos = message.Split(',');
            X = Convert.ToInt32(pos[0].Trim());
            Y = Convert.ToInt32(pos[1].Trim());
        }

        public override string ToString()
        {
            return $"Position <{X}, {Y}>";
        }

        public static Position operator -(Position a, Position b)
        {
            return new Position(a.X - b.X, a.Y - b.Y);
        }
    }
}
