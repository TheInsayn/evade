using System.Windows.Media;

namespace Evade
{
    class Block : Entity
    {
        public Block(int size, Color c, int speed) : this(size,c, speed, 0, 0) 
        { }
        public Block(int size, Color c, int speed, int x, int y) : base(size, c, x, y)
        {
            this.MoveSpeed = speed;
        }

        public int MoveSpeed { get; set; }
    }
}
