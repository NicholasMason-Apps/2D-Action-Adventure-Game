using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Roguelite_Game.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Sprites
{
    class EnemyRoom : Room
    {
        public List<Enemy> Enemies;
        public EnemyManager _enemyManager;
        private Random rnd = new Random();
        private Texture2D _obstacleTexture;
        
        public EnemyRoom(ContentManager content, Vector2 offsetMultiplier, EnemyManager enemyManager) : base(content, offsetMultiplier)
        {
            _enemyManager = enemyManager;

            Enemies = new List<Enemy>();
            isCleared = false;
            _obstacleTexture = content.Load<Texture2D>("MapContent/Obstacle_1");
        }
        public Obstacle GetObstacle(Vector2 position)
        {
            return new Obstacle(_obstacleTexture)
            {
                Position = position,
                ParentRoom = this,
                Layer = 0.2f,
            };
        }

        public void GenerateObstacles()
        {
            var obstacleLayout = rnd.Next(1, 5);
            var obstacleOrigin = Game1.TileSize / 2;
            if (obstacleLayout == 1) //No obstacles
            {
                return;
            }
            else if (obstacleLayout == 2) //First obstacle layout
            {
                for (int i = Game1.TileSize * 2; i < Game1.ScreenWidth - (Game1.TileSize * 2); i += Game1.TileSize)
                {
                    if (i >= 576 && i <= 640)
                        continue;
                    else
                    {
                        Sprites.Add(GetObstacle(new Vector2(i + obstacleOrigin, 8 + obstacleOrigin + Game1.TileSize * 3) + Offset));
                        Sprites.Add(GetObstacle(new Vector2(i + obstacleOrigin, 8 + obstacleOrigin + Game1.TileSize * 7) + Offset));
                    }
                }
            }
            else if (obstacleLayout == 3) //Second obstacle layout
            {
                for (int i = Game1.TileSize * 2; i < Game1.ScreenWidth - (Game1.TileSize * 2); i += Game1.TileSize)
                {
                    if (i >= 576 && i <= 640)
                        continue;
                    else
                    {
                        Sprites.Add(GetObstacle(new Vector2(i + obstacleOrigin, 8 + obstacleOrigin + Game1.TileSize * 2) + Offset));
                        Sprites.Add(GetObstacle(new Vector2(i + obstacleOrigin, 8 + obstacleOrigin + Game1.TileSize * 8) + Offset));
                    }
                }
                for (int i = Game1.TileSize * 3; i < Game1.ScreenHeight - (Game1.TileSize * 3); i += Game1.TileSize)
                {
                    if (i == 320)
                        continue;
                    else
                    {
                        Sprites.Add(GetObstacle(new Vector2(Game1.TileSize * 2 + obstacleOrigin, 8 + i + obstacleOrigin) + Offset));
                        Sprites.Add(GetObstacle(new Vector2((Game1.ScreenWidth - Game1.TileSize * 3) + obstacleOrigin, 8 + i + obstacleOrigin) + Offset));
                    }
                }
            }
            else if (obstacleLayout == 4)
            {
                for (int i = Game1.TileSize * 2; i < Game1.ScreenHeight - (Game1.TileSize * 3); i += Game1.TileSize)
                {
                    if (i == 320)
                        continue;
                    else
                    {
                        Sprites.Add(GetObstacle(new Vector2(Game1.TileSize * 6 + obstacleOrigin, 8 + i + obstacleOrigin) + Offset));
                        Sprites.Add(GetObstacle(new Vector2(Game1.TileSize * 5 + obstacleOrigin, 8 + i + obstacleOrigin) + Offset));

                        Sprites.Add(GetObstacle(new Vector2(Game1.ScreenWidth - (Game1.TileSize * 7) + obstacleOrigin, 8 + i + obstacleOrigin) + Offset));
                        Sprites.Add(GetObstacle(new Vector2(Game1.ScreenWidth - (Game1.TileSize * 6) + obstacleOrigin, 8 + i + obstacleOrigin) + Offset));
                    }
                }
            }
        }
    }
}
