﻿using System;
using System.Collections.Generic;
using GameFramework;
using System.Drawing;
using OpenTK;

namespace Shooting {
    class Game {
        protected Point spawnTile = new Point(2, 1);
        protected PlayerCharacter hero = null;
        public OpenTK.GameWindow Window = null;
        public bool GameOver = false;
        protected readonly int tileSize = 30;
        protected List<Bullet> projectiles = null;

        protected string spriteSheets = "Assets/HouseTiles.png";
        protected string heroSheet = "Assets/Link.png";
        protected string npcSheet = "Assets/NPC.png";

        protected Map room1 = null;
        protected int[][] room1Layout = new int[][] {
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 1, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 1, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 2 },
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 }
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
            new Rectangle(466,32,30,30),
            new Rectangle(466,1,30,30),
            new Rectangle(32,187,30,30)
        };
        public Tile GetTile(PointF pixelPoint) {
            return currentMap[(int)pixelPoint.Y / 30][(int)pixelPoint.X / 30];
        }
        public Rectangle GetTileRect(PointF pixelPoint) {
            int xTile = (int)pixelPoint.X / 30;//integer math
            int yTile = (int)pixelPoint.Y / 30;
            Rectangle result = new Rectangle(xTile * 30, yTile * 30, 30, 30);
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
            window.ClientSize = new Size(room1Layout[0].Length * tileSize, room1Layout.Length * tileSize);
            projectiles = new List<Bullet>();
            TextureManager.Instance.UseNearestFiltering = true;

            hero = new PlayerCharacter(heroSheet, new Point(spawnTile.X * tileSize, spawnTile.Y * tileSize));
            room1 = new Map(room1Layout, spriteSheets, spriteSources, 2, 0);
            room2 = new Map(room2Layout, spriteSheets, spriteSources, 0, 2);
            room1[4][7].MakeDoor(room2, new Point(1, 1));
            room2[1][0].MakeDoor(room1, new Point(6, 4));
            currentMap = room1;

            room1.AddEnemy(npcSheet, new Point(6 * tileSize, 1 * tileSize), true);
            room2.AddEnemy(npcSheet, new Point(1 * tileSize, 4 * tileSize), false);
        }
        public void Update(float dt) {
            if (!GameOver) {
                currentMap = currentMap.ResolveDoors(hero);
                hero.Update(dt);
                if (InputManager.Instance.KeyPressed(OpenTK.Input.Key.Space)) {
                    PointF velocity = new PointF(0.0f, 0.0f);
                    if (hero.currentSprite == "up" ) {
                        velocity.Y = -100.0f;
                    }
                    else if (hero.currentSprite == "down") {
                        velocity.Y = 100.0f;
                    }
                    if (hero.currentSprite == "left") {
                        velocity.X = -100.0f;
                    }
                    else if(hero.currentSprite == "right") {
                        velocity.X = 100.0f;
                    }
                    projectiles.Add(new Bullet(hero.Center, velocity));
                }
                for (int i = projectiles.Count-1; i > 0; i--) {
                    projectiles[i].Update(dt);
                }
                currentMap.Update(dt, hero,projectiles);
            }
            else {
                if (InputManager.Instance.KeyPressed(OpenTK.Input.Key.Space)) {
                    currentMap = room1;
                    hero.Position = new Point(spawnTile.X * tileSize, spawnTile.Y * tileSize);
                    GameOver = false;
                }
            }
        }
        public void Render() {
            currentMap.Render();
            /*
            foreach (PointF corner in hero.Corners) {
                if (!GetTile(corner).Walkable) {
                    GraphicsManager.Instance.DrawRect(GetTileRect(corner), Color.Violet);
                }
                else {
                    GraphicsManager.Instance.DrawRect(GetTileRect(corner), Color.Blue);
                }
            }*/
            hero.Render();
            for (int i = 0; i < projectiles.Count; i++) {
                projectiles[i].Render();
            }
            if (GameOver) {
                GraphicsManager.Instance.DrawRect(new Rectangle(0, 70, 240, 50), Color.CadetBlue);

                GraphicsManager.Instance.DrawString("Game Over", new PointF(70, 80), Color.Black);
                GraphicsManager.Instance.DrawString("Game Over", new PointF(69, 79), Color.White);

                GraphicsManager.Instance.DrawString("Press Space to play again", new PointF(5, 96), Color.Black);
                GraphicsManager.Instance.DrawString("Press Space to play again", new PointF(4, 95), Color.White);

            }
        }
        public void Shutdown() {
            room1.Destroy();
            room2.Destroy();
            hero.Destroy();

        }
    }
}
