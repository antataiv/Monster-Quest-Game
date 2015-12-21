using Microsoft.Xna.Framework.Graphics;
using MonsterQuest.Interfaces;
using MonsterQuest.Models.Entities.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Core.Data
{
    public class Data : IData
    {
        private ICollection<Enemy> enemies ;
        private ICollection<IItem> items;
        private ICollection<Texture2D> bulletImages = new List<Texture2D>();

        public Data()
        {
            this.Enemies = new List<Enemy>();
            this.Items = new List<IItem>();
            this.BulletImages = new List<Texture2D>();
        }

        public ICollection<Enemy> Enemies { get; private set; }

        public ICollection<IItem> Items { get; private set; }

        public ICollection<Texture2D> BulletImages { get; private set; }

        public void AddEnemies(Enemy enemy)
        {
            this.enemies.Add(enemy);
        }

        public void AddItems(IItem item)
        {
            this.items.Add(item);
        }

        public void AddBulletImage(Texture2D bulletImage)
        {
            this.bulletImages.Add(bulletImage);
        }

        public void RemoveInactiveElements()
        {
            var activeEnemies = this.Enemies.Where(e => e.IsAlive == true).ToList();
            this.Enemies = activeEnemies;

            var activeItems = this.Items.Where(i => i.IsActive == true).ToList();
            this.Items = activeItems;
        }
    }
}
