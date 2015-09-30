using System;
using System.Collections.Generic;
using GameFramework;
using System.Drawing;
using OpenTK;

namespace Isometric {
    class Game {
        protected Point spawnTile = new Point(2, 1);
        protected PlayerCharacter hero = null;
        public OpenTK.GameWindow Window = null;
        public bool GameOver = false;
        public static bool ViewWorldSpace = false;
        public static readonly int TILE_W = 69;
        public static readonly int TILE_H = 70;
        public int Score = 0;
        protected List<Bullet> projectiles = null;
        protected int Sprite = 0;
        protected string spriteSheets = "Assets/isometric.png";
        protected string heroSheet = "Assets/isometric.png";
        protected string npcSheet = "Assets/isometric.png";

        protected Map room1 = null;
        protected int[][] room1Layout = new int[][] {
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1},
            new int[] { 1, 0, 0, 0, 0, 0, 0, 1},
            new int[] { 1, 0, 0, 0, 1, 0, 0, 1},
            new int[] { 1, 0, 1, 0, 0, 0, 0, 1},
            new int[] { 1, 0, 0, 0, 0, 0, 0, 2},
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1},
            };
        protected Map room2 = null;
        protected int[][] room2Layout = new int[][] {
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            new int[] { 2, 0, 0, 0, 1, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 1, 0, 0, 1 },
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 }
        };
        protected Map currentMap = null;

        protected Rectangle[] spriteSources = new Rectangle[] {
            /* 0*/new Rectangle(120,166,138,70),
            /* 1*/new Rectangle(294,147,138,90),
            /* 2*/new Rectangle(120,166,138,70)
        };
        public Tile GetTile(PointF pixelPoint) {
            return currentMap[(int)pixelPoint.Y / TILE_H][(int)pixelPoint.X / TILE_W];
        }
        public Rectangle GetTileRect(PointF pixelPoint) {
            int xTile = (int)pixelPoint.X / TILE_W;//integer math
            int yTile = (int)pixelPoint.Y / TILE_H;
            Rectangle result = new Rectangle(xTile * TILE_W, yTile * TILE_H, TILE_W, TILE_H);
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
            //window.ClientSize = new Size(room1Layout[0].Length * tileSize, room1Layout.Length * tileSize);
            //Window.ClientSize = new Size(8 * tileSize, 6 * tileSize);
            Window.ClientSize = new Size(990, 550);
            projectiles = new List<Bullet>();
            Sprite = TextureManager.Instance.LoadTexture(spriteSheets);
            TextureManager.Instance.UseNearestFiltering = true;

            GraphicsManager.Instance.SetDepthRange(0, 21 * 21);

            hero = new PlayerCharacter(heroSheet, new Point(spawnTile.X * TILE_W, spawnTile.Y * TILE_H), 20);
            room1 = new Map(room1Layout, spriteSheets, spriteSources, 2, 0);
            room2 = new Map(room2Layout, spriteSheets, spriteSources, 0, 2);
            room1[4][7].MakeDoor(room2, new Point(1, 1));
            room2[1][0].MakeDoor(room1, new Point(6, 4));
            currentMap = room1;

            room1.AddEnemy(npcSheet, new Point(6 * TILE_W, 1 * TILE_H), true);
            room2.AddEnemy(npcSheet, new Point(1 * TILE_W, 4 * TILE_H), false);
            room1.AddItem(spriteSheets, new Rectangle(20, 198, 44, 49), 10, new Point(3 * TILE_W, 2 * TILE_H + 7));
            room1.AddItem(spriteSheets, new Rectangle(20, 198, 44, 49), 20, new Point(5 * TILE_W, 4 * TILE_H + 7));
            room2.AddItem(spriteSheets, new Rectangle(20, 198, 44, 49), 30, new Point(4 * TILE_W, 2 * TILE_H + 7));
        }
        public void Update(float dt) {
            if (InputManager.Instance.KeyPressed(OpenTK.Input.Key.U)) {
                if (ViewWorldSpace == false) {
                    ViewWorldSpace = true;
                }
                else {
                    ViewWorldSpace = false;
                }
            }
            if (!GameOver) {
                currentMap = currentMap.ResolveDoors(hero);
                hero.Update(dt);
                if (InputManager.Instance.KeyPressed(OpenTK.Input.Key.Space)) {
                    PointF velocity = new PointF(0.0f, 0.0f);
                    if (hero.currentSprite == "up") {
                        velocity.Y = -100.0f;
                    }
                    else if (hero.currentSprite == "down") {
                        velocity.Y = 100.0f;
                    }
                    if (hero.currentSprite == "left") {
                        velocity.X = -100.0f;
                    }
                    else if (hero.currentSprite == "right") {
                        velocity.X = 100.0f;
                    }
                    projectiles.Add(new Bullet(hero.Center, velocity,Sprite));
                }
                for (int i = projectiles.Count - 1; i >= 0; i--) {
                    projectiles[i].Update(dt);
                }
                currentMap.Update(dt, hero, projectiles);
            }
            else {
                if (InputManager.Instance.KeyPressed(OpenTK.Input.Key.Space)) {
                    currentMap = room1;
                    hero.Position = new Point(spawnTile.X * TILE_W, spawnTile.Y * TILE_H);
                    GameOver = false;
                }
            }
        }
        public void Render() {
            PointF offsetPosition = new PointF();
            //temp code
            if (!ViewWorldSpace) {
                offsetPosition.X = -200.0f;
                offsetPosition.Y = 150.0f;
            }
            
            /*
            offsetPosition.X = hero.Position.X - (float)(4 * tileSize);
            offsetPosition.Y = hero.Position.Y - (float)(3 * tileSize);
            
            // If the hero is less than half the camera close to the left or top corner
            if (hero.Position.X < 4 * tileSize) {
                offsetPosition.X = 0;
            }
            if (hero.Position.Y < 3 * tileSize) {
                offsetPosition.Y = 0;
            }
            // If the hero is less than half the camera close to the bottom or right corner
            if (hero.Position.X > (currentMap[0].Length - 4) * tileSize) {
                offsetPosition.X = (currentMap[0].Length - 8) * tileSize;
            }
            if (hero.Position.Y > (currentMap.Length - 3) * tileSize) {
                offsetPosition.Y = (currentMap.Length - 6) * tileSize;
            }
            */
            currentMap.Render(offsetPosition, hero.Center);
            /*
            foreach (PointF corner in hero.Corners) {
                if (!GetTile(corner).Walkable) {
                    GraphicsManager.Instance.DrawRect(GetTileRect(corner), Color.Violet);
                }
                else {
                    GraphicsManager.Instance.DrawRect(GetTileRect(corner), Color.Blue);
                }
            }*/
            hero.Render(offsetPosition);
            for (int i = 0; i < projectiles.Count; i++) {
                projectiles[i].Render(offsetPosition);
            }
            if (GameOver) {
                GraphicsManager.Instance.SetDepth(20 * 20);
                GraphicsManager.Instance.DrawRect(new Rectangle(0, 70, 240, 50), Color.CadetBlue);

                GraphicsManager.Instance.DrawString("Game Over", new PointF(70, 80), Color.Black);
                GraphicsManager.Instance.DrawString("Game Over", new PointF(69, 79), Color.White);

                GraphicsManager.Instance.DrawString("Press Space to play again", new PointF(5, 96), Color.Black);
                GraphicsManager.Instance.DrawString("Press Space to play again", new PointF(4, 95), Color.White);

            }
            GraphicsManager.Instance.DrawRect(new Rectangle(150, 0, 90, 20), Color.CadetBlue);
            GraphicsManager.Instance.DrawString("Score:" + Score, new PointF(155, 3), Color.Black);
            GraphicsManager.Instance.DrawString("Score:" + Score, new PointF(154, 2), Color.White);
        }
        public void Shutdown() {
            room1.Destroy();
            room2.Destroy();
            hero.Destroy();
            TextureManager.Instance.UnloadTexture(Sprite);
        }
    }
}
