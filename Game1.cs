using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Maze_Generation_Depth_First
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Random rand;
        Texture2D wall;
        int width, height;

        Cell[,] cells;
        Stack<Cell> stack;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            rand = new Random();
            stack = new Stack<Cell>(50);

            Cell.width = 10;
            Cell.height = 10;

            width = 21;
            height = 21;

            initialiseCells();

            graphics.PreferredBackBufferWidth = width * Cell.width;
            graphics.PreferredBackBufferHeight = height * Cell.height;
            graphics.ApplyChanges();

            generate(1, 1);

            base.Initialize();
        }

        private void initialiseCells()
        {
            cells = new Cell[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    cells[x, y] = new Cell(width, height, x, y);
                }
            }
        }

        private void generate(int x, int y)
        {
            Cell current = cells[x, y];
            current.visited = true;

            if (current.sides.Count != 0)
            {

                Direction dir = current.sides.ElementAt(rand.Next(current.sides.Count));
                //Console.WriteLine(current.sides.Count + " " + dir);
                current.sides.Remove(dir);

                stack.Push(current);

                switch (dir)
                {
                    case Direction.NORTH:
                        cells[x, y - 2].sides.Remove(Direction.SOUTH);
                        if (!cells[x, y - 2].visited)
                        {
                            cells[x, y - 1].visited = true;
                            generate(x, y - 2);
                        }
                        else
                        {
                            generate(x, y);
                        }
                        break;
                    case Direction.WEST:
                        cells[x - 2, y].sides.Remove(Direction.EAST);
                        if (!cells[x - 2, y].visited)
                        {
                            cells[x - 1, y].visited = true;
                            generate(x - 2, y);
                        }
                        else
                        {
                            generate(x, y);
                        }
                        break;

                    case Direction.SOUTH:
                        if (!cells[x, y + 2].visited)
                        {
                            cells[x, y + 2].sides.Remove(Direction.NORTH);
                            cells[x, y + 1].visited = true;
                        }
                        else
                        {
                            generate(x, y);
                        }                   
                        generate(x, y + 2);
                        break;
                    case Direction.EAST:
                        cells[x + 2, y].sides.Remove(Direction.WEST);
                        if (!cells[x + 2, y].visited)
                        {
                            cells[x + 1, y].visited = true;
                            generate(x + 2, y);
                        }
                        else
                        {
                            generate(x, y);
                        }
                        break;
                }

                Cell g = stack.Pop();
                generate(g.x, g.y);
            }



            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            wall = Content.Load<Texture2D>("Textures\\wall");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    cells[x, y].Draw(spriteBatch, wall);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
