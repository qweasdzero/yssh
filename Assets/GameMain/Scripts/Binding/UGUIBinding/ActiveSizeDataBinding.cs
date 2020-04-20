using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

#pragma warning disable 0649
namespace StarForce
{
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class ActiveSizeDataBinding : BooleanBinding
    {
        [SerializeField] private Vector2 m_ActiveSize;
        [SerializeField] private Vector2 m_DeActiveSize;
        [SerializeField, InspectorReadOnly] private RectTransform m_RectTransform;

        protected override void ApplyNewValue(bool newValue)
        {
            m_RectTransform.sizeDelta = newValue?m_ActiveSize:m_DeActiveSize;
        }


        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            if (m_RectTransform == null)
            {
                m_RectTransform = GetComponent<RectTransform>();
            }
        }
    }
}