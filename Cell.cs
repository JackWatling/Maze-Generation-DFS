using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Maze_Generation_Depth_First
{
    class Cell
    {
        public static int width;
        public static int height;

        public int x, y;
        public List<Direction> sides;
        public bool visited;

        public Cell(int width, int height, int x, int y)
        {
            this.x = x;
            this.y = y;
            visited = false;
            sides = new List<Direction>();
            validateSides(width, height);
        }

        private void validateSides(int width, int height)
        {
            if (y > 2) sides.Add(Direction.NORTH);
            if (y < height - 2) sides.Add(Direction.SOUTH);
            if (x > 2) sides.Add(Direction.WEST);
            if (x < width - 2) sides.Add(Direction.EAST);
        }

        public void removeSide(Direction side)
        {
            sides.Remove(side);
        }

        public void Draw(SpriteBatch s, Texture2D t)
        {
            s.Draw(t, new Vector2(x * Cell.width, y * Cell.height), visited ? Color.White : Color.Red);
        }

    }
}
