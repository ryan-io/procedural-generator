using BCL;
using CommunityToolkit.HighPerformance;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProceduralGeneration {
	public class TileMapper {
		Grid              GridObj                { get; }
		GeneratorTools    GeneratorTools         { get; }
		TileMapDictionary TileMapDictionary      { get; }
		TileDictionary    TileDictionary         { get; }
		int               MapWidth               { get; }
		int               MapHeight              { get; }
		bool              ShouldCreateTileLabels { get; }

		public void FillGround(int x, int y) {
			var position = new Vector3Int(x, y, 0);

			if (!TileMapDictionary.ContainsKey(TileMapType.Ground))
				return;
			GeneratorTools.SetTile(
				TileMapDictionary[TileMapType.Ground],
				TileDictionary[TileId.GROUND],
				position);
		}

		public void FillBoundary(int x, int y) {
			var position = new Vector3Int(x, y, 0);

			if (!TileMapDictionary.ContainsKey(TileMapType.Boundary))
				return;

			GeneratorTools.SetTile(
				TileMapDictionary[TileMapType.Boundary],
				TileDictionary[TileId.BOUNDARY],
				position);
		}

		public void FillAngles(Span2D<int> span, int x, int y) {
			if (Utility.IsBoundary(MapWidth, MapHeight, x, y))
				return;

			if (IsNorthWestTile(span, x, y))
				CreateNorthWestAngle(x, y);

			else if (IsNorthEastTile(span, x, y))
				CreateNorthEastAngle(x, y);

			else if (IsSouthWestAngle(span, x, y))
				CreateSouthWestAngle(x, y);

			else if (IsSouthEastAngle(span, x, y))
				CreateSouthEastAngle(x, y);
		}

		public bool FillPockets(Span2D<int> span, int x, int y) {
			if (Utility.IsBoundary(MapWidth, MapHeight, x, y))
				return false;

			if (IsNorthPocket(span, x, y)) {
				CreateNorthPocket(x, y);

				return true;
			}

			if (IsSouthPocket(span, x, y)) {
				CreateSouthPocket(x, y);

				return true;
			}

			if (IsEastPocket(span, x, y)) {
				CreateEastPocket(x, y);

				return true;
			}

			if (IsWestPocket(span, x, y)) {
				CreateWestPocket(x, y);

				return true;
			}

			return false;
		}

		public void FillOutlines(Tilemap tilemap, TileBase tile, TileMask bit, int x, int y) {
			if (!GeneratorTools.IsSouthOutline(bit))
				return;

			var position = new Vector3Int(x, y, 0);
			GeneratorTools.SetTile(tilemap, tile, position);
		}

		void CreateWestPocket(int x, int y) {
			if (ShouldCreateTileLabels)
				GeneratorTools.CreateTileLabel(x, y, "W-P");

			GeneratorTools.SetTile(
				TileMapDictionary[TileMapType.Boundary],
				TileDictionary[TileId.WEST_POCKET],
				new Vector3Int(x, y, 0));
		}

		void CreateEastPocket(int x, int y) {
			if (ShouldCreateTileLabels)
				GeneratorTools.CreateTileLabel(x, y, "E-P");

			GeneratorTools.SetTile(
				TileMapDictionary[TileMapType.Boundary],
				TileDictionary[TileId.EAST_POCKET],
				new Vector3Int(x, y, 0));
		}

		void CreateSouthPocket(int x, int y) {
			if (ShouldCreateTileLabels)
				GeneratorTools.CreateTileLabel(x, y, "S-P");

			GeneratorTools.SetTile(
				TileMapDictionary[TileMapType.Boundary],
				TileDictionary[TileId.SOUTH_POCKET],
				new Vector3Int(x, y, 0));
		}

		void CreateNorthPocket(int x, int y) {
			if (ShouldCreateTileLabels)
				GeneratorTools.CreateTileLabel(x, y, "N-P");

			GeneratorTools.SetTile(
				TileMapDictionary[TileMapType.Boundary],
				TileDictionary[TileId.NORTH_POCKET],
				new Vector3Int(x, y, 0));
		}

		void CreateSouthEastAngle(int x, int y) {
			if (ShouldCreateTileLabels)
				GeneratorTools.CreateTileLabel(x, y, "SE");

			GeneratorTools.SetTile(
				TileMapDictionary[TileMapType.Boundary],
				TileDictionary[TileId.SOUTH_EAST_ANGLE],
				new Vector3Int(x, y, 0));
		}

		void CreateSouthWestAngle(int x, int y) {
			if (ShouldCreateTileLabels)
				GeneratorTools.CreateTileLabel(x, y, "SW");

			GeneratorTools.SetTile(
				TileMapDictionary[TileMapType.Boundary],
				TileDictionary[TileId.SOUTH_WEST_ANGLE],
				new Vector3Int(x, y, 0));
		}

		void CreateNorthEastAngle(int x, int y) {
			if (ShouldCreateTileLabels)
				GeneratorTools.CreateTileLabel(x, y, "NE");

			GeneratorTools.SetTile(
				TileMapDictionary[TileMapType.Boundary],
				TileDictionary[TileId.NORTH_EAST_ANGLE],
				new Vector3Int(x, y, 0));
		}

		void CreateNorthWestAngle(int x, int y) {
			if (ShouldCreateTileLabels)
				GeneratorTools.CreateTileLabel(x, y, "NW");

			GeneratorTools.SetTile(
				TileMapDictionary[TileMapType.Boundary],
				TileDictionary[TileId.NORTH_WEST_ANGLE],
				new Vector3Int(x, y, 0));
		}

		bool IsSouthEastAngle(Span2D<int> span, int x, int y) =>
			!GeneratorTools.IsFilled(span, x - 1, y) &&
			(CaseFour(span, x, y)                || CaseEight(span, x, y) || CaseTwelve(span, x, y)
			 || CaseSouthEastEdgeOne(span, x, y) || CaseSouthEastEdgeTwo(span, x, y));

		bool IsSouthWestAngle(Span2D<int> span, int x, int y) =>
			(!GeneratorTools.IsFilled(span, x + 1, y) &&
			 (CaseThree(span, x, y)               || CaseSeven(span, x, y) || CaseEleven(span, x, y)
			  || CaseSouthWestEdgeOne(span, x, y) || CaseSouthWestEdgeTwo(span, x, y)))
			|| CaseSouthWestEdgeThree(span, x, y);

		bool IsNorthEastTile(Span2D<int> span, int x, int y) =>
			(!GeneratorTools.IsFilled(span, x - 1, y) &&
			 (CaseTwo(span, x, y)              || CaseSix(span, x, y) || CaseTen(span, x, y) ||
			  CaseNorthEastEdgeOne(span, x, y) ||
			  CaseNorthEastEdgeTwo(span, x, y))) ||
			CaseNorthEastEdgeThree(span, x, y);


		bool IsNorthWestTile(Span2D<int> span, int x, int y) =>
			!GeneratorTools.IsFilled(span, x + 1, y) &&
			(CaseOne(span, x, y)              || CaseFive(span, x, y) || CaseNine(span, x, y) ||
			 CaseNorthWestEdgeOne(span, x, y) || CaseNorthWestEdgeTwo(span, x, y));

		bool IsNorthPocket(Span2D<int> span, int x, int y) =>
			(CaseThree(span, x, y) && CaseFour(span, x, y)) ||
			CaseNorthPocketEdgeOne(span, x, y)              ||
			CaseNorthPocketEdgeTwo(span, x, y);

		bool IsSouthPocket(Span2D<int> span, int x, int y) =>
			(CaseOne(span, x, y) && CaseTwo(span, x, y)) ||
			(CaseOne(span, x, y) && GeneratorTools.IsFilled(span, x + 1, y));

		bool IsEastPocket(Span2D<int> span, int x, int y) =>
			(CaseOne(span, x, y)   && CaseThree(span, x, y))                   ||
			(CaseOne(span, x, y)   && GeneratorTools.IsFilled(span, x, y - 1)) ||
			(CaseThree(span, x, y) && GeneratorTools.IsFilled(span, x, y + 1));

		bool IsWestPocket(Span2D<int> span, int x, int y) =>
			(CaseTwo(span, x, y) && CaseFour(span, x, y)) ||
			CaseWestPocketEdgeOne(span, x, y)             ||
			CaseWestPocketEdgeTwo(span, x, y);

		bool CaseWestPocketEdgeTwo(Span2D<int> span, int x, int y) =>
			CaseFour(span, x, y) && GeneratorTools.IsFilled(span, x, y + 1);

		bool CaseWestPocketEdgeOne(Span2D<int> span, int x, int y) =>
			CaseFour(span, x, y) && GeneratorTools.IsFilled(span, x, y + 1) &&
			GeneratorTools.IsFilled(span,                         x    - 1, y + 1);

		bool CaseOne(Span2D<int> span, int x, int y) =>
			GeneratorTools.IsFilled(span, x - 1, y) && GeneratorTools.IsFilled(span, x - 1, y + 1) &&
			GeneratorTools.IsFilled(span, x,     y                                     + 1);

		bool CaseTwo(Span2D<int> span, int x, int y) =>
			GeneratorTools.IsFilled(span, x, y + 1) && GeneratorTools.IsFilled(span, x + 1, y + 1) &&
			GeneratorTools.IsFilled(span, x    + 1,                                  y);

		bool CaseThree(Span2D<int> span, int x, int y) =>
			GeneratorTools.IsFilled(span, x - 1, y) && GeneratorTools.IsFilled(span, x - 1, y - 1) &&
			GeneratorTools.IsFilled(span, x,     y                                     - 1);

		bool CaseFour(Span2D<int> span, int x, int y) =>
			GeneratorTools.IsFilled(span, x + 1, y) && GeneratorTools.IsFilled(span, x + 1, y - 1) &&
			GeneratorTools.IsFilled(span, x,     y                                     - 1);

		bool CaseFive(Span2D<int> span, int x, int y) =>
			CaseOne(span, x, y) && GeneratorTools.IsFilled(span, x + 1, y + 1) &&
			GeneratorTools.IsFilled(span,                        x - 1, y + 1);

		bool CaseSix(Span2D<int> span, int x, int y)
			=> CaseTwo(span, x, y) && GeneratorTools.IsFilled(span, x - 1, y + 1) &&
			   GeneratorTools.IsFilled(span,                        x + 1, y - 1);

		bool CaseSeven(Span2D<int> span, int x, int y) =>
			CaseThree(span, x, y) && GeneratorTools.IsFilled(span, x - 1, y + 1) &&
			GeneratorTools.IsFilled(span,                          x + 1, y - 1);

		bool CaseNorthPocketEdgeOne(Span2D<int> span, int x, int y) =>
			CaseThree(span, x, y) && GeneratorTools.IsFilled(span, x + 1, y + 1) &&
			GeneratorTools.IsFilled(span,                          x + 1, y);

		bool CaseNorthPocketEdgeTwo(Span2D<int> span, int x, int y) =>
			CaseFour(span, x, y) && GeneratorTools.IsFilled(span, x - 1, y);

		bool CaseEight(Span2D<int> span, int x, int y) =>
			CaseFour(span, x, y) && GeneratorTools.IsFilled(span, x + 1, y + 1) &&
			GeneratorTools.IsFilled(span,                         x - 1, y + 1);

		bool CaseNine(Span2D<int> span, int x, int y) =>
			GeneratorTools.IsFilled(span, x - 1, y - 1) &&
			GeneratorTools.IsFilled(span, x - 1, y)     &&
			GeneratorTools.IsFilled(span, x,     y + 1) &&
			GeneratorTools.IsFilled(span, x        + 1, y + 1);

		bool CaseTen(Span2D<int> span, int x, int y) =>
			GeneratorTools.IsFilled(span, x - 1, y + 1)    &&
			GeneratorTools.IsFilled(span, x,     y + 1)    &&
			GeneratorTools.IsFilled(span, x        + 1, y) &&
			GeneratorTools.IsFilled(span, x        + 1, y - 1);

		bool CaseEleven(Span2D<int> span, int x, int y) =>
			GeneratorTools.IsFilled(span, x - 1, y + 1) &&
			GeneratorTools.IsFilled(span, x - 1, y)     &&
			GeneratorTools.IsFilled(span, x,     y - 1) &&
			GeneratorTools.IsFilled(span, x        + 1, y - 1);

		bool CaseTwelve(Span2D<int> span, int x, int y) =>
			GeneratorTools.IsFilled(span, x + 1, y + 1) &&
			GeneratorTools.IsFilled(span, x + 1, y)     &&
			GeneratorTools.IsFilled(span, x,     y - 1) &&
			GeneratorTools.IsFilled(span, x        - 1, y - 1);

		bool CaseNorthWestEdgeOne(Span2D<int> span, int x, int y) =>
			GeneratorTools.IsFilled(span, x - 1, y)     &&
			GeneratorTools.IsFilled(span, x,     y + 1) &&
			GeneratorTools.IsFilled(span, x        + 1, y + 1);

		bool CaseNorthWestEdgeTwo(Span2D<int> span, int x, int y) =>
			CaseOne(span, x, y) && GeneratorTools.IsFilled(span, x + 1, y - 1);

		bool CaseSouthEastEdgeOne(Span2D<int> span, int x, int y) =>
			GeneratorTools.IsFilled(span, x + 1, y)     &&
			GeneratorTools.IsFilled(span, x,     y - 1) &&
			GeneratorTools.IsFilled(span, x        - 1, y - 1);

		bool CaseSouthEastEdgeTwo(Span2D<int> span, int x, int y) =>
			CaseFour(span, x, y) && GeneratorTools.IsFilled(span, x - 1, y + 1);

		bool CaseSouthWestEdgeOne(Span2D<int> span, int x, int y) =>
			GeneratorTools.IsFilled(span, x - 1, y + 1) && GeneratorTools.IsFilled(span, x - 1, y) &&
			GeneratorTools.IsFilled(span, x,     y - 1);

		bool CaseSouthWestEdgeTwo(Span2D<int> span, int x, int y) =>
			CaseThree(span, x, y) && GeneratorTools.IsFilled(span, x + 1, y + 1);

		bool CaseSouthWestEdgeThree(Span2D<int> span, int x, int y) =>
			GeneratorTools.IsFilled(span, x - 1, y) && GeneratorTools.IsFilled(span, x, y - 1);

		bool CaseNorthEastEdgeOne(Span2D<int> span, int x, int y) =>
			GeneratorTools.IsFilled(span, x, y + 1)    &&
			GeneratorTools.IsFilled(span, x    + 1, y) &&
			GeneratorTools.IsFilled(span, x    + 1, y - 1);

		bool CaseNorthEastEdgeTwo(Span2D<int> span, int x, int y) =>
			CaseTwo(span, x, y) && GeneratorTools.IsFilled(span, x - 1, y - 1);

		bool CaseNorthEastEdgeThree(Span2D<int> span, int x, int y) =>
			GeneratorTools.IsFilled(span, x + 1, y) && GeneratorTools.IsFilled(span, x, y + 1);

		public TileMapper(ProceduralConfig config, TileMapDictionary dictionary, Grid grid, StopWatchWrapper stopWatch) {
			GeneratorTools         = new GeneratorTools(config, grid, stopWatch);
			TileDictionary         = config.TileDictionary;
			ShouldCreateTileLabels = config.ShouldCreateTileLabels;
			MapWidth               = config.Rows;
			MapHeight              = config.Columns;
			GridObj                = grid;
			TileMapDictionary      = dictionary;
		}
	}
}