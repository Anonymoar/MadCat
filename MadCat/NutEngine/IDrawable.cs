using Microsoft.Xna.Framework.Graphics;

namespace NutEngine
{
    interface IDrawable
    {
        void Draw(Transform2D currentTransform, SpriteBatch spriteBatch);
    }
}
