using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	/// <summary>
	///   This class is responsible for generating the map, mesh, collider, and serializing the data
	///     Verifies the scene contains the required components in order to run procedural generation logic
	///     This class will also kickoff the generation process
	/// 
	///     *** The Procedural Generator requires unsafe context ***
	/// 
	/// rowsOrHeight = GetLength(0)
	/// this is clearly opposite of what I thought
	/// https://stackoverflow.com/questions/4260207/how-do-you-get-the-width-and-height-of-a-multi-dimensional-array
	/// </summary>
	[HideMonoScript]
	public class ProceduralGenerator : Singleton<ProceduralGenerator, ProceduralGenerator>, IOwner {
		public GameObject Go => gameObject;
		
		internal Coordinates GeneratedCoordinates { get; private set; }

		/// <summary>
		///  Loads the generator. This is the entry point for the generator.
		///  Specify whether to generate or deserialize.
		///  If 'ShouldGenerate', will create a RunGenerator instance and invoke Run().
		///  else if 'ShouldDeserialize', will create a DeserializeGenerator instance and invoke Run().
		///  Otherwise, will do nothing.
		/// </summary>
		void Load() {
			var actions = new Actions(this)
				{ ProceduralConfig = _config, SpriteShapeConfig = _spriteShapeConfig };

			IMachine machine = GenerationMachine.Create(actions).Run();

			try {
				var onCompleteLog = string.Empty;

				if (!_config.ShouldGenerate && !_config.ShouldDeserialize) {
					actions.LogWarning(Message.NOT_SET_TO_RUN, nameof(Load));
					return;
				}

				var run = Initialize(machine, actions, !_config.ShouldDeserialize);
				machine.InvokeEvent(StateObservableId.ON_RUN);

				if (_config.ShouldGenerate) {
					onCompleteLog = GenerateMap(actions, run);
				}

				else if (_config.ShouldDeserialize) {
					onCompleteLog = DeserializeMap(actions, run);
				}

				Dispose(machine);
				Complete(actions, machine, onCompleteLog);
			}
			catch (Exception e) {
				machine.InvokeEvent(StateObservableId.ON_ERROR);
				actions.LogError(e.Message, nameof(Load));
			}
		}

		Run Initialize(IMachine machine, IActions actions, bool initSeed = true) {
			if (initSeed)
				new SeedValidator(_config).Validate(actions.GetSeed());
			
			machine.InvokeEvent(StateObservableId.ON_INIT);
			new InitializationService(actions).Run(_config);
			machine.InvokeEvent(StateObservableId.ON_CLEAN);

			return new Run(actions);
		}

		string GenerateMap(IActions actions, Run run) {
			new GeneratorSceneSetupService(actions).Run();
			
			run.Generation();
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
			
			actions.StopTimer();
			machine.InvokeEvent(StateObservableId.ON_COMPLETE);

			actions.Log(onCompleteLog,                                         nameof(Load));
			actions.Log(Message.TOTAL_TIME_ELAPSED + actions.GetTimeElapsed() + " sec.", nameof(Load));
		}

#region INSPECTOR

		[BoxGroup("Actions"),
		 HorizontalGroup("Actions/Buttons1"),
		 ButtonGroup("Actions/Buttons1/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge),
		 ShowIf("@IsTileDictNullOrEmpty")]
		void PopulateTileDictionary() {
			if (_config.TileDictionary.Count < 1)
				_config.PopulateTileDictionary();
		}

		bool IsTileDictNullOrEmpty => _config.TileDictionary.IsEmptyOrNull();

		[BoxGroup("Actions", centerLabel: true),
		 HorizontalGroup("Actions/Buttons2"),
		 ButtonGroup("Actions/Buttons2/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge)]
		void ForceClean() => new GeneratorCleaner(new Actions(this)).Clean(_config, true);

		[BoxGroup("Actions"),
		 HorizontalGroup("Actions/Buttons1"),
		 ButtonGroup("Actions/Buttons1/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge)]
		void FindGraphCutters() => _config.FindGraphColliderCuttersInScene();

		[BoxGroup("Actions"),
		 HorizontalGroup("Actions/Buttons1"),
		 ButtonGroup("Actions/Buttons1/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge)]
		void FindPathfinder() => _config.FindPathfinderInScene();
		
		[BoxGroup("Actions"),
		 HorizontalGroup("Actions/Buttons1"),
		 ButtonGroup("Actions/Buttons1/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge)]
		void RefreshSpriteShapes() => new SpriteShapeRefreshService(gameObject).Run();

		[BoxGroup("Actions"),
		 HorizontalGroup("Actions/Buttons2"),
		 ButtonGroup("Actions/Buttons2/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge)]
		void DeleteSelectedSerialized() {
			if (string.IsNullOrWhiteSpace(_config.NameSeedIteration))
				return;
			
			DirectoryAction.DeleteDirectory(_config.NameSeedIteration, default, default);
			_config.NameSeedIteration = Help.GetAllSeeds().FirstOrDefault();
		}

		[Button(ButtonSizes.Medium, ButtonStyle.CompactBox, Icon = SdfIconType.Signal,
			IconAlignment = IconAlignment.RightOfText)]
		void ScaleMap(float scale) => Scale.Current(gameObject, scale);

		[Button(ButtonSizes.Large, ButtonStyle.CompactBox, Icon = SdfIconType.Gear,
			IconAlignment = IconAlignment.RightOfText)]
		void Generate() {
			Load();
		}
		
		[field: SerializeField, Required, BoxGroup("Configuration"), HideLabel]
		ProceduralConfig _config = null!;

		[field: SerializeField, Required, BoxGroup("Configuration"), HideLabel]
		SpriteShapeConfig _spriteShapeConfig = null!;
  
		//[SerializeField, HideInInspector] MapData _data;
		
#endregion

	}
}