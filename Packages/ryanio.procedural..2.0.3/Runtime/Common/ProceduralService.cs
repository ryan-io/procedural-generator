// ProceduralGeneration

using System;

namespace ProceduralGeneration {
	internal static class ProceduralService {
		internal static FillMapSolver GetFillMapSolver(Func<FillMapSolver> constructor) => constructor.Invoke();

		internal static SmoothMapSolver GetSmoothMapSolver(Func<SmoothMapSolver> constructor) => constructor.Invoke();

		internal static RegionSolver GetRoomsAndWallsSolver(Func<RegionSolver> constructor)
			=> constructor.Invoke();

		internal static TileTypeSolver GetTileSetterSolver(Func<TileTypeSolver> constructor) => constructor.Invoke();

		internal static MeshSolver GetMeshSolver(Func<MeshSolver> constructor) => constructor.Invoke();

		internal static NavigationSolver GetNavigationSolver(Func<NavigationSolver> constructor)
			=> constructor.Invoke();

		internal static ColliderSolver GetColliderSolver(Func<ColliderSolver> constructor) => constructor.Invoke();

		internal static SpriteShapeBorderSolver GetSpriteShapeBorderSolver(Func<SpriteShapeBorderSolver> constructor)
			=> constructor.Invoke();

		public static CreateBoundaryColliders GetCutCollidersSolver(Func<CreateBoundaryColliders> constructor)
			=> constructor.Invoke();

		public static GridCharacteristicsSolver GetGridCharacteristicsSolver(Func<GridCharacteristicsSolver> constructor) {
			return constructor.Invoke();
		}
	}
}