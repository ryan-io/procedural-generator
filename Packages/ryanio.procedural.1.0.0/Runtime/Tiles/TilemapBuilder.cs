// ProceduralGeneration

using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProceduralGeneration {
	public class TilemapBuilder {
		public TileMapDictionary Build(GameObject container) {
			var dict = new TileMapDictionary() {
				{ TileMapType.Boundary, SetupNewTilemap(container.transform,           TileMapType.Boundary) },
				{ TileMapType.Ground, SetupNewTilemap(container.transform,             TileMapType.Ground) },
				{ TileMapType.Obstacles, SetupNewTilemap(container.transform,          TileMapType.Obstacles) },
				{ TileMapType.ForegroundOne, SetupNewTilemap(container.transform,      TileMapType.ForegroundOne) },
				{ TileMapType.ForegroundTwo, SetupNewTilemap(container.transform,      TileMapType.ForegroundTwo) },
				{ TileMapType.ForegroundThree, SetupNewTilemap(container.transform,    TileMapType.ForegroundThree) },
				{ TileMapType.PlatformEffector, SetupNewTilemap(container.transform,   TileMapType.PlatformEffector) },
				{ TileMapType.PlatformNoEffector, SetupNewTilemap(container.transform, TileMapType.PlatformNoEffector) }
			};

			return dict;
		}

		Tilemap SetupNewTilemap(Transform root, TileMapType type) {
			var obj = new GameObject(type.ToString()) {
				transform = {
					position = Vector3.zero,
					parent   = root
				}
			};

			var tilemap      = obj.AddComponent<Tilemap>();
			var tileRenderer = obj.AddComponent<TilemapRenderer>();

			SetTilemap(tilemap);
			SetTileRenderer(tileRenderer, type);

			return tilemap;
		}

		void SetTilemap(Tilemap tilemap) {
			tilemap.orientation = Tilemap.Orientation.XY;
			tilemap.tileAnchor  = new Vector3(0.5f, 0.5f, 0);
		}

		void SetTileRenderer(TilemapRenderer tileRenderer, TileMapType type) {
			tileRenderer.sortOrder = TilemapRenderer.SortOrder.BottomLeft;
			tileRenderer.mode = type is TileMapType.Ground or TileMapType.Boundary
				                    ? TilemapRenderer.Mode.Chunk
				                    : TilemapRenderer.Mode.Individual;

			tileRenderer.detectChunkCullingBounds = TilemapRenderer.DetectChunkCullingBounds.Auto;
			tileRenderer.sortingLayerName         = DetermineSortingLayer(type);
		}

		string DetermineSortingLayer(TileMapType type) {
			switch (type) {
				case TileMapType.Ground:
					return Constants.SortingLayer.GROUND;
				case TileMapType.Boundary:
					return Constants.SortingLayer.BOUNDARY;
				case TileMapType.ForegroundOne:
					return Constants.SortingLayer.FOREGROUND_ONE;
				case TileMapType.ForegroundTwo:
					return Constants.SortingLayer.FOREGROUND_TWO;
				case TileMapType.ForegroundThree:
					return Constants.SortingLayer.FOREGROUND_THREE;
				case TileMapType.PlatformEffector:
				case TileMapType.PlatformNoEffector:
				case TileMapType.Obstacles:
					return Constants.SortingLayer.OBSTACLES;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}

		public TilemapBuilder() {
		}
	}
}