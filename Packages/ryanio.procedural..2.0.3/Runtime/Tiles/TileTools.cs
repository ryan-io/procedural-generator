using System;
using System.Collections.Generic;
using BCL;
using CommunityToolkit.HighPerformance;
using UnityBCL;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Tilemaps;

namespace ProceduralGeneration {
	public class GeneratorTools {
		StopWatchWrapper StopWatch { get; }
		Grid             GridObj   { get; }
		int              MapWidth  { get; }
		int              MapHeight { get; }

		public void SetTileNullAtXY(int x, int y, KeyValuePair<TileMapType, Tilemap> map) {
			var position = new Vector3Int(x, y, 0);
			SetTile(map.Value, null, position);
		}

		public void LogHeapMemoryAllocated() {
			GenLogging.Instance.LogWithTimeStamp(
				LogLevel.Normal,
				StopWatch.TimeElapsed,
				Message.HEAP_ALLOCATION + TotalMemoryAllocated,
				HEAP_ALLOCATION);
		}

		public double TotalMemoryAllocated
			=> Math.Round(Profiler.GetTotalAllocatedMemoryLong() / 1000000000f, 2);

		public void CreateTileLabel(int x, int y, string text) {
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

		public TileMask SolveMask(Span2D<int> span, int x, int y, bool isBoundary) {
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

		public bool IsSouthOutline(TileMask bit) => IsBit(bit, SOUTH_OUTLINE);

		public bool HasNoNeighbors(TileMask bit) => bit == TileMask.None;

		public bool HasAllNeighbors(TileMask bit) => bit == ALL_MASK;

		public bool IsWall(TileMask bit) => bit != ALL_MASK;

		public bool ContainsAnyBits(TileMask comparer, params TileMask[] mask) {
			var count = mask.Length;

			for (var i = 0; i < count; i++) {
				var hasBit = (comparer & mask[i]) != 0;

				if (!hasBit)
					return false;
			}

			return true;
		}

		public static bool IsFilled(Span2D<int> span, int x, int y) => span[x, y] == 1;

		public void SetTile(Tilemap map, TileBase tile, Vector3Int position) {
			if (map == null) {
				var log = new UnityLogging();
				log.Warning("The Tilemap map parameter, 'map', is null. Now returning.");

				return;
			}

			map.SetTile(position, tile);
		}

		public void SetOriginWrtMap(GameObject go) {
			go.transform.position = new(
				Mathf.CeilToInt(-MapWidth   / 2f),
				Mathf.FloorToInt(-MapHeight / 2f),
				0);
		}

		public void SetGridScale(int newScale) {
			GridObj.gameObject.transform.localScale = new(newScale, newScale, newScale);
		}

		bool IsBit(TileMask check, TileMask against) => check == against;

		public GeneratorTools(ProceduralConfig config, Grid grid, StopWatchWrapper stopWatch) {
			GridObj   = grid;
			MapWidth  = config.Rows;
			MapHeight = config.Columns;
			StopWatch = stopWatch;
		}

		const TileMask SOUTH_OUTLINE =
			TileMask.NorthWest | TileMask.North | TileMask.NorthEast | TileMask.West | TileMask.East;

		const TileMask ALL_MASK = TileMask.NorthWest | TileMask.North     | TileMask.NorthEast | TileMask.West |
		                          TileMask.East      | TileMask.SouthWest | TileMask.South     | TileMask.SouthEast;

		const string HEAP_ALLOCATION = "HeapAllocation";
	}
}