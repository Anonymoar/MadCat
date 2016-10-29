using Microsoft.Xna.Framework;
using NutEngine;

namespace MadCat
{
    /// <summary>
    /// Класс игры "вращающиеся котики".
    /// </summary>
    public class RotatingCatsGame : Application
    {
        private const int SCREEN_WIDTH = 960;
        private const int SCREEN_HEIGHT = 540;

        public RotatingCatsGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.ToggleFullScreen();
            graphics.ApplyChanges();

            Content.RootDirectory = "Content"; /// Папка со всеми ресурсами
        }

        protected override void Initialize()
        {
            base.Initialize();
            var startScene = new RotatingCatsScene(this); ///Запустить первую сцены игры

            runWithScene(startScene);
        }
    }
}
