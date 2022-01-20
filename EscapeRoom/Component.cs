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
    class Component: DrawableGameComponent
    {
        public Game game;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        public TimeSpan timer;
        public Viewport viewport;


        public int SelectedMenu = 0;
        public int Back = 0;
        public bool EndGame = false;
        public bool showError = false;


        private Texture2D background;
        public List<Texture2D> images = new List<Texture2D>();
        public List<Rectangle> buttons;
        private string scene;

        public bool skullClicked = false;
        int x = 656;

        private Puzzle.Tile tile;
        private Memory.Memory memory;
        private Smoke smoke;


        private KeyboardState previousState;

        public bool puzzleUp = false;
        public bool puzzleWin = false;
        public bool puzzleLoad = false;

        public Texture2D puzzleFull;
        public bool memoryUp = false;
        public bool memoryLoad = false;
        public bool memoryWin = false;

        public List<string> scores = new List<string>();
        private string numbers = "1.\n2.\n3.\n4.\n5.\n6.\n7.\n8.\n9.\n10.";
        private string names = "";
        private string times = "";




        public Component(string scene, Game game, SpriteBatch spriteBatch, Texture2D background, List<Rectangle> buttons, List<Texture2D> images, SpriteFont font) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.background = background;
            this.buttons = buttons;
            this.images = images;
            this.scene = scene;
            this.font = font;
            this.game = game;

            this.smoke = new Smoke(game.Content);


            if (scene == "playScene")
            {
                viewport = new Viewport(10, 10, 400, 400);
                Vector2 screenDimension = new Vector2(viewport.Width, viewport.Height);

                puzzleFull = game.Content.Load<Texture2D>("puzzle/Images/Full_image");

            }
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            int mx = mouseState.X;
            int my = mouseState.Y;

            // Back button from scenes About, Help and Score
            if(scene == "aboutScene" || scene == "helpScene" || scene == "scoreScene")
            {
                if (mx >= buttons[0].X && mx <= buttons[0].X + buttons[0].Width)
                {
                    if (my >= buttons[0].Y && my <= buttons[0].Y + buttons[0].Height)
                    {
                        Back = 1;
                    }
                }
                else
                {
                    Back = 0;

                }
            }

            // Show high scores from file
            if (scene == "scoreScene" && !Shared.scoreLoaded)
            {
                names = "";
                times = "";
                scores = FileHandler.Load();

                foreach (string s in scores)
                {
                    string[] array = s.Split(';');
                    names += array[0] + "\n";
                    times += array[1] + "\n";

                }
                Shared.scoreLoaded = true;
            }

            // Handle minigames 
            if (scene == "playScene")
            {
                if (!puzzleWin)
                {
                    if (!puzzleLoad)
                    {
                        tile = new Puzzle.Tile(game.Content, this);
                        puzzleLoad = true;
                    }
                    KeyboardState currentState = Keyboard.GetState();

                    tile.Update(gameTime, currentState, previousState);

                    previousState = currentState;
                }

                if (!memoryWin)
                {
                    if (!memoryLoad)
                    {
                        memory = new Memory.Memory(game.Content, this);
                        memoryLoad = true;

                    }
                    smoke.Update(gameTime);
                    memory.Update(gameTime);

                }

            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            for (int i = 0; i < images.Count; i++)
            {
                spriteBatch.Draw(images[i], buttons[i], Color.White);
            }

            if (scene == "startScene" && font != null)
            {
                if (!string.IsNullOrEmpty(Shared.name))
                {
                    spriteBatch.DrawString(font, Shared.name, new Vector2(796, 342), Color.Black);  // Name typed
                    showError = false;
                }
                
                if (showError)
                {
                    spriteBatch.DrawString(font, "Please enter name", new Vector2(755, 383), Color.Red);

                }

            }

            if (scene == "playScene" && font != null)
            {
                spriteBatch.DrawString(font, timer.ToString(@"hh\:mm\:ss"), new Vector2(892, 28), Color.Black);

                if (puzzleUp)
                {
                    tile.Draw(gameTime, spriteBatch);
                }

                if (puzzleWin)
                {
                    if (!Shared.smokeLoaded)
                    {
                        smoke.Enable = true;
                        Shared.smokeLoaded = true;
                    }
                    spriteBatch.Draw(puzzleFull, new Rectangle(651, 159, 115, 115), Color.White);
                    smoke.Draw(gameTime, spriteBatch);

                }

                if (memoryUp)
                {
                    memory.Draw(gameTime, spriteBatch);
                }

                if (skullClicked)
                {
                    if(x < 706)
                    {

                        buttons[1] = new Rectangle(x, 350, 94, 119); //move skull
                        x++;
                    }
                    else
                    {
                        buttons[1] = new Rectangle(706, 350, 94, 119); 
                    }

                }
            }

            if (scene == "scoreScene" && font != null)
            {
                spriteBatch.DrawString(font, numbers, new Vector2(310, 230), Color.Black);
                spriteBatch.DrawString(font, names, new Vector2(345, 230), Color.Black);
                spriteBatch.DrawString(font, times, new Vector2(600, 230), Color.Black);
            }

            spriteBatch.End();



            base.Draw(gameTime);
        }

    }
}
