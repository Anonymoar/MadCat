using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NutEngine;
using System.Collections.Generic;
using System.Linq;

namespace MadCat
{
    public class RotatingCatsScene : Scene
    {
        private int screenWidth;
        private int screenHeight;

        class RotatingCat
        {
            public Sprite sprite;
            public Vector2 velocity;
            public int direction;
        }

        private Random random;
        private Texture2D catTexture;
        private List<RotatingCat> cats;

        private void AddRandomCat()
        {
            RotatingCat cat = new RotatingCat();

            cat.velocity = new Vector2(
                  random.Next(50, 500)
                , random.Next(50, 500)
                );
            cat.sprite = new Sprite(catTexture);
            cat.direction = random.Next(-1, 2);

            cat.sprite.Position = new Vector2(
                  random.Next(0, screenWidth)
                , random.Next(0, screenHeight)
                );
            var scale = (float)random.NextDouble() % 2 + 0.1f;
            cat.sprite.Scale = new Vector2(scale, scale);
            cat.sprite.Rotation = MathHelper.ToRadians(random.Next(0, 360));

            cats.Add(cat);
            RootNode.AddChild(cat.sprite);
        }

        public RotatingCatsScene(Application app) : base(app)
        {
            cats = new List<RotatingCat>();
            random = new Random();
            catTexture = Content.Load<Texture2D>("cat"); /// Загрузить картинку

            screenHeight = app.GraphicsDevice.PresentationParameters.BackBufferHeight;
            screenWidth = app.GraphicsDevice.PresentationParameters.BackBufferWidth;
        }

        public override void Update(float deltaTime)
        {
            /// Выйти при нажатии escape. Точно также делается
            /// любая другая обработка нажатий.
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                App.Exit();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter)) {
                AddRandomCat();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Delete) && cats.Count > 0) {
                RootNode.RemoveChild(cats[0].sprite);
                cats.RemoveAt(0);
            }

            foreach (var cat in cats) {
                cat.sprite.Position += cat.velocity * deltaTime;

                /// Отскакивание от границ окна.
                if (cat.sprite.Position.X < 0.0f || cat.sprite.Position.X > screenWidth) {
                    cat.velocity.X *= -1.0f;
                }

                if (cat.sprite.Position.Y < 0.0f || cat.sprite.Position.Y > screenHeight) {
                    cat.velocity.Y *= -1.0f;
                }

                cat.sprite.Rotation = (
                      (cat.sprite.Rotation + 5 * deltaTime * cat.direction)
                    % (2 * MathHelper.Pi)
                    );
            }
        }
    }
}
