using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EscapeRoom
{
    class EndScene : GameScene
    {
        private SpriteBatch spriteBatch;
        private Texture2D endSprite;

        public Component component;


        public List<Rectangle> buttons = new List<Rectangle>();
        public List<Texture2D> images = new List<Texture2D>();

        public EndScene(Game game) : base(game)
        {
            GameWorld g = (GameWorld)game;
            this.spriteBatch = g.spriteBatch;
            endSprite = g.Content.Load<Texture2D>("background/background_black");

            component = new Component("endScene", game, spriteBatch, endSprite, buttons, images, null);
            this.Components.Add(component);
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }
    }
}
