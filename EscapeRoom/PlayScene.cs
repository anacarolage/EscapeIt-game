using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


namespace EscapeRoom
{
    class PlayScene : GameScene
    {
        private Game game;
        private SpriteBatch spriteBatch;
        private Texture2D playSprite;

        public Component component;

        public List<Rectangle> buttons = new List<Rectangle>();
        public List<Texture2D> images = new List<Texture2D>();

        private MouseState previousState;
        private bool movedSkull = false;
        private bool paperUp = false;
        private bool lockUp = false;
        private bool hintUp = false;

        public string password = "";

        private SoundEffect buzzer;
        private SoundEffect beep;



        string time;
        TimeSpan timer, eTime, cTime;
        SpriteFont font;

        public PlayScene(Game game) : base(game)
        {
            GameWorld g = (GameWorld)game;
            this.game = game;
            this.spriteBatch = g.spriteBatch;
            playSprite = g.Content.Load<Texture2D>("background/background_gameplay");

            images.Add(game.Content.Load<Texture2D>("buttons/btnHint"));
            images.Add(game.Content.Load<Texture2D>("room-items/skull"));
            images.Add(game.Content.Load<Texture2D>("room-items/painting_scrambled"));
            images.Add(game.Content.Load<Texture2D>("buttons/challenge0"));
            images.Add(game.Content.Load<Texture2D>("room-items/timer"));
            buttons.Add(new Rectangle(10, 530, 50, 50));  //hint
            buttons.Add(new Rectangle(656, 350, 94, 119));  //skull
            buttons.Add(new Rectangle(651, 159, 115, 115));  //picture
            buttons.Add(new Rectangle(10, 10, 131, 51));  //challenge tracker
            buttons.Add(new Rectangle(850, 10, 135, 51));  //timer background

            buzzer = game.Content.Load<SoundEffect>("sounds/buzzer");
            beep = game.Content.Load<SoundEffect>("sounds/beep");

            this.time = "";
            this.cTime = new TimeSpan(0, 0, 0);

            font = game.Content.Load<SpriteFont>("input");

            component = new Component("playScene", game, spriteBatch, playSprite, buttons, images, font);
            this.Components.Add(component);


        }

        public override void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime;
            eTime = gameTime.TotalGameTime;
            if (eTime.Minutes < 10)
                time = "Time - 0" + (int)(eTime.Minutes);
            else
                time = "Time - " + (int)(eTime.Minutes);
            if (eTime.Seconds < 10)
                time += ":0" + (int)(eTime.Seconds);
            else
                time += ":" + (int)(eTime.Seconds);

            component.timer = timer;
            this.Components[0] = component;


            MouseState currentState = Mouse.GetState();

            int mx = currentState.X;
            int my = currentState.Y;

