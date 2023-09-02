using System;
using System.Collections.Generic;
using BCL;
using CommunityToolkit.HighPerformance;
using UnityBCL;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Tilemaps;

namespace ProceduralGeneration {
	internal class GeneratorTools {
		StopWatchWrapper StopWatch { get; }
		Grid             GridObj   { get; }
		int              MapWidth  { get; }
		int              MapHeight { get; }

		internal void CompressTilemaps() => new TileMapCompressor(GridObj.gameObject).Compress();

		internal void SetTileNullAtXY(int x, int y, KeyValuePair<TileMapType, Tilemap> map) {
			var position = new Vector3Int(x, y, 0);
			SetTile(map.Value, null, position);
		}

		internal double TotalMemoryAllocated
			=> Math.Round(Profiler.GetTotalAllocatedMemoryLong() / 1000000000f, 2);

		internal void CreateTileLabel(int x, int y, string text) {
			return;
			/*
			
			var tileSizeOffset = GridObj.cellSize;

			var position = new Vector3(
				-MapWidth  / 2f + x + tileSizeOffset.x / 2f,
				-MapHeight / 2f + y + tileSizeOffset.y / 2f,
				10);

			position *= Constants.CELL_SIZE;
			*/

			//TODO - a world txt function will need to be created if we want this functionality back in the future
			// var label = global::Utility.CreateWorldTextTMP(
			// 	text, null, 6, Color.yellow,
			// 	TextAlignmentOptions.Center, position);
			//
			// label.sortingLayerID = SortingLayer.NameToID(TileMapper.AboveSortingLayer);
			// label.gameObject.tag = TileMapper.Label;
		}

		internal TileMask SolveMask(Span2D<int> span, int x, int y, bool isBoundary) {
			var bit = TileMask.None;

			if (isBoundary)
				return bit;

			if (IsFilled(span, x - 1, y + 1)) //Debug.Msg("Is NW");
				bit |= TileMask.NorthWest;

			if (IsFilled(span, x, y + 1)) //Debug.Msg("Is N");
				bit |= TileMask.North;

			if (IsFilled(span, x + 1, y + 1)) //Debug.Msg("Is NE");
				bit |= TileMask.NorthEast;

			if (IsFilled(span, x - 1, y)) //Debug.Msg("Is W");
				bit |= TileMask.West;

			if (IsFilled(span, x + 1, y)) //Debug.Msg("Is E");
				bit |= TileMask.East;

			if (IsFilled(span, x - 1, y - 1)) //Debug.Msg("Is SW");
				bit |= TileMask.SouthWest;

			if (IsFilled(span, x, y - 1)) //Debug.Msg("Is S");	
				bit |= TileMask.South;

			if (IsFilled(span, x + 1, y - 1)) //Debug.Msg("Is SE");
				bit |= TileMask.SouthEast;

			return bit;
		}

		internal bool IsSouthOutline(TileMask bit) => IsBit(bit, SOUTH_OUTLINE);

		internal bool HasNoNeighbors(TileMask bit) => bit == TileMask.None;

		internal bool HasAllNeighbors(TileMask bit) => bit == ALL_MASK;

		internal bool IsWall(TileMask bit) => bit != ALL_MASK;

		internal bool ContainsAnyBits(TileMask comparer, params TileMask[] mask) {
			var count = mask.Length;

			for (var i = 0; i < count; i++) {
				var hasBit = (comparer & mask[i]) != 0;

				if (!hasBit)
					return false;
			}

			return true;
		}

		internal static bool IsFilled(Span2D<int> span, int x, int y) => span[x, y] == 1;

		internal void SetTile(Tilemap map, TileBase tile, Vector3Int position) {
			if (map == null) {
				var log = new UnityLogging();
				log.Warning("The Tilemap map parameter, 'map', is null. Now returning.");

				return;
			}

			map.SetTile(position, tile);
		}

		internal void SetOriginWrtMap(GameObject go) {
			go.transform.position = new(
				Mathf.CeilToInt(-MapWidth   *Constants.CELL_SIZE / 2f),
				Mathf.FloorToInt(-MapHeight *Constants.CELL_SIZE / 2f),
				0);
		}

		internal void SetGridScale(int newScale) {
			GridObj.gameObject.transform.localScale = new(newScale, newScale, newScale);
		} 

		bool IsBit(TileMask check, TileMask against) => check == against;

		internal GeneratorTools(GeneratorToolsCtx ctx) {
			GridObj   = ctx.Grid;
			MapWidth  = ctx.Dimensions.Rows;
			MapHeight = ctx.Dimensions.Columns;
		}

		const TileMask SOUTH_OUTLINE =
			TileMask.NorthWest | TileMask.North | TileMask.NorthEast | TileMask.West | TileMask.East;

		const TileMask ALL_MASK = TileMask.NorthWest | TileMask.North     | TileMask.NorthEast | TileMask.West |
		                          TileMask.East      | TileMask.SouthWest | TileMask.South     | TileMask.SouthEast;

		const string HEAP_ALLOCATION = "HeapAllocation";
	}
}