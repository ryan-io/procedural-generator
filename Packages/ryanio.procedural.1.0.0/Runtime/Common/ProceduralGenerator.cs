using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityBCL;
using UnityEditor;
using UnityEngine;

namespace ProceduralGeneration {
	/// <summary>
	///   This class is responsible for generating the map, mesh, collider, and serializing the data
	///     Verifies the scene contains the required components in order to run procedural generation logic
	///     This class will also kickoff the generation or deserialization process
	/// 
	///     *** The Procedural Generator requires unsafe context ***
	/// 
	/// </summary>
	[HideMonoScript]
	public partial class ProceduralGenerator : Singleton<ProceduralGenerator, ProceduralGenerator>, IOwner {
		public GameObject Go => gameObject;

		internal             Coordinates         GeneratedCoordinates { get; private set; }
		[CanBeNull] internal MapData             Data                 { get; private set; }
		internal             IReadOnlyList<Room> Rooms                { get; private set; }

		IActions Actions { get; set; }
		
		/// <summary>
		///  Loads the generator. This is the entry point for the generator.
		///  Specify whether to generate or deserialize.
		///  If 'ShouldGenerate', will create a RunGenerator instance and invoke Run().
		///  else if 'ShouldDeserialize', will create a DeserializeGenerator instance and invoke Run().
		///  Otherwise, will do nothing.
		/// </summary>
		void Load() {
			Actions = new Actions(this)
				{ ProceduralConfig = _config, SpriteShapeConfig = _spriteShapeConfig };

			IMachine machine = GenerationMachine.Create(Actions).Run();
			var onCompleteLog = string.Empty;

			try {

				if (!_config.ShouldGenerate && !_config.ShouldDeserialize) {
					Actions.LogWarning(Message.NOT_SET_TO_RUN, nameof(Load));
					return;
				}

				var run = Initialize(machine, Actions, !_config.ShouldDeserialize);
				machine.InvokeEvent(StateObservableId.ON_RUN);

				if (_config.ShouldGenerate) {
					onCompleteLog = GenerateMap(Actions, run);
				}

				else if (_config.ShouldDeserialize) {
					onCompleteLog = DeserializeMap(Actions, run);
				}

				Dispose(machine);
				Complete(Actions, machine, onCompleteLog);
			}
			catch (ObjectDisposedException) {
				Actions.LogWarning("Machine was disposed.", nameof(ObjectDisposedException));
				Complete(Actions, machine, onCompleteLog);
				throw;
				// this is thrown when the machine is disposed
			}
			catch (Exception e) {
				machine.InvokeEvent(StateObservableId.ON_ERROR);
				Actions.LogError(e.Message, nameof(Load));
			}
			finally {
				//AssetDatabase.Refresh(); // standard settings: 1.32sec, 210.1MB GC Alloc
			}
		}

		Run Initialize(IMachine machine, IActions actions, bool initSeed = true) {
			if (initSeed)
				new SeedValidator(_config).Validate(actions.GetSeed());

			machine.InvokeEvent(StateObservableId.ON_INIT);
			new InitializationService(actions).Run(_config);
			machine.InvokeEvent(StateObservableId.ON_CLEAN);

			return new Run(actions, machine);
		}

		string GenerateMap(IActions actions, Run run) {
			new GeneratorSceneSetupService(actions).Run();

			Data = run.Generation();
			run.Serialization();

			return Message.GENERATION_COMPLETE;
		}

		static string DeserializeMap(IActions actions, Run run) {
			new DeserializationService(actions).Run(actions);
			run.Deserialization();

			return Message.DESERIALIZATION_COMPLETE;
		}

		static void Dispose(IMachine machine) {
			machine.InvokeEvent(StateObservableId.ON_DISPOSE);
		}

		void Complete(IActions actions, IMachine machine, string onCompleteLog) {
			GeneratedCoordinates = actions.GetCoordinates();
			Rooms                = actions.GetRooms();

			actions.StopTimer();
			machine.InvokeEvent(StateObservableId.ON_COMPLETE);

			actions.Log(onCompleteLog,                                                   nameof(Load));
			actions.Log(Message.TOTAL_TIME_ELAPSED + actions.GetTimeElapsed() + " sec.", nameof(Load));
		}
	}
}