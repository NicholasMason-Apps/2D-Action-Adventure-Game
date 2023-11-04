using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Sprites
{
    public interface ICollidable
    {
        void OnCollide(Sprite sprite);
    }
}
