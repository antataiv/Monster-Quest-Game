using Microsoft.Xna.Framework.Graphics;
using MonsterQuest.Models.Entities.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Interfaces
{
    public interface IData
    {
        ICollection<Enemy> Enemies { get; }

        ICollection<IItem> Items { get; }

        ICollection<Texture2D> BulletImages { get; }

        void AddEnemies(Enemy enemy);

        void AddItems(IItem item);

        void AddBulletImage(Texture2D bulletImage);

        void RemoveInactiveElements();
    }
}
