using System;
using System.Collections.Generic;
using System.Linq;

namespace TowersOfHanoi
{
    class Program
    {
        static List<Tower> ThreePoles;

        static uint RingHeight;

        static void Main(string[] args)
        {
            ThreePoles = new List<Tower>() {
                new Tower(),
                new Tower(),
                new Tower(),
            };

            try
            {
                RingHeight = uint.Parse(args[0]);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            ThreePoles[0] = TowerRingBuilder(ThreePoles[0], RingHeight);

            Move(0, 1, RingHeight-1);
            Move(0, 2, 1);
            Move(1, 2, RingHeight-1);

            ThreePoles[2].Print();

            Console.WriteLine("Press enter to quit");
            Console.ReadLine();
        }

        public static void Move(int startTowerIndex, int endTowerIndex, uint numberOfRingsToMove)
        {
            if(numberOfRingsToMove == 1)
            {
                if(ThreePoles[endTowerIndex].Rings.Count() == 0||
                    ThreePoles[startTowerIndex].Rings.Peek().Width <= ThreePoles[endTowerIndex].Rings.Peek().Width)
                {
                    Ring topRing = ThreePoles[startTowerIndex].Rings.Pop();
                    ThreePoles[endTowerIndex].Rings.Push(topRing);
                }
                else
                {
                    Console.WriteLine("Invalid Move Occurred");
                }
            }
            else
            {
                Move(startTowerIndex, 3 - startTowerIndex - endTowerIndex, numberOfRingsToMove - 1);
                Move(startTowerIndex, endTowerIndex, 1);
                Move(3 - startTowerIndex - endTowerIndex, endTowerIndex, numberOfRingsToMove - 1);
            }
        }

        public static Tower TowerRingBuilder(Tower tower, uint height)
        {
            while (height > 0)
            {
                tower.Rings.Push(Ring.CreateRing(height));

                height--;
            }

            return tower;
        }
    }
}
