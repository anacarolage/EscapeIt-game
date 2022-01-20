using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EscapeRoom
{
    class StartScene : GameScene
    {
        public Component component;
        private SpriteBatch spriteBatch;


        SpriteFont font;
        public Texture2D background;

        public string name = "";
        public Rectangle inputName;

        public List<Rectangle> buttons = new List<Rectangle>();
        public List<Texture2D> images = new List<Texture2D>();

        public int SelectedMenu = 0;

        private Keys[] lastPressedKeys = new Keys[5];
        private string[] alphabet = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };


        public StartScene(Game game) : base(game)
        {
            GameWorld g = (GameWorld)game;

            this.spriteBatch = g.spriteBatch;
            background = game.Content.Load<Texture2D>("background/background_start");

            font = game.Content.Load<SpriteFont>("input");

            component = new Component("startScene", game, spriteBatch, background, buttons, images, font);
            this.Components.Add(component);


        }
        public override void Update(GameTime gameTime)
        {
            GetKeys();

            MouseState mouseState = Mouse.GetState();
            int mx = mouseState.X;
            int my = mouseState.Y;

            if ((mx >= 111 && mx <= 239) && (my >= 237 && my <= 292))  //Score button
            {
                SelectedMenu = 1;
            }
            else if ((mx >= 111 && mx <= 239) && (my >= 329 && my <= 382))  //Help button
            {
                SelectedMenu = 2;
            }
            else if ((mx >= 111 && mx <= 239) && (my >= 426 && my <= 478))  //About button
            {
                SelectedMenu = 3;
            }
            else if ((mx >= 772 && mx <= 899) && (my >= 423 && my <= 476))  //Play button
            {
                SelectedMenu = 4;
            }
            else
            {
                SelectedMenu = 0;
            }

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(font, Shared.name, new Vector2(796, 342), Color.Black);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }


        public void GetKeys()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            Keys[] currentKeys = keyboardState.GetPressedKeys();

            foreach (Keys key in currentKeys)
            {
                if (!lastPressedKeys.Contains(key))
                {
                    if (key == Keys.Back && Shared.name.Length > 0)
                    {
                        Shared.name = Shared.name.Remove(Shared.name.Length - 1);
                    }
                    else if (key == Keys.Space && Shared.name.Length < 15)
                    {
                        Shared.name += " ";
                    }
                    else if (alphabet.Contains(key.ToString()) && Shared.name.Length < 15)
                    {
                        Shared.name += key.ToString();
                    }

                }
            }

            lastPressedKeys = currentKeys;
        }

    }
}
