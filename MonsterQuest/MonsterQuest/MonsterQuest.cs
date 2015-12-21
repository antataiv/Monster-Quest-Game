using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonsterQuest.Core.Data;
using MonsterQuest.Core.Factories;
using MonsterQuest.Interfaces;
using MonsterQuest.Models.Entities.Characters;
using MonsterQuest.Models.Items;
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
        private SpriteFont titleFont;

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            playerImage = this.Content.Load<Texture2D>("Images/transparentElf");
            backgroundLevel0 = this.Content.Load<Texture2D>("Images/level0_background");
            buttonImage = this.Content.Load<Texture2D>("Images/buttons_background");
            backgroundLevel1 = this.Content.Load<Texture2D>("Images/space_background");

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
                bullet.ApplyDamage(this.data.Enemies);
            }

            ////this.Window.Title = character.keys.ToString();
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
    }
}
