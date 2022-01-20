using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EscapeRoom
{
    class Smoke
    {

        private Texture2D sprite;
        public Vector2 Position;
        private Vector2 dimension;
        private List<Rectangle> frames;
        private int frameIndex = -1;
        private int delay = 5;
        private int delayCounter = 0;
        public bool Enable;

        public Smoke(ContentManager Content)
        {
            sprite = Content.Load<Texture2D>("room-items/smoke_sm");
            dimension = new Vector2(166, 166);
            Position = new Vector2(630, 139);
            CreateFrames();
            Enable = false;
        }


        private void CreateFrames()
        {
            frames = new List<Rectangle>();
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    int x = col * (int)dimension.X;
                    int y = row * (int)dimension.Y;
                    Rectangle grid = new Rectangle(x, y, (int)dimension.X, (int)dimension.Y);
                    frames.Add(grid);
                }
            }
        }


        // game loop
        public void Update(GameTime gameTime)
        {
            if (Enable)
            {
                delayCounter++;
                if (delayCounter > delay)
                {
                    frameIndex++;
                    if (frameIndex > 8)
                    {
                        frameIndex = -1;
                        Enable = false;
                    }
                    delayCounter = 0;
                }
            }
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Enable && (frameIndex >= 0))
                spriteBatch.Draw(sprite, Position, frames[frameIndex], Color.White);
        }


    }
}
