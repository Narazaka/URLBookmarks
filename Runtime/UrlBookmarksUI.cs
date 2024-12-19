using UnityEngine;
using VRC.SDKBase;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Narazaka.VRChat.UrlBookmarks.EditorOnly
{
    public class UrlBookmarksUI : MonoBehaviour, IEditorOnly
    {
        [SerializeField] GameObject UrlBookmarkPrefab;
        [SerializeField] Transform UrlBookmarksParent;

#if UNITY_EDITOR
        [CustomEditor(typeof(UrlBookmarksUI))]
        public class UrlBookmarksUIEditor : UnityEditor.Editor
        {
            SerializedProperty Script;
            SerializedProperty UrlBookmarkPrefab;
            SerializedProperty UrlBookmarksParent;
            SerializedObject so;
            SerializedProperty InputField;
            SerializedProperty Target;
            SerializedProperty TargetVariableName;
            SerializedProperty TargetMethodName;
            SerializedProperty TargetInputField;

            bool FoldoutInternal;

            void OnEnable()
            {
                Script = serializedObject.FindProperty("m_Script");
                UrlBookmarkPrefab = serializedObject.FindProperty("UrlBookmarkPrefab");
                UrlBookmarksParent = serializedObject.FindProperty("UrlBookmarksParent");
                ReSerialize();
            }

            public override void OnInspectorGUI()
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(Script);
                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginChangeCheck();
                var count = EditorGUILayout.IntField("Count", so.targetObjects.Length);
                if (EditorGUI.EndChangeCheck())
                {
                    var urlBookmarks = GetUrlBookmarks();
                    if (count < urlBookmarks.Length)
                    {
                        for (int i = count; i < urlBookmarks.Length; i++)
                        {
                            DestroyImmediate(urlBookmarks[i].transform.parent.gameObject);
                        }
                    }
                    else if (count > urlBookmarks.Length)
                    {
                        var prefab = (target as UrlBookmarksUI).UrlBookmarkPrefab;
                        var parent = (target as UrlBookmarksUI).UrlBookmarksParent;
                        for (int i = urlBookmarks.Length; i < count; i++)
                        {
                            var obj = PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
                            GameObjectUtility.EnsureUniqueNameForSibling(obj);
                        }
                    }
                    ReSerialize();
                }

                if (so.targetObjects.Length > 0)
                {
                    so.UpdateIfRequiredOrScript();
                    EditorGUILayout.PropertyField(TargetInputField);
                    EditorGUILayout.PropertyField(Target);
                    EditorGUILayout.PropertyField(TargetVariableName);
                    EditorGUILayout.PropertyField(TargetMethodName);
                    so.ApplyModifiedProperties();
                }

                if (FoldoutInternal = EditorGUILayout.Foldout(FoldoutInternal, "internal"))
                {
                    serializedObject.UpdateIfRequiredOrScript();
                    EditorGUILayout.PropertyField(UrlBookmarkPrefab);
                    EditorGUILayout.PropertyField(UrlBookmarksParent);
                    serializedObject.ApplyModifiedProperties();
                }
            }

            void ReSerialize()
            {
                var urlBookmarks = GetUrlBookmarks();
                so = new SerializedObject(urlBookmarks);
                TargetInputField = so.FindProperty("TargetInputField");
                Target = so.FindProperty("Target");
                TargetVariableName = so.FindProperty("TargetVariableName");
                TargetMethodName = so.FindProperty("TargetMethodName");
            }

            UrlBookmark[] GetUrlBookmarks() => (target as UrlBookmarksUI).transform.GetComponentsInChildren<UrlBookmark>();
        }
#endif
    }
}
