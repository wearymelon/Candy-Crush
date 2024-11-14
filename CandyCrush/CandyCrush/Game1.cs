using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace CandyCrush
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;

        Texture2D backgroundTexture;
        Texture2D texture;

        Board board;

        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferHeight = 900;
            _graphics.PreferredBackBufferWidth = 900;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


            base.Initialize();

        }

        protected override void LoadContent()
        {
            backgroundTexture = Content.Load<Texture2D>("ducks");

           

            texture = new Texture2D(GraphicsDevice, 1, 1);

            texture.SetData<Color>(new Color[] { Color.White });

            Color color;
            color = Color.White;

            

            Dictionary<CandyTypes, Texture2D> fjfj = new Dictionary<CandyTypes, Texture2D>();

            fjfj.Add(CandyTypes.GumSquare, Content.Load<Texture2D>("CC gum square"));
            fjfj.Add(CandyTypes.JellyBean, Content.Load<Texture2D>("CC jelly bean"));
            fjfj.Add(CandyTypes.JujubeCluster, Content.Load<Texture2D>("CC jujube cluster"));
            fjfj.Add(CandyTypes.LemonDrop, Content.Load<Texture2D>("CC lemon drop"));
            fjfj.Add(CandyTypes.LollipopHead, Content.Load<Texture2D>("CC lollipop head"));
            fjfj.Add(CandyTypes.Lozenge, Content.Load<Texture2D>("CC lozenge"));


            spriteBatch = new SpriteBatch(GraphicsDevice);

            board = new Board(texture, fjfj, position: new Vector2(0, 0), size: new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), color);


            color.A = 255;


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            board.Update(Mouse.GetState());




            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            

            // TODO: Add your drawing code here


            spriteBatch.Begin(blendState: BlendState.NonPremultiplied);

            board.Draw(spriteBatch);

            

            spriteBatch.End();


            base.Draw(gameTime);
        }


    }
}
