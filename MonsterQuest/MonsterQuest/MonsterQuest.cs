using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonsterQuest.Core.Data;
using MonsterQuest.Core.Factories;
using MonsterQuest.Interfaces;
using MonsterQuest.Models.Entities.Characters;
using MonsterQuest.Models.Entities.Enemies;
using MonsterQuest.Models.Items;
using System;
using System.Collections.Generic;

namespace MonsterQuest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MonsterQuest : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private IBulletFactory bulletFactory;
        private IEnemyFactory enemyFactory;
        private IItemFactory itemFactory;
        private IData data;
        private Character player;

        private Texture2D playerImage;
        private Texture2D bulletImage;
        private Texture2D backgroundLevel0;
        private Texture2D backgroundLevel1;
        private Texture2D buttonImage;
        private Texture2D enemyImage;
        private SpriteFont titleFont;

        TimeSpan enemySpawnTime;
        TimeSpan giftSpawnTime;
        TimeSpan goldSpawnTime;

        TimeSpan previousSpawnTime;
        TimeSpan previousSpawnTimePotion;
        TimeSpan previousSpawnTimeGold;

        private Enemy enemy;
        private IItem gold;
        private IItem potion;

        public MonsterQuest()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.bulletFactory = new BulletFactory();
            this.enemyFactory = new EnemyFactory();
            this.itemFactory = new ItemFactory();
            this.data = new Data();

            previousSpawnTime = TimeSpan.Zero;
            previousSpawnTimePotion = TimeSpan.Zero;
            previousSpawnTimeGold = TimeSpan.Zero;
            enemySpawnTime = TimeSpan.FromSeconds(6.0f);
            giftSpawnTime = TimeSpan.FromSeconds(5.0f);
            goldSpawnTime = TimeSpan.FromSeconds(8.0f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            playerImage = this.Content.Load<Texture2D>("Images/transparentElf");
            backgroundLevel0 = this.Content.Load<Texture2D>("Images/level0_background");
            buttonImage = this.Content.Load<Texture2D>("Images/buttons_background");
            backgroundLevel1 = this.Content.Load<Texture2D>("Images/space_background");
            enemyImage = this.Content.Load<Texture2D>("Images/skeleton");

            titleFont = Content.Load<SpriteFont>("Fonts/title");
            bulletImage = this.Content.Load<Texture2D>("Images/Fireball");
            this.data.AddBulletImage(bulletImage);

            player = new Elf(playerImage, bulletFactory, data, new Vector2(25, 15), new Vector2(8, 0), new Vector2(0, 330));
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            //DetectClick();
            this.GenerateObjects(gameTime);

            player.Update(gameTime);
            //character.IntersectWithEnemies(enemies, this.character.Score);
            foreach (var enemy in this.data.Enemies)
            {
                enemy.Update(gameTime);
                var isColided = player.CollisionDetected(enemy);
                if (isColided && !enemy.HasAppliedDamage)
                {
                    player.ReceiveDamage(enemy.Damage);
                    enemy.HasAppliedDamage = true;
                }
                else if (!isColided && enemy.HasAppliedDamage)
                {
                    enemy.HasAppliedDamage = false;
                }
            }

            foreach (var item in this.data.Items)
            {
                item.Update(gameTime);
                var isColided = player.CollisionDetected(item);
                if (isColided && item.IsActive)
                {
                    player.CollectItem(item);
                    item.IsActive = false;
                }
            }

            foreach (var bullet in player.Bullets)
            {
                bullet.Update(gameTime);

                foreach (var enemy in this.data.Enemies)
                {
                    if (bullet.CollisionDetected(enemy))
                    {
                        enemy.ReceiveDamage(bullet.Damage);
                        bullet.IsActive = false;
                        if (!enemy.IsAlive)
                        {
                            this.player.IncrementScore(enemy.Score);
                        }
                    }
                }

                bullet.ApplyDamage(this.data.Enemies);
            }

            this.Window.Title = this.data.Enemies.Count.ToString();

            this.data.RemoveInactiveElements();
            this.player.RemoveInactiveBullets();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundLevel1, new Rectangle(0, 0, 800, 480), Color.White);

            spriteBatch.DrawString(titleFont, string.Format("Health: {0}", this.player.Health),
                new Vector2(5, 5),
                Color.White);
            spriteBatch.DrawString(titleFont, string.Format("Score: {0}", this.player.Score),
                new Vector2(150, 5),
                Color.White);
            spriteBatch.DrawString(titleFont, string.Format("Gold: {0}", this.player.Gold),
                new Vector2(270, 5),
                Color.White);

            //Draw Hero
            this.player.Draw(spriteBatch);

            //Draw Enemies
            foreach (var enemy in this.data.Enemies)
            {
                enemy.Draw(spriteBatch);
            }

            //Draw bullets
            foreach (var bullet in this.player.Bullets)
            {
                bullet.Draw(spriteBatch);
            }

            foreach (var item in this.data.Items)
            {
                item.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void GenerateObjects(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;

                enemy = this.enemyFactory.CreateEnemy("Skeleton", enemyImage);

                //enemy = new Enemy(texture, 900, 330, 2.5f);

                this.data.AddEnemies(enemy);
            }

            if (gameTime.TotalGameTime - previousSpawnTimePotion > giftSpawnTime)
            {
                previousSpawnTimePotion = gameTime.TotalGameTime;
                Texture2D texture = Content.Load<Texture2D>("Images/transparentItems");

                potion = this.itemFactory.CreateItem("Potion", texture);

                this.data.AddItems(potion);
            }

            if (gameTime.TotalGameTime - previousSpawnTimeGold > goldSpawnTime)
            {
                previousSpawnTimeGold = gameTime.TotalGameTime;
                Texture2D texture = Content.Load<Texture2D>("Images/transparentItems");

                gold = this.itemFactory.CreateItem("Gold", texture);

                this.data.AddItems(gold);
            }
        }
    }
}
