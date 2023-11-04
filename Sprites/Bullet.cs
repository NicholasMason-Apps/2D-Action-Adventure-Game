using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Sprites
{
    public class Bullet : Sprite, ICollidable
    {
        private float _timer;
        public Explosion Explosion;
        public float LifeSpan { get; set; }
        public Vector2 Velocity;
        public int Damage { get; set; }

        public Bullet(Texture2D texture) : base(texture)
        {
        }
        public override void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer >= LifeSpan)
                IsRemoved = true;

            Position += Velocity;
        }

        public void OnCollide(Sprite sprite)
        {
            if (sprite is Wall)
            {
                IsRemoved = true;
                AddExplosion();
            } else if (sprite is Obstacle)
            {
                IsRemoved = true;
                AddExplosion();
            }
            else
                return;
        }

        public void AddExplosion()
        {
            if (Explosion == null)
                return;

            var explosion = Explosion.Clone() as Explosion;
            explosion.Position = this.Position;

            Children.Add(explosion);
        }
    }
}
