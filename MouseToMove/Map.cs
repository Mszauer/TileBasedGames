﻿using System;
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
        protected string nextRoom = null;

        public Map(string mapPath, PlayerCharacter hero){
            if (System.IO.File.Exists(mapPath)){
                Console.WriteLine("Loading map...");
                List<int> walkableTile = new List<int>();
                int doorIndex = -1;
                Dictionary<int, Rectangle> spriteSources = new Dictionary<int, Rectangle>();
                List<List<int>> mapFormat = new List<List<int>>();

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
                            tileMap[i][j].IsDoor = mapFormat[i][j] == doorIndex ? true : false;
                            tileMap[i][j].DoorPath = tileMap[i][j].IsDoor ? nextRoom : null;
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
                            result = new Map(tileMap[row][col].DoorPath,hero);
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
