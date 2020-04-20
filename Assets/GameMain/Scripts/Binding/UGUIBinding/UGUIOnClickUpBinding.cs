using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    [RequireComponent(typeof(Button))]
    public class UGUIOnClickUpBinding : ActionBinding, IPointerUpHandler
    {
        private float m_LastClickTime;

        [SerializeField] private bool m_Block;

        [SerializeField, InspectorReadOnly] private Button m_Button;

        [SerializeField] private float m_ClickInterval = 0.5f;

        public bool Block
        {
            get { return m_Block; }
            set { m_Block = value; }
        }

        public Button Button
        {
            get { return m_Button; }
            set { m_Button = value; }
        }

        protected override bool Bind()
        {
            if (base.Bind())
            {
                return true;
            }

            return false;
        }

        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            if (m_Button == null)
            {
                m_Button = GetComponent<Button>();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (m_Action != null && !Block && m_Button.interactable)
            {
                var interval = Time.realtimeSinceStartup - m_LastClickTime;
                if (interval < m_ClickInterval) return;
                m_LastClickTime = Time.realtimeSinceStartup;
                m_Action.Invoke();
            }
        }
    }
}