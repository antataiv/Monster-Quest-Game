using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Interfaces
{
    public interface IEnemyFactory
    {
        IEntity CreateEntity(string enemyName,Texture2D enemyImage);
    }
}
