// public class MapProcessor {
//   public async UniTask ProcessMap(ProceduralMapStateMachine stateMachine, CancellationToken token) {
//     // var sw = Stopwatch.StartNew();
//     // log.Msg(RemovingWallRegionsDeemedTooSmall, $"{Strings.ProcGen} Map Processing");
//     // stateMachine.SetInProgressState(ProceduralMapStateMachine.ProgressState.RemovingWalls);
//     await RemoveWallLogicAsync(token);
//     // log.Msg($"Total tile to remove walls: {sw.ElapsedMilliseconds / 1000f} seconds");
//     // log.Msg(RemovingRoomRegionsDeemedTooSmall, $"{Strings.ProcGen} Map Processing");
//     // stateMachine.SetInProgressState(ProceduralMapStateMachine.ProgressState.RemovingRegions);
//     await RemoveRoomRegionsAsync(stateMachine, token);
//     // sw.Stop();
//     // log.Msg($"Total tile to remove room regions: {sw.ElapsedMilliseconds / 1000f} seconds");
//   }
//
//   public RoomData Save(string hash, string seed) {
//     log.Msg("Invoking save from the map process... ", size: 15, italic: true, bold: true,
//       ctx: $"{Strings.ProcGen} Saving Rooms - SO Saver");
//     var instance = ScriptableObject.CreateInstance<RoomData>();
//     var date     = DateTime.UtcNow.ToString("HH-mm-ss_dd-M-yyyy");
//     instance.Inject(_rooms, date + "_hash-" + hash + "_" + _owner + RoomDataSuffix + "_" +
//                             seed);
//     return instance;
//   }
//
//
//   async UniTask RemoveWallLogicAsync(CancellationToken token) {
//     await UniTask.Run(() => {
//                         var regions = GetRegions(1).Result;
//
//                         foreach (var region in regions)
//                           if (region.Count < _wallRemovalThreshold)
//                             foreach (var tile in region) {
//                               if (token.IsCancellationRequested) {
//                                 log.Warning("Cancellation of generation was requested.",
//                                   "Async Cancellation");
//                                 break;
//                               }
//
//                               _map[tile.x, tile.y] = 0;
//                             }
//                       }, cancellationToken: token);
//   }
//
//   async UniTask RemoveRoomRegionsAsync(ProceduralMapStateMachine stateMachine, CancellationToken token) {
//     await UniTask.Run(async () => {
//                         var regions        = GetRegions(0).Result;
//                         var survivingRooms = new List<Room>();
//
//                         foreach (var region in regions)
//                           if (region.Count < _roomRemovalThreshold)
//                             foreach (var tile in region) {
//                               if (token.IsCancellationRequested) {
//                                 log.Warning("Cancellation of generation was requested.",
//                                   "Async Cancellation");
//                                 break;
//                               }
//
//                               _map[tile.x, tile.y] = 1;
//                             }
//                           else
//                             survivingRooms.Add(new Room(region, _map));
//
//                         if (!survivingRooms.Any()) return;
//                         survivingRooms.Sort();
//                         survivingRooms[0].SetIsMainRoom(true);
//                         survivingRooms[0].SetIsAccessibleToMainRoomDirect(true);
//
//                         stateMachine.SetInProgressState(
//                           ProceduralMapStateMachine.ProgressState.ConnectingRoomAndRegions);
//                         await _connectionSolver.Connect(_map, survivingRooms, _dimensions, token);
//                         _rooms = survivingRooms;
//                       }, cancellationToken: token);
//   }
//
// #region PLUMBING
//
//   public MapProcessor(int[,] map, int mapWidth, int mapHeight, int wallRemovalThreshold,
//     int floorRemovalThreshold,
//     int cellWidth, Vector2Int corridorWidth, string owner) {
//     _map                  = map;
//     _dimensions           = new MapDimensions(map, mapWidth, mapHeight, cellWidth, corridorWidth);
//     _wallRemovalThreshold = wallRemovalThreshold;
//     _roomRemovalThreshold = floorRemovalThreshold;
//     _connectionSolver     = new MapConnectionSolver();
//     _owner                = owner;
//   }
//
//   List<Room> _rooms;
//
//   const string RoomDataSuffix = "_RoomData";
//
//   readonly MapConnectionSolver _connectionSolver;
//   readonly MapDimensions       _dimensions;
//
//   readonly int[,] _map;
//   readonly int    _roomRemovalThreshold;
//   readonly int    _wallRemovalThreshold;
//   readonly string _owner;
//
// #endregion
// }

