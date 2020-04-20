using UnityEditor;
using UnityEditor.UI;

namespace SG1
{
	[CustomEditor(typeof(ImageCompletion),true)]
	[CanEditMultipleObjects]
	public class ImageCompletionEditor : ImageEditor
	{
		private SerializedProperty MirrorDir;

		protected override void OnEnable()
		{
			base.OnEnable();
			MirrorDir = serializedObject.FindProperty("MirrorDir");
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			EditorGUILayout.Space();
			serializedObject.Update();
			
			EditorGUILayout.PropertyField(MirrorDir);
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}