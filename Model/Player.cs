namespace Evade.Model
{
    class Player : Entity
    {
        public Player() : this(0, 0) { }
        public Player(int x, int y) : base(Constants.PLAYERSIZE, Constants.PLAYERCOLOR, x, y) { }
    }
}
