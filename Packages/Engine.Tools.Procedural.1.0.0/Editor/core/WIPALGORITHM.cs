//
//  * WIP Algorithm
//  * if (_tileConfig == null)
// 				return;
//
// 			await UniTask.Yield();
// 			var mapWidth         = _generatorConfig.mapWidth;
// 			var mapHeight        = _generatorConfig.mapHeight;
// 			var positionsAlloc   = new Vector3Int[mapWidth * mapHeight];
// 			var tilesAllocGround = new TileBase[positionsAlloc.Length];
// 			var tilesAllocWalls  = new TileBase[positionsAlloc.Length];
// 			//var tasks          = new ConcurrentBag<UniTask<TileData>>();
// 			var dto = new TileMapperData(_tileConfig, _tileSceneObjects, _generatorConfig, _model.Map);
//
// 			var counter = 0;
// 			var limit   = positionsAlloc.Length;
// 			var x       = 0;
// 			var y       = 0;
//
// 			while (counter < limit) {
// 				positionsAlloc[counter] = new Vector3Int(x, y, 0);
// 				var isFilled   = TileLogic.IsFilled(_model.Map, x, y);
// 				var isBoundary = TileLogic.IsBoundary(_generatorConfig.mapWidth, _generatorConfig.mapHeight, x, y);
// 				var bit        = TileLogic.SolveMask(_model.Map, x, y, isBoundary);
//
// 				if (isFilled && isBoundary) {
// 					tilesAllocWalls[counter]  = _tileConfig.Boundary;
// 					tilesAllocGround[counter] = null;
// 				}
//
// 				else if (isFilled) {
// 					if (TileHandler.IsWall(bit))
// 						FillTiles(x, y, ref dto, ref bit);
// 					else
// 						tilesAllocGround[counter] = _tileConfig.Ground;
// 				}
// 				else
// 					tilesAllocGround[counter] = null;
//
// 				// add to hashtable, create tiledata
//
// 				counter++;
// 				x++;
//
// 				if (x >= _generatorConfig.mapWidth) {
// 					y++;
// 					x = 0;
// 				}
//
// 				if (x > _generatorConfig.mapWidth || y > _generatorConfig.mapHeight) {
// 					log.Error("x or y in the generation algorithm is greater than config heightor width");
// 					break;
// 				}
// 			}
//
// 			try {
// 				ProceduralMapStateMachine.Global.SetInProgressState(MapGeneratorStateMachine.ProgressState.SettingTiles);
// 				Ground.SetTiles(positionsAlloc, tilesAllocGround);
// 				//var rewrew = await UniTask.WhenAll(tasks);
// 			}
//
// 			catch (AggregateException e) {
// 				foreach (var exception in e.Flatten().InnerExceptions) {
// 					log.Error($"Exception thrown when setting tiles. {e.Message}");
// #if UNITY_EDITOR
// 					UnityEditor.EditorApplication.isPlaying = false;
// #endif
// 				}
// 			}
// 		}
//  */

