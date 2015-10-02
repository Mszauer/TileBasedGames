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
        List<List<int>> mapFormat = new List<List<int>>();
        protected Dictionary<int,Rectangle> spriteSources = new Dictionary<int, Rectangle>();
        protected Point spawnTile = new Point(0, 0);
        protected string tileSheet = null;

        public Map(string mapPath){
            if (System.IO.File.Exists(mapPath)){
                Console.WriteLine("Loading map...");
                List<int> walkableTile = new List<int>();
                string nextRoom = null;
                int doorIndex = -1;

                using (TextReader reader = File.OpenText(mapPath)) {
                    string contents = reader.ReadLine();

                    while (contents != null) {
                        string[] content = contents.Split(' ');
                        //load rows
                        if (System.Convert.ToInt32(content[0]) >= 0) {
                            //create new row
                            mapFormat.Add(new List<int>());
                            for (int i = 0; i < content.Length; i++) {
                                //add numbers to row
                                 mapFormat[mapFormat.Count-1].Add(System.Convert.ToInt32(content[i]));
                            }
                        }
                        //load texture
                        if (content[0] == "T") {
                            //texture path
                            string path = content[1];
                            for (int i = 1; i < content.Length; i++) {
                                //add the chars into a string
                                path += content[i];
                            }
                            tileSheet = path;
                        }
                        //load source rects
                        if (content[0] == "R") {
                            //source rect
                            Rectangle r = new Rectangle(System.Convert.ToInt32(content[1]), System.Convert.ToInt32(content[2]), System.Convert.ToInt32(content[3]), System.Convert.ToInt32(content[4]));
                            //adds rect index and source rect to dictionary
                            spriteSources.Add(System.Convert.ToInt32(content[1]), r);
                        }
                        //walkable tile indices
                        if (content[0] == "W") {
                            for(int i = 1; i < content.Length; i++) {
                                walkableTile.Add(System.Convert.ToInt32(content[i]));
                            }
                        }
                        //door tiles
                        if (content[0] == "D") {
                            //identifies which tile is a door
                            doorIndex = System.Convert.ToInt32(content[1]);
                            //door destination
                            nextRoom = content[2];
                        }
                        //starting tile
                        if (content[0] == "S") {
                            spawnTile = new Point(System.Convert.ToInt32(content[1]), System.Convert.ToInt32(content[2]));
                        }
                        contents = reader.ReadLine();
                    }
                }
                Console.WriteLine("Map has been loaded");
            }
            else {
                Console.WriteLine("Map not found!");
            }
        }
        
        /*public Map(int[][] layout, string sheets, Rectangle[] sources, params int[] walkable) {
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
                    result[i][j].Walkable = false;
                    result[i][j].IsDoor = false;
                    result[i][j].WorldPosition = worldPosition;
                    result[i][j].Scale = scale;
                    foreach (int w in walkable) {
                        if (layout[i][j] == w) {
                            tileMap[i][j].Walkable = true;
                        }
                    }
                }
                tileMap = result;
            }
        }
        */
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
                            result = tileMap[row][col].DoorTarget;
                            hero.Position.X = tileMap[row][col].DoorLocation.X * 30;
                            hero.Position.Y = tileMap[row][col].DoorLocation.Y * 30;
                            hero.SetTargetTile(tileMap[row][col].DoorLocation);
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
        }
        public void Destroy() {
            for (int h = 0; h < tileMap.Length; h++) {
                for (int w = 0; w < tileMap[h].Length; w++) {
                    tileMap[h][w].Destroy();
                }
            }
        }
    }
}
