// ProceduralGeneration

using System;

namespace ProceduralGeneration {
	internal class GenerationMachine : IMachine {
		IActions        Actions { get; }
		GeneratorEvents Events  { get; set; }
		GeneratorState  State   { get; set; }

		void IMachine.RegisterEvent(string identifier, Action action) => Events.RegisterEvent(identifier, action);

		void IMachine.InvokeEvent(string identifier) => Events.InvokeEvent(identifier);

		internal GenerationMachine Run() {
			Events = new GeneratorEvents(Actions);
			State  = new GeneratorState(Actions);
			GeneratorAction.RegisterStateChangeToEvents(Actions, Events, State);

			return this;
		}

		GenerationMachine(IActions actions) {
			Actions = actions;
		}

		internal static GenerationMachine Create(IActions actions) => new(actions);

		readonly ProceduralConfig  _proceduralConfig;
		readonly SpriteShapeConfig _spriteShapeConfig;
	}
}