using System.Collections.Generic;
using UnityEditor;

namespace InventoryEngine.Editor
{	
	[CustomEditor(typeof(InventoryItem),true)]
	/// <summary>
	/// Custom editor for the InventoryItem component
	/// </summary>
	public class InventoryItemEditor : UnityEditor.Editor 
	{
		/// <summary>
		/// Gets the target inventory component.
		/// </summary>
		/// <value>The inventory target.</value>
		public InventoryItem ItemTarget 
		{ 
			get 
			{ 
				return (InventoryItem)target;
			}
		} 
	   
	   /// <summary>
	   /// Custom editor for the inventory panel.
	   /// </summary>
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			List<string> excludedProperties = new List<string>();
			if (!ItemTarget.Equippable)
			{
				excludedProperties.Add("TargetEquipmentInventoryName");
				excludedProperties.Add("EquippedSound");
			}
			if (!ItemTarget.Usable)
			{
				excludedProperties.Add("UsedSound");
			}
			UnityEditor.Editor.DrawPropertiesExcluding(serializedObject, excludedProperties.ToArray());
			serializedObject.ApplyModifiedProperties();
		}
	}
}