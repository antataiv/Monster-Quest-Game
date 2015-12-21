using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonsterQuest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Models.Entities.Characters
{
    public delegate void GameOverEventHandler(object sender, EventArgs e);

    public class Elf : Character
    {
        private const int DefaultElfHealth = 100;
        private const int DefaultElfDamage = 0;
        private const int DefaultElfSpriteRows = 4;
        private const int DefaultElfSpriteCols = 5;
        private Vector2 DefaultElfOffset = new Vector2(25, 15);
        private Vector2 DefaultElfVelocity = new Vector2(8, 0);
        private Vector2 DefaultElfPosition = new Vector2(0, 330);
        private const int WalkingLeftInitialFrame = 0;
        private const int WalkingRightInitialFrame = 5;
        private const int WalkingLeftLastFrame = 5;
        private const int WalkingRightLastFrame = 10;

        public event GameOverEventHandler PointChanged;

        public Elf(Texture2D image, IBulletFactory bulletFactory, IData data,
            Vector2 DefaultElfOffset, Vector2 DefaultElfVelocity, Vector2 DefaultElfPosition)
            : base(image,
                DefaultElfHealth, DefaultElfDamage, DefaultElfSpriteRows, DefaultElfSpriteCols,
                WalkingLeftInitialFrame, WalkingRightInitialFrame, WalkingLeftLastFrame, WalkingRightLastFrame,
                bulletFactory, data, DefaultElfOffset, DefaultElfVelocity, DefaultElfPosition)
        {
            this.Image = image;
            this.CharacterPosition = DefaultElfPosition;
            this.Velocity = DefaultElfVelocity;
            this.BoundsOffset = DefaultElfOffset;
        }

        protected virtual void OnPointChanged(EventArgs e)
        {
            if (PointChanged != null)
                PointChanged(this, e);
        }

        public override void Attack()
        {
            throw new NotImplementedException();
        }
    }
}
