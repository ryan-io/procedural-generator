// Algorthims

using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Engine.Procedural.Runtime {
	public class SpriteCreator : MonoBehaviour {
		public Texture2D texture;

#if UNITY_EDITOR
		[Button]
		void CreateSprite() {
			if (texture == null || Application.isPlaying) return;
			var sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new
				Vector2(0.5f, 0.5f), 32f);
			AssetDatabase.CreateAsset(sprite, @"Assets\Testing");
		}
#endif
	}
}