using System.Threading;
using Cysharp.Threading.Tasks;
using Engine.Tools.Serializer;
using UnityBCL;

namespace Engine.Procedural {
	//TODO - verify if we need to include RoomSolver


	public class ProceduralMapSolver : AsyncUnitOfWork<ProcessStep> {
		//RoomSolver            RoomSolver          { get; set; }
		// public Map Model { get; private set; }
		//
		//
		// protected override UniTask TaskLogic(Process process, CancellationToken token) {
		// }
		//
		// public async UniTask Init(CancellationToken token) {
		// }
		//
		// public UniTask Enable(CancellationToken token) => new();
		

		// public UniTask Begin(CancellationToken token) {
		// 	// var fillMapSolverModel         = new MapSolverModel(config);
		// 	// var borderAndBoundsSolverModel = new BorderAndBoundsSolverModel(Model, config);
		// 	// FillMapSolver       = new CellularAutomataFillMapSolver(fillMapSolverModel);
		// 	// SmoothMapSolver     = new MarchingSquaresSmoothMapSolver(fillMapSolverModel);
		// 	//BorderBoundsSolver  = new IterativeBorderAndBoundsSolver(fillMapSolverModel, borderAndBoundsSolverModel);
		// 	//RegionRemoverSolver = new FloodRegionRemovalSolver(fillMapSolverModel);
		//
		// 	return new UniTask();
		// }

		// public UniTask End(CancellationToken token) => new();
		//
		//
		// public UniTask Dispose(CancellationToken token) =>
		// 	//await _roomSolver.Dispose();
		// 	new();


		// public async UniTask Progress_PopulatingMap(CancellationToken token)
		// 	=> Model.Core = await FillMapSolver.Fill(Model.Core, token);
		//
		// public async UniTask Progress_SmoothingMap(CancellationToken token)
		// 	=> Model.Core = await SmoothMapSolver.Smooth(Model.Core, token);
		//
		// public async UniTask Progress_RemovingRegions(CancellationToken token)
		// 	=> Model.Core = await RegionRemoverSolver.Remove(Model.Core, token);

		// public async UniTask Progress_CreatingBoundary(CancellationToken token)
		// 	=> Model.Border = await BorderBoundsSolver.Determine(Model.Border, Model.Core, token);

		public async UniTask Progress_CompilingData(CancellationToken token) {
			// RoomSolver = new SimpleRoomSolver();
			// await RoomSolver.Solve();

			_logger.Test("Serializing tracker data", ctx: "Data Serialization", bold: true);
			var mapStats = new MapStats(_lastSeed, _lastIteration, MonoModel.ProceduralConfig.Name);
			ProceduralMapStatsHelper.WriteNewSeed(mapStats, GetComponent<SerializerSetup>());
		}

		// public override UniTask Solve() => new();
		//
		// public override UniTask Dispose() => new();
		//
		// public UniTask Progress_PreparingAndSettingTiles(CancellationToken token) => new();
		// public UniTask Progress_GeneratingMesh(CancellationToken token)           => new();
		//
		// public UniTask Progress_CalculatingPathfinding(PathfindingProgressData progressData, CancellationToken token)
		// 	=> new();

		// public ProceduralMapSolver() {
		// 	Map     = new Map();
		// }

		//const string INTERNAL_STRING = "Map Solver";
	}
}