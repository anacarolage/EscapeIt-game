using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Memory
{
    class Memory
    {
        private Texture2D question;
        private Texture2D numbers;
        private Texture2D memoryAnswer;

        private Dictionary<int, Rectangle> numbersPosition = new Dictionary<int, Rectangle>();
        private List<Board> gameFields = new List<Board>();
        private Point gameStart = new Point(200, 50);

        private int selectedFirst = -1;
        private int selectedSecond = -1;
        private double elapsedTime = 0;
        private double visibleTime = 2000;

        private EscapeRoom.Component component;
        private bool winState;

        private SoundEffect winSound;


        public Memory(ContentManager Content, EscapeRoom.Component component)
        {
            this.component = component;

            question = Content.Load<Texture2D>("memory/question");
            numbers = Content.Load<Texture2D>("memory/numbers");
            memoryAnswer = Content.Load<Texture2D>("room-items/memory_answer");
            winSound = Content.Load<SoundEffect>("puzzle/Sounds/WinSound");

            StartGame();
        }

        public void StartGame()
        {
            var id = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    numbersPosition.Add(id, new Rectangle(j * 100, i * 100, 100, 100));
                    id++;
                }
            }

            List<int> randomNumbers = new List<int>();

            for (int i = 0; i < 8; i++)
            {
                randomNumbers.Add(i);
                randomNumbers.Add(i);
            }

            var random = new Random();

            id = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var index = random.Next(0, randomNumbers.Count - 1);
                    var number = randomNumbers[index];
                    randomNumbers.RemoveAt(index);

                    gameFields.Add(new Board
                    {
                        id = id++,
                        isHidden = true,
                        position = new Rectangle(gameStart.X + j * 100, gameStart.Y + i * 100, 100, 100),
                        numberId = number,
                        isWinner = false
                    });
                }
            }

            winState = false;

        }

        public void Update(GameTime gameTime)
        {
            if (!winState && component.memoryUp)
            {
                if (selectedFirst != -1)
                {
                    elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (elapsedTime > visibleTime)
                    {
                        elapsedTime = 0;

                        var result = gameFields.Where(item => item.id == selectedFirst || item.id == selectedSecond)
                            .ToArray();

                        if (result.Length == 2)
                        {
                            if (result.First().numberId == result.Last().numberId)
                            {
                                foreach (var item in gameFields)
                                {
                                    if (item.id == selectedFirst || item.id == selectedSecond)
                                    {
                                        item.isHidden = false;
                                        item.isWinner = true;
                                    }
                                }
                            }
                        }

                        selectedFirst = -1;
                        selectedSecond = -1;

                        foreach (var item in gameFields)
                        {
                            if (!item.isWinner && !item.isHidden)
                            {
                                item.isHidden = true;
                            }
                        }
                    }
                }

                var mouseState = Mouse.GetState();

                foreach (var item in gameFields)
                {
                    if ((mouseState.LeftButton == ButtonState.Pressed) && item.position.Contains(mouseState.Position))
                    {
                        if (selectedFirst == -1 && selectedSecond == -1)
                        {
                            item.isHidden = false;
                            selectedFirst = item.id;
                        }
                        else if (selectedFirst != -1 && selectedSecond == -1 && selectedFirst != item.id)
                        {
                            item.isHidden = false;
                            selectedSecond = item.id;
                        }
                    }
                }

                if (CheckWin())
                {
                    winSound.Play();
                }
            }
                
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (winState)
            {
                spriteBatch.Draw(memoryAnswer, new Rectangle(150, 150, 417, 142), Color.White);
            }
            else
            {
                foreach (var item in gameFields)
                {
                    if (item.isHidden)
                    {
                        spriteBatch.Draw(question, item.position, null, Color.White);
                    }
                    else if (item.isWinner || !item.isHidden)
                    {
                        spriteBatch.Draw(numbers, item.position, numbersPosition[item.numberId], Color.White);
                    }
                }

            }
        }


        private bool CheckWin()
        {
            winState = true;
            component.memoryWin = true;
            foreach (Board item in gameFields)
            {
                if (item.isHidden)
                {
                    winState = false;
                    component.memoryWin = false;

                    break;
                }
            }
            return winState;
        }
    }
}
