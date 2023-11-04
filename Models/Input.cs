using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Models
{
    ///The Input class is used to save code by allowing for different key inputs to be set to the same overall function
    ///E.g. a W key press and an UP arrow key press can be both set to an overall UP key for movement
    public class Input
    {
        public Keys Up { get; set; }
        public Keys Down { get; set; }
        public Keys Left { get; set; }
        public Keys Right { get; set; }
        public ButtonState Shoot { get; set; }
    }
}
