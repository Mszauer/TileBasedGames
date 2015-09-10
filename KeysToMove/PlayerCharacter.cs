using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GameFramework;

namespace KeysToMove {
    class PlayerCharacter : Character {
        float speed = 90.0f;

        public PlayerCharacter(string spriteSheet, Point startPos) : base(spriteSheet, startPos) {
            AddSprite("Down", new Rectangle(59, 1, 24, 30));
            AddSprite("Up", new Rectangle(115, 3, 22, 28));
            AddSprite("Left", new Rectangle(1, 1, 26, 30));
            AddSprite("Right", new Rectangle(195, 1, 26, 30));
            SetSprite("Down");
        }
        public void Update(float deltaTime) {
            InputManager i = InputManager.Instance;
            PointF positionCpy = Position;
            if (i.KeyDown(OpenTK.Input.Key.A)|| i.KeyDown(OpenTK.Input.Key.Left)) {
                positionCpy.X -= speed * deltaTime;
            }
            else if (i.KeyDown(OpenTK.Input.Key.D) || i.KeyDown(OpenTK.Input.Key.Right)) {
                positionCpy.X += speed * deltaTime;
            }
            else if (i.KeyDown(OpenTK.Input.Key.W) || i.KeyDown(OpenTK.Input.Key.Up)) {
                positionCpy.Y -= speed * deltaTime;
            }
            else if (i.KeyDown(OpenTK.Input.Key.S) || i.KeyDown(OpenTK.Input.Key.Down)) {
                positionCpy.Y += speed * deltaTime;
            }
            Position = positionCpy;
        }
    }
}
