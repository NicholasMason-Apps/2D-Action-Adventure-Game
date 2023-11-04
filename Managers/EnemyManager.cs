using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Roguelite_Game.Models;
using Roguelite_Game.Sprites;
using Roguelite_Game.Sprites.Enemies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Managers
{
    public class EnemyManager
    {
        private float _timer;
        private Dictionary<string, Animation> _animations;
        private Vector2 _offset;
        public bool CanAdd { get; set; }
        public Bullet Bullet { get; set; }
        public int MaxEnemies { get; set; }
        public float SpawnTimer { get; set; }
        public bool EnemiesAdded { get; set; }
        public int Count { get; set; }
        public List<Player> Players { get; set; }

        public EnemyManager(ContentManager content, Vector2 offsetMultiplier)
        {
            _animations = new Dictionary<string, Animation>()
            {
                {"Idle", new Animation(content.Load<Texture2D>("EnemyContent/L1E"), 1) },
            };
            MaxEnemies = 3;
            SpawnTimer = 2.5f;
            _offset = new Vector2(1280, 720) * offsetMultiplier;
            EnemiesAdded = false;
            Count = 0;
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            CanAdd = false;

            if (_timer > SpawnTimer)
            {
                CanAdd = true;
                _timer = 0f;
            }
            if (Count >= MaxEnemies)
            {
                EnemiesAdded = true;
            }
        }
        public Enemy GetEnemy(Room room)
        {
            return new Brawler(_animations)
            {
                Colour = Color.Red,
                Bullet = Bullet,
                Health = 4,
                Layer = 0.2f,
                Position = new Vector2(Game1.Random.Next(128, Game1.ScreenWidth - 128), Game1.ScreenHeight / 2) + _offset,
                Speed = 2f,
                Damage = 1,
                ShootTimer = 1.5f,
                ParentRoom = room,
                Player = Players[0],
                Offset = _offset,
            };
        }
    }
}
