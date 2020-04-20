using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
namespace StarForce
{
    [RequireComponent(typeof(Graphic))]
    [DisallowMultipleComponent]
    public class ActivePositionBinding : BooleanBinding
    {
        [SerializeField] private Vector3 m_ActivePos;
        [SerializeField] private Vector3 m_DeActivePos;
        [SerializeField, InspectorReadOnly] private RectTransform m_Transform;

        protected override void ApplyNewValue(bool newValue)
        {
            m_Transform.anchoredPosition = newValue ? m_ActivePos : m_DeActivePos;
        }

        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            if (m_Transform == null)
            {
                m_Transform = GetComponent<RectTransform>();
            }
        }
    }
}