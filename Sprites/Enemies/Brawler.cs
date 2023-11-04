using Microsoft.Xna.Framework;
using Roguelite_Game.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Sprites.Enemies
{
    public class Brawler : Enemy
    {

        private bool _moveComplete;
        public Brawler(Dictionary<string, Animation> animations) : base(animations)
        {
            Speed = 3f;
            _moveComplete = false;
        }
        public override void Update(GameTime gameTime)
        {
            if (NextPosition.X != 0 && !_moveComplete && TargetPosition == Vector2.Zero)
            {
                TargetPosition = new Vector2(Player.Position.X, Position.Y);
                MovedY = true;
                NextPosition = Vector2.Zero;
                _moveComplete = false;
            }
            else if (NextPosition.Y != 0 && !_moveComplete && TargetPosition == Vector2.Zero)
            {
                TargetPosition = new Vector2(Position.X, Player.Position.Y);
                NextPosition = Vector2.Zero;
                MovedX = true;
                _moveComplete = false;
            }
            else if (TargetPosition == Vector2.Zero)
            {
                TargetPosition = Player.Position;
                CollisionMoveType = Vector2.Zero;
                NextPosition = Vector2.Zero;
                _moveComplete = false;
            }

            
            if (!_moveComplete && (Math.Sqrt(Math.Pow(TargetPosition.X, 2) + Math.Pow(Position.X, 2))) >
                                        (Math.Sqrt(Math.Pow(TargetPosition.Y, 2) + Math.Pow(Position.Y, 2))))
            {
                if (Position.X < TargetPosition.X && !MovedX)
                {
                    Velocity.X += Speed;
                    base.Update(gameTime);
                    if (Position.X >= TargetPosition.X)
                        MovedX = true;
                }
                else if (Position.X > TargetPosition.X && !MovedX)
                {
                    Velocity.X -= Speed;
                    base.Update(gameTime);
                    if (Position.X <= TargetPosition.X)
                        MovedX = true;
                }
                else if (Position.Y > TargetPosition.Y && !MovedY)
                {
                    Velocity.Y -= Speed;
                    base.Update(gameTime);
                    if (Position.Y <= TargetPosition.Y)
                        MovedY = true;
                }
                else if (Position.Y < TargetPosition.Y && !MovedY)
                {
                    Velocity.Y += Speed;
                    base.Update(gameTime);
                    if (Position.Y >= TargetPosition.Y)
                        MovedY = true;
                }
                else
                {
                    _moveComplete = true;
                    Velocity = Vector2.Zero;
                }
            }
            else if (!_moveComplete && (Math.Sqrt(Math.Pow(TargetPosition.X, 2) + Math.Pow(Position.X, 2))) <=
                                        (Math.Sqrt(Math.Pow(TargetPosition.Y, 2) + Math.Pow(Position.Y, 2))))
            {
                if (Position.Y > TargetPosition.Y && !MovedY)
                {
                    Velocity.Y -= Speed;
                    base.Update(gameTime);
                    if (Position.Y <= TargetPosition.Y)
                        MovedY = true;
                }
                else if (Position.Y < TargetPosition.Y && !MovedY)
                {
                    Velocity.Y += Speed;
                    base.Update(gameTime);
                    if (Position.Y >= TargetPosition.Y)
                        MovedY = true;
                }
                else if (Position.X < TargetPosition.X && !MovedX)
                {
                    Velocity.X += Speed;
                    base.Update(gameTime);
                    if (Position.X >= TargetPosition.X)
                        MovedX = true;
                }
                else if (Position.X > TargetPosition.X && !MovedX)
                {
                    Velocity.X -= Speed;
                    base.Update(gameTime);
                    if (Position.X <= TargetPosition.X)
                        MovedX = true;
                }
                else
                {
                    _moveComplete = true;
                    Velocity = Vector2.Zero;
                }
            }
            else
            {
                TargetPosition = Vector2.Zero;
                MovedX = false;
                MovedY = false;
                _moveComplete = false;
                HasCollided = false;
            }
        }
        /*
        public override void OnCollide(Sprite sprite)
        {
            
        }
        */
    }
}
