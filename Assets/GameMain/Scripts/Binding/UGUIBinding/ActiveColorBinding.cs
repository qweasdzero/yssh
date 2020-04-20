using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
namespace StarForce
{
    [RequireComponent(typeof(Graphic))]
    [DisallowMultipleComponent]
    public class ActiveColorBinding : BooleanBinding
    {
        [SerializeField] private Color m_ActiveColor;
        [SerializeField] private Color m_DeActiveColor;
        [SerializeField, InspectorReadOnly] private Graphic m_Graphic;

        protected override void ApplyNewValue(bool newValue)
        {
            m_Graphic.color = newValue ? m_ActiveColor : m_DeActiveColor;
        }
        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            if (m_Graphic == null)
            {
                m_Graphic = GetComponent<Graphic>();
            }
        }
    }
}