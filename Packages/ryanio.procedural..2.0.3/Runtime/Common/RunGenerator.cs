// ProceduralGeneration

namespace ProceduralGeneration {
	internal class GenerateService {
		IOwner          Owner  { get; }
		GeneratorEvents Events { get; set; }
		GeneratorState  State  { get; set; }

		internal void Run() {
			Events = new GeneratorEvents(_proceduralConfig);
			State  = new GeneratorState(Owner, Events);
            GeneratorAction.RegisterStateToEvents(Events, State);
		}

		internal GenerateService(IOwner owner, ProceduralConfig config, SpriteShapeConfig spriteShapeConfig) {
			Owner = owner;
			_proceduralConfig  = config;
			_spriteShapeConfig = spriteShapeConfig;
			_generatorModel    = new GeneratorModel();
		}

		readonly ProceduralConfig  _proceduralConfig;
		readonly SpriteShapeConfig _spriteShapeConfig;
		readonly GeneratorModel    _generatorModel;
	}
}