namespace TowersOfHanoi
{
    class Ring
    {
        private Ring(uint width)
        {
            if (width > 0){
                Width = width;
            }
        }
        public uint Width{get;}

        public static Ring CreateRing(uint i)
        {
            return new Ring(i);
        }
    }
}
