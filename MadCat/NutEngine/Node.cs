using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace NutEngine
{
    /// <summary>
    /// Класс узла графа сцены.
    /// Граф сцены представляет собой дерево.
    /// Если один узел является ребенком другого, это значит,
    /// что помимо своих преобразований к нему будут применяться 
    /// все те же преобразования, что и к родителю.
    /// Это позволяет "прицеплять" один узел к другому.
    /// </summary>
    /// <remarks>
    /// Например, мы прицепили к коту шляпу, теперь нам не нужно
    /// двигать отдельно кота и отдельно шляпу, они стали одним целым.
    /// </remarks>
    public class Node
    {
        /// Родитель узла. Если он равен null, то
        /// это корень графа сцены.
        private Node parent;
        private List<Node> children; /// Дети узла

        /// Позиция, поворот и масштаб узла, хранимые
        /// в одной матрице. Она необходима для того, 
        /// чтобы можно было прицепить один узел к другому,
        /// и тогда положение ребенка будет определяется
        /// положением его родителя.
        protected Transform2D transform;

        public Node Parent { get { return parent; } }
        public List<Node> Children { get { return children; } }
        public Vector2 Position { get; set; } /// Позиция
        public Vector2 Scale { get; set; } /// Масштаб
        public float Rotation { get; set; } /// Поворот
        public int ZOrder { get; set; } /// Z индекс
        public bool Visible { get; set; } /// Скрыт ли узел

        public Node()
        {
            transform = new Transform2D();
            children = new List<Node>();
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0.0f;
            ZOrder = 0;
            Visible = true;
        }

        /// <summary>
        /// Применяем к узлу преобразования родителя,
        /// затем сортируем детей по ZOrder и рисуем в таком порядке:
        /// Дети с ZOrder меньше 0 -> сам узел -> дети с ZOrder больше либо равно 0
        /// </summary>
        public virtual void Visit(Transform2D currentTransform, SpriteBatch spriteBatch)
        {
            if (!Visible)
            {
                return;
            }

            /// TODO: Делать это только тогда, когда необходимо,
            /// то есть изменились Scale, Rotation и Position.
            transform.SetTransform(Scale, Rotation, Position); /// Пересчитать матрицу.

            currentTransform = transform * currentTransform; /// Перейти в новую систему координат

            var it = children.OrderBy(node => node.ZOrder).GetEnumerator();
            bool next = it.MoveNext();

            while (next && it.Current.ZOrder < 0) /// Узлы с ZOrder меньше нуля
            {
                it.Current.Visit(currentTransform, spriteBatch);
                next = it.MoveNext();
            }

            if (this is IDrawable) /// Отрисовать сам узел при необходимости
            {
                var drawable = (IDrawable)this; /// Так даже на MSDN делают
                drawable.Draw(currentTransform, spriteBatch);
            }

            while (next) /// Узлы с ZOrder больше либо равно нулю
            {
                it.Current.Visit(currentTransform, spriteBatch);
                next = it.MoveNext();
            }
        }

        /// <summary>
        /// Добавить ребенка.
        /// </summary>
        public void AddChild(Node child)
        {
            child.parent = this;
            children.Add(child);
        }

        /// <summary>
        /// Удалить ребенка.
        /// </summary>
        public void RemoveChild(Node child)
        {
            child.parent = null;
            children.Remove(child);
        }
    }
}
