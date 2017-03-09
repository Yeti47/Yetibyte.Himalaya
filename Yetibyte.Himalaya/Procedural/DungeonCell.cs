using System;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya.Procedural {

	public class DungeonCell {
		
		// Constants
		
		public const int MIN_WIDTH = 20;
		public const int MAX_WIDTH = 60;
		
		public const int MIN_HEIGHT = 20;
		public const int MAX_HEIGHT = 60;
		
		public const float SPLIT_CHANCE = 0.7f;
		public const float SPLIT_DIRECTION_THRESHOLD = 0.25f;
		
		// Fields
		
		private Random _random;
		
		// Properties
		
		public Point Origin { get; private set; }
		
		public int Width { get; private set; }
		public int Height { get; private set; }
		
		public DungeonCell LeftChild { get; private set; }
		public DungeonCell RightChild { get; private set; }
		
		public DungeonRoom Room { get; private set; }
		
		public bool HasChildCells {
			
			get { return (LeftChild != null || RightChild != null); }
			
		}
		
		// Constructor
		
		public DungeonCell(Random random, Point origin, int width, int height) {
			
			this._random = random;
			this.Origin = origin;
			this.Width = width;
			this.Height = height;
			
		}
		
		// Methods
		
		/// <summary>
		/// Splits this DungeonCell at a random Position and stores the two newly created DungeonCell objects in the LeftChild and RightChild property
		/// respectively. The DungeonCell will not be split in case it is already split or if any of the resulting DungeonCell objects would be smaller than the
		/// minimum size.
		/// </summary>
		/// <returns>True if the DungeonCell was split successfully, false otherwise.</returns>
		public bool Split() {
			
			if(HasChildCells)
				return false;
			
			if(Width <= MAX_WIDTH && Height <= MAX_HEIGHT && _random.NextDouble() > SPLIT_CHANCE)
				return false;
			
			// Determine the split direction
						
			bool willSplitVertically = _random.NextDouble() > 0.5d;
						
			bool canSplitVertically = Width / 2 >= MIN_WIDTH;
			bool canSplitHorizontally = Height / 2 >= MIN_HEIGHT;
			
			if(canSplitVertically && !canSplitHorizontally)
				willSplitVertically = true;
			else if(canSplitHorizontally && !canSplitVertically)
				willSplitVertically = false;
			else if(!canSplitHorizontally && !canSplitVertically)
				return false; // can't split this cell at all, since it would become too small
			
			if(Width / Height >= 1f + SPLIT_DIRECTION_THRESHOLD && canSplitVertically)
				willSplitVertically = true;
			else if(Height / Width >= 1f + SPLIT_DIRECTION_THRESHOLD && canSplitHorizontally)
				willSplitVertically = false;
			
			int splitOffset = willSplitVertically ?
				_random.Next(MIN_WIDTH, (Width - MIN_WIDTH) + 1) :
				_random.Next(MIN_HEIGHT, (Height - MIN_HEIGHT) + 1);
			
			// actually split the child cells
						
			if(willSplitVertically) {
				
				LeftChild = new DungeonCell(_random, Origin, splitOffset, Height);
				RightChild = new DungeonCell(_random, new Point(Origin.X + splitOffset, Origin.Y), Width - splitOffset, Height);
				
			}
			else {
				
				LeftChild = new DungeonCell(_random, Origin, Width, splitOffset);
				RightChild = new DungeonCell(_random, new Point(Origin.X, Origin.Y + splitOffset), Width, Height - splitOffset);
				
			}
			
			return true;
			
		}
		
		/// <summary>
		/// Creates a random room within this DungeonCell, but only if it has no child cells. If there are child Cells, this method will be
		/// called for each of them.
		/// </summary>
		public void GenerateRoom() {
			
			if(HasChildCells) {
				
				if(LeftChild != null)
					LeftChild.GenerateRoom();
				
				if(RightChild != null)
					RightChild.GenerateRoom();
				
				return;
				
			}
			
			int roomWidth = _random.Next(DungeonRoom.MIN_WIDTH, (Width - 2) + 1);
			int roomHeight = _random.Next(DungeonRoom.MIN_HEIGHT, (Height - 2) + 1);
			
			int roomPosX = _random.Next(1, Width - roomWidth);
			int roomPosY = _random.Next(1, Height - roomHeight);
			
			int detailLevel = _random.Next(0, DungeonRoom.MAX_DETAIL_LEVEL + 1);
			
			Room = new DungeonRoom(_random, new Point(roomPosX, roomPosY), roomWidth, roomHeight);
			Room.Generate(detailLevel);
			
		}
		
	}
	
}
