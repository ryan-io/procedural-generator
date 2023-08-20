using System;
using System.Collections.Generic;
using System.Linq;
using BCL;
using CommunityToolkit.HighPerformance;
using Sirenix.OdinInspector;
using StateMachine;
using TMPro;
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

				machine.InvokeEvent(StateObservableId.ON_INIT);
				new InitializationService(actions).Run(_config);
				machine.InvokeEvent(StateObservableId.ON_CLEAN);
				var run = new Run(actions);

				machine.InvokeEvent(StateObservableId.ON_RUN);

				if (_config.ShouldGenerate) {
					new SeedValidator(_config).Validate(actions.GetSeed());
					new GeneratorSceneSetupService(actions).Run();

					run.Generation();
					run.Serialization();

					onCompleteLog = Message.GENERATION_COMPLETE;
				}

				else if (_config.ShouldDeserialize) {
					new DeserializationService(actions).Run(actions);
					run.Deserialization();

					onCompleteLog = Message.DESERIALIZATION_COMPLETE;
				}

				// dispose
				machine.InvokeEvent(StateObservableId.ON_DISPOSE);

				// complete
				actions.StopTimer();
				machine.InvokeEvent(StateObservableId.ON_COMPLETE);
				actions.Log(onCompleteLog, nameof(Load));
			}
			catch (Exception e) {
				machine.InvokeEvent(StateObservableId.ON_ERROR);
				actions.LogError(e.Message, nameof(Load));
			}
		}

#region TO-DO

		//DataProcessor = new DataProcessor(_config, _data, TileMapDictionary, Grid, RegionRemoverSolver.Rooms);
		//RegionRemovalSolver     RegionRemoverSolver     { get; set; }
		//DataProcessor       DataProcessor       { get; set; }

#endregion

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
		void ForceClean() => new GeneratorCleaner(new Actions(this)).Clean(_config);

		[BoxGroup("Actions"),
		 HorizontalGroup("Actions/Buttons1"),
		 ButtonGroup("Actions/Buttons1/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge)]
		void FindGraphCutters() => _config.FindGraphColliderCuttersInScene();

		[BoxGroup("Actions"),
		 HorizontalGroup("Actions/Buttons1"),
		 ButtonGroup("Actions/Buttons1/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge)]
		void FindPathfinder() => _config.FindPathfinderInScene();

		[BoxGroup("Actions"),
		 HorizontalGroup("Actions/Buttons2"),
		 ButtonGroup("Actions/Buttons2/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge)]
		void DeleteSelectedSerialized() 
			=> DirectoryAction.DeleteDirectory(_config.NameSeedIteration, default, default);

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

		[SerializeField, HideInInspector] MapData _data;
	}
}