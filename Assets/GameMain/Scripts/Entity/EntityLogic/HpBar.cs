using StarForce;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public sealed class HpBar : Entity
    {
        [SerializeField] private HpBarData m_hpBarData = null;
        private bool m_Hide = false;
        private const float m_Y = 1f;
        [SerializeField] private Slider m_HpSlider = null;
        [SerializeField] private Slider m_PowSlider = null;

        private RoleImpactData m_Data;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_HpSlider = transform.Find("HpSlider").GetComponent<Slider>();
            m_PowSlider = transform.Find("PowerSlider").GetComponent<Slider>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_hpBarData = userData as HpBarData;
            if (m_hpBarData == null)
            {
                Log.Error("My player arrow data is invalid.");
                return;
            }

            m_Data = m_hpBarData.Father.GetImpact();
            m_hpBarData.Hp = m_Data.Hp;
            m_hpBarData.HpMax = m_Data.HpMax;
            m_hpBarData.Power = 0;
            m_hpBarData.Position = m_Data.Position + new Vector3(0, m_Y, 0);
            CachedTransform.position = m_hpBarData.Position;
            m_Hide = false;
            Refresh();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (!m_hpBarData.Father.Available)
            {
                Hide();
                return;
            }
        }

        private void Hide()
        {
            if (!m_Hide)
            {
                GameEntry.Entity.HideEntity(this);
                m_Hide = true;
            }
        }

        public void ChangePower(int power)
        {
            m_hpBarData.Power = power;
            Refresh();
        }

        public void ChangeHp(int hp)
        {
            m_hpBarData.Hp = hp;
            Refresh();
        }

        private void Refresh()
        {
            m_HpSlider.value = (float) m_hpBarData.Hp / m_hpBarData.HpMax;
            m_PowSlider.value = (float) m_hpBarData.Power / RoleUtility.PowerMax;
            if (m_HpSlider.value <= 0)
            {
                Hide();
            }
        }
    }
}