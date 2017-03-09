using System;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya.Procedural {

	public class DungeonRoom {
		
		// Constants
		
		public const int MIN_WIDTH = 5;
		public const int MIN_HEIGHT = 5;
		
		public const int MIN_DETAIL_CHUNK_SIZE = 5;
		public const int MAX_DETAIL_LEVEL = 8;
		
		// Fields
		
		private Random _random;
		
		// Properties
				
		public Point Origin { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		public TileType[][] TileMatrix { get; private set; }

		// Constructor
		
		public DungeonRoom(Random random, Point origin, int width, int height) {
			
			this._random = random;
			this.Origin = origin;
			this.Width = width;
			this.Height = height;
			
		}
		
		// Methods
		
		private void InitTiles() {
			
			TileMatrix = new TileType[Width][];
			
			for (int x = 0; x < Width; x++) {
				
				TileMatrix[x] = new TileType[Height];
				
				for (int y = 0; y < Height; y++)
					TileMatrix[x][y] = TileType.Blocked;
				
			}
			
		}
		
		public bool Generate(int detailLevel) {
			
			InitTiles();
			detailLevel = MathHelper.Clamp(0, MAX_DETAIL_LEVEL, detailLevel);
			
			// Check if room can be detailed horizontally
			
			int detailLevelHorizontal = detailLevel;
			
			while((Width / (detailLevelHorizontal + 1)) < MIN_DETAIL_CHUNK_SIZE && detailLevelHorizontal > 0)
				detailLevelHorizontal--;
			
			bool canDetailHorizontally = detailLevelHorizontal > 0;
			
			// Check if room can be detailed vertically
			
			int detailLevelVertical = detailLevel;
			
			while((Height / (detailLevelVertical + 1)) < MIN_DETAIL_CHUNK_SIZE && detailLevelVertical > 0)
				detailLevelVertical--;
			
			bool canDetailVertically = detailLevelVertical > 0;

			// Determine detail direction

			bool detailHorizontally = _random.NextDouble() > 0.5d;
			
			if(canDetailHorizontally && !canDetailVertically)
				detailHorizontally = true;
			else if(canDetailVertically && !canDetailHorizontally)
				detailHorizontally = false;
									
			// Generate detail chunks
						
			detailLevel = detailHorizontally ? detailLevelHorizontal : detailLevelVertical;
				
			int chunkCount = detailLevel + 1;
							
			Rectangle[] chunks = new Rectangle[chunkCount];
			
			for (int i = 0; i < chunkCount; i++) {
				
				int chunkSize = detailHorizontally ? Width / chunkCount : Height / chunkCount;
				int remainder = 0;
				
				// Add the remaining width to the last chunk
				if(i == chunkCount -1 ) {
					
					remainder = detailHorizontally ? Width % chunkCount : Height % chunkCount;
					chunkSize += remainder;
					
				}
				
				int maxOffset = 0;
				
				if(detailLevel > 0) {
					
					maxOffset = (detailHorizontally ? Height - MIN_HEIGHT : Width - MIN_WIDTH) / 2 + 1;
					
				}
				
				int randomOffsetA = _random.Next(0, maxOffset);
				int randomOffsetB = _random.Next(0, maxOffset);
				
				//randomOffset = 0; // TEST
												
				Point chunkRectOrigin = detailHorizontally 
					? new Point(i * (chunkSize - remainder), randomOffsetA)
					: new Point(randomOffsetA, i * (chunkSize - remainder));

                Point chunkRectSize = detailHorizontally 
					? new Point(chunkSize, Height - randomOffsetA - randomOffsetB)
					: new Point(Width - randomOffsetA - randomOffsetB, chunkSize);
				
				chunks[i] = new Rectangle(chunkRectOrigin, chunkRectSize);
								
				for (int y = 0; y < chunks[i].Height; y++) {
					
					for (int x = 0; x < chunks[i].Width; x++) {
						
						TileMatrix[chunkRectOrigin.X + x][chunkRectOrigin.Y + y] = TileType.Clear;
						
					}
					
				}
				
			}
			
			return true;
						
		}
		
	}
	
}
