using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace Yetibyte.Himalaya.Procedural {
	
	public class DungeonGenerator {
		
		// Fields
		
		private Random _random;
		private List<DungeonCell> _cells;
		private DungeonCell _rootCell;
		
		// Properties
		
		public TileType[][] TileMatrix { get; private set; }
		
		public int DungeonWidth {
			
			get;
			private set;
			
		}
		
		public int DungeonHeight {
			
			get;
			private set;
			
		}
		
		// Constructors
		
		public DungeonGenerator(int seed) {
			
			this._random = new Random(seed);
			
		}
		
		public DungeonGenerator() {
			
			this._random = new Random();
			
		}
		
		// Methods
		
		/// <summary>
		/// Provides a seed for the random number generation.
		/// </summary>
		/// <param name="seed">The seed to use.</param>
		public void SetSeed(int seed) {
			
			_random = new Random(seed);
			
		}
		
		private void InitTiles() {
			
			TileMatrix = new TileType[DungeonWidth][];
			
			for (int x = 0; x < DungeonWidth; x++) {
				
				TileMatrix[x] = new TileType[DungeonHeight];
				
				for (int y = 0; y < DungeonHeight; y++)
					TileMatrix[x][y] = TileType.Blocked;
				
			}
			
		}
		
		/// <summary>
		/// Generates a new random dungeon with the given dimensions. If provided, a seed will be used for random number generation. Dungeons generated
		/// using the same seed will always be identical.
		/// </summary>
		/// <param name="width">The width of the dungeon.</param>
		/// <param name="height">The height of the dungeon.</param>
		public void Generate(int width, int height) {
			
			DungeonWidth = width;
			DungeonHeight = height;
			
			_cells = new List<DungeonCell>();
			_rootCell = new DungeonCell(_random, new Point(0, 0), DungeonWidth, DungeonHeight);
			_cells.Add(_rootCell);
			
			for (int i = 0; i < _cells.Count; i++) {
				
				if(_cells[i].Split()) {
					
					_cells.Add(_cells[i].LeftChild);
					_cells.Add(_cells[i].RightChild);
					
				}
				
			}
			
			_rootCell.GenerateRoom();
			
			InitTiles();
			
			foreach (DungeonCell cell in _cells) {
				
				if(!cell.HasChildCells) {
					
					for (int y = 0; y < cell.Room.Height; y++) {
						
						for (int x = 0; x < cell.Room.Width; x++) {
						
							TileMatrix[cell.Origin.X + cell.Room.Origin.X + x][cell.Origin.Y + cell.Room.Origin.Y + y] = cell.Room.TileMatrix[x][y];
						
						}
						
					}
					
				}
				
			}
			
			
			
		}
		
		/// <summary>
		/// Prints all the generated DungeonCells to the debug console as a string.
		/// </summary>
		public void PrintAreasToConsole() {
			
			Debug.WriteLine("Generating DungeonCell string...");
			
			string[][] stringMap = new string[DungeonWidth][];
			
			for (int x = 0; x < DungeonWidth; x++)
				stringMap[x] = new string[DungeonHeight];

			
			string charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			int charIndex = 0;
			
			foreach(DungeonCell cell in _cells) {
				
				if(!cell.HasChildCells) {
					
					for (int y = 0; y < cell.Height; y++) {
						
						for (int x = 0; x < cell.Width; x++) {
							
							int indexX = cell.Origin.X + x;
							int indexY = cell.Origin.Y + y;
							
							stringMap[indexX][indexY] += charset[charIndex];
														
						}
						
					}
					
					charIndex++;
					
					if(charIndex >= charset.Length)
						charIndex = 0;
					
				}
				
			}
						
			for (int y = 0; y < DungeonHeight; y++) {
				
				string outputLine = string.Empty;
				
				for (int x = 0; x < DungeonWidth; x++) {
					
					outputLine += stringMap[x][y];
					
					if(x == DungeonWidth - 1)
						Debug.WriteLine(outputLine);
				
				}
				
			}
			
			Debug.WriteLine("Done generating!");
			
		}
		
        /// <summary>
        /// Prints all the generated DungeonRooms to the debug console as a string.
        /// The "#" characters represent wall tiles, while the "." characters represent floor tiles.
        /// </summary>
		public void PrintRoomsToConsole() {
			
			for (int y = 0; y < DungeonHeight; y++) {
				
				string line = string.Empty;
				
				for (int x = 0; x < DungeonWidth; x++) {
					
					if(TileMatrix[x][y] == TileType.Blocked)
						line += "# ";
					else if(TileMatrix[x][y] == TileType.Clear)
						line += ". ";
					
				}
				
				Debug.WriteLine(line);
				
			}
						
        }

        public void PrintCellInfoToConsole() {

            int horizontalCount = 0;
            int verticalCount = 0;
            int squareCount = 0;

            foreach (DungeonCell cell in _cells) {

                if (!cell.HasChildCells) {

                    if (cell.Width > cell.Height)
                        horizontalCount++;
                    else if (cell.Height > cell.Width)
                        verticalCount++;
                    else
                        squareCount++;

                }

            }

            Debug.WriteLine("");
            Debug.WriteLine("Number of horizontal DungeonCells: " + horizontalCount);
            Debug.WriteLine("Number of vertical DungeonCells: " + verticalCount);
            Debug.WriteLine("Number of square DungeonCells: " + squareCount);
            Debug.WriteLine("");

        }

        /// <summary>
        /// Converts the tile matrix of this DungeonGenerator to a Texture where a black pixel represents a wall tile and
        /// a white pixel represents a floor tile.
        /// </summary>
        /// <param name="graphicsDevice">The Graphics Device the returned Texture2D should live on.</param>
        /// <returns>A texture representing the Tile Matrix of the generated Dungeon.</returns>
        public Texture2D ConvertToTexture(GraphicsDevice graphicsDevice) {

            Texture2D texture = new Texture2D(graphicsDevice, DungeonWidth, DungeonHeight);

            Color[] data = new Color[DungeonWidth * DungeonHeight];

            for (int y = 0; y < DungeonHeight; y++) {

                for (int x = 0; x < DungeonWidth; x++) {

                    if (TileMatrix[x][y] == TileType.Blocked)
                        data[y * DungeonWidth + x] = Color.Black;
                    else
                        data[y * DungeonWidth + x] = Color.White;

                }

            }

            texture.SetData(data);

            return texture;

        }
				
	}
	
}
