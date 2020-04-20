using UnityEngine;
using UnityEngine.UI;

namespace StarForce
{
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class UGUIPositionBinding : VectorBinding
    {
        [SerializeField, InspectorReadOnly] private RectTransform m_RectTransform;

        protected override void ApplyNewValue(Vector3 newValue)
        {
            m_RectTransform.position = newValue;
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