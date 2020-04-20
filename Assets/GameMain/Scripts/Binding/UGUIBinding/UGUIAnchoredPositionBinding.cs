using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class UGUIAnchoredPositionBinding : VectorBinding
    {
        [SerializeField, InspectorReadOnly] private RectTransform m_RectTransform;

        protected override void ApplyNewValue(Vector3 newValue)
        {
            m_RectTransform.localPosition = newValue;
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