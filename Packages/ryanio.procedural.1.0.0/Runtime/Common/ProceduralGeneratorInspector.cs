// ProceduralGeneration

using Sirenix.OdinInspector;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	public partial class ProceduralGenerator {
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

		[BoxGroup("Actions", centerLabel: true), HorizontalGroup("Actions/Buttons2"),
		 ButtonGroup("Actions/Buttons2/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge),
		 GUIColor(242f / 255, 170f / 255, 34f / 255, 0.9f)]
		void ForceClean() => new GeneratorCleaner(new Actions(this)).Clean(_config, true);

		[BoxGroup("SpriteShape Configuration"), Button(ButtonSizes.Large, Stretch = false, ButtonAlignment = 1),
		 GUIColor(154 / 255f, 208f / 255, 254f / 255, 1f)]
		void RefreshSpriteShapes() => new SpriteShapeRefreshService(gameObject).Run();

		[Button(ButtonSizes.Large, ButtonStyle.Box, Icon = SdfIconType.Gear, Name = "@ActionLabel",
			 IconAlignment = IconAlignment.RightOfText), GUIColor(8f / 255, 195f / 255, 108f / 255, 0.9f)]
		void Generate() {
			Load();
		}

		[field: SerializeField, Required, BoxGroup("Map Configuration"), HideLabel]
		ProceduralConfig _config = null!;

		[field: SerializeField, Required, BoxGroup("SpriteShape Configuration"), HideLabel]
		SpriteShapeConfig _spriteShapeConfig = null!;

#endregion

		void CreatePathfinder() {
			if (AstarPath.active && _config !=null && !_config.Pathfinder && AstarPath.active.gameObject) {
				_config.Pathfinder = AstarPath.active.gameObject;
			}
		}

		void OnValidate() {
			if (_config != null)
				ActionLabel = _config.ShouldGenerate ? GENERATE : DESERIALIZE;

			CreatePathfinder();
		}

		string ActionLabel { get; set; } = string.Empty;

		const string GENERATE    = "Generate";
		const string DESERIALIZE = "Deserialize";
	}
}