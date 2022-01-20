using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


namespace EscapeRoom
{
    public class GameWorld : Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        private const float delay = 3; // seconds
        private float remainingDelay = delay;

        private StartScene startScene;
        private AboutScene aboutScene;
        private PlayScene playScene;
        private HelpScene helpScene;
        private EndScene endScene;
        private ScoreScene scoreScene;

        private MouseState previousState;
        
        private SoundEffect open_door;

        Song soundtrack;

        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {


            base.Initialize();
        }

        private void hideAllScenes()
        {
            GameScene gs = null;
            foreach (GameComponent item in Components)
            {
                if (item is GameScene)
                {
                    gs = (GameScene)item;
                    gs.hide();
                }
            }
            Shared.soundOn = false;

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 613;
            graphics.ApplyChanges();

            startScene = new StartScene(this);
            this.Components.Add(startScene);
            startScene.show();

            aboutScene = new AboutScene(this);
            this.Components.Add(aboutScene);

            helpScene = new HelpScene(this);
            this.Components.Add(helpScene);

            playScene = new PlayScene(this);
            this.Components.Add(playScene);

            endScene = new EndScene(this);
            this.Components.Add(endScene);

            scoreScene = new ScoreScene(this);
            this.Components.Add(scoreScene);

            open_door = Content.Load<SoundEffect>("sounds/open_door");
            soundtrack = Content.Load<Song>("sounds/soundtrack");


        }

        protected override void Update(GameTime gameTime)
        {
            int selectedMenu;
            int back;
            
            var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;


            MouseState currentState = Mouse.GetState();

            if (startScene.Enabled)  //Menu screen
            {
                selectedMenu = startScene.SelectedMenu;  //Depending on mouse position, defines which button is being clicked

                if (selectedMenu == 1 && currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
                {
                    // Score screen
                    Shared.scoreLoaded = false;
                    hideAllScenes();
                    scoreScene.show();
                }
                else if (selectedMenu == 2 && currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
                {
                    // Help screen
                    hideAllScenes();
                    helpScene.show();
                }
                else if (selectedMenu == 3 && currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
                {
                    // About screen
                    hideAllScenes();
                    aboutScene.show();
                }
                else if (selectedMenu == 4 && currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
                {
                    // Play screen
                    if (!string.IsNullOrEmpty(Shared.name))
                    {
                        hideAllScenes();
                        MediaPlayer.IsRepeating = true;
                        MediaPlayer.Play(soundtrack);
                        Shared.smokeLoaded = false;

                        playScene.show();
                    }
                    else
                    {
                        startScene.component.showError = true;
                    }
                }
            }
            else if (aboutScene.Enabled)
            {
                back = aboutScene.component.Back;

                if (back == 1 && currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
                {
                    hideAllScenes();
                    startScene.show();
                }
            }
            else if (helpScene.Enabled)
            {
                back = helpScene.component.Back;

                if (back == 1 && currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
                {
                    hideAllScenes();
                    startScene.show();
                }
            }
            else if (playScene.Enabled)
            {
                back = playScene.component.Back;

                if (back == 1 && currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
                {
                    hideAllScenes();
                    startScene.show();
                }

                if (playScene.component.EndGame)
                {
                    hideAllScenes();
                    endScene.show();

                    open_door.Play();
                    MediaPlayer.Stop();

                }
            }
            else if (endScene.Enabled)
            {
                remainingDelay -= timer;

                if (remainingDelay <= 0)
                {
                    playScene.component.EndGame = false;
                    hideAllScenes();
                    startScene.show();
                    remainingDelay = delay;
                }
            }
            else if (scoreScene.Enabled)
            {
                back = scoreScene.component.Back;

                if (back == 1 && currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
                {
                    hideAllScenes();
                    startScene.show();
                }
            }

            previousState = currentState;


            base.Update(gameTime);
        }


    }
}
