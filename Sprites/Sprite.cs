using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelite_Game.Managers;
using Roguelite_Game.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelite_Game.Sprites
{
    public class Sprite : Component, ICloneable
    {
        #region Fields
        protected Dictionary<string, Animation> _animations;
        protected AnimationManager _animationManager;
        protected float _layer { get; set; }
        protected Vector2 _origin { get; set; }
        protected Vector2 _position { get; set; }
        protected float _rotation { get; set; }
        protected float _scale { get; set; }
        protected Texture2D _texture;
        #endregion

        #region Properties
        public Rectangle _rectangle;
        public Room ParentRoom { get; set; }
        public List<Sprite> Children { get; set; }
        public Color Colour { get; set; }
        public bool IsRemoved { get; set; }
        public float Layer //this get + set method layout is repeated for all the properties as it both sets the value of the field within Sprite.cs as well as AnimationManager.cs
        {
            get { return _layer; }
            set
            {
                _layer = value;

                if (_animationManager != null)
                    _animationManager.Layer = _layer;
            }
        }
        public Vector2 Origin
        {
            get { return _origin; }
            set
            {
                _origin = value;
                if (_animationManager != null)
                    _animationManager.Origin = _origin;
            }
        }
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }
        public Rectangle Rectangle
        {
            get
            {
                if (_texture != null) //checks if a static texture is loaded
                    return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, _texture.Width, _texture.Height);
                
                if (_animationManager != null) //checks if an animation is loaded
                {
                    var animation = _animations.FirstOrDefault().Value;
                    return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, animation.FrameWidth, animation.FrameHeight);
                }

                throw new Exception("Unknown Sprite");
            }
        }
        public float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                if (_animationManager != null)
                    _animationManager.Rotation = _rotation;
            }
        }
        public Sprite Parent;
        #endregion

        #region Methods
        public Sprite(Texture2D texture)
        {
            _texture = texture;
            Children = new List<Sprite>();
            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            Colour = Color.White;
        }

        public Sprite(Dictionary<string, Animation> animations)
        {
            Children = new List<Sprite>();
            Colour = Color.White;
            _animations = animations;
            var animation = _animations.First().Value;
            _animationManager = new AnimationManager(animation);
            Origin = new Vector2(animation.FrameWidth / 2, animation.FrameHeight / 2);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture, Position, null, Colour, _rotation, Origin, 1f, SpriteEffects.None, Layer);
            else if (_animationManager != null)
                _animationManager.Draw(spriteBatch);
        }
        protected virtual void DetermineAnimations(Vector2 velocity)
        {
            if (_animationManager == null)
                return;

            if (velocity.X > 0)
            {
                _animationManager.Play(_animations["WalkRight"]);
            }
            else if (velocity.X < 0)
            {

                _animationManager.Play(_animations["WalkLeft"]);

            }
            /*
            else if (velocity.Y > 0)
            {

                _animationManager.Play(_animations["WalkDown"]);
            }
            else if (velocity.Y < 0)
            {
                _animationManager.Play(_animations["WalkUp"]);
            }*/
            else
            {
                _animationManager.Play(_animations["Idle"]);
            }

        }
        public virtual bool IsTouchingLeft(Sprite sprite)
        {
            return this._rectangle.Right > sprite._rectangle.Left &&
                this._rectangle.Left < sprite._rectangle.Left &&
                this._rectangle.Bottom > sprite._rectangle.Top &&
                this._rectangle.Top < sprite._rectangle.Bottom;
        }
        public virtual bool IsTouchingRight(Sprite sprite)
        {
            return this._rectangle.Left < sprite._rectangle.Right &&
                this._rectangle.Right > sprite._rectangle.Right &&
                this._rectangle.Bottom > sprite._rectangle.Top &&
                this._rectangle.Top < sprite._rectangle.Bottom;
        }
        public virtual bool IsTouchingTop(Sprite sprite)
        {
            return this._rectangle.Bottom > sprite._rectangle.Top &&
                this._rectangle.Top < sprite._rectangle.Top &&
                this._rectangle.Right > sprite._rectangle.Left &&
                this._rectangle.Left < sprite._rectangle.Right;
        }
        public virtual bool IsTouchingBottom(Sprite sprite)
        {
            return this._rectangle.Top < sprite._rectangle.Bottom &&
                this._rectangle.Bottom > sprite._rectangle.Bottom &&
                this._rectangle.Right > sprite._rectangle.Left &&
                this._rectangle.Left < sprite._rectangle.Right;
        }
        public bool Intersects(Sprite sprite)
        {
            if ((this._rectangle.Right > sprite._rectangle.Left &&
                this._rectangle.Left < sprite._rectangle.Left &&
                this._rectangle.Bottom > sprite._rectangle.Top &&
                this._rectangle.Top < sprite._rectangle.Bottom) || //Checking Left of Sprite
                
                (this._rectangle.Left < sprite._rectangle.Right &&
                this._rectangle.Right > sprite._rectangle.Right &&
                this._rectangle.Bottom > sprite._rectangle.Top &&
                this._rectangle.Top < sprite._rectangle.Bottom) || //Checking Right of Sprite
                
                (this._rectangle.Bottom > sprite._rectangle.Top &&
                this._rectangle.Top < sprite._rectangle.Top &&
                this._rectangle.Right > sprite._rectangle.Left &&
                this._rectangle.Left < sprite._rectangle.Right) || //Checking Top of Sprite
                
                (this._rectangle.Top < sprite._rectangle.Bottom &&
                this._rectangle.Bottom > sprite._rectangle.Bottom &&
                this._rectangle.Right > sprite._rectangle.Left &&
                this._rectangle.Left < sprite._rectangle.Right)) //Checking Bottom of Sprite
                return true;
            //No intersection occurred
            return false;
        }

        public object Clone()
        {
            var sprite = this.MemberwiseClone() as Sprite;

            if (_animations != null)
            {
                sprite._animations = this._animations.ToDictionary(c => c.Key, v => v.Value.Clone() as Animation);
                sprite._animationManager = sprite._animationManager.Clone() as AnimationManager;
            }

            return sprite;
        }
        #endregion

    }
}