            if (currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
            {
         
                if (!component.puzzleUp && !movedSkull && !component.memoryUp && (mx >= buttons[1].X && mx <= buttons[1].X + buttons[1].Width) && (my >= buttons[1].Y && my <= buttons[1].Y + buttons[1].Height))
                {
                    component.skullClicked = true;

                    movedSkull = true;
                }
                else if (!component.puzzleUp && !paperUp && movedSkull && !component.memoryUp && (mx >= 672 && mx <= 705) && (my >= 436 && my <= 451))
                {
                    component.images.Add(game.Content.Load<Texture2D>("room-items/X"));
                    component.buttons.Add(new Rectangle(25, 20, 30, 30));  //show X
                    component.images.Add(game.Content.Load<Texture2D>("room-items/paper_balls"));
                    component.buttons.Add(new Rectangle(100, 100, 521, 177));  //show paper_balls
                    this.Components[0] = component;
                    paperUp = true;
                }
                else if (paperUp)
                {
                    component.images.RemoveAt(images.Count - 1);  //remove paper_balls
                    component.buttons.RemoveAt(buttons.Count - 1);

                    this.Components[0] = component;
                    paperUp = false;
                }

                if (!lockUp && !hintUp && !paperUp && !component.puzzleUp && !component.memoryUp && (mx >= 651 && mx <= 766) && (my >= 159 && my <= 274))
                {
                    component.images.Add(game.Content.Load<Texture2D>("room-items/X"));
                    component.buttons.Add(new Rectangle(60, 20, 30, 30));  //show X

                    component.puzzleUp = true;
                    component.puzzleLoad = false;

                    this.Components[0] = component;
                }
                else if (component.puzzleUp && (mx <= 200 || mx >= 623 || my <= 100 || my >= 523))
                {
                    component.puzzleUp = false;

                }

                if (!component.memoryUp && !hintUp && !paperUp && !component.puzzleUp &&(mx >= 164 && mx <= 221) && (my >= 536 && my <= 552))
                {

                    component.memoryUp = true;
                    component.memoryLoad = false;

                    component.images.Add(game.Content.Load<Texture2D>("room-items/X"));
                    component.buttons.Add(new Rectangle(95, 20, 30, 30));  //show X
                    this.Components[0] = component;
                }
                else if (component.memoryUp && (mx <= 200 || mx >= 596 || my <= 50 || my >= 446))
                {

                    component.memoryUp = false;

                }

                if (!component.puzzleUp && !hintUp && (mx >= 10 && mx <= 60) && (my >= 530 && my <= 580))
                {
                    component.images.Add(game.Content.Load<Texture2D>("room-items/hint"));
                    component.buttons.Add(new Rectangle(200, 110, 512, 351));  //show hint
                    this.Components[0] = component;
                    hintUp = true;
                }
                else if (hintUp)
                {
                    component.images.RemoveAt(images.Count - 1);  //remove hint
                    component.buttons.RemoveAt(buttons.Count - 1);

                    this.Components[0] = component;
                    hintUp = false;
                }

                if (!component.puzzleUp && !lockUp && (mx >= 548 && mx <= 573) && (my >= 291 && my <= 350))
                {
                    component.images.Add(game.Content.Load<Texture2D>("room-items/number_lock"));
                    component.buttons.Add(new Rectangle(200, 110, 181, 351));  //show number_lock
                    this.Components[0] = component;
                    lockUp = true;
                    password = "";
                }
                else if (lockUp)
                {
                    if (mx <= 200 || mx >= 381 || my <= 110 || my >= 461)
                    {
                        component.images.RemoveAt(images.Count - 1);  //remove number_lock
                        component.buttons.RemoveAt(buttons.Count - 1);

                        this.Components[0] = component;
                        lockUp = false;
                    }
                    else
                    {
                        if ((mx >= 242 && mx <= 340) && (my >= 158 && my <= 289))
                        {
                            beep.Play();
                        }
                        if ((mx >= 247 && mx <= 265) && (my >= 158 && my <= 178))
                        {
                            password += "1";
                        }
                        else if ((mx >= 282 && mx <= 305) && (my >= 193 && my <= 216))
                        {
                            password += "5";
                        }
                        else if ((mx >= 318 && mx <= 339) && (my >= 230 && my <= 251))
                        {
                            password += "9";
                        }
                        else if ((mx >= 320 && mx <= 337) && (my >= 158 && my <= 179))
                        {
                            password += "3";
                        }
                        else if ((mx >= 248 && mx <= 267) && (my >= 267 && my <= 288))
                        {
                            if (password == "5193")
                            {
                                Shared.time = timer;
                                images = new List<Texture2D>();
                                buttons = new List<Rectangle>();
                                images.Add(game.Content.Load<Texture2D>("buttons/btnHint"));
                                images.Add(game.Content.Load<Texture2D>("room-items/skull"));
                                images.Add(game.Content.Load<Texture2D>("room-items/painting_scrambled"));
                                images.Add(game.Content.Load<Texture2D>("buttons/challenge0"));
                                buttons.Add(new Rectangle(10, 530, 50, 50));  //hint
                                buttons.Add(new Rectangle(656, 350, 94, 119));  //skull
                                buttons.Add(new Rectangle(651, 159, 115, 115));  //picture
                                buttons.Add(new Rectangle(10, 10, 131, 51));  //challenge tracker

                                component = new Component("playScene", game, spriteBatch, playSprite, buttons, images, font);
                                this.Components.Add(component);

                                component.EndGame = true;

                                movedSkull = false;
                                lockUp = false;
                                paperUp = false;
                                password = "";
                                timer = cTime;
                                

                                Score score = new Score(Shared.name, Shared.time);

                                FileHandler.Save(score);

                                Shared.name = "";
                            }
                            else
                            {
                                password = "";
                                buzzer.Play();
                            }

                        }
                    }

                }
            
            }

            previousState = currentState;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
