using GameFramework.Resource;
using StarForce;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public sealed class Effect : Entity
    {
        [SerializeField] private EffectData m_EffectData = null;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_EffectData = userData as EffectData;
            if (m_EffectData == null)
            {
                Log.Error("My player arrow data is invalid.");
                return;
            }

            m_EffectData.Scale = Vector3.one * 0.4f;
            CachedTransform.localScale = m_EffectData.Scale;
            GameEntry.Entity.AttachEntity(Id, m_EffectData.Father);
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
            CachedTransform.localPosition = Vector3.zero;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        public void Delect()
        {
            GameEntry.Entity.HideEntity(this);
        }
    }
}