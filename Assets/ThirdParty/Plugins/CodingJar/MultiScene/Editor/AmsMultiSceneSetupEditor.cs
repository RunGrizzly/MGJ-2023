using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System.Linq;

using CodingJar;
using UnityEngine.SceneManagement;

namespace CodingJar.MultiScene.Editor
{
	/// <summary>
	/// Custom 
	/// </summary>
	[CustomEditor(typeof(AmsMultiSceneSetup), true)]
	class AmsMultiSceneSetupEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI ()
		{
			AmsMultiSceneSetup target = (AmsMultiSceneSetup)this.target;

			// Help give us better hints at what the different SceneSetup modes refer to...
			if ( target.sceneSetupMode == AmsMultiSceneSetup.SceneSetupManagement.Automatic )
			{
				bool isActiveScene = (SceneManager.GetActiveScene() == target.gameObject.scene);
				if ( isActiveScene )
					EditorGUILayout.HelpBox("Scene Setup is automatically generated and saved with the scene based on the hierarchy.", MessageType.Info);
				else
					EditorGUILayout.HelpBox("Scene Setup will not be updated or saved unless this Scene is set as Active", MessageType.Warning);

				if ( target.GetSceneSetup().Count < 1 )
				{
					DrawPropertiesExcluding( serializedObject, "m_Script", "_sceneSetup" );
					EditorGUILayout.HelpBox( "This scene was never saved as the Active Scene.\nTherefore this Scene will not auto-load other scenes.", MessageType.Info );
				}
				else
					DrawPropertiesExcluding( serializedObject, "m_Script" );
			}
			else if ( target.sceneSetupMode == AmsMultiSceneSetup.SceneSetupManagement.Manual )
			{
				EditorGUILayout.HelpBox("Scene Setup will not changed unless you modify it manually (or change Scene Setup Mode).", MessageType.Info);
				DrawPropertiesExcluding( serializedObject, "m_Script" );
			}
			else
			{
				EditorGUILayout.HelpBox("Scene Setup will not be saved", MessageType.Warning);
				DrawPropertiesExcluding( serializedObject, "m_Script", "_sceneSetup" );
			}

			EditorGUILayout.HelpBox("Note: This behaviour is always required for cross-scene referencing to work.", MessageType.Info);

			// Since we're not using SerializedProperties, we need to update the SerializedObject ourselves.
			serializedObject.ApplyModifiedProperties();
			serializedObject.Update();

			// If anything changed, we should resetup the SceneSetup and repaint the hierarchy
			if ( GUI.changed )
			{
				AmsMultiSceneSetup.OnSceneSaving( target.gameObject.scene, target.scenePath );
				EditorApplication.RepaintHierarchyWindow();
			}
		}
	} // class


	/// <summary>
	/// Draw the scene name instead of "Element 0" when listing a Multi-Scene Entry.
	/// </summary>
	[CustomPropertyDrawer(typeof(AmsMultiSceneSetup.SceneEntry))]
	class AmsMultiSceneEntryDrawer : UnityEditor.PropertyDrawer
	{
		public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
		{
			return EditorGUI.GetPropertyHeight( property, null, true );
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var propScene = property.FindPropertyRelative( "scene" );
			if ( propScene != null )
			{
				var propName = propScene.FindPropertyRelative( "name" );
				if ( propName != null )
				{
					label.text = propName.stringValue;
				}
			}

			EditorGUI.PropertyField( position, property, label, true );
		}
	}
} // namespace

