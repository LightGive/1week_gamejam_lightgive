using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(RangeInteger))]
public class RangeIntEditor : RangeEditor
{
	protected override void ApplyValue(SerializedProperty i_minProperty, SerializedProperty i_maxProperty)
	{
		// 小さい数値を基準にして、大きい数値が小さい数値より小さくならないようにしてみよう。
		if (i_maxProperty.intValue < i_minProperty.intValue)
		{
			i_maxProperty.intValue = i_minProperty.intValue;
		}
	}

} // class RangeIntEditor

[CustomPropertyDrawer(typeof(RangeFloat))]
public class RangeFloatEditor : RangeEditor
{
	protected override void ApplyValue(SerializedProperty i_minProperty, SerializedProperty i_maxProperty)
	{
		// 小さい数値を基準にして、大きい数値が小さい数値より小さくならないようにしてみよう。
		if (i_maxProperty.floatValue < i_minProperty.floatValue)
		{
			i_maxProperty.floatValue = i_minProperty.floatValue;
		}
	}

} // class RangeFloatEditor

public class RangeEditor : PropertyDrawer
{
	private static readonly GUIContent MIN_LABEL = new GUIContent("Min");
	private static readonly GUIContent MAX_LABEL = new GUIContent("Max");

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var minProperty = property.FindPropertyRelative("m_minValue");
		var maxProperty = property.FindPropertyRelative("m_maxValue");

		ApplyValue(minProperty, maxProperty);

		label = EditorGUI.BeginProperty(position, label, property);

		// プロパティの名前部分を描画。
		Rect contentPosition = EditorGUI.PrefixLabel(position, label);

		// MinとMaxの2つのプロパティを表示するので、残りのフィールドを半分こ。
		contentPosition.width /= 2.0f;

		// Rangeを配列でもっている際は、その分インデントが深くなっている。揃えたいので0に。
		EditorGUI.indentLevel = 0;

		// 3文字なら、これぐらいの幅があればいいんじゃないかな。
		EditorGUIUtility.labelWidth = 30f;


		EditorGUI.PropertyField(contentPosition, minProperty, MIN_LABEL);

		contentPosition.x += contentPosition.width;

		EditorGUI.PropertyField(contentPosition, maxProperty, MAX_LABEL);


		EditorGUI.EndProperty();
	}

	protected virtual void ApplyValue(SerializedProperty i_minProperty, SerializedProperty i_maxProperty)
	{

	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUIUtility.singleLineHeight;
	}

}