using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using System.Drawing;

namespace MouseToMove {
    class PlayerCharacter : Character {
        public float speed = 90.0f;
        float animFPS = 1.0f / 9.0f;
        float animTimer = 0f;
        protected Point targetTile = new Point(2, 1);
        public void SetTargetTile(Point target) {
            targetTile = new Point(target.X, target.Y);
        }
        public PlayerCharacter(string spritePath) : base(spritePath) {
            AddSprite("Down", new Rectangle(59, 1, 24, 30), new Rectangle(87, 1, 24, 30));
            AddSprite("Up", new Rectangle(115, 3, 22, 28), new Rectangle(141, 3, 22, 30));
            AddSprite("Left", new Rectangle(1, 1, 26, 30), new Rectangle(31, 1, 26, 30));
            AddSprite("Right", new Rectangle(195, 1, 26, 30), new Rectangle(167, 1, 26, 30));
            SetSprite("Down");
        }
        public void Update(float deltaTime) {
            InputManager i = InputManager.Instance;
            Point currentTile = new Point((int)Position.X / Game.TILE_SIZE, (int)Position.Y / Game.TILE_SIZE);
            //Keyboard movement
            if (targetTile.X < currentTile.X) {
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
            else if (targetTile.X > currentTile.X) {
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
            else if (Position.X != currentTile.X) {
                //More than two pixels away, walk
                if (Math.Abs(Position.X - currentTile.X * Game.TILE_SIZE) > 2) {
                    SetSprite("Left");
                    Animate(deltaTime);
                    Position.X -= speed * deltaTime;
                }
                //two pixels away, snap
                else {
                    Position.X = currentTile.X * Game.TILE_SIZE;
                }
            }
            if (targetTile.Y < currentTile.Y) {
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
            else if (targetTile.Y > currentTile.Y) {
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
            else if (Position.Y != currentTile.Y) {
                if (Math.Abs(Position.Y - currentTile.Y * Game.TILE_SIZE) > 2) {
                    SetSprite("Up");
                    Animate(deltaTime);
                    Position.Y -= speed * deltaTime;
                }
                else {
                    Position.Y = currentTile.Y * Game.TILE_SIZE;
                }
            }
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
