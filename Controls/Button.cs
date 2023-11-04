using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Controls
{
    public class Button : Component
    {
        #region Fields
        //Fields are privated to this class and store the data
        private MouseState _currentMouse;
        private SpriteFont _font;
        private bool _isHovering;
        private MouseState _previousMouse;
        private Texture2D _texture;
        #endregion

        #region Properties
        //Properties are used as a way to access fields without affecting other things within the class
        public EventHandler Click;
        public bool Clicked { get; private set; }
        public float Layer { get; set; }
        public Vector2 Origin
        {
            get
            {
                return new Vector2(_texture.Width / 2, _texture.Height / 2); //returns the centre of the button
            }
        }
        public Color PenColour { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X - ((int)Origin.X), (int)Position.Y - (int)Origin.Y, _texture.Width, _texture.Height); //Used for collision between mouse and Button
            }
        }
        public string Text { get; set; }
        #endregion

        #region Methods
        public Button(Texture2D texture, SpriteFont font) //Button Constructor
        {
            _texture = texture;
            _font = font;
            PenColour = Color.Black;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) //Overriding the Draw method from the Component Class
        {
            var colour = Color.White;

            if (_isHovering)
                colour = Color.Gray;

            spriteBatch.Draw(_texture, Position, null, colour, 0f, Origin, 1f, SpriteEffects.None, Layer);

            if (!string.IsNullOrEmpty(Text)) //Checks if we have text by doing the opposite of IsNullOrEmpty
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, Layer + 0.01f); //Draws Text
            }
        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;
                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                    Click?.Invoke(this, new EventArgs());
            }
        }
        #endregion
    }
}