using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Sprites.Items
{
    public class Item : Sprite, ICollidable
    {
        private string _description { get; set; }
        private Buff _buff { get; set; }
        private string _splashText { get; set; }

        public bool inInventory { get; set; }
        public bool doHighlight { get; set; }
        private bool _playerCollision;
        public Buff Buff
        {
            get { return _buff; }
            set { _buff = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public string SplashText
        {
            get { return _splashText; }
            set { _splashText = value; }
        }
        public Item(Texture2D texture) : base(texture)
        {
        }
        public override void Update(GameTime gameTime)
        {
            if (!_playerCollision)
            {
                doHighlight = false;
            }
            _playerCollision = false;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!inInventory)
            {
                if (doHighlight)
                {
                    Colour = Color.Red;
                }
                else
                {
                    Colour = Color.White;
                }

                base.Draw(gameTime, spriteBatch);
            }
        }

        public void OnCollide(Sprite sprite)
        {
            if (sprite is Player)
            {
                doHighlight = true;
                _playerCollision = true;
            }
        }
    }
}
