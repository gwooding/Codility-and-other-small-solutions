using System;
using System.Collections.Generic;

namespace TowersOfHanoi
{
    class Tower
    {
        public Stack<Ring> Rings = new Stack<Ring>();

        public void Print()
        {
            int stackLength = Rings.Count;
            for (int i = 0; i < stackLength; i++)
            {
                Console.WriteLine(Rings.Peek().Width);
                Ring removedRing = Rings.Pop();
            }
        }
    }
}
