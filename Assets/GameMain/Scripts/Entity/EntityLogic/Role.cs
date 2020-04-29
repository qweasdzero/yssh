using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using SG1;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class Role : Entity
    {
        private RoleData m_RoleData;
        private Animator m_Anim;
        private static readonly int Atk = Animator.StringToHash("Atk");
        private int target;
        private HpBar m_HpBar;
        private int m_HpId;
        private SpriteRenderer m_Sprite;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_Anim = GetComponent<Animator>();
            m_Sprite = GetComponent<SpriteRenderer>();
        }

        private RoleImpactData m_ImpactData;
        private static readonly int Skill = Animator.StringToHash("Skill");

        public RoleImpactData GetImpact()
        {
            return m_ImpactData;
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_RoleData = userData as RoleData;
            if (m_RoleData == null)
            {
                return;
            }

            m_ImpactData = new RoleImpactData(m_RoleData);
            if (m_RoleData.Camp == CampType.Player)
            {
                m_RoleData.Position = RoleUtility.MyRolePos[m_RoleData.Seat];
                m_Anim.CrossFade("Idle_B", 0);
            }

            if (m_RoleData.Camp == CampType.Enemy)
            {
                m_RoleData.Position = RoleUtility.EnemyRolePos[m_RoleData.Seat];
                m_Anim.CrossFade("Idle_P", 0);
            }

            CachedTransform.localPosition = m_RoleData.Position;
            m_HpId = GameEntry.Entity.GenerateSerialId();
            GameEntry.Entity.ShowHpBar(new HpBarData(m_HpId, 2000, this));

            GameEntry.Event.Subscribe(AtkEventArgs.EventId, OnAtk);
            GameEntry.Event.Subscribe(HurtEventArgs.EventId, OnAtkEnd);
            GameEntry.Event.Subscribe(ExertBuffEventArgs.EventId, OnExertBuff);
            GameEntry.Event.Subscribe(SkillEventArgs.EventId, OnSkill);
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowHpBar);
        }

        private void OnShowHpBar(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = e as ShowEntitySuccessEventArgs;
            if (ne == null)
            {
                return;
            }

            if (m_HpId == ne.Entity.Id)
            {
                m_HpBar = ne.Entity.Logic as HpBar;
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            GameEntry.Event.Unsubscribe(AtkEventArgs.EventId, OnAtk);
            GameEntry.Event.Unsubscribe(HurtEventArgs.EventId, OnAtkEnd);
            GameEntry.Event.Unsubscribe(ExertBuffEventArgs.EventId, OnExertBuff);
            GameEntry.Event.Unsubscribe(SkillEventArgs.EventId, OnSkill);
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowHpBar);
        }

        private void OnSkill(object sender, GameEventArgs e)
        {
            SkillEventArgs ne = e as SkillEventArgs;
            if (ne == null)
            {
                return;
            }

            if (ne.CampType == m_RoleData.Camp && ne.Seat == m_RoleData.Seat)
            {
                m_Anim.SetTrigger(Skill);
            }
        }


        private void OnAtkEnd(object sender, GameEventArgs e)
        {
            HurtEventArgs ne = e as HurtEventArgs;
            if (ne == null)
            {
                return;
            }

            if (ne.CampType == m_RoleData.Camp && ne.Seat.Contains(m_RoleData.Seat))
            {
                Log.Info("Seat:" + m_RoleData.Seat + "__Camp:" + m_RoleData.Camp + "__Hurt:" + ne.Hurt);
                m_RoleData.Hp -= ne.Hurt;
                if (m_RoleData.Hp > m_RoleData.HpMax)
                {
                    m_RoleData.Hp = m_RoleData.HpMax;
                }

                m_HpBar.ChangeHp(m_RoleData.Hp);
                if (m_RoleData.Hp <= 0)
                {
                    m_Sprite.enabled = false;
                    m_RoleData.Die = true;
                    GameEntry.Event.Fire(this,
                        ReferencePool.Acquire<RoleDieEventArgs>().Fill(m_RoleData.Seat, m_RoleData.Camp));
                }
            }
        } 
        
        private void OnExertBuff(object sender, GameEventArgs e)
        {
            ExertBuffEventArgs ne = e as ExertBuffEventArgs;
            if (ne == null)
            {
                return;
            }

            if (ne.CampType == m_RoleData.Camp && ne.Seat.Contains(m_RoleData.Seat))
            {
                m_RoleData.BuffState.Add(new BuffState(ne.Buff,ne.BuffTime,ne.BuffValue));
            }
        }

        private void OnAtk(object sender, GameEventArgs e)
        {
            AtkEventArgs ne = e as AtkEventArgs;
            if (ne == null)
            {
                return;
            }

            if (ne.CampType == m_RoleData.Camp && ne.Seat == m_RoleData.Seat)
            {
                m_Anim.SetTrigger(Atk);
                target = ne.EnemySeat;
            }
        }

        public void AtkEnd()
        {
            if (m_RoleData.Power >= 100)
            {
                m_RoleData.Power = 0;
                m_HpBar.ChangePower(m_RoleData.Power);
                GameEntry.Event.Fire(this,
                    ReferencePool.Acquire<AtkEndEventArgs>().Fill(m_RoleData.Seat, m_RoleData.Camp, 0));
            }
            else
            {
                m_RoleData.Power += 50;
                m_HpBar.ChangePower(m_RoleData.Power);
                GameEntry.Event.Fire(this,
                    ReferencePool.Acquire<AtkEndEventArgs>()
                        .Fill(m_RoleData.Seat, m_RoleData.Camp, m_RoleData.SkillId));
            }
        }

        public void SkillEnd()
        {
            GameEntry.Event.Fire(this,
                ReferencePool.Acquire<AtkEndEventArgs>().Fill(m_RoleData.Seat, m_RoleData.Camp, m_RoleData.PowerId));
        }
    }
}