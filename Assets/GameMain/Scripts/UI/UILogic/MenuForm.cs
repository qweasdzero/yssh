//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class MenuFormModel : UGuiFormModel<MenuForm,MenuFormModel>
    {
        #region ButtonProperty

        private readonly Property<bool> _privateButtonProperty = new Property<bool>();

        public Property<bool> ButtonProperty
        {
            get { return _privateButtonProperty; }
        }

        public bool Button
        {
            get { return _privateButtonProperty.GetValue(); }
            set { _privateButtonProperty.SetValue(value); }
        }

        #endregion

        #region GameOverProperty

        private readonly Property<bool> _privateGameOverProperty = new Property<bool>();

        public Property<bool> GameOverProperty
        {
            get { return _privateGameOverProperty; }
        }

        public bool GameOver
        {
            get { return _privateGameOverProperty.GetValue(); }
            set { _privateGameOverProperty.SetValue(value); }
        }

        #endregion
        
        public void StartGame()
        {
            GameEntry.Event.Fire(this,ReferencePool.Acquire<StartGameEventArgs>().Fill());
            GameEntry.UI.OpenUIForm(UIFormId.BattlePage);
            Button = false;
        }
    }

    public class MenuForm : UGuiFormPage<MenuForm,MenuFormModel>
    {
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Model.Button = true;
            GameEntry.Event.Subscribe(GameOverEventArgs.EventId,OnGameOver);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            GameEntry.Event.Unsubscribe(GameOverEventArgs.EventId,OnGameOver);
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
        
        
    }
}
