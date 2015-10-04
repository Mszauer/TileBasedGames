using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GameFramework;
using System.IO;

namespace MouseToMove {
    class Map {
        protected Tile[][] tileMap = null;

        public Tile[] this[int i] {
            get {
                return tileMap[i];
            }
        }
        public int Length {
            get {
                return tileMap.Length;
            }
        }
        protected Point spawnTile = new Point(0, 0);
        protected string tileSheet = null;
        protected Dictionary<string, Point> nextRoom = null;
        protected List<EnemyCharacter> enemies = null;

        public Map(string mapPath, PlayerCharacter hero){
            if (System.IO.File.Exists(mapPath)){
                Console.WriteLine("Loading map...");
                List<int> walkableTile = new List<int>();
                List<int> doorIndex = new List<int>();
                Dictionary<int, Rectangle> spriteSources = new Dictionary<int, Rectangle>();
                List<List<int>> mapFormat = new List<List<int>>();
                nextRoom = new Dictionary<string, Point>();
                Dictionary<int,string> nextMap = new Dictionary<int, string>();

                using (TextReader reader = File.OpenText(mapPath)) {
                    string contents = reader.ReadLine();

                    while (contents != null) {
                        string[] content = contents.Split(' ');
                        
                        //load texture
                        if (content[0] == "T") {
                            //texture path
                            string path = content[1];
                            tileSheet = path;
                            Console.WriteLine("Texture Path: " + tileSheet);
                        }
                        //load source rects
                        else if (content[0] == "R") {
                            //source rect
                            Rectangle r = new Rectangle(System.Convert.ToInt32(content[2]), System.Convert.ToInt32(content[3]), System.Convert.ToInt32(content[4]), System.Convert.ToInt32(content[5]));
                            //adds rect index and source rect to dictionary
                            spriteSources.Add(System.Convert.ToInt32(content[1]), r);
                            Console.WriteLine("Rectangle added: " + r);
                        }
                        //walkable tile indices
                        else if (content[0] == "W") {
                            for(int i = 1; i < content.Length; i++) {
                                walkableTile.Add(System.Convert.ToInt32(content[i]));
                                Console.WriteLine("Walkable Tiles: " + System.Convert.ToInt32(content[i]));
                            }
                        }
                        //door tiles
                        else if (content[0] == "D") {
                            //identifies which tile is a door
                            doorIndex.Add(System.Convert.ToInt32(content[1]));
                            //door destination
                            nextRoom.Add(content[2], new Point(System.Convert.ToInt32(content[3]), System.Convert.ToInt32(content[4])));
                            //door spawn destination
                            nextMap.Add(System.Convert.ToInt32(content[1]), content[2]);
                            Console.WriteLine("Door tile index: " + System.Convert.ToInt32(content[1]));
                            Console.WriteLine("Next room path: " + content[2]);
                            Console.WriteLine("Door spawn location: " + content[3] + ", " + content[4]);
                        }
                        //starting tile
                        else if (content[0] == "S") {
                            spawnTile = new Point(System.Convert.ToInt32(content[1]), System.Convert.ToInt32(content[2]));
                            Console.WriteLine("Starting tile: " + System.Convert.ToInt32(content[1]) + ", " +  System.Convert.ToInt32(content[2]));
                        }
                        //add enemies
                        else if (content[0] == "E") {
                            if (enemies == null) {
                                enemies = new List<EnemyCharacter>();
                            }
                            bool upDownMove = content[2] == "X" ? false : true;
                            enemies.Add(new EnemyCharacter(content[1],upDownMove));
                            Console.WriteLine("Enemy added, Up Down Movement: " + upDownMove);
                            Console.WriteLine("Enemy sprite path: " + content[1]);
                            enemies[enemies.Count - 1].Position.X = System.Convert.ToInt32(content[3])*Game.TILE_SIZE;
                            enemies[enemies.Count - 1].Position.Y = System.Convert.ToInt32(content[4])*Game.TILE_SIZE;
                        }
                        //load rows
                        else if (System.Convert.ToInt32(content[0]) >= 0) {
                            //create new row
                            mapFormat.Add(new List<int>());
                            for (int i = 0; i < content.Length; i++) {
                                //add numbers to row
                                if (string.IsNullOrEmpty(content[i])) {
                                    continue;
                                }
                                mapFormat[mapFormat.Count - 1].Add(System.Convert.ToInt32(content[i]));
                            }
                            Console.WriteLine("Row created");
                        }
                        contents = reader.ReadLine();
                    }
                    //create map
                    int rows = mapFormat.Count;
                    int cols = mapFormat[0].Count;
                    tileMap = new Tile[rows][];
                    for (int i = 0; i < rows; ++i) {
                        tileMap[i] = new Tile[cols];
                        //create individual tile
                        for (int j = 0; j < cols; ++j) {
                            Rectangle source = spriteSources[mapFormat[i][j]];
                            //mapFormat[i][j] == individual tile
                            Point worldPosition = new Point();
                            worldPosition.X = (j * source.Width);
                            worldPosition.Y = (i * source.Height);
                            tileMap[i][j] = new Tile(tileSheet, source);
                            
                            tileMap[i][j].Walkable = false;

                            for (int k = 0; k < doorIndex.Count; k++) {
                                tileMap[i][j].IsDoor = mapFormat[i][j] == doorIndex[k] ? true : false;
                            }
                            if (tileMap[i][j].IsDoor) {
                                tileMap[i][j].DoorPath = nextMap[mapFormat[i][j]];
                            }
                            tileMap[i][j].WorldPosition = worldPosition;
                            tileMap[i][j].Scale = 1.0f;
                            foreach (int w in walkableTile) {
                                if (mapFormat[i][j] == w) {
                                    tileMap[i][j].Walkable = true;
                                }
                            }
                             
                        }
                    }
                }
                hero.Position.X = spawnTile.X * Game.TILE_SIZE;
                hero.Position.Y = spawnTile.Y * Game.TILE_SIZE;
                hero.SetTargetTile(spawnTile);
                Console.WriteLine("Map has been loaded");
            }
            else {
                Console.WriteLine("Map not found!");
            }
        }
        public void Update(float dTime,PlayerCharacter hero, List<Bullet> projectiles) {
                for (int i = projectiles.Count - 1; i >= 0; i--) {
                    int xTile = (int)projectiles[i].Position.X / Game.TILE_SIZE;
                    int yTile = (int)projectiles[i].Position.Y / Game.TILE_SIZE;
                    if (xTile >= this[0].Length || xTile < 0) {
                        projectiles.RemoveAt(i);
                    }
                    else if (yTile >= this.Length || yTile < 0) {
                        projectiles.RemoveAt(i);
                    }
                    else if (!this[yTile][xTile].Walkable) {
                        projectiles.RemoveAt(i);
                    }
                }
            for (int i = enemies.Count - 1; i >= 0; i--) {
                enemies[i].Update(dTime);
                Rectangle intersection = Intersections.Rect(hero.Rect, enemies[i].Rect);
                if (intersection.Width * intersection.Height > 0) {
                    //Game.Instance.GameOver = true;
                    Console.WriteLine("Collision between player and enemy detected");
                }
            }
        }

