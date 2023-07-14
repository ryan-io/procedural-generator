using UnityEditor;
using UnityEngine;

namespace MMTools.Editor
{	

	[CustomPropertyDrawer(typeof(MMHiddenAttribute))]

	public class MMHiddenAttributeDrawer : PropertyDrawer
	{
	    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	    {
	    	return 0f;
	    }
		
		#if  UNITY_EDITOR
	    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	    {
	       
	    }
	    #endif
	}
}