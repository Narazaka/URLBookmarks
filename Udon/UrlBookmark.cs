using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

namespace Narazaka.VRChat.UrlBookmarks
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class UrlBookmark : UdonSharpBehaviour
    {
        [SerializeField] VRCUrlInputField InputField;
        [Header("Target (optional)")]
        [SerializeField] VRCUrlInputField TargetInputField;
        [SerializeField] UdonBehaviour Target;
        [SerializeField] string TargetVariableName;
        [SerializeField] string TargetMethodName;

        [UdonSynced, FieldChangeCallback(nameof(Url))] VRCUrl _Url;
        public VRCUrl Url
        {
            get => _Url;
            set
            {
                if (_Url == value) return;
                _Url = value;
                InputField.SetUrl(value);
            }
        }

        public VRCUrl NonNullUrl => Url != null ? Url : VRCUrl.Empty;

        public void OnUrlChange()
        {
            Url = InputField.GetUrl();
            RequestSerialization();
        }

        public void OnUseUrl()
        {
            if (TargetInputField != null)
            {
                TargetInputField.SetUrl(NonNullUrl);
            }
            if (Target != null)
            {
                if (!string.IsNullOrEmpty(TargetVariableName))
                {
                    Target.SetProgramVariable(TargetVariableName, NonNullUrl);
                }
                if (!string.IsNullOrEmpty(TargetMethodName))
                {
                    Target.SendCustomEvent(TargetMethodName);
                }
            }
        }
    }
}
