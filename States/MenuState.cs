using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Roguelite_Game.Controls;
using Roguelite_Game.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.States
{
    public class MenuState : State
    {
        private List<Component> _components;
        
        public MenuState(Game1 game, ContentManager content) : base(game, content)
        {

        }

        public override void LoadContent()
        {
            var buttonTexture = _content.Load<Texture2D>("Button");
            var buttonFont = _content.Load<SpriteFont>("Font");

            _components = new List<Component>()
            {
                new Sprite(_content.Load<Texture2D>("Backgrounds/TestMainMenuBG"))
                {
                    Layer = 0.0f,
                    Position = new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), //Change later for dynamic background
                },
                new Button(buttonTexture, buttonFont)
                {
                    Text = "Play",
                    Position = new Vector2(Game1.ScreenWidth / 2, 450),
                    Click = new EventHandler(Button_Play_Clicked),
                    Layer = 0.1f,
                },
                new Button(buttonTexture, buttonFont)
                {
                    Text = "Settings",
                    Position = new Vector2(Game1.ScreenWidth / 2, 500),
                    Click = new EventHandler(Button_Settings_Clicked),
                    Layer = 0.1f,
                },
                new Button(buttonTexture, buttonFont)
                {
                    Text = "Exit",
                    Position = new Vector2(Game1.ScreenWidth / 2, 550),
                    Click = new EventHandler(Button_Exit_Clicked),
                    Layer = 0.1f,
                },
            };
        }

        private void Button_Play_Clicked(object sender, EventArgs args)
        {
            _game.ChangeState(new GameState(_game, _content)
            {
                PlayerCount = 1,
            });
        }
        private void Button_Settings_Clicked(object sender, EventArgs args)
        {
            _game.ChangeState(new SettingsState(_game, _content));
        }
        private void Button_Exit_Clicked(object sender, EventArgs args)
        {
            _game.Exit();
        }
        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }
        public override void PostUpdate(GameTime gameTime)
        {
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
    }
}
