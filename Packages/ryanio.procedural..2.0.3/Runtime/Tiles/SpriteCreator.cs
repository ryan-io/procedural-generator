// Algorthims

using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ProceduralGeneration {
	public class SpriteCreator : MonoBehaviour {
		public Texture2D texture;

		[Button]
		void CreateSprite() {
			if (texture == null || Application.isPlaying) return;
			var sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new
				Vector2(0.5f, 0.5f), 32f);
			AssetDatabase.CreateAsset(sprite, @"Assets\Testing");
		}
	}
}