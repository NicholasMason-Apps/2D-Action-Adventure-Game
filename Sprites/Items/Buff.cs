using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Sprites.Items
{
    public class Buff
    {
        private string _type { get; set; }
        /// <summary>
        /// Valid Buff.cs types:
        ///     Movement
        ///     Health
        ///     GunDamage
        ///     GunFireRate
        ///     GunEffect
        ///     SkillDamage
        ///     Invulnerability
        ///     
        /// </summary>
        private float _boost { get; set; }

        public string Type 
        {
            get { return _type; }
        }
        public float Boost
        {
            get { return _boost; }
        }
        public Buff(string type, float boost)
        {
            _type = type;
            _boost = boost;
        }
    }
}
