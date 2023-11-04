using Microsoft.Xna.Framework;
using Roguelite_Game.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Sprites
{
    public class Explosion : Sprite
    {
        private float _timer = 0f;

        public Explosion(Dictionary<string, Animation> animations) : base(animations)
        {
        }

        public override void Update(GameTime gameTime)
        {
            _animationManager.Update(gameTime);
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //removes the sprite once the animation is finished
            if (_timer > _animationManager.CurrentAnimation.FrameCount * _animationManager.CurrentAnimation.FrameSpeed)
                IsRemoved = true;
        }
    }
}
