using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;
using UnityEngine;

public class PointerEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [System.Serializable]
    public enum eState
    {
        Up,
        Down
    }

    [System.Serializable]
    public struct EventInfo
    {
        public PointerEventData.InputButton button;
        public eState state;
        public UnityEvent UEvent;
    }

    public EventInfo[] eventInfos;

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (EventInfo EI in eventInfos)
        {
            if (eventData.button == EI.button && EI.state == eState.Up)
            {
                EI.UEvent.Invoke();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach(EventInfo EI in eventInfos)
        {
            if(eventData.button == EI.button && EI.state == eState.Down)
            {
                EI.UEvent.Invoke();
            }
        }
    }
}
