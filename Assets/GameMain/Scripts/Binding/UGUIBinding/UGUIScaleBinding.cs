using UnityEngine;
using UnityEngine.UI;

namespace StarForce
{
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class UGUIScaleBinding : VectorBinding
    {
        [SerializeField, InspectorReadOnly] private RectTransform m_RectTransform;

        protected override void ApplyNewValue(Vector3 newValue)
        {
            m_RectTransform.localScale = newValue;
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