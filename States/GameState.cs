using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Roguelite_Game.Core;
using Roguelite_Game.Managers;
using Roguelite_Game.Sprites;
using Roguelite_Game.Sprites.Enemies;
using Roguelite_Game.Sprites.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelite_Game.States
{
    public class GameState : State
    {
        #region Fields
        private MapManager _mapManager;
        private SpriteFont _font;
        public List<Player> _player;
        public List<Sprite> _sprites;
        public List<Room> _rooms;
        #endregion

        #region Properties
        public int PlayerCount; //May be used if 2-player is added
        public Camera _camera;
        #endregion

        #region Methods
        public GameState(Game1 game, ContentManager content) : base(game, content)
        {
        }
        
        public override void LoadContent()
        {
            _camera = new Camera();

            var playerAnimations = new Dictionary<string, Models.Animation>()
            {
                {"WalkRight", new Models.Animation(_content.Load<Texture2D>("PlayerContent/Kitsune_Right_Walk"), 8) },
                {"WalkLeft", new Models.Animation(_content.Load<Texture2D>("PlayerContent/Kitsune_Left_Walk"), 8) },
                {"Idle", new Models.Animation(_content.Load<Texture2D>("PlayerContent/Kitsune_Front_WIP"), 7) },
            };
            var bulletTexture = _content.Load<Texture2D>("Bullet");

            _font = _content.Load<SpriteFont>("Font");
            _sprites = new List<Sprite>();
            _rooms = new List<Room>();

            var bulletPrefab = new Bullet(bulletTexture)
            {
                Explosion = new Explosion(new Dictionary<string, Models.Animation>()
                {
                    {"Explode", new Models.Animation(_content.Load<Texture2D>("Explosion"), 3) { FrameSpeed = 0.1f, } }
                })
                {
                    Layer = 0.5f,
                }
            };
            if (PlayerCount >= 1)
            {
                _sprites.Add(new Player(playerAnimations)
                {
                    Colour = Color.White,
                    Position = new Vector2((Game1.ScreenWidth - playerAnimations.First().Value.FrameWidth) / 2, (Game1.ScreenHeight - playerAnimations.First().Value.FrameHeight) / 2),
                    Layer = 0.3f,
                    Bullet = bulletPrefab,
                    Input = new Models.Input()
                    {
                        Up = Keys.W,
                        Down = Keys.S,
                        Left = Keys.A,
                        Right = Keys.D,
                        Shoot = Mouse.GetState().LeftButton,
                    },
                    Health = 6,
                    Camera = _camera,
                    Damage = 1, //Temporary as Damage will be affected by the equipped Gun's Damage
                });
            }
            _player = _sprites.Where(c => c is Player).Select(c => (Player)c).ToList();
            
            _mapManager = new MapManager(_content, 1, 9)
            {
                Bullet = bulletPrefab,
            };
            
            foreach (var room in _mapManager.RoomsList)
            {
                _rooms.Add(room);
                room.GenerateRoom();
                if (room is ItemRoom)
                {
                    ((ItemRoom)room).GenerateItem();
                }
                else if (room is EnemyRoom)
                {
                    ((EnemyRoom)room)._enemyManager.Players = _player;
                    ((EnemyRoom)room).GenerateObstacles();
                }
                foreach (var sprite in room.Sprites)
                {
                    _sprites.Add(sprite);
                }
            }

            _camera.MoveTo(_mapManager.RoomsList[0]);
        }
        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ChangeState(new MenuState(_game, _content));

            List<Sprite> sprites = new List<Sprite>();
            var collidableSprites = _sprites.Where(c => c is ICollidable);
            foreach (var collidableSprite in collidableSprites)
            {
                if (collidableSprite is Wall || collidableSprite is Obstacle)
                    sprites.Add(collidableSprite);
            }

            foreach (var sprite in _sprites)
            {
                if (sprite is Player)
                {
                    ((Player)sprite).CollidableSprites = sprites;
                }
                else if (sprite is Enemy)
                {
                    ((Enemy)sprite).CollidableSprites = sprites;
                }
                sprite.Update(gameTime);
                sprite._rectangle = sprite.Rectangle;
            }

            var enemyRooms = _rooms.Where(c => c is EnemyRoom);

            foreach (var room in enemyRooms)
            {
                if (((EnemyRoom)room).isVisible)
                {
                    ((EnemyRoom)room)._enemyManager.Update(gameTime);
                    if (((EnemyRoom)room)._enemyManager.CanAdd && !((EnemyRoom)room)._enemyManager.EnemiesAdded)
                    {
                        var enemy = ((EnemyRoom)room)._enemyManager.GetEnemy(room);
                        ((EnemyRoom)room)._enemyManager.Count++;
                        ((EnemyRoom)room).Enemies.Add(enemy);
                        _sprites.Add(enemy);
                    }  else if ((((EnemyRoom)room).Enemies.Count == 0) && ((EnemyRoom)room)._enemyManager.EnemiesAdded)
                    {
                        room.isCleared = true;
                    }
                }
                else
                {
                    foreach (var enemy in ((EnemyRoom)room).Enemies)
                    {
                        _sprites.Remove(enemy);
                    }
                }
            }

            /*
            //Camera code used only for debugging as it follows the player's exact movements
            foreach (var player in _player)
                _camera.Follow(player);
            */
        }
        public override void PostUpdate(GameTime gameTime)
        {
            //Add the children sprites to _sprites (the list of sprites)
            int spriteCount = _sprites.Count;
            for (int i = 0; i < spriteCount; i++)
            {
                var sprite = _sprites[i];
                foreach (var child in sprite.Children)
                    _sprites.Add(child);

                sprite.Children = new List<Sprite>();
            }

            for (int i = 0; i < _sprites.Count; i++)
            {
                if (_sprites[i].IsRemoved)
                {
                    if (_sprites[i].ParentRoom != null && _sprites[i].ParentRoom is EnemyRoom)
                    {
                        var parentRoom = _sprites[i].ParentRoom;
                        var enemyIndex = ((EnemyRoom)parentRoom).Enemies.IndexOf((Enemy)_sprites[i]);
                        ((EnemyRoom)parentRoom).Enemies.RemoveAt(enemyIndex);
                        _sprites.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        _sprites.RemoveAt(i);
                        i--;
                    }
                }
                else if (_sprites[i] is Door)
                {
                    var parentRoom = ((Door)_sprites[i]).ParentRoom;
                    var connectedRoom = ((Door)_sprites[i]).ConnectedRoom;
                    if (!parentRoom.isVisible)
                    {
                        _sprites.RemoveAt(i);
                        i--;
                    }
                    if (connectedRoom.isVisible)
                    {
                        foreach (var sprite in connectedRoom.Sprites)
                            _sprites.Add(sprite);
                    }
                }
                else if (_sprites[i] is Wall)
                {
                    if (!((Wall)_sprites[i]).ParentRoom.isVisible)
                    {
                        _sprites.RemoveAt(i);
                        i--;
                    }
                }
                else if (_sprites[i] is Floor)
                {
                    if (!((Floor)_sprites[i]).ParentRoom.isVisible)
                    {
                        _sprites.RemoveAt(i);
                        i--;
                    }
                }
                else if (_sprites[i] is Obstacle)
                {
                    if (!((Obstacle)_sprites[i]).ParentRoom.isVisible)
                    {
                        _sprites.RemoveAt(i);
                        i--;
                    }
                }
            }

            var collidableSprites = _sprites.Where(c => c is ICollidable);
            foreach (var spriteA in collidableSprites)
            {
                if (spriteA is Wall) //Walls themselves inherit from ICollidable, but their OnCollide() method does nothing. Thus, we skip it to save performance
                    continue;
                if (spriteA is Obstacle) //Obstacles themselves inherit from ICollidable as they require collision detection, but have no need for an OnCollide() method call
                    continue;
                if (spriteA is Door) //Doors themselves inherit from ICollidable as they require collision detection, but have no need for an OnCollide() method call
                    continue;
                foreach (var spriteB in collidableSprites)
                {
                    if (spriteA == spriteB) //If they are the same sprite dont do anything
                        continue;

                    if (spriteA.Intersects(spriteB))
                    {
                        ((ICollidable)spriteA).OnCollide(spriteB);
                    }
                }
            }

            if (_player.All(c => c.IsDead))
            {
                _game.ChangeState(new MenuState(_game, _content));
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: _camera.Transform);

            foreach (var sprite in _sprites)
                sprite.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();
            float x = 10f;
            foreach (var player in _player)
            {
                spriteBatch.DrawString(_font, "Health: " + player.Health, new Vector2(x, 10f), Color.White);
            }
            spriteBatch.End();
        }
        #endregion
    }
}