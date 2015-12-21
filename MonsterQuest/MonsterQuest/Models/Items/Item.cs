using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonsterQuest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Models.Items
{
    public abstract class Item : IItem
    {
        private int activeTimeLimit;
        private int activeFrom = 0;
        private const int minY = 330;
        private Vector2 boundOffset;
        private bool isActive;
        private bool isOnTheGround = false;
        private Texture2D image;
        private Rectangle bounds;
        private Vector2 position;
        private Vector2 velocity;
        private int numOfRows;
        private int numOfCols;
        private int row;
        private int col;

        protected Item(int activeTimeLimit, Texture2D image,
            int numOfRows, int numOfCols, int row, int col)
        {
            this.activeTimeLimit = activeTimeLimit;
            this.Image = image;
            this.NumOfCols = numOfCols;
            this.NumOfRows = numOfRows;
            this.Row = row;
            this.Col = col;
            this.IsActive = true;
        }
        public Vector2 BoundsOffset { get;protected set; }

        public int NumOfCols { get; protected set; }

        public int NumOfRows { get; protected set; }

        public int Row { get; protected set; }

        public int Col { get; protected set; }

        public bool IsActive { get; set; }

        public Texture2D Image { get; protected set; }

        public Rectangle Bounds { get; protected set; }

        public Vector2 Position { get; protected set; }

        public Vector2 Velocity { get; protected set; }

        public void Update(GameTime gameTime)
        {
            this.activeFrom += gameTime.ElapsedGameTime.Milliseconds;
            if (this.activeFrom > this.activeTimeLimit)
            {
                this.isActive = false;
            }

            if (this.isActive)
            {
                if (this.position.Y >= minY)
                {
                    this.isOnTheGround = true;
                }
                if (!isOnTheGround)
                {
                    this.position += velocity;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                var width = image.Width / this.numOfCols;
                var height = image.Height / this.numOfRows;
               
                var sourceRectangle = new Rectangle(width * this.col, height * this.row, width, height);
                var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, width, height);

                this.bounds = new Rectangle((int)position.X + (int)boundOffset.X,
                                (int)position.Y + (int)boundOffset.Y,
                                width - 2 * (int)boundOffset.X,
                                height - 2 * (int)boundOffset.Y);

                spriteBatch.Draw(this.image, destinationRectangle, sourceRectangle, Color.White);
            }
        }

        public bool CollisionDetected(IGameObject target)
        {
            if (this.bounds.Intersects(target.Bounds))
            {
                return true;
            }
            return false;
        }

        protected int GenerateRandomPosition()
        {
            Random random = new Random();
            int widthOfScreen = 800;
            int xPosition = random.Next(0, widthOfScreen);
            return xPosition;
        }

    }
}
