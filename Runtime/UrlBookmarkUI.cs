using UnityEngine;
using VRC.SDKBase;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Narazaka.VRChat.UrlBookmarks.EditorOnly
{
    public class UrlBookmarkUI : MonoBehaviour, IEditorOnly
    {
#if UNITY_EDITOR
        [CustomEditor(typeof(UrlBookmarkUI))]
        [CanEditMultipleObjects]
        public class UrlBookmarkUIEditor : UnityEditor.Editor
        {
            SerializedObject so;
            SerializedProperty InputField;
            SerializedProperty Target;
            SerializedProperty TargetVariableName;
            SerializedProperty TargetMethodName;
            SerializedProperty TargetInputField;

            void OnEnable()
            {
                var urlBookmarks = targets.Select(target => (target as UrlBookmarkUI).transform.GetComponentInChildren<UrlBookmark>()).ToArray();
                so = new SerializedObject(urlBookmarks);
                TargetInputField = so.FindProperty("TargetInputField");
                Target = so.FindProperty("Target");
                TargetVariableName = so.FindProperty("TargetVariableName");
                TargetMethodName = so.FindProperty("TargetMethodName");
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                so.UpdateIfRequiredOrScript();
                EditorGUILayout.PropertyField(TargetInputField);
                EditorGUILayout.PropertyField(Target);
                EditorGUILayout.PropertyField(TargetVariableName);
                EditorGUILayout.PropertyField(TargetMethodName);
                so.ApplyModifiedProperties();
            }
        }
#endif
    }
}
