using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

#pragma warning disable 0649
namespace StarForce
{
    [RequireComponent(typeof(Text))]
    [DisallowMultipleComponent]
    public class ActiveTextBinding : BooleanBinding
    {
        [SerializeField] private string m_ActiveText;
        [SerializeField] private string m_DeActiveText;
        [SerializeField, InspectorReadOnly] private Text m_Text;

        protected override void ApplyNewValue(bool newValue)
        {
            m_Text.text = newValue ? m_ActiveText : m_DeActiveText;
        }


        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            if (m_Text == null)
            {
                m_Text = GetComponent<Text>();
            }
        }
    }
}