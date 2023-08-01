using BCL;
using CommunityToolkit.HighPerformance;
using UnityEngine;

namespace Engine.Procedural.Runtime {
	public class SetAllTilesSyncTileTypeSolver : TileTypeSolver {
		TileHashset       TileHashset       { get; }
		TileMapDictionary TileMapDictionary { get; }
		TileDictionary    TileDictionary    { get; }
		int               MapWidth          { get; }
		int               MapHeight         { get; }


		/// <summary>
		/// span is still allocated on the stack during this invocation
		/// </summary>
		/// <param name="span">Pre-stack allocated span for generating primary amap</param>
		public override void SetTiles(Span2D<int> span) {
			for (var i = 0; i < MapWidth * MapHeight; i++) {
				var row   = i / MapHeight;
				var column = i % MapHeight;
				
				TileTaskCreator(span, row, column);
			}

			//
			// for (var x = 0; x < MapWidth; x++) {
			// 	for (var y = 0; y < MapHeight; y++)
			// 		TileTaskCreator(span, x, y);
			// }
		}

		void TileTaskCreator(Span2D<int> span, int currentX, int currentY) {
			var isBoundary = Utility.IsBoundary(MapWidth, MapHeight, currentX, currentY);
			var bit        = _generatorTools.SolveMask(span, currentX, currentY, isBoundary);

			if (_generatorTools.IsWall(bit))
				FillTiles(span, currentX, currentY, ref bit);
		}

		void FillTiles(Span2D<int> span, int x, int y, ref TileMask bit) {
			var isFilled   = GeneratorTools.IsFilled(span, x, y);
			var location   = new Vector2Int(x, y);
			var isBoundary = Utility.IsBoundary(MapWidth, MapHeight, x, y);
			var isPocket   = false;

			_tileMapper.FillGround(x, y);

			var outLineRandom = _tileWeightDictionary["Outlines"];
			var value         = outLineRandom.Pop();
			var isLocalBorder = false;

			if (isFilled) {
				_tileMapper.FillBoundary(x, y);
				isLocalBorder = true;
			}

			else {
				_tileMapper.FillAngles(span, x, y);
				isPocket = _tileMapper.FillPockets(span, x, y);
			}

			var hasAll  = _generatorTools.HasAllNeighbors(bit);
			var hasNone = _generatorTools.HasNoNeighbors(bit);
			var data    = new TileRecord(location, bit, isBoundary, isLocalBorder);

			if (!hasAll && !hasNone && !isPocket && value == 1)
				RunFillOutlineLogic(x, y, data);

			AddToHashSet(data);
		}

		void RunFillOutlineLogic(int x, int y, TileRecord data) {
			var foregroundTable = TileMapDictionary[TileMapType.ForegroundOne];
			var tile            = TileDictionary[TileId.SOUTH_OUTLINE];
			_tileMapper.FillOutlines(foregroundTable, tile, data.Bit, x, y - 1);
		}

		void AddToHashSet(TileRecord record) => TileHashset.Add(record);

		public SetAllTilesSyncTileTypeSolver(
			ProceduralConfig config,
			TileHashset tileHashset, TileMapDictionary dictionary, Grid grid,
			StopWatchWrapper stopWatch) {
			TileMapDictionary = dictionary;
			MapWidth          = config.Rows;
			MapHeight         = config.Columns;
			TileDictionary    = config.TileDictionary;
			TileHashset       = tileHashset;

			_generatorTools = new GeneratorTools(config, grid, stopWatch);
			_tileMapper     = new TileMapper(config, dictionary, grid, stopWatch);

			_tileWeightDictionary = new TileWeightDictionary {
				{ OUTLINES, new WeightedRandom<int> { { 0, 75 }, { 1, 25 } } }
			};
		}

		readonly TileWeightDictionary _tileWeightDictionary;
		readonly TileMapper           _tileMapper;
		readonly GeneratorTools       _generatorTools;
		const    string               OUTLINES = "Outlines";
	}
}