        public Map ResolveDoors(PlayerCharacter hero) {
            Map result = this;
            for (int row = 0; row < tileMap.Length; row++) {
                for (int col = 0; col < tileMap[row].Length; col++) {
                    if (tileMap[row][col].IsDoor) {
                        //get doors bouding rectangle
                        Rectangle doorRect = new Rectangle(col * 30, row * 30, 30, 30);
                        //get a small rectangle in center of the player
                        Rectangle playerCenter = new Rectangle((int)hero.Center.X - 2, (int)hero.Center.Y - 2, 4, 4);
                        //look for an intersection
                        Rectangle intersection = Intersections.Rect(doorRect, playerCenter);
                        if (intersection.Width * intersection.Height > 0) {
                            this.Destroy();
                            result = new Map(tileMap[row][col].DoorPath,hero);
                            hero.Position.X = nextRoom[tileMap[row][col].DoorPath].X * Game.TILE_SIZE;
                            hero.Position.Y = nextRoom[tileMap[row][col].DoorPath].Y * Game.TILE_SIZE;
                            hero.SetTargetTile(new Point((int)hero.Position.X/Game.TILE_SIZE,(int)hero.Position.Y/Game.TILE_SIZE));
                        }
                    }
                }
            }
            return result;
        }
        public void Render() {
            for (int h = 0; h < tileMap.Length; h++) {
                for (int w = 0; w < tileMap[h].Length; w++) {
                    tileMap[h][w].Render();
                }
            }
            for (int i = enemies.Count - 1; i >= 0; i--) {
                enemies[i].Render();
            }
        }
        public void Destroy() {
            for (int h = 0; h < tileMap.Length; h++) {
                for (int w = 0; w < tileMap[h].Length; w++) {
                    tileMap[h][w].Destroy();
                }
            }
            for (int i = enemies.Count-1; i >= 0; i--) {
                enemies[i].Destroy();
            }
        }
    }
}
