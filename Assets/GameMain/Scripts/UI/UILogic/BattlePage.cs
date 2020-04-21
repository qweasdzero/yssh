using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class BattlePageModel : UGuiFormModel<BattlePage, BattlePageModel>
    {
        #region Skill1Property

        private readonly Property<bool> _privateSkill1Property = new Property<bool>();

        public Property<bool> Skill1Property
        {
            get { return _privateSkill1Property; }
        }

        public bool Skill1
        {
            get { return _privateSkill1Property.GetValue(); }
            set { _privateSkill1Property.SetValue(value); }
        }

        #endregion

        #region Skill2Property

        private readonly Property<bool> _privateSkill2Property = new Property<bool>();

        public Property<bool> Skill2Property
        {
            get { return _privateSkill2Property; }
        }

        public bool Skill2
        {
            get { return _privateSkill2Property.GetValue(); }
            set { _privateSkill2Property.SetValue(value); }
        }

        #endregion

        #region Skill3Property

        private readonly Property<bool> _privateSkill3Property = new Property<bool>();

        public Property<bool> Skill3Property
        {
            get { return _privateSkill3Property; }
        }

        public bool Skill3
        {
            get { return _privateSkill3Property.GetValue(); }
            set { _privateSkill3Property.SetValue(value); }
        }

        #endregion

        #region Skill4Property

        private readonly Property<bool> _privateSkill4Property = new Property<bool>();

        public Property<bool> Skill4Property
        {
            get { return _privateSkill4Property; }
        }

        public bool Skill4
        {
            get { return _privateSkill4Property.GetValue(); }
            set { _privateSkill4Property.SetValue(value); }
        }

        #endregion

        #region Skill5Property

        private readonly Property<bool> _privateSkill5Property = new Property<bool>();

        public Property<bool> Skill5Property
        {
            get { return _privateSkill5Property; }
        }

        public bool Skill5
        {
            get { return _privateSkill5Property.GetValue(); }
            set { _privateSkill5Property.SetValue(value); }
        }

        #endregion

        public void UseSkill1()
        {
            Page.UseSkill(1);
        }

        public void UseSkill2()
        {
            Page.UseSkill(2);
        }

        public void UseSkill3()
        {
            Page.UseSkill(3);
        }

        public void UseSkill4()
        {
            Page.UseSkill(4);
        }

        public void UseSkill5()
        {
            Page.UseSkill(5);
        }

        private bool m_GameOver;

        public bool GameOver
        {
            get { return m_GameOver; }
            set { m_GameOver = value; }
        }
    }

    public class BattlePage : UGuiFormPage<BattlePage, BattlePageModel>
    {
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            GameEntry.Event.Subscribe(NextRoundEventArgs.EventId, OnNextRound);
            GameEntry.Event.Subscribe(RoleDieEventArgs.EventId, OnRoleDie);
            GameEntry.Event.Subscribe(GameOverEventArgs.EventId, OnGameOver);
        }

        private void OnGameOver(object sender, GameEventArgs e)
        {
            GameOverEventArgs ne = e as GameOverEventArgs;
            if (ne == null)
            {
                return;
            }

            Model.GameOver = true;
        }

        private void OnNextRound(object sender, GameEventArgs e)
        {
            NextRoundEventArgs ne = e as NextRoundEventArgs;
            if (ne == null)
            {
                return;
            }

            Refresh();
            GameEntry.Skill.Skill1Cd -= 1;
            GameEntry.Skill.Skill2Cd -= 1;
            GameEntry.Skill.Skill3Cd -= 1;
            GameEntry.Skill.Skill4Cd -= 1;
            GameEntry.Skill.Skill5Cd -= 1;
        }

        private void Refresh()
        {
            Model.Skill1 = !GameEntry.Role.MyRole[1].GetImpact().Die &&
                           GameEntry.Skill.Skill1Consume <= GameEntry.Skill.Power && GameEntry.Skill.Skill1Cd <= 0;
            Model.Skill2 = !GameEntry.Role.MyRole[2].GetImpact().Die &&
                           GameEntry.Skill.Skill2Consume <= GameEntry.Skill.Power && GameEntry.Skill.Skill2Cd <= 0;
            Model.Skill3 = !GameEntry.Role.MyRole[3].GetImpact().Die &&
                           GameEntry.Skill.Skill3Consume <= GameEntry.Skill.Power && GameEntry.Skill.Skill3Cd <= 0;
            Model.Skill4 = !GameEntry.Role.MyRole[4].GetImpact().Die &&
                           GameEntry.Skill.Skill4Consume <= GameEntry.Skill.Power && GameEntry.Skill.Skill4Cd <= 0;
            Model.Skill5 = !GameEntry.Role.MyRole[5].GetImpact().Die &&
                           GameEntry.Skill.Skill5Consume <= GameEntry.Skill.Power && GameEntry.Skill.Skill5Cd <= 0;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(NextRoundEventArgs.EventId, OnNextRound);
            GameEntry.Event.Unsubscribe(RoleDieEventArgs.EventId, OnRoleDie);
            GameEntry.Event.Unsubscribe(GameOverEventArgs.EventId, OnGameOver);
        }

        private void OnRoleDie(object sender, GameEventArgs e)
        {
            RoleDieEventArgs ne = e as RoleDieEventArgs;
            if (ne == null)
            {
                return;
            }

            Refresh();
        }

        public void UseSkill(int i)
        {
            if (Model.GameOver)
            {
                return;
            }

            GameEntry.Event.Fire(this, ReferencePool.Acquire<ActiveSkillEventArgs>().Fill(i, CampType.Player));
            switch (i)
            {
                case 1:
                    GameEntry.Skill.Power -= GameEntry.Skill.Skill1Consume;
                    GameEntry.Skill.Skill1Cd = GameEntry.Skill.Skill1CdMax;
                    break;
                case 2:
                    GameEntry.Skill.Power -= GameEntry.Skill.Skill2Consume;
                    GameEntry.Skill.Skill2Cd = GameEntry.Skill.Skill2CdMax;
                    break;
                case 3:
                    GameEntry.Skill.Power -= GameEntry.Skill.Skill3Consume;
                    GameEntry.Skill.Skill3Cd = GameEntry.Skill.Skill3CdMax;
                    break;
                case 4:
                    GameEntry.Skill.Power -= GameEntry.Skill.Skill4Consume;
                    GameEntry.Skill.Skill4Cd = GameEntry.Skill.Skill4CdMax;
                    break;
                case 5:
                    GameEntry.Skill.Power -= GameEntry.Skill.Skill5Consume;
                    GameEntry.Skill.Skill5Cd = GameEntry.Skill.Skill5CdMax;
                    break;
            }

            Refresh();
        }
    }
}