using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using System.Drawing;

namespace Isometric {
    class Character {
        public int Sprite { get; set; }
        public PointF Position = new PointF(0f, 0f);
        public Dictionary<string, Rectangle[]> SpriteSources { get; private set; }
        public string currentSprite { get; set; }
        public int currentFrame = 0;
        protected float height = -1;
        float animFPS = 1.0f / 9.0f;
        float animTimer = 0f;
        public Rectangle Rect {
            get {
                if (height >= 0) {
                    return new Rectangle((int)Position.X, (int)Position.Y, SpriteSources[currentSprite][currentFrame].Width, (int)height);
                }
                return new Rectangle((int)Position.X, (int)Position.Y, SpriteSources[currentSprite][currentFrame].Width, SpriteSources[currentSprite][currentFrame].Height);
            }
        }
        public PointF Center {
            get {
                return new PointF(Rect.X + (Rect.Width / 2), Rect.Y + (Rect.Height / 2));
            }
        }
        public PointF[] Corners {
            get {
                float w = SpriteSources[currentSprite][currentFrame].Width;
                float h = SpriteSources[currentSprite][currentFrame].Height; ;
                if (height >= 0) {
                    h = height;
                }
                return new PointF[] {
                    new PointF(Position.X,Position.Y),
                    new PointF(Position.X+w,Position.Y),
                    new PointF(Position.X,Position.Y+h),
                    new PointF(Position.X+w,Position.Y+h)
                };
            }
        }
        public static readonly int CORNER_TOP_LEFT = 0;
        public static readonly int CORNER_TOP_RIGHT = 1;
        public static readonly int CORNER_BOTTOM_LEFT = 2;
        public static readonly int CORNER_BOTTOM_RIGHT = 3;
        public Character(string spritePath, Point pos, float height) {
            Sprite = TextureManager.Instance.LoadTexture(spritePath);
            Position = pos;
            this.height = height;
        }
        public void Render(PointF offsetPosition) {
            int tileX = (int)Corners[CORNER_BOTTOM_RIGHT].X / 30;
            int tileY = (int)(Corners[CORNER_BOTTOM_RIGHT].Y - 1) / 30;
            GraphicsManager.Instance.SetDepth(tileY * 20 + tileX + 0.5f);

            Point renderPosition = new Point((int)Position.X, (int)Position.Y);
            renderPosition.X -= (int)offsetPosition.X;
            renderPosition.Y -= (int)offsetPosition.Y;
            if (height >= 0) {
                //find depth offset
                int difference = SpriteSources[currentSprite][currentFrame].Height - (int)height;
                //apply  depth offset
                renderPosition.Y -= difference;
            }
            //GraphicsManager.Instance.DrawRect(Rect, Color.Red);
            TextureManager.Instance.Draw(Sprite, renderPosition, 1.0f, SpriteSources[currentSprite][currentFrame]);
            /*Rectangle center = new Rectangle((int)Center.X - 5, (int)Center.Y - 5, 10, 10);
            GraphicsManager.Instance.DrawRect(center, Color.Yellow);
            foreach (PointF corner in Corners) {
                Rectangle rect = new Rectangle((int)corner.X - 5, (int)corner.Y - 5, 10, 10);
                GraphicsManager.Instance.DrawRect(rect, Color.Green);
            }*/
            //get a 3x3 rect, centered on the top left pixel
            Rectangle positionLocator = new Rectangle((int)Position.X - 1, (int)Position.Y - 1, 3, 3);
            //apply offset
            positionLocator.X -= (int)offsetPosition.X;
            positionLocator.Y -= (int)offsetPosition.Y;
            //draw indicator
            GraphicsManager.Instance.DrawRect(positionLocator, Color.Yellow);
        }
        public void Destroy() {
            TextureManager.Instance.UnloadTexture(Sprite);
        }
        public void SetSprite(string name) {
            name = name.ToLower();
            if (SpriteSources.ContainsKey(name)) {
                currentSprite = name;
            }
            else {
                Console.WriteLine("Texture not found: " + name);
            }
        }
        public void AddSprite(string name, params Rectangle[] source) {
            name = name.ToLower();
            if (SpriteSources == null) {
                SpriteSources = new Dictionary<string, Rectangle[]>();
            }
            if (currentSprite == null) {
                currentSprite = name;
            }
            SpriteSources.Add(name, source);

        }
        protected void Animate(float dTime) {
            animTimer += dTime;
            if (animTimer > animFPS) {
                animTimer -= animFPS;
                currentFrame += 1;
                if (currentFrame > SpriteSources[currentSprite].Length - 1) {
                    currentFrame = 0;
                }
            }
        }
    }
}
