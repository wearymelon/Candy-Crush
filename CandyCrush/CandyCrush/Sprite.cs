using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyCrush
{
    internal abstract class Sprite
    {

        public Vector2 Position;
        public Texture2D Texture;
        public Vector2 Speed;
        public Point Size;
        public Color color;
        public Rectangle Hitbox => new Rectangle(new Point((int)Position.X, (int)Position.Y), Size);

        public Sprite(Vector2 position, Texture2D texture, Vector2 speed, Point size, Color color)
        {
            Position = position;
            Texture = texture;
            Speed = speed;
            this.Size = size;
            this.color = color;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
