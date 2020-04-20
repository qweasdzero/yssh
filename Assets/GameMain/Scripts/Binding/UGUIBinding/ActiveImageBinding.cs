using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
namespace StarForce
{
    [RequireComponent(typeof(Image))]
    [DisallowMultipleComponent]
    public class ActiveImageBinding : BooleanLoadAssetBinding
    {
        [SerializeField] private string m_ActivePath;
        [SerializeField] private string m_DeActivePath;
        [SerializeField, InspectorReadOnly] private Image m_Image;

        protected override void ApplyNewValue(bool newValue)
        {
            if (!string.IsNullOrEmpty(newValue ? m_ActivePath : m_DeActivePath))
            {
                GameEntry.Resource.LoadAsset(newValue ? m_ActivePath : m_DeActivePath, typeof(Sprite),
                    LoadAssetCallbacks);
            }
        }

        protected override void OnLoadAssetSuccess(string assetname, object asset, float duration, object userdata)
        {
            Sprite sprite = asset as Sprite;
            if (sprite != null && m_Image != null)
            {
                m_Image.sprite = sprite;
            }
        }

        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            if (m_Image == null)
            {
                m_Image = GetComponent<Image>();
            }
        }
    }
}