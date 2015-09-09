using System;
using System.Collections.Generic;
using GameFramework;
using System.Drawing;

namespace HeroPractice {
    class Game {
        protected Point spawnTile = new Point(2, 1);
        protected Character hero = null;
        protected string heroSheet = "Assets/Link.png";
        public OpenTK.GameWindow Window = null;
        protected Tile[][] map = null;
        protected int[][] mapLayout = new int[][] {
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 1, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 1, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 }
        };

        protected string spriteSheets = "Assets/HouseTiles.png";
        protected Rectangle[] spriteSources = new Rectangle[] {
            new Rectangle(466,32,30,30),
            new Rectangle(466,1,30,30)
        };
        Tile[][] GenerateMap(int[][] layout, string sheets, Rectangle[] sources) {
            Tile[][] result = new Tile[layout.Length][];
            float scale = 1.0f;
            for (int i = 0; i < layout.Length; i++) {
                result[i] = new Tile[layout[i].Length];

                for (int j = 0; j < layout[i].Length; j++) {
                    Rectangle source = sources[layout[i][j]];

                    Point worldPosition = new Point();
                    worldPosition.X = (int)(j * source.Width);
                    worldPosition.Y = (int)(i * source.Height);
                    result[i][j] = new Tile(sheets, source);
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
        public void Initialize(OpenTK.GameWindow window) {
            Window = window;
            window.ClientSize = new Size(mapLayout[0].Length*30, mapLayout.Length*30);
            TextureManager.Instance.UseNearestFiltering = true;
            map = GenerateMap(mapLayout, spriteSheets, spriteSources);
            hero = new Character(heroSheet, new Point(spawnTile.X * 30, spawnTile.Y * 30));

            hero.AddSprite("Down",new Rectangle(59,1,24,30));
            hero.AddSprite("Up", new Rectangle(115,3,22,28));
            hero.AddSprite("Left",new Rectangle(1,1,26,30));
            hero.AddSprite("Right", new Rectangle(195, 1, 26, 30));
            hero.SetSprite("Down");
        }
        public void Update(float dt) {
            InputManager i = InputManager.Instance; //local ref to input manager
            //using i just saves time
            if (i.KeyDown(OpenTK.Input.Key.Left) || i.KeyDown(OpenTK.Input.Key.A)) {
                hero.SetSprite("Left");
            }
            if (i.KeyDown(OpenTK.Input.Key.Right) || i.KeyDown(OpenTK.Input.Key.D)){
                hero.SetSprite("Right");
            }
            if (i.KeyDown(OpenTK.Input.Key.Up) || i.KeyDown(OpenTK.Input.Key.W)) {
                hero.SetSprite("Up");
            }
            if (i.KeyDown(OpenTK.Input.Key.Down) || i.KeyDown(OpenTK.Input.Key.S)) {
                hero.SetSprite("Down");
            }
        }
        public void Render() {
            for (int h = 0; h < map.Length; h++) {
                for (int w = 0; w < map[h].Length; w++) {
                    map[h][w].Render();
                }
            }
            hero.Render();
            
        }
        public void Shutdown() {
            for (int h = 0; h < map.Length; h++) {
                for (int w = 0; w < map[h].Length; w++) {
                    map[h][w].Destroy();
                }
            }
            hero.Destroy();
        }
    }
}
