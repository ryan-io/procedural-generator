// ProceduralGeneration

namespace ProceduralGeneration {
	internal class ContextCreator {
		IActions Actions { get; }

		internal FillMapSolverCtx GetNewFillMapCtx()
			=> new(Actions.GetWallFillPercentage(), Actions.GetSeed().Seed.GetHashCode());

		internal SmoothMapSolverCtx GetNewSmoothMapCtx() => new(
			Actions.GetMapDimensions(),
			Actions.GetUpperNeighborLimit(),
			Actions.GetLowerNeighborLimit(),
			Actions.GetSmoothingIterations());

		internal RemoveRegionsSolverCtx GetNewRemoveRegionsCtx() => new(
			Actions.GetMapDimensions(),
			Actions.GetCorridorWidth(),
			Actions.GetWallRemoveThreshold(),
			Actions.GetRoomRemoveThreshold());

		internal ContextCreator(IActions actions) {
			Actions = actions;
		}
	}
}