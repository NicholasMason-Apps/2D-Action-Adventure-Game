using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelite_Game.Sprites
{
    public class Room
    {
        public bool isVisible;
        public bool isCleared = true;
        public List<Sprite> Sprites;

        private Vector2 _offset;
        private List<Texture2D> _wallTextures;
        private Texture2D _floorTexture;
        private List<Texture2D> _doorTextures;
        private List<Door> _doors;

        public Vector2 Offset
        {
            get { return _offset; }
        }

        public Room(ContentManager content, Vector2 offsetMultiplier)
        {
            isVisible = false;

            _offset = new Vector2(1280, 720) * offsetMultiplier;

            Sprites = new List<Sprite>();
            _doors = new List<Door>();
            _wallTextures = new List<Texture2D>()
            { 
                content.Load<Texture2D>("MapContent/Wall_Flat_2"), 
                content.Load<Texture2D>("MapContent/Wall_Corner_1") 
            };
            _floorTexture = content.Load<Texture2D>("MapContent/Floor_1");
            _doorTextures = new List<Texture2D>
            {
                content.Load<Texture2D>("MapContent/door temp"),
                //Add Load method for Boss Door later
            };
        }
        public Wall GetWall(Vector2 position, float rotation, string type)
        {
            if (type == "corner")
            {
                return new Wall(_wallTextures.ElementAt(1))
                {
                    Position = position,
                    Rotation = rotation,
                    ParentRoom = this,
                    Layer = 0.9f,
                };
            }
            else
            {
                return new Wall(_wallTextures.ElementAt(0))
                {
                    Position = position,
                    Rotation = rotation,
                    ParentRoom = this,
                    Layer = 0.9f
                };
            }
        }
        public Floor GetFloor(Vector2 position)
        {
            return new Floor(_floorTexture)
            {
                Position = position,
                ParentRoom = this,
                Layer = 0.1f,
            };
        }
        public Door GetDoor(Vector2 position, float rotation, string type, Room connectedRoom)
        {
            if (type == "boss")
            {
                return new Door(_doorTextures.ElementAt(1))
                {
                    Position = position,
                    Rotation = rotation,
                    Layer = 1f,
                    ParentRoom = this,
                    ConnectedRoom = connectedRoom,
                };
            }
            else
            {
                return new Door(_doorTextures.ElementAt(0))
                {
                    Position = position,
                    Rotation = rotation,
                    Layer = 1f,
                    ParentRoom = this,
                    ConnectedRoom = connectedRoom,
                };
            }
        }
        public void GenerateRoom()
        {
            //Wall Generation
            var wallOrigin = _wallTextures.ElementAt(0).Width / 2;

            Sprites.Add(GetWall(new Vector2(wallOrigin, wallOrigin + 8) + _offset , 0f, "corner"));
            Sprites.Add(GetWall(new Vector2(Game1.ScreenWidth - wallOrigin, wallOrigin + 8) + _offset, (float)Math.PI / 2, "corner"));
            Sprites.Add(GetWall(new Vector2(wallOrigin, Game1.ScreenHeight - wallOrigin - 8) + _offset, (float)Math.PI * 3 / 2, "corner"));
            Sprites.Add(GetWall(new Vector2(Game1.ScreenWidth - wallOrigin, Game1.ScreenHeight - wallOrigin - 8) + _offset, (float)Math.PI, "corner"));
            for (int i = _wallTextures.ElementAt(0).Width; i < Game1.ScreenWidth - _wallTextures.ElementAt(0).Width; i += _wallTextures.ElementAt(0).Width)
            {
                Sprites.Add(GetWall(new Vector2(i + wallOrigin, wallOrigin + 8) + _offset, 0f, "flat"));
                Sprites.Add(GetWall(new Vector2(i + wallOrigin, Game1.ScreenHeight - wallOrigin - 8) + _offset, (float)Math.PI, "flat"));
            }
            for (int i = _wallTextures.ElementAt(0).Height; i < Game1.ScreenHeight - 2 * _wallTextures.ElementAt(0).Height; i += _wallTextures.ElementAt(0).Height)
            {
                Sprites.Add(GetWall(new Vector2(wallOrigin, i + wallOrigin + 8) + _offset, (float)Math.PI * 3 / 2, "flat"));
                Sprites.Add(GetWall(new Vector2(Game1.ScreenWidth - wallOrigin, i + wallOrigin + 8) + _offset, (float)Math.PI / 2, "flat"));
            }

            //Floor Generation
            Sprites.Add(GetFloor(new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2) + _offset));
        }

        public void GenerateDoor(string direction, Room connectedRoom)
        {

            var doorOrigin = Game1.TileSize / 2;
            if (direction == "right")
            {
                Sprites.Add(GetDoor(new Vector2(Game1.ScreenWidth - doorOrigin, (64 * 5) + doorOrigin + 8) + _offset, (float)Math.PI / 2, "normal", connectedRoom));
            } 
            else if (direction == "left")
            {
                Sprites.Add(GetDoor(new Vector2(doorOrigin, (64 * 5) + doorOrigin + 8) + _offset, (float)Math.PI * 3/ 2, "normal", connectedRoom));
            }
            else if (direction == "up")
            {
                Sprites.Add(GetDoor(new Vector2(doorOrigin + (64 * 9) + 32, doorOrigin + 8) + _offset, 0f, "normal", connectedRoom));
            }
            else if (direction == "down")
            {
                Sprites.Add(GetDoor(new Vector2(doorOrigin + (64 * 9) + 32, Game1.ScreenHeight - doorOrigin - 8) + _offset, (float)Math.PI, "normal", connectedRoom));
            }
        }
    }
}
