using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEngine.EventSystems;

//改写过的Input，为GameItem
//在移动端，可拖拽ScrollView，且不会触发Input的软键盘
//在update中监听软件盘回车事件或者PC上Tab键
public class SuperInputField : InputField
{
//Input所在的scrollrect，此处楼主用的是静态变量，你也可以直接用一个public变量单独对它进行赋值
    private ScrollRect scrollRect
    {
        get
        {
            return transform.GetComponentInParent<ScrollRect>();
        }
    }    

    bool isOnDrag = false;

//移动端处理点击事件
//由于PC上无软键盘，无需延迟处理
#if (UNITY_IOS || UNITY_ANDROID)
    public override void OnPointerDown(PointerEventData eventData)
    {
        Invoke("onPointerDown", 0.1f);
    }

    void onPointerDown()
    {
        if (isOnDrag)
        {
            isOnDrag = false;
            return;
        }

        base.OnPointerDown(new PointerEventData(EventSystem.current));
    }
#endif

//监听ScrollRect的拖拽
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
#if (UNITY_IOS || UNITY_ANDROID)
        isOnDrag = true;
//如果由于用户手指停留时间过长，已经触发了软键盘，但拖拽后可隐藏软键盘
        EventSystem.current.SetSelectedGameObject(null);
#endif
        scrollRect.OnBeginDrag(eventData);
    }


    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        scrollRect.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        scrollRect.OnEndDrag(eventData);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
    }
}