using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CandyCrush
{
    internal class Particle : Sprite
    {

        static Random random = new Random();

        
        /*
         * Make particles go along the random x and y speed
         * Random color for particles
         * Move at a constant speed
         */

        public Particle(Vector2 position, Texture2D texture, Vector2 speed, Point size, Color color)
            : base(position, texture, speed, size, color)
        {
            Speed.X = (float)(random.NextDouble() * 20 - 10);
            Speed.Y = (float)(random.NextDouble() * 20 - 10);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle(Position.ToPoint(), Size), null, color);
        }

        public void Update()
        {
            Color newColor = new Color(color, color.A - 1);

            Speed.Y += 0.1f;

            Position.X += Speed.X;
            Position.Y += Speed.Y;

            color = newColor;
        }

        public void Reset(MouseState mouse)
        {
            if (mouse.LeftButton == ButtonState.Pressed && Hitbox.Contains(mouse.Position))
            {
                Position.X = mouse.X;
                Position.Y = mouse.Y;
            }
        }

    }
}
