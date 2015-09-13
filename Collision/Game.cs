using System;
using System.Collections.Generic;
using GameFramework;
using System.Drawing;


namespace Collision {
    class Game {
        protected Point spawnTile = new Point(2, 1);
        protected PlayerCharacter hero = null;
        protected string spriteSheet = "Assets/HouseTiles.png";
        protected string heroSheet = "Assets/Link.png";
        public OpenTK.GameWindow window = null;
        protected Tile[][] map = null;
        protected Rectangle[] spriteSources = new Rectangle[] {
            new Rectangle(466,32,30,30),
            new Rectangle(466,1,30,30)
        };
        protected int[][] mapLayout = new int[][] {
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 1, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 1, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 }
        };
        Tile[][] GenerateMap(int[][] layout,string sheet, Rectangle[] sources) {
            Tile[][] result = new Tile[layout.Length][];
            float scale = 1.0f;
            for (int i = 0; i < layout.Length; i++) {
                result[i] = new Tile[layout[i].Length];
                for (int j = 0; j < layout[i].Length; j++) {
                    Rectangle source = sources[layout[i][j]];
                    Point WorldPosition = new Point(0, 0);
                    WorldPosition.X = (int)(i * WorldPosition.X);
                    WorldPosition.Y = (int)(j * WorldPosition.Y);
                    result[i][j] = new Tile(sheet, source);
                    result[i][j].Walkable = layout[i][j] == 0;
                    result[i][j].WorldPosition = WorldPosition;
                    result[i][j].Scale = scale;
                }
            }
            return result;
        }
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
        public void Initialize(OpenTK.GameWindow windw) {
            window = windw;
            window.ClientSize = new Size(mapLayout[0].Length * 30, mapLayout.Length * 30);
            TextureManager.Instance.UseNearestFiltering = true;
            map = GenerateMap(mapLayout, spriteSheet, spriteSources);
            hero = new PlayerCharacter(heroSheet, new Point(spawnTile.X * 30, spawnTile.Y * 30));
        }
        public void Update(float deltaTime) {
            hero.Update(deltaTime);
        }
        public void Shutdown() {
            for (int i = 0; i < map.Length; i++) {
                for(int j = 0; j < map[i].Length; j++) {
                    map[i][j].Destroy();
                }
            }
            hero.Destroy();
        }
        public void Render() {
            hero.Render();
            for (int i = 0; i < map.Length; i++) {
                for (int j = 0; j < map[i].Length; j++) {
                    map[i][j].Render();
                }
            }
        }
    }
}
