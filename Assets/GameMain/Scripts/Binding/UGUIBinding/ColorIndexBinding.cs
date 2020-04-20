using UnityEngine;
using UnityEngine.UI;

namespace StarForce
{
    [RequireComponent(typeof(Graphic))]
    [DisallowMultipleComponent]
    public class ColorIndexBinding : IndexBinding<Color>
    {
        [SerializeField, InspectorReadOnly] private Graphic m_Graphic;

        public Graphic Graphic
        {
            get { return m_Graphic; }
            set { m_Graphic = value; }
        }

        protected override void ApplyNewValue(Color value)
        {
            Graphic.color = value;
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