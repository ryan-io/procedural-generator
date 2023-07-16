// 	public abstract class RoomSolver {
// 		public abstract UniTask Solve();
// 		public abstract UniTask Dispose();
// 	}
//
// public class SimpleRoomSolver : RoomSolver {
// 	[field: SerializeField]
// 	[field: Title("Room Data - Generated")]
// 	[field: ReadOnly]
// 	public RoomData RoomDataGenerated { get; set; }
//
// 	[field: SerializeField]
// 	[field: Title("Room Data - Serialized")]
// 	[field: HideLabel]
// 	[field: ReadOnly]
// 	public RoomData RoomDataPreGenerated { get; set; }
//
//
// 	CreateMapModel(MonoModel.BoundaryTilemap, MonoModel.GroundTilemap);
// 	EventManager.TriggerEvent(_cachedMapModel);
// 	CreateRoomsDirector(_cachedMapModel);
//
// 	var config = Model.ProceduralProceduralMapConfig;
//
// 		if (ProceduralMapStateMachine.Global.ShouldNotGenerateAtRuntime) {
// 		if (Model.RoomDataPreGenerated == null) {
// 			log.Error(GeneratorMessages.NoRoomDataAssigned);
//
// 			return;
// 		}
//
// 		_model.MapHandler = new MapHandler(Model.RoomDataPreGenerated, data, _model.SeedValue);
// 	}
//
// 	else {
// 		var roomData = _model.MapProcessor.Save(_model.SeedValue.ToString(), config.seed);
//
// 		Model.RoomDataGenerated = Model.RoomDataPreGenerated = roomData;
// 		_model.MapHandler       = new MapHandler(roomData, data, _model.SeedValue);
// 	}
//
// 	ProceduralMapStateMachine.Global.SetInProgressStateWithData(
// 		ProgressState.SyncingPathfinding, _cachedModel);
//
// 	SetStateToComplete();
//
//
// 	var config = MonoModel.ProceduralProceduralMapConfig;
// 		if (!Application.isPlaying || !config.ShouldNotGenerate)
// 	MonoModel.RoomDataGenerated = null;
// }
