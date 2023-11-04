using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelite_Game.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Managers
{
    public class AnimationManager : ICloneable
    {
        #region Fields
        private Animation _animation;
        private float _timer;
        #endregion

        #region Properties
        public Animation CurrentAnimation
        {
            get
            {
                return _animation;
            }
        }
        public float Layer { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }
        public Color Colour { get; set; }
        public int ColourA
        {
            get { return Colour.A; }
            set { Colour = new Color(Colour.R, Colour.G, Colour.B, Colour.A + value); }
        }
        #endregion

        #region Methods
        public AnimationManager(Animation animation)
        {
            _animation = animation;
            Scale = 1f;
            Colour = Color.White;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_animation.Texture,
                Position,
                new Rectangle((_animation.CurrentFrame * _animation.FrameWidth), 0, _animation.FrameWidth, _animation.FrameHeight),
                Colour,
                Rotation,
                Origin,
                Scale,
                SpriteEffects.None,
                Layer);
            ///(_animation.CurrentFrame * _animation.FrameWidth) -- The x position of the bounding rectangle. This changes depending on the current animation frame, and thus moves when a new frame is incremeneted
            ///0 -- the y position of the bounding rectangle
            ///_animation.FrameWidth -- the width of the bounding rectangle
            ///_animation.FrameHeight -- the height of the bounding rectangle
        }
        public void Play(Animation animation)
        {
            if (_animation == animation)
                return; //breaks out the method if an animation is playing

            _animation = animation;
            _animation.CurrentFrame = 0;
            _timer = 0;
        }
        public void Stop()
        {
            _timer = 0f;
            _animation.CurrentFrame = 0;
        }
        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds; //adds the game time that has elapsed between animation frames to the animation timer

            if (_timer > _animation.FrameSpeed) //used to check if the amount of time needed to go through a frame has passed
            {
                _timer = 0f;
                _animation.CurrentFrame++;

                if (_animation.CurrentFrame >= _animation.FrameCount) //checks if we are at the end of the sprite animation
                    _animation.CurrentFrame = 0;
            }
        }
        public object Clone()
        {
            var animationManager = this.MemberwiseClone() as AnimationManager; //creating a variable and assigining the shallow clone to that variable as an AnimationManager object
            animationManager._animation = animationManager._animation.Clone() as Animation; //cloning the _animation field
            return animationManager;
            //this results in a deep clone which uses the clone method within Models\Animation.cs
        }
        #endregion
    }
}
