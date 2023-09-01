using System;
using System.Collections.Generic;
using BCL;
using CommunityToolkit.HighPerformance;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProceduralGeneration {
	internal class StandardTileSetterSolver : TileTypeSolver {
		TileHashset       TileHashset       { get; }
		TileMapDictionary TileMapDictionary { get; }
		TileDictionary    TileDictionary    { get; }
		bool              ShouldRenderTiles { get; }
		int               MapWidth          { get; }
		int               MapHeight         { get; }

		/// <summary>
		/// span is still allocated on the stack during this invocation
		/// </summary>
		/// <param name="span">Pre-stack allocated span for generating primary amap</param>
		internal override void Set(Span2D<int> span) {
			// List<Vector3Int> groundTilePos = new List<Vector3Int>();
			// List<Vector3Int> boundaryTilePos = new List<Vector3Int>();
			// List<Vector3Int> groundTilePos = new List<Vector3Int>();
			//
			for (var i = 0; i < MapWidth * MapHeight; i++) {
				var row    = i / MapHeight;
				var column = i % MapHeight;

				TileTaskCreator(span, row, column);
			}

			if (!ShouldRenderTiles) {
				foreach (var tilemap in TileMapDictionary.Values) {
					var rend = tilemap.gameObject.GetComponent<TilemapRenderer>();

					if (!rend)
						continue;

					rend.enabled = false;
				}
			}
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

		internal StandardTileSetterSolver(TileSolversCtx ctx, TileMapperCtx mapperCtx, GeneratorToolsCtx toolsCtx) {
			MapWidth          = ctx.Dimensions.Rows;
			MapHeight         = ctx.Dimensions.Columns;
			TileMapDictionary = ctx.TileMapDictionary;
			TileDictionary    = ctx.TileDictionary;
			TileHashset       = ctx.TileHashset;
			ShouldRenderTiles = ctx.ShouldRenderTiles;

			_generatorTools = new GeneratorTools(toolsCtx);
			_tileMapper     = new TileMapper(mapperCtx, toolsCtx, ctx);

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