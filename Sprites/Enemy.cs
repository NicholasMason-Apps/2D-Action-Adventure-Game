using Microsoft.Xna.Framework;
using Roguelite_Game.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Sprites
{
    public class Enemy : Entity
    {
        protected Player _player;
        public Random rnd = new Random();
        public Vector2 CollisionDirection;
        public Vector2 TargetPosition;
        public Vector2 NextPosition;

        public bool MovedX;
        public bool MovedY;
        public bool PrioritiseX;
        public bool PrioritiseY;

        public Vector2 CollisionMoveType;
        public bool HasCollided;
        public Vector2 PostVelocity;

        public float ShootTimer = 1.75f;
        public List<Sprite> CollidableSprites;
        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }
        public Vector2 Offset { get;set; }
        public Enemy(Dictionary<string, Animation> animations) : base(animations)
        {
            TargetPosition = Vector2.Zero;
            CollisionMoveType = Vector2.Zero;
            HasCollided = false;
            MovedX = false;
            MovedY = false;
        }
        public override void Update(GameTime gameTime)
        {
            foreach (var sprite in CollidableSprites) //Loops through all the Wall and Obstacle sprites to check if the Enemy has collided with them and thus change the AI movement
            {
                if (IsTouchingLeft(sprite) && Position.X + Velocity.X < (Game1.TileSize * 1.5) + Offset.X) //Checks if the Enemy is stuck in the LEFT of the Screen
                {
                    Velocity.X = 0;
                    TargetPosition.X = 0;
                    Position += new Vector2(Speed, 0);
                    break;
                } 
                else if (IsTouchingRight(sprite) && Position.X + Velocity.X > Game1.ScreenWidth - (Game1.TileSize * 1.5) + Offset.X) //Checks if the Enemy is stuck in the RIGHT of the Screen
                {
                    Velocity.X = 0;
                    TargetPosition.X = 0;
                    Position += new Vector2(-Speed, 0);
                    break;
                } 
                else if (IsTouchingTop(sprite) && Position.Y + Velocity.Y > Game1.ScreenHeight - (Game1.TileSize * 1.5) - 8 + Offset.Y) //Checks if the Enemy is stuck in the BOTTOM of the Screen
                {
                    Velocity.Y = 0;
                    TargetPosition.Y = 0;
                    Position += new Vector2(0, -Speed);
                    break;
                }
                else if (IsTouchingBottom(sprite) && Position.Y + Velocity.Y < (Game1.TileSize * 1.5) + 8 + Offset.Y) //Checks if the Enemy is stuck in the TOP of the Screen
                {
                    Velocity.Y = 0;
                    TargetPosition.Y = 0;
                    Position += new Vector2(0, Speed);
                    break;
                }
                else if (IsTouchingBottom(sprite) && //Checks if the Enemy is stuck in the TOP LEFT corner of an Obstacle layout
                    ((Position.X > (Game1.TileSize * 2) + Offset.X) && (Position.X < (Game1.TileSize * 4) + Offset.X)) &&
                    (Position.Y > (Game1.TileSize * 2) + 8 + Offset.Y) && (Position.Y < (Game1.TileSize * 4) + 8 + Offset.Y))
                {
                    Velocity = Vector2.Zero;
                    Position += new Vector2(Speed, Speed);
                    if (Player.Position.X < Position.X)
                    {
                        TargetPosition = new Vector2(Position.X, Game1.ScreenHeight / 2 + Offset.Y);
                        NextPosition = new Vector2(Player.Position.Y, 0);
                        MovedX = true;
                        MovedY = false;
                        break;
                    }
                    else
                    {
                        TargetPosition = new Vector2(Position.X, Game1.ScreenHeight / 2 + Offset.Y);
                        NextPosition = new Vector2(Game1.ScreenWidth / 2 + Offset.X, 0);
                        MovedX = true;
                        MovedY = false;
                        break;
                    }
                }
                else if (IsTouchingBottom(sprite) && //Checks if the Enemy is stuck in the TOP RIGHT corner of an Obstacle layout
                    ((Position.X > Game1.ScreenWidth - (Game1.TileSize * 4) + Offset.X) && (Position.X < Game1.ScreenWidth - (Game1.TileSize * 2) + Offset.X)) &&
                    (Position.Y > (Game1.TileSize * 2) + 8 + Offset.Y) && (Position.Y < (Game1.TileSize * 4) + 8 + Offset.Y))
                {
                    Velocity = Vector2.Zero;
                    Position += new Vector2(-Speed, Speed);
                    if (Player.Position.X > Position.X)
                    {
                        TargetPosition = new Vector2(Position.X, Game1.ScreenHeight / 2 + Offset.Y);
                        NextPosition = new Vector2(Player.Position.Y, 0);
                        MovedX = true;
                        MovedY = false;
                        break;
                    }
                    else
                    {
                        TargetPosition = new Vector2(Position.X, Game1.ScreenHeight / 2 + Offset.Y);
                        NextPosition = new Vector2(Game1.ScreenWidth / 2 + Offset.X, 0);
                        MovedX = true;
                        MovedY = false;
                        break;
                    }
                }
                else if (IsTouchingTop(sprite) && //Checks if the Enemy is stuck in the BOTTOM LEFT corner of an Obstacle layout
                    ((Position.X > (Game1.TileSize * 2) + Offset.X) && (Position.X < (Game1.TileSize * 4) + Offset.X)) &&
                    (Position.Y > Game1.ScreenHeight - (Game1.TileSize * 4) - 8 + Offset.Y) && (Position.Y < Game1.ScreenHeight - (Game1.TileSize * 2) - 8 + Offset.Y))
                {
                    Velocity = Vector2.Zero;
                    Position += new Vector2(Speed, -Speed);
                    if (Player.Position.X < Position.X)
                    {
                        TargetPosition = new Vector2(Position.X, Game1.ScreenHeight / 2 + Offset.Y);
                        NextPosition = new Vector2(Player.Position.Y, 0);
                        MovedX = true;
                        MovedY = false;
                        break;
                    }
                    else
                    {
                        TargetPosition = new Vector2(Position.X, Game1.ScreenHeight / 2 + Offset.Y);
                        NextPosition = new Vector2(Game1.ScreenWidth / 2 + Offset.X, 0);
                        MovedX = true;
                        MovedY = false;
                        break;
                    }
                }
                else if (IsTouchingTop(sprite) && //Checks if the Enemy is stuck in the TOP RIGHT corner of an Obstacle layout
                    ((Position.X > Game1.ScreenWidth - (Game1.TileSize * 4) + Offset.X) && (Position.X < Game1.ScreenWidth - (Game1.TileSize * 2) + Offset.X)) &&
                    (Position.Y > Game1.ScreenHeight - (Game1.TileSize * 4) - 8 + Offset.Y) && (Position.Y < Game1.ScreenHeight - (Game1.TileSize * 2) - 8 + Offset.Y))
                {
                    Velocity = Vector2.Zero;
                    Position += new Vector2(-Speed, -Speed);
                    if (Player.Position.X > Position.X)
                    {
                        TargetPosition = new Vector2(Position.X, Game1.ScreenHeight / 2 + Offset.Y);
                        NextPosition = new Vector2(Player.Position.Y, 0);
                        MovedX = true;
                        MovedY = false;
                        break;
                    }
                    else
                    {
                        TargetPosition = new Vector2(Position.X, Game1.ScreenHeight / 2 + Offset.Y);
                        NextPosition = new Vector2(Game1.ScreenWidth / 2 + Offset.X, 0);
                        MovedX = true;
                        MovedY = false;
                        break;
                    }
                }
                else if (IsTouchingTop(sprite) || IsTouchingBottom(sprite))
                {
                    Velocity = Vector2.Zero;
                    if (Position.X < (Game1.ScreenWidth / 2) + Offset.X)
                    {
                        if (Position.X < (Game1.ScreenWidth / 4) + Game1.TileSize + Offset.X)
                        {
                            TargetPosition = new Vector2((Game1.TileSize * 3 / 2) + Offset.X, Position.Y);
                            NextPosition = new Vector2(0, Player.Position.Y);
                            MovedY = true;
                            MovedX = false;
                            break;
                        }
                        else
                        {
                            TargetPosition = new Vector2((Game1.ScreenWidth / 2) + Offset.X, Position.Y);
                            NextPosition = new Vector2(0, Player.Position.Y);
                            MovedY = true;
                            MovedX = false;
                            break;
                        }
                    }
                    else
                    {
                        if (Position.X > (Game1.ScreenWidth * 3 / 4) - Game1.TileSize + Offset.X)
                        {
                            TargetPosition = new Vector2(Game1.ScreenWidth - (Game1.TileSize * 3 / 2) + Offset.X, Position.Y);
                            NextPosition = new Vector2(0, Player.Position.Y);
                            MovedY = true;
                            MovedX = false;
                            break;
                        }
                        else
                        {
                            TargetPosition = new Vector2((Game1.ScreenWidth / 2) + Offset.X, Position.Y);
                            NextPosition = new Vector2(0, Player.Position.Y);
                            MovedY = true;
                            MovedX = false;
                            break;
                        }
                    }
                }
                else if (IsTouchingLeft(sprite) || IsTouchingRight(sprite))
                {
                    Velocity = Vector2.Zero;
                    if (Position.Y < (Game1.ScreenHeight / 2) + Offset.Y + 8)
                    {
                        if (Position.Y < (Game1.ScreenHeight / 4) + Game1.TileSize + Offset.Y + 8)
                        {
                            TargetPosition = new Vector2(Position.X, (Game1.TileSize * 3 / 2) + Offset.Y + 8);
                            NextPosition = new Vector2(Player.Position.X, 0);
                            MovedX = true;
                            MovedY = false;
                            break;
                        }
                        else
                        {
                            TargetPosition = new Vector2(Position.X, (Game1.ScreenHeight / 2) + Offset.Y);
                            NextPosition = new Vector2(Player.Position.X, 0);
                            MovedX = true;
                            MovedY = false;
                            break;
                        }
                    }
                    else
                    {
                        if (Position.Y > (Game1.ScreenHeight * 3 / 4) - Game1.TileSize - 8 + Offset.Y)
                        {
                            TargetPosition = new Vector2(Position.X, Game1.ScreenHeight - (Game1.TileSize * 3 / 2) - 8 + Offset.Y);
                            NextPosition = new Vector2(Player.Position.X, 0);
                            MovedX = true;
                            MovedY = false;
                            break;
                        }
                        else
                        {
                            TargetPosition = new Vector2(Position.X, (Game1.ScreenHeight / 2) + Offset.Y);
                            NextPosition = new Vector2(Player.Position.X, 0);
                            MovedX = true;
                            MovedY = false;
                            break;
                        }
                    }
                }
            }

            Position += Velocity;
            _rectangle.X += (int)Velocity.X;
            _rectangle.Y += (int)Velocity.Y;
            Velocity = Vector2.Zero;
        }
        public override void OnCollide(Sprite sprite)
        {
            if (sprite is Bullet && ((Bullet)sprite).Parent is Player)
            {
                ((Bullet)sprite).IsRemoved = true;
                ((Bullet)sprite).AddExplosion();

                Health = Health - ((Bullet)sprite).Damage;
                if (Health <= 0)
                {
                    IsRemoved = true;
                }
                
            }
            else if (sprite is Wall)
            {
                return;
            } 
            else if (sprite is Obstacle)
            {
                return;
            }
        }
    }
}
