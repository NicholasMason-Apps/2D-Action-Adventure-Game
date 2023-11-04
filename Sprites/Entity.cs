using Microsoft.Xna.Framework;
using Roguelite_Game.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Sprites
{
    public class Entity : Sprite, ICollidable
    {
        public int Health { get; set; }
        public Bullet Bullet { get; set; }
        public float Speed { get; set; }
        public int Damage { get; set; }

        public Vector2 Velocity;

        public Entity(Dictionary<string, Animation> animations) : base(animations)
        {
        }
        public void Shoot(float speed, float angle)
        {
            if (Bullet == null)
                return;

            var bullet = Bullet.Clone() as Bullet;
            bullet.Position = this.Position;
            bullet.Colour = this.Colour;
            bullet.Layer = 0.2f;
            bullet.LifeSpan = 5f;
            bullet.Damage = Damage;
            bullet.Velocity = new Vector2((float)(speed * Math.Cos(angle)),(float)(speed * Math.Sin(angle)));
            bullet.Parent = this;

            Children.Add(bullet);
        }
        public override bool IsTouchingLeft(Sprite sprite)
        {
            return this._rectangle.Right + this.Velocity.X > sprite._rectangle.Left &&
                this._rectangle.Left < sprite._rectangle.Left &&
                this._rectangle.Bottom > sprite._rectangle.Top &&
                this._rectangle.Top < sprite._rectangle.Bottom;
        }
        public override bool IsTouchingRight(Sprite sprite)
        {
            return this._rectangle.Left + this.Velocity.X < sprite._rectangle.Right &&
                this._rectangle.Right > sprite._rectangle.Right &&
                this._rectangle.Bottom > sprite._rectangle.Top &&
                this._rectangle.Top < sprite._rectangle.Bottom;
        }
        public override bool IsTouchingTop(Sprite sprite)
        {
            return this._rectangle.Bottom + this.Velocity.Y > sprite._rectangle.Top &&
                this._rectangle.Top < sprite._rectangle.Top &&
                this._rectangle.Right > sprite._rectangle.Left &&
                this._rectangle.Left < sprite._rectangle.Right;
        }
        public override bool IsTouchingBottom(Sprite sprite)
        {
            return this._rectangle.Top + this.Velocity.Y < sprite._rectangle.Bottom &&
                this._rectangle.Bottom > sprite._rectangle.Bottom &&
                this._rectangle.Right > sprite._rectangle.Left &&
                this._rectangle.Left < sprite._rectangle.Right;
        }

        public virtual void OnCollide(Sprite sprite)
        {
            throw new NotImplementedException();
        }
    }
}
