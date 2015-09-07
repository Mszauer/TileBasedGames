using System;
using System.Collections.Generic;
using GameFramework;
using System.Drawing;

namespace Skeleton {
    class Game {
        protected Tile[][] map = null;
        protected int[][] mapLayout = new int[][] {
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 1, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 1, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 }
        };

        protected string[] spriteSheets = new string[] {
            "Assets/HouseTiles.png",
            "Assets/HouseTiles.png"
        };
        protected Rectangle[] spriteSources = new Rectangle[] {
            new Rectangle(466,32,30,30),
            new Rectangle(466,1,30,30)
        };
        Tile[][] GenerateMap(int[][] layout, string[] sheets, Rectangle[] sources) {
            Tile[][] result = new Tile[layout.Length][];
            float scale = 1.0f;
            for (int i = 0; i < layout.Length; i++) {
                result[i] = new Tile[layout[i].Length];

                for (int j = 0; j < layout[i].Length; j++) {
                    string sheet = sheets[layout[i][j]];
                    Rectangle source = sources[layout[i][j]];

                    Point worldPosition = new Point();
                    worldPosition.X = (int)(j * source.Width);
                    worldPosition.Y = (int)(i * source.Height);
                    result[i][j] = new Tile(sheet, source);
                    result[i][j].Walkable = layout[i][j] == 0;
                    result[i][j].WorldPosition = worldPosition;
                    result[i][j].Scale = scale;
                }
            }
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
        public void Initialize() {
            TextureManager.Instance.UseNearestFiltering = true;
            map = GenerateMap(mapLayout, spriteSheets, spriteSources);
        }
        public void Update(float dt) {

        }
        public void Render() {
            for (int h = 0; h < map.Length; h++) {
                for (int w = 0; w < map[h].Length; w++) {
                    map[h][w].Render();
                }
            }
            
        }
        public void Shutdown() {

        }
    }
}
