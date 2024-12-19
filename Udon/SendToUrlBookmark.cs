using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Narazaka.VRChat.UrlBookmarks
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class SendToUrlBookmark : UdonSharpBehaviour
    {
        UrlBookmark UrlBookmark;

        void Start()
        {
            var components = GetComponentsInChildren<UrlBookmark>();
            foreach (var component in components)
            {
                if (Networking.IsOwner(component.gameObject))
                {
                    UrlBookmark = component;
                    break;
                }
            }
        }

        public void OnUrlChange()
        {
            UrlBookmark.OnUrlChange();
        }

        public void OnUseUrl()
        {
            UrlBookmark.OnUseUrl();
        }
    }
}
