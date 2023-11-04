using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Roguelite_Game.Core;
using Roguelite_Game.Models;
using Roguelite_Game.Sprites.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Sprites
{
    public class Player : Entity
    {
        private float _shootTimer = 0f; //Later this will change depending on the weapon equipped
        private float _invincibilityTimer;

        public bool IsDead
        {
            get
            {
                return Health <= 0;
            }
        }

        public Input Input;
        public Vector2 PostVelocity;
        public List<Sprite> CollidableSprites;

        private Vector2 _mousePosition;
        private List<Item> _items; //Will be used for inventory
        private float _velocityMultiplier; //Will be used for items that add speed boosts

        public Camera Camera { get; set; }
        public bool CanPickUp { get; set; }
        public bool isHit { get; set; }
        private bool _doFadeIncrease { get; set; }

        public Player(Dictionary<string, Animation> animations) : base(animations)
        {
            Speed = 4f;
            _items = new List<Item>();
            isHit = false;
            _doFadeIncrease = true;
        }
        

        public override void Update(GameTime gameTime)
        {
            if (IsDead)
                return;

            var mouse = Mouse.GetState();
            _mousePosition.X = (float)Mouse.GetState().Position.X;
            _mousePosition.Y = (float)Mouse.GetState().Position.Y;

            _mousePosition += Camera.CameraPosition;

            if (Keyboard.GetState().IsKeyDown(Input.Up))
            {
                Velocity.Y -= Speed;
            } if (Keyboard.GetState().IsKeyDown(Input.Down))
            {
                Velocity.Y += Speed;
            } if (Keyboard.GetState().IsKeyDown(Input.Left))
            {
                Velocity.X -= Speed;
            } if (Keyboard.GetState().IsKeyDown(Input.Right))
            {
                Velocity.X += Speed;
            }

            //var distance = new Vector2(Position.X + Origin.X - (Game1.ScreenWidth / 2), Position.Y + Origin.Y - (Game1.ScreenHeight / 2));
            //_mousePosition += distance;

            if (isHit)
            {
                _invincibilityTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_animationManager.Colour.A == 50)
                    _doFadeIncrease = true;
                else if (_animationManager.Colour.A == 155)
                    _doFadeIncrease = false;

                if (_doFadeIncrease)
                    _animationManager.ColourA = 7;
                else if (!_doFadeIncrease)
                    _animationManager.ColourA = -7;

                if (_invincibilityTimer > 1.5f)
                {
                    isHit = false;
                    _invincibilityTimer = 0f;
                    _animationManager.Colour = Color.White;
                }
            }

            _shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if ((mouse.LeftButton == ButtonState.Pressed) && _shootTimer > 0.25f)
            {
                Shoot(7f, (float)Math.Atan2((_mousePosition.Y - Position.Y), (_mousePosition.X - Position.X)));

                _shootTimer = 0f;
            }

            foreach (var sprite in CollidableSprites)
            {
                if ((Velocity.X > 0 && IsTouchingLeft(sprite)) ||
                    (Velocity.X < 0 && IsTouchingRight(sprite)))
                    Velocity.X = 0;

                if ((Velocity.Y > 0 && IsTouchingTop(sprite)) ||
                    (Velocity.Y < 0 && IsTouchingBottom(sprite)))
                    Velocity.Y = 0;
            }

            DetermineAnimations(Velocity);
            _animationManager.Update(gameTime);

            Position += Velocity;
            _rectangle.X += (int)Velocity.X;
            _rectangle.Y += (int)Velocity.Y;
            //PostVelocity = Velocity;
            Velocity = Vector2.Zero;


            /*
            if(Velocity.X == CollisionDirection.X && _collisionCount == 1)
            {
                Velocity.X = -(CollisionDirection.X);
                Position += Velocity;
            } else if (Velocity.Y == CollisionDirection.Y && _collisionCount == 1)
            {
                Velocity.Y = -(CollisionDirection.Y);
                Position += Velocity;
            }
            if (Velocity.X == CollisionDirection.X)
            {
                Velocity.X = 0;
            }
            if (Velocity.Y == CollisionDirection.Y)
            {
                Velocity.Y = 0;
            }
            CollisionDirection = Vector2.One;
            */


        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsDead)
                return;

            base.Draw(gameTime, spriteBatch);
        }
        public override void OnCollide(Sprite sprite)
        {
            if (IsDead)
                return;

            if (sprite is Bullet && ((Bullet)sprite).Parent is Enemy)
            {
                ((Bullet)sprite).IsRemoved = true;
                ((Bullet)sprite).AddExplosion();
                if (!isHit)
                {
                    isHit = true;
                    Health = Health - ((Bullet)sprite).Damage;
                }
            }
            else if (sprite is Enemy)
            {
                if (!isHit)
                {
                    Health = Health - ((Enemy)sprite).Damage;
                    isHit = true;
                    _animationManager.Colour = new Color(255, 255, 255, 50);
                    _doFadeIncrease = true;
                }
            }
            else if (sprite is Wall)
            {
                /*
                _collisionCount++;
                if (CollisionDirection != new Vector2(1, 1))
                    return;
                
                if (IsTouchingBottom(sprite))
                {
                    CollisionDirection.Y = -Speed;
                }
                if (IsTouchingTop(sprite))
                {
                    CollisionDirection.Y = Speed;
                }
                if (IsTouchingLeft(sprite))
                {
                    CollisionDirection.X = Speed;
                }
                if (IsTouchingRight(sprite))
                {
                    CollisionDirection.X = -Speed;
                }

                if ((Velocity.X > 0 && IsTouchingLeft(sprite)) ||
                    (Velocity.X < 0 && IsTouchingRight(sprite)))
                    Velocity.X = 0;

                if ((Velocity.Y > 0 && IsTouchingTop(sprite)) ||
                    (Velocity.Y < 0 && IsTouchingBottom(sprite)))
                    Velocity.Y = 0;
                */
                return;
            }
            else if (sprite is Obstacle)
            {
                /*
                CollisionDirection = new Vector2(1, 1);
                _collisionCount++;
                if (IsTouchingBottom(sprite))
                {
                    CollisionDirection.Y = -Speed;
                    //if (CollisionDirection.X != 1 && CollisionDirection.Y != 1)
                    //return;
                }
                if (IsTouchingLeft(sprite))
                {
                    CollisionDirection.X = Speed;
                    //if (CollisionDirection.X != 1 && CollisionDirection.Y != 1)
                    //return;
                }
                if (IsTouchingRight(sprite))
                {
                    CollisionDirection.X = -Speed;
                    //if (CollisionDirection.X != 1 && CollisionDirection.Y != 1)
                    //return;
                }
                if (IsTouchingTop(sprite))
                {
                    CollisionDirection.Y = Speed;
                    //if (CollisionDirection.X != 1 && CollisionDirection.Y != 1)
                    //return;
                }
                */
                return;
            }
            else if (sprite is Door)
            {
                if (((Door)sprite).ParentRoom.isCleared)
                {
                    ((Door)sprite).ConnectedRoom.isVisible = true;
                    ((Door)sprite).ParentRoom.isVisible = false;

                    Camera.MoveTo(((Door)sprite).ConnectedRoom);
                    if (sprite.Rotation == (float)Math.PI / 2) //Checks if the door is facing right
                    {
                        Position += new Vector2(64 * 2, 0);
                    }
                    else if (sprite.Rotation == (float)Math.PI * 3 / 2) //Checks if the door is facing left
                    {
                        Position += new Vector2(-64 * 2, 0);
                    }
                    else if (sprite.Rotation == 0f) //Checks if the door is facing up
                    {
                        Position += new Vector2(0, -64 * 2);
                    }
                    else if (sprite.Rotation == (float)Math.PI) //Cheks if the door is facing down
                    {
                        Position += new Vector2(0, 64 * 2);
                    }
                }
            }
            else if (sprite is Item)
            {
                CanPickUp = true;
            }
        }

    }
}
