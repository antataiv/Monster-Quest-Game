using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonsterQuest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Models.Entities
{
    public abstract class Entity : IEntity
    {
        private Texture2D image;
        private Rectangle bounds;
        private Vector2 position;
        private Vector2 velocity;
        private int width;
        private int height;
        private int health;
        private int damage;

        private bool isAlive = true;

        protected Entity(Texture2D image, int health, int damage)
        {
            this.Image = image;
            this.Bounds = bounds;
            this.Position = new Vector2(0, 330);
            //this.Velocity = velocity;
            this.Health = health;
            this.Damage = damage;

            this.SetBounds();
        }

        public Texture2D Image { get; protected set; }

        public Rectangle Bounds { get; protected set; }

        public Vector2 Position { get; protected set; }

        public Vector2 Velocity { get; protected set; }

        public int Width { get; protected set; }

        public int Height { get; protected set; }

        public int Health { get; protected set; }

        public int Damage { get; private set; }

        public bool IsAlive
        {
            get { return this.isAlive; }
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Attack();

        //public abstract void ApplyDamage(IGameObject target);

        //To do
        private void SetBounds()
        {
        }

        public bool CollisionDetected(IGameObject target)
        {
            if (this.bounds.Intersects(target.Bounds))
            {
                return true;
            }
            return false;
        }

        public void ReceiveDamage(int damage)
        {
            this.health -= damage;

            if (this.health <= 0)
            {
                this.isAlive = false;
            }
        }
    }
}
