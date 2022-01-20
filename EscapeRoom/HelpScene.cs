using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EscapeRoom
{
    class HelpScene : GameScene
    {
        private SpriteBatch spriteBatch;
        private Texture2D helpSprite;

        public Component component;

        public Texture2D btnBack_image;
        public Rectangle btnBack;

        public List<Rectangle> buttons = new List<Rectangle>();
        public List<Texture2D> images = new List<Texture2D>();

        public HelpScene(Game game) : base(game)
        {
            GameWorld g = (GameWorld)game;
            this.spriteBatch = g.spriteBatch;
            helpSprite = g.Content.Load<Texture2D>("background/background_help");
            btnBack_image = game.Content.Load<Texture2D>("buttons/btnBack");

            images.Add(btnBack_image);

            btnBack = new Rectangle(50, 500, 110, 50);
            buttons.Add(btnBack);

            component = new Component("helpScene", game, spriteBatch, helpSprite, buttons, images, null);
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
