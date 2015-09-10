using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using GameFramework;
using System.Drawing;

namespace MovingPractice {
    class PlayerCharacter : Character{
        float speed = 90.0f;
        bool animating = false;
        float animFPS = 1.0f / 9.0f;
        float animTimer = 0.0f;
        public PlayerCharacter(string spritePath,Point pos) : base(spritePath,pos) {
            AddSprite("Down", new Rectangle(59, 1, 24, 30), new Rectangle(87, 1, 24, 30));
            AddSprite("Up", new Rectangle(115, 3, 22, 28), new Rectangle(141, 3, 22, 28));
            AddSprite("Left", new Rectangle(1, 1, 26, 30), new Rectangle(31, 1, 26, 31));
            AddSprite("Right", new Rectangle(195, 1, 26, 30), new Rectangle(167, 1, 26, 29));
            SetSprite("Down");
        }
        public void Update(float deltaTime) {
            InputManager i = InputManager.Instance;
            PointF positionCpy = Position;
            if (i.KeyDown(OpenTK.Input.Key.A) || i.KeyDown(OpenTK.Input.Key.Left)) {
                animating = true;
                if (animating) {
                    animTimer += deltaTime;
                    positionCpy.X -= speed * deltaTime;
                    if (animTimer > animFPS) {
                        animTimer -= animFPS;
                        currentFrame += 1;
                        if (currentFrame > SpriteSource[currentSprite].Length-1) {
                            currentFrame = 0;
                        }
                    }
                }
                animating = false;
            }
            else if (i.KeyDown(OpenTK.Input.Key.D) || i.KeyDown(OpenTK.Input.Key.Right)) {
                animating = true;
                if (animating) {
                    animTimer += deltaTime;
                    positionCpy.X += speed * deltaTime;
                    if (animTimer > animFPS) {
                        currentFrame += 1;
                        animTimer -= animFPS;
                        if (currentFrame > SpriteSource[currentSprite].Length-1) {
                            currentFrame = 0;
                        }
                    }
                }
                animating = false;
            }
            else if (i.KeyDown(OpenTK.Input.Key.W) || i.KeyDown(OpenTK.Input.Key.Up)) {
                animating = true;
                if (animating) {
                    animTimer += deltaTime;
                    positionCpy.Y -= speed * deltaTime;
                    if (animTimer > animFPS) {
                        currentFrame += 1;
                        animTimer -= animFPS;
                        if (currentFrame > SpriteSource[currentSprite].Length-1) {
                            currentFrame = 0;
                        }
                    }
                }
                animating = false;
            }
            else if (i.KeyDown(OpenTK.Input.Key.S) || i.KeyDown(OpenTK.Input.Key.Down)) {
                animating = true;
                if (animating) {
                    animTimer += deltaTime;
                    positionCpy.Y += speed * deltaTime;
                    if (animTimer > animFPS) {
                        currentFrame += 1;
                        animTimer -= animFPS;
                        if (currentFrame > SpriteSource[currentSprite].Length-1) {
                            currentFrame = 0;
                        }
                    }
                }
                animating = false;
            }
            Position = positionCpy;
        }
    }
}
