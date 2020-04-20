using GameFramework;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public abstract class TextLoadAssetBinding : TextBinding
    {
        [SerializeField] private LoadAssetCallbacks m_LoadAssetCallbacks;

        public LoadAssetCallbacks LoadAssetCallbacks
        {
            get { return m_LoadAssetCallbacks; }
            protected set { m_LoadAssetCallbacks = value; }
        }

//        protected override void Awake()
//        {
//            
//        }

        protected override bool Bind()
        {
            m_LoadAssetCallbacks = new LoadAssetCallbacks(OnLoadAssetSuccess, OnLoadAssetFailure);
            return base.Bind();
        }

        protected abstract void OnLoadAssetSuccess(string assetname, object asset, float duration, object userdata);

        protected virtual void OnLoadAssetFailure(string assetname, LoadResourceStatus status, string errormessage,
            object userdata)
        {
            Log.Error(Utility.Text.Format("Error in '{0}',LoadresoureceStatus:{1},Error Message:{2}", assetname,
                status, errormessage));
        }
    }
}