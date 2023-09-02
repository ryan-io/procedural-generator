// ProceduralGeneration

namespace ProceduralGeneration {
	internal interface IConfigurations : IMapConfig, IColliderConfig, IPathfindingConfig {
		SpriteShapeConfig GetSpriteShapeConfig();
	}
}