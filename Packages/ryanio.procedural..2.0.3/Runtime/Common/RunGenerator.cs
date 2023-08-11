// ProceduralGeneration

namespace ProceduralGeneration {
	internal class GenerateService {
		GeneratorEvents Events { get; set; }
		GeneratorState  State  { get; set; }

		internal void Run() {
			Events = new GeneratorEvents(_proceduralConfig);
			State  = new GeneratorState(_owner);
			
			
		}

		internal GenerateService(IOwner owner, ProceduralConfig config, SpriteShapeConfig spriteShapeConfig) {
			_proceduralConfig  = config;
			_spriteShapeConfig = spriteShapeConfig;
		}

		readonly IOwner            _owner;
		readonly ProceduralConfig  _proceduralConfig;
		readonly SpriteShapeConfig _spriteShapeConfig;
	}
}