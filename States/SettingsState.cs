using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Roguelite_Game.Controls;
using Roguelite_Game.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.States
{
    public class SettingsState : State
    {
        #region Fields
        private List<Component> _components;
        private SpriteFont _font;
        private SettingsManager _settingsManager;
        #endregion

        #region Methods
        public SettingsState(Game1 game, ContentManager content) : base(game, content)
        {
        }

        public override void LoadContent()
        {
            _font = _content.Load<SpriteFont>("Font");
            //_settingsManager = SettingsManager.Load();

            var buttonTexture = _content.Load<Texture2D>("Button");
            var buttonFont = _content.Load<SpriteFont>("Font");

            _components = new List<Component>()
            {
                new Button(buttonTexture, buttonFont)
                {
                    Text = "Main Menu",
                    Position = new Vector2(Game1.ScreenWidth / 2, 560),
                    Click = new EventHandler(Button_MainMenu_Clicked),
                    Layer = 0.1f
                },
            };
        }

        private void Button_MainMenu_Clicked(object sender, EventArgs args)
        {
            _game.ChangeState(new MenuState(_game, _content));
        }
        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Button_MainMenu_Clicked(this, new EventArgs());

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
        #endregion
    }
}