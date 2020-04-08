using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Evade.Model
{
    class CanvasAdapter
    {
        private Player player;
        private List<Block> blocks = new List<Block>();
        private readonly Canvas canvas;

        private readonly Random rand = new Random();

        public CanvasAdapter(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void CreateBlock()
        {
            int size = rand.Next(Constants.BLOCKMINSIZE, Constants.BLOCKMAXSIZE);
            int pos = rand.Next((int)canvas.Width - size);
            int speed = rand.Next(Constants.BLOCKMINSPEED, Constants.BLOCKMAXSPEED);
            Block block = new Block(size, speed)
            {
                X = pos,
                Y = -size // appear from outside canvas
            };
            blocks.Add(block);
            canvas.Children.Add(block.Rect);
            Canvas.SetLeft(block.Rect, block.X);
            Canvas.SetTop(block.Rect, block.Y);
        }

        public bool CheckBlockCollisions(float gamespeed)
        {
            var toRemove = new List<Block>();
            foreach (Block b in blocks)
            {
                // update position
                int y = b.Y;
                y += (int)(b.MoveSpeed * gamespeed);
                b.Y = y;
                Canvas.SetTop(b.Rect, y);
                // check collision
                if (b.Area.IntersectsWith(player.Area))
                    return true;
                // remove if touches bottom
                if (y + b.Size >= canvas.Height)
                    toRemove.Add(b);
            }
            foreach (Block b in toRemove)
            {
                blocks.Remove(b);
                canvas.Children.Remove(b.Rect);
            }
            return false; // no collisions with player
        }

        public void AddPlayer()
        {
            player = new Player();
            canvas.Children.Add(player.Rect);
            player.X = ((int)canvas.Width) / 2 - Constants.PLAYERSIZE / 2;
            player.Y = ((int)canvas.Height) - Constants.PLAYERSIZE * 5 / 3; // TODO: why?
            Canvas.SetLeft(player.Rect, player.X);
            Canvas.SetTop(player.Rect, player.Y);
        }

        public void UpdatePlayerPosition(int direction)
        {
            int movement = direction switch
            {
                -1 => -Constants.PLAYERMOVESPEED,
                1 => Constants.PLAYERMOVESPEED,
                _ => 0
            };

            if (movement != 0)
            {
                int newPos = player.X + movement;
                newPos = (int)Math.Clamp(newPos, 0, canvas.Width - Constants.PLAYERSIZE);
                player.X = newPos;
                Canvas.SetLeft(player.Rect, newPos);
            }
        }

        public void Reset()
        {
            blocks = new List<Block>();
            canvas.Children.Clear();
            player.X = (int)canvas.Width / 2 - Constants.PLAYERSIZE / 2;
            canvas.Children.Add(player.Rect);
            Canvas.SetLeft(player.Rect, player.X);
        }
    }
}
