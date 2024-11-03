using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomHorizontalLayoutGroup))]
public class CustomHorizontalLayoutGroupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!(target is CustomHorizontalLayoutGroup layoutGroup))
        {
            return;
        }

        if (GUILayout.Button("下のやつを一気に登録"))
        {
            var targetList = new List<RectTransform>();
            for (var i = 0; i < layoutGroup.transform.childCount; i++)
            {
                var child = layoutGroup.transform.GetChild(i);
                targetList.Add(child.transform as RectTransform);
            }

            layoutGroup.SetLayoutTarget(targetList);
            EditorUtility.SetDirty(target);
        }

        if (GUILayout.Button("整列させる"))
        {
            layoutGroup.SetLayoutTarget(layoutGroup.TargetList).Align();
            EditorUtility.SetDirty(target);
        }
    }
}

