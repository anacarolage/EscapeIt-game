using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzle
{
    class Tile
    {
        private Texture2D fullImage;

        private Texture2D empty;
        public Texture2D puzzleAnswer;
        public Texture2D background;

        private Texture2D[] textures = new Texture2D[16];

        private Rectangle[,] positions = new Rectangle[4,4];

        private bool[,] empties = new bool[4,4];

        private Texture2D[,] texturesShuffled = new Texture2D[4,4];

        string[,] answerSheet = new string[4, 4];

        private Random random = new Random();

        public Vector2 FullImagePosition;

        private SoundEffect tileSound;
        private SoundEffect winSound;

        private bool winState;

        private EscapeRoom.Component component;

        public Tile(ContentManager Content, EscapeRoom.Component component)
        {
            this.component = component;
            fullImage = Content.Load<Texture2D>("puzzle/Images/Full_image");

            puzzleAnswer = Content.Load<Texture2D>("room-items/puzzle_answer");
            background = Content.Load<Texture2D>("room-items/puzzle_background");


            empty = Content.Load<Texture2D>("puzzle/Images/Untitled");
            empty.Tag = "empty";

            textures[0] = Content.Load<Texture2D>("puzzle/Images/image_part_001");
            textures[1] = Content.Load<Texture2D>("puzzle/Images/image_part_002");
            textures[2] = Content.Load<Texture2D>("puzzle/Images/image_part_003");
            textures[3] = Content.Load<Texture2D>("puzzle/Images/image_part_004");
            textures[4] = Content.Load<Texture2D>("puzzle/Images/image_part_005");
            textures[5] = Content.Load<Texture2D>("puzzle/Images/image_part_006");
            textures[6] = Content.Load<Texture2D>("puzzle/Images/image_part_007");
            textures[7] = Content.Load<Texture2D>("puzzle/Images/image_part_008");
            textures[8] = Content.Load<Texture2D>("puzzle/Images/image_part_009");
            textures[9] = Content.Load<Texture2D>("puzzle/Images/image_part_010");
            textures[10] = Content.Load<Texture2D>("puzzle/Images/image_part_011");
            textures[11] = Content.Load<Texture2D>("puzzle/Images/image_part_012");
            textures[12] = Content.Load<Texture2D>("puzzle/Images/image_part_013");
            textures[13] = Content.Load<Texture2D>("puzzle/Images/image_part_014");
            textures[14] = Content.Load<Texture2D>("puzzle/Images/image_part_015");



            for (int i = 0; i < 15; i++)
            {
                textures[i].Tag = i;
            }

            tileSound = Content.Load<SoundEffect>("puzzle/Sounds/TileSound");
            winSound = Content.Load<SoundEffect>("puzzle/Sounds/WinSound");

            StartGame();
        }


        public void StartGame()
        {
            //List<int> randomNumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            //randomNumbers = randomNumbers.OrderBy(item => random.Next(0, 80)).ToList();

            List<int> randomNumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 9, 10, 11, 14, 8, 12, 13};


            int m = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (!(i == 3 && j == 3))
                    {
                        texturesShuffled[i, j] = textures[randomNumbers[m]];
                        empties[i, j] = false;
                        m++;
                    }
                    else
                    {
                        texturesShuffled[3, 3] = empty;
                        empties[3, 3] = true;
                    }
                }
            }

            int n = 0;
            Vector2 vector = new Vector2(0, 0);
            int gapX = 0;
            int gapY = 0;

            for (int i = 0; i < 4; i++)
            {
                gapX++;
                gapY = 0;
                for (int j = 0; j < 4; j++)
                {
         
                    positions[i, j] = new Rectangle(j * 100 + 210 + gapY, i * 100 + 110 + gapX, 100, 100);
                    n++;
                    gapY++;
                }
            }

            int tag = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j<4; j++)
                {
                    answerSheet[i, j] = tag.ToString();
                    tag++;
                }

            }
            answerSheet[3, 3] = "empty";

            winState = false;

        }



        //game loop
        public void Update(GameTime gameTime, KeyboardState currentState, KeyboardState previousState)
        {
            if (!winState && component.puzzleUp)
            {
                MouseState mouseState = Mouse.GetState();

                int mx = mouseState.X;
                int my = mouseState.Y;
                float tx;
                float ty;

                int iFrom = -1;
                int jFrom = -1;

                int iTo = -1;
                int jTo = -1;

                bool valid = false;

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (empties[i, j])
                        {
                            iTo = i;
                            jTo = j;
                        }
                    }
                }

                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            tx = positions[i, j].X;
                            ty = positions[i, j].Y;
                            if (mx >= tx && mx <= (tx + 100))
                            {
                                if (my >= ty && my <= ty + 100)
                                {
                                    iFrom = i;
                                    jFrom = j;
                                }
                            }

                        }
                    }
                }
                else
                {
                    if (currentState.IsKeyDown(Keys.Right) && previousState.IsKeyUp(Keys.Right))
                    {
                        if (jTo != 0)
                        {
                            jFrom = jTo - 1;
                            iFrom = iTo;
                        }

                    }
                    else if (currentState.IsKeyDown(Keys.Left) && previousState.IsKeyUp(Keys.Left))
                    {
                        if (jTo != 3)
                        {
                            jFrom = jTo + 1;
                            iFrom = iTo;
                        }
                    }
                    else if (currentState.IsKeyDown(Keys.Up) && previousState.IsKeyUp(Keys.Up))
                    {
                        if (iTo != 3)
                        {
                            iFrom = iTo + 1;
                            jFrom = jTo;
                        }
                    }
                    else if (currentState.IsKeyDown(Keys.Down) && previousState.IsKeyUp(Keys.Down))
                    {
                        if (iTo != 0)
                        {
                            iFrom = iTo - 1;
                            jFrom = jTo;
                        }
                    }
                }

                if (iFrom != -1)
                {

                    if (iFrom == iTo && (jFrom == jTo + 1 || jFrom == jTo - 1))
                    {
                        valid = true;
                    }
                    else if (jFrom == jTo && (iFrom == iTo + 1 || iFrom == iTo - 1))
                    {
                        valid = true;

                    }
                }

                if (valid)
                {
                    tileSound.Play();
                    empties[iTo, jTo] = false;
                    empties[iFrom, jFrom] = true;
                    Texture2D temp = texturesShuffled[iFrom, jFrom];
                    texturesShuffled[iFrom, jFrom] = texturesShuffled[iTo, jTo];
                    texturesShuffled[iTo, jTo] = temp;

                }

                if (CheckWin(texturesShuffled))
                {
                    winSound.Play();
                    winState = true;
                    component.puzzleWin = true;
                }

            }
        }



        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            if (component.puzzleWin)
            {
                spriteBatch.Draw(puzzleAnswer, new Rectangle(150, 150, 417, 142), Color.White);
            }
            else
            {
                spriteBatch.Draw(background, new Rectangle(200, 100, 423, 423), Color.White);

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        spriteBatch.Draw(texturesShuffled[i, j], positions[i, j], Color.White);

                    }

                }
            }

        }


        private bool CheckWin(Texture2D[,] texturesShuffled)
        {
            for (int i = 0; i<4; i++)
            {
                for (int j = 0; j<4; j++)
                {
                    if (answerSheet[i,j] != texturesShuffled[i, j].Tag.ToString())
                    {
                        return false;
                    }
                }
            }
            return true;

        }

    }
}
