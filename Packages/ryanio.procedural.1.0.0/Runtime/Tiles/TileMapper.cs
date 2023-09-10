using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProceduralGeneration {
	internal class TileMapper {
		Grid              GridObj                { get; }
		GeneratorTools    GeneratorTools         { get; }
		TileMapDictionary TileMapDictionary      { get; }
		TileDictionary    TileDictionary         { get; }
		int               MapWidth               { get; }
		int               MapHeight              { get; }
		bool              ShouldCreateTileLabels { get; }

		internal void FillGround(int x, int y) {
			var position = new Vector3Int(x, y, 0);

			if (!TileMapDictionary.ContainsKey(TileMapType.Ground))
				return;
			
			GeneratorTools.SetTile(
				TileMapDictionary[TileMapType.Ground],
				TileDictionary[TileId.GROUND],
				position);
		}

		internal void FillBoundary(int x, int y) {
			var position = new Vector3Int(x, y, 0);

			if (!TileMapDictionary.ContainsKey(TileMapType.Boundary))
				return;

			GeneratorTools.SetTile(
				TileMapDictionary[TileMapType.Boundary],
				TileDictionary[TileId.BOUNDARY],
				position);
		}

		internal void FillAngles(ref int[,] map, int x, int y) {
			if (Utility.IsBoundary(MapWidth, MapHeight, x, y))
				return;

			if (IsNorthWestTile(ref map, x, y))
				CreateNorthWestAngle(x, y);

			else if (IsNorthEastTile(ref map, x, y))
				CreateNorthEastAngle(x, y);

			else if (IsSouthWestAngle(ref map, x, y))
				CreateSouthWestAngle(x, y);

			else if (IsSouthEastAngle(ref map, x, y))
				CreateSouthEastAngle(x, y);
		}

		internal bool FillPockets(ref int[,] map, int x, int y) {
			if (Utility.IsBoundary(MapWidth, MapHeight, x, y))
				return false;

			if (IsNorthPocket(ref map, x, y)) {
				CreateNorthPocket(x, y);

				return true;
			}

			if (IsSouthPocket(ref map, x, y)) {
				CreateSouthPocket(x, y);

				return true;
			}

			if (IsEastPocket(ref map, x, y)) {
				CreateEastPocket(x, y);

				return true;
			}

			if (IsWestPocket(ref map, x, y)) {
				CreateWestPocket(x, y);

				return true;
			}

			return false;
		}

		internal void FillOutlines(Tilemap tilemap, TileBase tile, TileMask bit, int x, int y) {
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

		bool IsSouthEastAngle(ref int[,] map, int x, int y) =>
			!GeneratorTools.IsFilled(ref map, x - 1, y) &&
			(CaseFour(ref map, x, y)                || CaseEight(ref map, x, y) || CaseTwelve(ref map, x, y)
			 || CaseSouthEastEdgeOne(ref map, x, y) || CaseSouthEastEdgeTwo(ref map, x, y));

		bool IsSouthWestAngle(ref int[,] map, int x, int y) =>
			(!GeneratorTools.IsFilled(ref map, x + 1, y) &&
			 (CaseThree(ref map, x, y)               || CaseSeven(ref map, x, y) || CaseEleven(ref map, x, y)
			  || CaseSouthWestEdgeOne(ref map, x, y) || CaseSouthWestEdgeTwo(ref map, x, y)))
			|| CaseSouthWestEdgeThree(ref map, x, y);

		bool IsNorthEastTile(ref int[,] map, int x, int y) =>
			(!GeneratorTools.IsFilled(ref map, x - 1, y) &&
			 (CaseTwo(ref map, x, y)              || CaseSix(ref map, x, y) || CaseTen(ref map, x, y) ||
			  CaseNorthEastEdgeOne(ref map, x, y) ||
			  CaseNorthEastEdgeTwo(ref map, x, y))) ||
			CaseNorthEastEdgeThree(ref map, x, y);


		bool IsNorthWestTile(ref int[,] map, int x, int y) =>
			!GeneratorTools.IsFilled(ref map, x + 1, y) &&
			(CaseOne(ref map, x, y)              || CaseFive(ref map, x, y) || CaseNine(ref map, x, y) ||
			 CaseNorthWestEdgeOne(ref map, x, y) || CaseNorthWestEdgeTwo(ref map, x, y));

		bool IsNorthPocket(ref int[,] map, int x, int y) =>
			(CaseThree(ref map, x, y) && CaseFour(ref map, x, y)) ||
			CaseNorthPocketEdgeOne(ref map, x, y)                  ||
			CaseNorthPocketEdgeTwo(ref map, x, y);

		bool IsSouthPocket(ref int[,] map, int x, int y) =>
			(CaseOne(ref map, x, y) && CaseTwo(ref map, x, y)) ||
			(CaseOne(ref map, x, y) && GeneratorTools.IsFilled(ref map, x + 1, y));

		bool IsEastPocket(ref int[,] map, int x, int y) =>
			(CaseOne(ref map, x, y)   && CaseThree(ref map, x, y))                   ||
			(CaseOne(ref map, x, y)   && GeneratorTools.IsFilled(ref map, x, y - 1)) ||
			(CaseThree(ref map, x, y) && GeneratorTools.IsFilled(ref map, x, y + 1));

		bool IsWestPocket(ref int[,] map, int x, int y) =>
			(CaseTwo(ref map, x, y) && CaseFour(ref map, x, y)) ||
			CaseWestPocketEdgeOne(ref map, x, y)                 ||
			CaseWestPocketEdgeTwo(ref map, x, y);

		bool CaseWestPocketEdgeTwo(ref int[,] map, int x, int y) =>
			CaseFour(ref map, x, y) && GeneratorTools.IsFilled(ref map, x, y + 1);

		bool CaseWestPocketEdgeOne(ref int[,] map, int x, int y) =>
			CaseFour(ref map, x, y) && GeneratorTools.IsFilled(ref map, x, y + 1) &&
			GeneratorTools.IsFilled(ref map,                            x    - 1, y + 1);

		bool CaseOne(ref int[,] map, int x, int y) =>
			GeneratorTools.IsFilled(ref map, x - 1, y) && GeneratorTools.IsFilled(ref map, x - 1, y + 1) &&
			GeneratorTools.IsFilled(ref map, x,     y                                        + 1);

		bool CaseTwo(ref int[,] map, int x, int y) =>
			GeneratorTools.IsFilled(ref map, x, y + 1) && GeneratorTools.IsFilled(ref map, x + 1, y + 1) &&
			GeneratorTools.IsFilled(ref map, x    + 1,                                     y);

		bool CaseThree(ref int[,] map, int x, int y) =>
			GeneratorTools.IsFilled(ref map, x - 1, y) && GeneratorTools.IsFilled(ref map, x - 1, y - 1) &&
			GeneratorTools.IsFilled(ref map, x,     y                                        - 1);

		bool CaseFour(ref int[,] map, int x, int y) =>
			GeneratorTools.IsFilled(ref map, x + 1, y) && GeneratorTools.IsFilled(ref map, x + 1, y - 1) &&
			GeneratorTools.IsFilled(ref map, x,     y                                        - 1);

		bool CaseFive(ref int[,] map, int x, int y) =>
			CaseOne(ref map, x, y) && GeneratorTools.IsFilled(ref map, x + 1, y + 1) &&
			GeneratorTools.IsFilled(ref map,                           x - 1, y + 1);

		bool CaseSix(ref int[,] map, int x, int y)
			=> CaseTwo(ref map, x, y) && GeneratorTools.IsFilled(ref map, x - 1, y + 1) &&
			   GeneratorTools.IsFilled(ref map,                           x + 1, y - 1);

		bool CaseSeven(ref int[,] map, int x, int y) =>
			CaseThree(ref map, x, y) && GeneratorTools.IsFilled(ref map, x - 1, y + 1) &&
			GeneratorTools.IsFilled(ref map,                             x + 1, y - 1);

		bool CaseNorthPocketEdgeOne(ref int[,] map, int x, int y) =>
			CaseThree(ref map, x, y) && GeneratorTools.IsFilled(ref map, x + 1, y + 1) &&
			GeneratorTools.IsFilled(ref map,                             x + 1, y);

		bool CaseNorthPocketEdgeTwo(ref int[,] map, int x, int y) =>
			CaseFour(ref map, x, y) && GeneratorTools.IsFilled(ref map, x - 1, y);

		bool CaseEight(ref int[,] map, int x, int y) =>
			CaseFour(ref map, x, y) && GeneratorTools.IsFilled(ref map, x + 1, y + 1) &&
			GeneratorTools.IsFilled(ref map,                            x - 1, y + 1);

		bool CaseNine(ref int[,] map, int x, int y) =>
			GeneratorTools.IsFilled(ref map, x - 1, y - 1) &&
			GeneratorTools.IsFilled(ref map, x - 1, y)     &&
			GeneratorTools.IsFilled(ref map, x,     y + 1) &&
			GeneratorTools.IsFilled(ref map, x        + 1, y + 1);

		bool CaseTen(ref int[,] map, int x, int y) =>
			GeneratorTools.IsFilled(ref map, x - 1, y + 1)    &&
			GeneratorTools.IsFilled(ref map, x,     y + 1)    &&
			GeneratorTools.IsFilled(ref map, x        + 1, y) &&
			GeneratorTools.IsFilled(ref map, x        + 1, y - 1);

		bool CaseEleven(ref int[,] map, int x, int y) =>
			GeneratorTools.IsFilled(ref map, x - 1, y + 1) &&
			GeneratorTools.IsFilled(ref map, x - 1, y)     &&
			GeneratorTools.IsFilled(ref map, x,     y - 1) &&
			GeneratorTools.IsFilled(ref map, x        + 1, y - 1);

		bool CaseTwelve(ref int[,] map, int x, int y) =>
			GeneratorTools.IsFilled(ref map, x + 1, y + 1) &&
			GeneratorTools.IsFilled(ref map, x + 1, y)     &&
			GeneratorTools.IsFilled(ref map, x,     y - 1) &&
			GeneratorTools.IsFilled(ref map, x        - 1, y - 1);

		bool CaseNorthWestEdgeOne(ref int[,] map, int x, int y) =>
			GeneratorTools.IsFilled(ref map, x - 1, y)     &&
			GeneratorTools.IsFilled(ref map, x,     y + 1) &&
			GeneratorTools.IsFilled(ref map, x        + 1, y + 1);

		bool CaseNorthWestEdgeTwo(ref int[,] map, int x, int y) =>
			CaseOne(ref map, x, y) && GeneratorTools.IsFilled(ref map, x + 1, y - 1);

		bool CaseSouthEastEdgeOne(ref int[,] map, int x, int y) =>
			GeneratorTools.IsFilled(ref map, x + 1, y)     &&
			GeneratorTools.IsFilled(ref map, x,     y - 1) &&
			GeneratorTools.IsFilled(ref map, x        - 1, y - 1);

		bool CaseSouthEastEdgeTwo(ref int[,] map, int x, int y) =>
			CaseFour(ref map, x, y) && GeneratorTools.IsFilled(ref map, x - 1, y + 1);

		bool CaseSouthWestEdgeOne(ref int[,] map, int x, int y) =>
			GeneratorTools.IsFilled(ref map, x - 1, y + 1) && GeneratorTools.IsFilled(ref map, x - 1, y) &&
			GeneratorTools.IsFilled(ref map, x,     y - 1);

		bool CaseSouthWestEdgeTwo(ref int[,] map, int x, int y) =>
			CaseThree(ref map, x, y) && GeneratorTools.IsFilled(ref map, x + 1, y + 1);

		bool CaseSouthWestEdgeThree(ref int[,] map, int x, int y) =>
			GeneratorTools.IsFilled(ref map, x - 1, y) && GeneratorTools.IsFilled(ref map, x, y - 1);

		bool CaseNorthEastEdgeOne(ref int[,] map, int x, int y) =>
			GeneratorTools.IsFilled(ref map, x, y + 1)    &&
			GeneratorTools.IsFilled(ref map, x    + 1, y) &&
			GeneratorTools.IsFilled(ref map, x    + 1, y - 1);

		bool CaseNorthEastEdgeTwo(ref int[,] map, int x, int y) =>
			CaseTwo(ref map, x, y) && GeneratorTools.IsFilled(ref map, x - 1, y - 1);

		bool CaseNorthEastEdgeThree(ref int[,] map, int x, int y) =>
			GeneratorTools.IsFilled(ref map, x + 1, y) && GeneratorTools.IsFilled(ref map, x, y + 1);

		internal TileMapper(TileMapperCtx ctx, GeneratorToolsCtx toolsCtx, TileSolversCtx tileSolversCtx) {
			GeneratorTools         = new GeneratorTools(toolsCtx);
			TileDictionary         = tileSolversCtx.TileDictionary;
			ShouldCreateTileLabels = ctx.ShouldCreateTileLabels;
			MapWidth               = tileSolversCtx.Dimensions.Rows;
			MapHeight              = tileSolversCtx.Dimensions.Columns;
			GridObj                = tileSolversCtx.Grid;
			TileMapDictionary      = tileSolversCtx.TileMapDictionary;
		}
	}
}