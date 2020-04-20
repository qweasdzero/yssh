using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    [RequireComponent(typeof(Graphic))]
    [DisallowMultipleComponent]
    public class UGUIAlphaBinding : NumericBinding
    {
        [InspectorReadOnly] [SerializeField] private Graphic m_Graphic;

        public Graphic Graphic
        {
            get { return m_Graphic; }
            set { m_Graphic = value; }
        }

        protected override void ApplyNewValue(double value)
        {
            Color c = m_Graphic.color;
            c.a = Mathf.Clamp01((float) value);
            m_Graphic.color = c;
        }

        protected override bool Bind()
        {
            SetNumericValue(Property, m_Graphic.color.a);
            return base.Bind();
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