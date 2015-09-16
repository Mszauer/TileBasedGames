//#define ENABLE_VERTICAL_MOVEMENT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using System.Drawing;

namespace Jumping {
    class PlayerCharacter : Character {
        public float speed = 90.0f;
        float animFPS = 1.0f / 9.0f;
        float animTimer = 0f;
        float impulse = -180.0f;//randomly chosen
        float velocity = 0.0f; // changes every frame
        protected float gravity = 7 * 30;//fall 7tiles / second
        public PlayerCharacter(string spritePath, Point pos) : base(spritePath, pos) {
            AddSprite("Down", new Rectangle(59, 1, 24, 30), new Rectangle(87, 1, 24, 30));
            AddSprite("Up", new Rectangle(115, 3, 22, 28), new Rectangle(141, 3, 22, 30));
            AddSprite("Left", new Rectangle(1, 1, 26, 30), new Rectangle(31, 1, 26, 30));
            AddSprite("Right", new Rectangle(195, 1, 26, 30), new Rectangle(167, 1, 26, 30));
            SetSprite("Down");
        }
        public void Update(float deltaTime) {
            InputManager i = InputManager.Instance;

            if (i.KeyDown(OpenTK.Input.Key.A) || i.KeyDown(OpenTK.Input.Key.Left)) {
                SetSprite("Left");
                Animate(deltaTime);
                Position.X -= speed * deltaTime;
                if (!Game.Instance.GetTile(Corners[CORNER_TOP_LEFT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_TOP_LEFT]));
                    if (intersection.Width * intersection.Height > 0) {
                        Position.X = intersection.Right;
                    }
                }
                if (!Game.Instance.GetTile(Corners[CORNER_BOTTOM_LEFT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_BOTTOM_LEFT]));
                    if (intersection.Width * intersection.Height > 0) {
                        Position.X = intersection.Right;
                    }
                }
            }
            else if (i.KeyDown(OpenTK.Input.Key.D) || i.KeyDown(OpenTK.Input.Key.Right)) {
                SetSprite("Right");
                Animate(deltaTime);
                Position.X += speed * deltaTime;
                if (!Game.Instance.GetTile(Corners[CORNER_TOP_RIGHT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_TOP_RIGHT]));
                    if (intersection.Width * intersection.Height > 0) {
                        Position.X = intersection.Left - Rect.Width;
                    }
                }
                if (!Game.Instance.GetTile(Corners[CORNER_BOTTOM_RIGHT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_BOTTOM_RIGHT]));
                    if (intersection.Width * intersection.Height > 0) {
                        Position.X = intersection.Left - Rect.Width;
                    }
                }
            }
#if ENABLE_VERTICAL_MOVEMENT
            if (i.KeyDown(OpenTK.Input.Key.W) || i.KeyDown(OpenTK.Input.Key.Up)) {
                SetSprite("Up");
                Animate(deltaTime);
                Position.Y -= speed * deltaTime;
                if (!Game.Instance.GetTile(Corners[CORNER_TOP_LEFT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_TOP_LEFT]));
                    if (intersection.Width * intersection.Height > 0) {
                        Position.Y = intersection.Bottom;
                    }
                }
                if (!Game.Instance.GetTile(Corners[CORNER_TOP_RIGHT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_TOP_RIGHT]));
                    if (intersection.Width * intersection.Height > 0) {
                        Position.Y = intersection.Bottom;
                    }
                }
            }
            else if (i.KeyDown(OpenTK.Input.Key.S) || i.KeyDown(OpenTK.Input.Key.Down)) {
                SetSprite("Down");
                Animate(deltaTime);
                Position.Y += speed * deltaTime;
                if (!Game.Instance.GetTile(Corners[CORNER_BOTTOM_LEFT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_BOTTOM_LEFT]));
                    if (intersection.Width * intersection.Height > 0) {
                        Position.Y = intersection.Top - Rect.Height;
                    }
                }
                if (!Game.Instance.GetTile(Corners[CORNER_BOTTOM_RIGHT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_BOTTOM_RIGHT]));
                    if (intersection.Width * intersection.Height > 0) {
                        Position.Y = intersection.Top - Rect.Height;
                    }
                }
            }
#else
            if (i.KeyPressed(OpenTK.Input.Key.Space)) {
                velocity = impulse;
            }
            velocity += gravity;
            if (velocity > gravity) {
                velocity = gravity;
            }
            Position.Y += velocity*deltaTime;
            if (!Game.Instance.GetTile(Corners[CORNER_BOTTOM_LEFT]).Walkable) {
                Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_BOTTOM_LEFT]));
                if (intersection.Width * intersection.Height > 0) {
                    Position.Y = intersection.Top - Rect.Height;
                }
            }
            if (!Game.Instance.GetTile(Corners[CORNER_BOTTOM_RIGHT]).Walkable) {
                Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_BOTTOM_RIGHT]));
                if (intersection.Width * intersection.Height > 0) {
                    Position.Y = intersection.Top - Rect.Height;
                }
            }
#endif
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
