using System.Windows.Media;

namespace Evade
{
    class Block : Entity
    {
        public Block(int size, int speed) : this(size, speed, 0, 0)
        { }
        public Block(int size, int speed, int x, int y) : base(size, Constants.BLOCKCOLOR, x, y)
        {
            this.MoveSpeed = speed;
        }

        public int MoveSpeed { get; set; }
    }
}
