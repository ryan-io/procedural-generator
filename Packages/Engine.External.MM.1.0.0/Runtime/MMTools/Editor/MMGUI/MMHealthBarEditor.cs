using UnityEditor;

namespace MMTools.Editor
{	
	[CustomEditor(typeof(MMHealthBar),true)]
	/// <summary>
	/// Custom editor for health bars (mostly a switch for prefab based / drawn bars
	/// </summary>
	public class HealthBarEditor : UnityEditor.Editor 
	{
		public MMHealthBar HealthBarTarget 
		{ 
			get 
			{ 
				return (MMHealthBar)target;
			}
		} 

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			if (HealthBarTarget.HealthBarType == MMHealthBar.HealthBarTypes.Prefab)
			{
				UnityEditor.Editor.DrawPropertiesExcluding(serializedObject, new string[] {"Size","BackgroundPadding", "SortingLayerName", "InitialRotationAngles", "ForegroundColor", "DelayedColor", "BorderColor", "BackgroundColor", "Delay", "LerpFrontBar", "LerpFrontBarSpeed", "LerpDelayedBar", "LerpDelayedBarSpeed", "BumpScaleOnChange", "BumpDuration", "BumpAnimationCurve" });
            }

			if (HealthBarTarget.HealthBarType == MMHealthBar.HealthBarTypes.Drawn)
			{
				UnityEditor.Editor.DrawPropertiesExcluding(serializedObject, new string[] {"HealthBarPrefab" });
			}

			serializedObject.ApplyModifiedProperties();
		}

	}
}