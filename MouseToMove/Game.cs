using System;
using System.Collections.Generic;
using GameFramework;
using System.Drawing;
using OpenTK;

namespace MouseToMove {
    class Game {
        protected PlayerCharacter hero = null;
        protected string heroSheet = "Assets/Link.png";
        protected Point cursorTile = new Point(0, 0);
        public OpenTK.GameWindow Window = null;
        public static readonly int TILE_SIZE = 30;
        protected Map currentMap = null;
        protected string startingMap = "Assets/FirstRoom.txt";
        List<Bullet> projectiles = null;
        
        public Tile GetTile(PointF pixelPoint) {
            return currentMap[(int)pixelPoint.Y / TILE_SIZE][(int)pixelPoint.X / TILE_SIZE];
        }
        public Rectangle GetTileRect(PointF pixelPoint) {
            int xTile = (int)pixelPoint.X / TILE_SIZE;//integer math
            int yTile = (int)pixelPoint.Y / TILE_SIZE;
            Rectangle result = new Rectangle(xTile * TILE_SIZE, yTile * TILE_SIZE, TILE_SIZE, TILE_SIZE);
            return result;
        }
        //Singleton
        private static Game instance = null;
        public static Game Instance {
            get {
                if (instance == null) {
                    instance = new Game();
                }
                return instance;
            }
        }

        protected Game() {

        }
        public void Initialize(OpenTK.GameWindow window) {
            Window = window;
            window.ClientSize = new Size(15 * TILE_SIZE, 7 * TILE_SIZE);
            TextureManager.Instance.UseNearestFiltering = true;
            hero = new PlayerCharacter(heroSheet);
            currentMap = new Map(startingMap,hero);
            projectiles = new List<Bullet>();
        }
        public void Update(float dt) {
            currentMap = currentMap.ResolveDoors(hero);
            hero.Update(dt);
            cursorTile = new Point(InputManager.Instance.MousePosition.X/TILE_SIZE, InputManager.Instance.MousePosition.Y / TILE_SIZE);
            if (InputManager.Instance.MousePosition.X  / TILE_SIZE < currentMap[0].Length ) {
                if (InputManager.Instance.MousePressed(OpenTK.Input.MouseButton.Left) || InputManager.Instance.KeyPressed(OpenTK.Input.Key.M)) {
                    if (currentMap[cursorTile.Y][cursorTile.X].Walkable) {
                        hero.SetTargetTile(cursorTile);
                    }
                }
            }
            if (InputManager.Instance.KeyPressed(OpenTK.Input.Key.Space)) {
                Console.WriteLine("Fire!");
                PointF velocity = new PointF(0.0f, 0.0f);
                if (hero.currentSprite == "up") {
                    velocity.Y -= 100.0f;
                }
                else if (hero.currentSprite == "down") {
                    velocity.Y += 100.0f;
                }
                if (hero.currentSprite == "left") {
                    velocity.X -= 100.0f;
                }
                else if (hero.currentSprite == "right") {
                    velocity.X += 100.0f;
                }
                Console.WriteLine("Direction shot: " + hero.currentSprite);
                Console.WriteLine("Added bullet, velocity: " + velocity);
                projectiles.Add(new Bullet(hero.Center, velocity));
            }
                for (int i = projectiles.Count - 1; i >= 0; i--) {
                    projectiles[i].Update(dt);
                }
            currentMap.Update(dt, hero,projectiles);
        }
        public void Render() {
            currentMap.Render();
            hero.Render();
            for (int i = 0; i < projectiles.Count; i++) {
                projectiles[i].Render();
            }
            //Draw mouse indicator
            Rectangle currentTile = GetTileRect(InputManager.Instance.MousePosition);
                //Top
            GraphicsManager.Instance.DrawLine(new Point(currentTile.X, currentTile.Y), new Point(currentTile.X + currentTile.Width, currentTile.Y), Color.Red);
                //Bottom
            GraphicsManager.Instance.DrawLine(new Point(currentTile.X, currentTile.Y + currentTile.Height), new Point(currentTile.X + currentTile.Width, currentTile.Y + currentTile.Height), Color.Red);
                //Left
            GraphicsManager.Instance.DrawLine(new Point(currentTile.X, currentTile.Y), new Point(currentTile.X, currentTile.Y + currentTile.Height), Color.Red);
                //Right
            GraphicsManager.Instance.DrawLine(new Point(currentTile.X + currentTile.Width, currentTile.Y), new Point(currentTile.X+currentTile.Width, currentTile.Y + currentTile.Height), Color.Red);
        }
        public void Shutdown() {
            currentMap.Destroy();
            hero.Destroy();
        }
    }
}
