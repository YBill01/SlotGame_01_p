using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Array2DSimpleAttribute))]
[CustomPropertyDrawer(typeof(Array2D<>))]
public class Array2DDrawer : PropertyDrawer
{
	private SerializedProperty size;

	private SerializedProperty data;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		bool isSimple = attribute as Array2DSimpleAttribute is not null;

		EditorGUI.BeginProperty(position, label, property);

		EditorGUILayout.Space(-20);
		
		if (!(property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, label, true)))
		{
			return;
		}

		size = property.FindPropertyRelative("m_size");
		data = property.FindPropertyRelative("m_data");

		int originalIndent = EditorGUI.indentLevel;

		EditorGUILayout.BeginVertical(GUI.skin.box);

		if (!isSimple)
		{
			EditorGUILayout.LabelField("Size:");
			EditorGUILayout.PropertyField(size, GUIContent.none);

			EditorGUILayout.Separator();
			EditorGUILayout.LabelField("Values:");
		}

		EditorGUI.indentLevel = originalIndent;

		GUILayoutOption guiLayautWidth = data.arrayElementType == "bool" ? GUILayout.Width(16) : GUILayout.MinWidth(50);

		Vector2Int sizeValue = size.vector2IntValue;

		for (int y = 0; y < sizeValue.y; y++)
		{
			EditorGUILayout.BeginHorizontal();

			for (int x = 0; x < sizeValue.x; x++)
			{
				if ((sizeValue.x * y) + x < data.arraySize)
				{
					EditorGUILayout.PropertyField(data.GetArrayElementAtIndex((sizeValue.x * y) + x), GUIContent.none, guiLayautWidth);
				}
			}

			EditorGUILayout.EndHorizontal();
		}

		EditorGUI.indentLevel = originalIndent;
		EditorGUI.EndProperty();
		EditorGUILayout.EndVertical();

		if (GUI.changed)
		{
			data.arraySize = sizeValue.y * sizeValue.x;
			property.serializedObject.ApplyModifiedProperties();
		}
	}
}