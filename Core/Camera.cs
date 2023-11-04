using Microsoft.Xna.Framework;
using Roguelite_Game.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Core
{
    public class Camera
    {
        public Matrix Transform { get; set; }
        public Vector2 CameraPosition { get; set; }

        //MoveTo() is the main Camera method
        public void MoveTo(Room target)
        {
            Transform = Matrix.CreateTranslation(
                -target.Offset.X - (Game1.ScreenWidth / 2),
                -target.Offset.Y - (Game1.ScreenHeight / 2),
                0) * Matrix.CreateTranslation(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2, 0);
            
            CameraPosition = target.Offset;
        }

        //Follow() is the debugging Camera method
        public void Follow(Sprite target)
        {
            Transform = Matrix.CreateTranslation(
                -target.Position.X - (target.Rectangle.Width / 2),
                -target.Position.Y - (target.Rectangle.Height / 2),
                0) * Matrix.CreateTranslation(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2, 0);
        }
    }
}
