using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Roguelite_Game.States;
using System;

namespace Roguelite_Game
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static Random Random;
        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;
        public static int TileSize = 64;
        public static int TargetFPS = 60;

        private State _currentState;
        private State _nextState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Random = new Random();

            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            /*
            _graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / TargetFPS);
            */
            _graphics.ApplyChanges();

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _currentState = new MenuState(this, Content);
            _currentState.LoadContent();

            _nextState = null;
        }

        protected override void Update(GameTime gameTime)
        {
            if (_nextState != null)
            {
                _currentState = _nextState;
                _currentState.LoadContent();
                _nextState = null;
            }
            _currentState.Update(gameTime);
            _currentState.PostUpdate(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _currentState.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }

        public void ChangeState(State state)
        {
            _nextState = state;
        }
    }
}
