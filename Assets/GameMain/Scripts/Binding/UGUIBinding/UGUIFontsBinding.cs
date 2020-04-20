using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace StarForce
{
    [RequireComponent(typeof(Font))]
    [DisallowMultipleComponent]
    public class UGUIFontsBinding : TextLoadAssetBinding
    {
        [SerializeField, InspectorReadOnly] private Text m_Text;

        public Text Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            if (m_Text == null)
            {
                m_Text = GetComponent<Text>();
            }
        }

        protected override void ApplyNewValue(string newValue)
        {
            if (!string.IsNullOrEmpty(newValue))
            {
                GameEntry.Resource.LoadAsset(newValue, typeof(Font), LoadAssetCallbacks);
            }
        }

        protected override void OnLoadAssetSuccess(string assetname, object asset, float duration, object userdata)
        {
            Font font = asset as Font;
            if (font != null && m_Text != null)
            {
                m_Text.font = font;
            }
        }
    }
}