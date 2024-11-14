using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CandyCrush
{
    internal class Board : Sprite
    {

        Dictionary<CandyTypes, Texture2D> TypeToTexture;

        //finish making the dictionary and then link the candytypes to their
        //respective texture in order to display a grid of random candy types

        const int candyAmount = 9;

        CandyTypes[,] candies = new CandyTypes[candyAmount, candyAmount];

        public List<Texture2D> candyTextures = new List<Texture2D>(6);

        Random random = new Random();

        Point candySize;

        Point selectedPiece;

        MouseState mouse;

        MouseState previousState;

        Particle[] particles;


        

        public enum Directions
        {
            None,
            Up,
            Down,
            Left,
            Right,
        }

        public Board(Texture2D texture, Dictionary<CandyTypes, Texture2D> candyTextures, Vector2 position, Point size, Color color)
            : base(position, texture, Vector2.Zero, size, color)
        {
            //set list values to the images and then use it to link the textures from the list
            //to the candy its related too from the enum

            candySize = new Point(size.X / 9, size.Y / 9);

            AssignCandies();

            TypeToTexture = candyTextures;

            particles = new Particle[100];

            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = new Particle(new Vector2(0, 0), texture, new Vector2(0, 0), new Point(10, 10), Color.OrangeRed);
            }

        }

        public void AssignCandies()
        {
            for (int i = 0; i < candies.GetLength(0); i++)
            {
                for (int j = 0; j < candies.GetLength(1); j++)
                {
                    do
                    {
                        candies[i, j] = (CandyTypes)random.Next(1, 7);
                    }
                    while ((i >= 2 && candies[i, j] == candies[i - 1, j] && candies[i, j] == candies[i - 2, j])
                        || (j >= 2 && candies[i, j] == candies[i, j - 1] && candies[i, j] == candies[i, j - 2]));
                }
            }
        }


        private void DrawGrid(SpriteBatch spriteBatch, Texture2D texture, int blockSize, int gridAmount, Color color)
        {
            int x = 0;
            int y;
            for (int i = 0; i < gridAmount; i++)
            {
                y = 0;

                for (int j = 0; j < gridAmount; j++)
                {
                    spriteBatch.Draw(texture, new Rectangle(x, y, blockSize, blockSize), color);

                    y += (blockSize << 1);

                    j++;
                }

                x += (blockSize << 1);

                i++;
            }

            x = 100;

            for (int i = 0; i < gridAmount; i++)
            {
                y = 100;

                for (int j = 0; j < gridAmount; j++)
                {
                    spriteBatch.Draw(texture, new Rectangle(x, y, blockSize, blockSize), color);

                    y += (blockSize << 1);

                    j++;
                }

                x += (blockSize << 1);

                i++;
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawGrid(spriteBatch, Texture, candySize.X, 9, new Color(255 / 3, 117 / 3, 24 / 3, 128));


            for (int i = 0; i < candies.GetLength(0); i++)
            {
                for (int j = 0; j < candies.GetLength(1); j++)
                {
                    if (candies[i,j] == CandyTypes.None)
                    {
                        for (int k = 0; k < particles.Length; k++)
                        {
                            particles[k].Draw(spriteBatch);
                        }

                        continue;
                    }

                    spriteBatch.Draw(TypeToTexture[candies[i, j]], new Rectangle((candySize.X * i) + 20, (candySize.Y * j) + 20, candySize.X - 40, candySize.Y - 40), color);
                }
            }

        }

        public Directions DirectionCheck(Point currentPosition, int xChange, int yChange)
        {
            if (xChange > yChange)
            {
                if (currentPosition.X > selectedPiece.X)
                {
                    return Directions.Right;
                }
                else if (currentPosition.X < selectedPiece.X)
                {
                    return Directions.Left;
                }
            }

            else if (xChange < yChange)
            {
                if (currentPosition.Y < selectedPiece.Y)
                {
                    return Directions.Up;
                }
                else if (currentPosition.Y > selectedPiece.Y)
                {
                    return Directions.Down;
                }
            }

            return Directions.None;
        }

        public Point CandySwap(Directions direction)
        {
            //finish making candy swap yes alright

            Point target = selectedPiece;

            switch (direction)
            {
                case Directions.Up:
                    if (target.Y > 0)
                    {
                        target.Y--;
                    }
                    

                    break;

                case Directions.Down:
                    if (target.Y < candies.GetUpperBound(1))
                    {
                        target.Y++;
                    }

                    
                    break;

                case Directions.Left:
                    if (target.X > 0)
                    {
                        target.X--;
                    }
                    break;

                case Directions.Right:
                    if (target.X < candies.GetUpperBound(0))
                    {
                        target.X++;
                    }
                    break;
            }

            Point whereSelectedPieceCameFrom = selectedPiece;

            (candies[selectedPiece.X, selectedPiece.Y], candies[target.X, target.Y]) = (candies[target.X, target.Y], candies[selectedPiece.X, selectedPiece.Y]);

            selectedPiece = target;

            return whereSelectedPieceCameFrom;
        }

        public List<Point> NumInARow(Point piece)
        {
            int total = 0;

            List<Point> backtoback = new List<Point>(3);
            backtoback.Add(piece);
            int numInRow = 0;
            while (piece.X + numInRow + 1 < candies.GetLength(0) && candies[piece.X + numInRow, piece.Y] == candies[piece.X + numInRow + 1, piece.Y])
            {
                numInRow++;

                backtoback.Add(new Point(piece.X + numInRow, piece.Y));
            }

            total += numInRow;
            numInRow = 0;

            while (piece.X - numInRow - 1 >= 0 && candies[piece.X - numInRow, piece.Y] == candies[piece.X - numInRow - 1, piece.Y])
            {
                numInRow++;

                backtoback.Add(new Point(piece.X - numInRow, piece.Y));
            }

            total += numInRow;

            //Return a list of which 3

            return backtoback;
        }

        public List<Point> NumInAColumn(Point piece)
        {
            int total = 1;
            int numInColumn = 0;

            List<Point> backtoback = new List<Point>(3);
            backtoback.Add(piece);

            while (piece.Y + numInColumn + 1 < candies.GetLength(0) && candies[piece.X, piece.Y + numInColumn] == candies[piece.X, piece.Y + numInColumn + 1])
            {
                numInColumn++;

                backtoback.Add(new Point(piece.X, piece.Y + numInColumn));
            }

            total += numInColumn;
            numInColumn = 0;

            while (piece.Y - numInColumn - 1 >= 0 && candies[piece.X, piece.Y - numInColumn] == candies[piece.X, piece.Y - numInColumn - 1])
            {
                numInColumn++;

                backtoback.Add(new Point(piece.X, piece.Y - numInColumn));
            }

            total += numInColumn;


            //Return a list of which 3

            return backtoback;
        }

        public void Update(MouseState currentState)
        {
            if (currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released && Hitbox.Contains(currentState.Position))
            {
                selectedPiece = new Point(currentState.Position.X / candySize.X, currentState.Position.Y / candySize.Y);
            }

            if (currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
            {
                int xChange = Math.Abs(selectedPiece.X - (currentState.Position.X / candySize.X));
                int yChange = Math.Abs(selectedPiece.Y - (currentState.Position.Y / candySize.Y));

                Directions DirectionCheckedDirection = DirectionCheck(new(currentState.Position.X / candySize.X, currentState.Position.Y / candySize.Y), xChange, yChange);

                Point pieceYouSwappedWith = CandySwap(DirectionCheckedDirection);

                Crush(selectedPiece);
                Crush(pieceYouSwappedWith);
            }

            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Update();
            }



            previousState = currentState;
        }

        private void Crush(Point piece)
        {
            List<Point> numRow = NumInARow(piece);

            List<Point> numCol = NumInAColumn(piece);

            int longerStreak = numCol.Count;

            if (numRow.Count > numCol.Count)
            {
                longerStreak = numRow.Count;
            }

            if (longerStreak < 3) return;




            //MAKE PARTICLES EXPLODE FROM EACH PIECE INDIVIDUALLY THAT'S IN A ROW (3, 4, 5)!!!


            //when you crush a candy, reset the opacity to zero on the particles


            for (int i = 0; i < longerStreak; i++)
            {
                if (longerStreak == numRow.Count)
                {
                    candies[numRow[i].X, numRow[i].Y] = CandyTypes.None;

                    particles[i].Position = new Vector2((numRow[i].X) * candyAmount, (numRow[i].Y) * candyAmount);

                }
                else
                {

                    candies[numCol[i].X, numCol[i].Y] = CandyTypes.None;

                    particles[i].Position = new Vector2((numCol[i].X) * candyAmount, (numCol[i].Y) * candyAmount);

                }
            }

            //doing special shit:
            switch (longerStreak)
            {
                case 4:
                    //striped candy
                    break;

                case 5:
                    //chocolate candy
                    break;
            }
        }
    }
}
