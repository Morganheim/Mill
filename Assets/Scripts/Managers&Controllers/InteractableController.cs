using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableController : EventTrigger
{
    [SerializeField] private InteractableData _data;

    private void Execute(EventTriggerType id, BaseEventData eventData)
    {
        var triggerCount = triggers.Count;

        for (int i = 0, imax = triggers.Count; i < imax; ++i)
        {
            var ent = triggers[i];
            if (ent.eventID == id && ent.callback != null)
                ent.callback.Invoke(eventData);
        }
    }

    private bool IsTriggerable(PointerEventData eventData)
    {
        if (eventData.pointerPressRaycast.distance > _data.Distance && _data.Distance > 0)
            return false;

        return _data.AllowedMouseKeys.Contains(eventData.button);
    }

    /// <summary>
    /// Called by the EventSystem when the pointer enters the object associated with this EventTrigger.
    /// </summary>
    public override void OnPointerEnter(PointerEventData eventData)
    {
        Execute(EventTriggerType.PointerEnter, eventData);
    }

    /// <summary>
    /// Called by the EventSystem when the pointer exits the object associated with this EventTrigger.
    /// </summary>
    public override void OnPointerExit(PointerEventData eventData)
    {
        Execute(EventTriggerType.PointerExit, eventData);
    }

    /// <summary>
    /// Called by the EventSystem when a PointerDown event occurs.
    /// </summary>
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!IsTriggerable(eventData))
            return;

        Execute(EventTriggerType.PointerDown, eventData);
    }

    /// <summary>
    /// Called by the EventSystem when a PointerUp event occurs.
    /// </summary>
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!IsTriggerable(eventData))
            return;

        Execute(EventTriggerType.PointerUp, eventData);
    }

    /// <summary>
    /// Called by the EventSystem when a Click event occurs.
    /// </summary>
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!IsTriggerable(eventData))
            return;

        Execute(EventTriggerType.PointerClick, eventData);
    }

    /// <summary>
    /// Called before a drag is started.
    /// </summary>
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsTriggerable(eventData))
            return;

        Execute(EventTriggerType.BeginDrag, eventData);
    }

    /// <summary>
    /// Called by the EventSystem every time the pointer is moved during dragging.
    /// </summary>
    public override void OnDrag(PointerEventData eventData)
    {
        if (!IsTriggerable(eventData))
            return;

        Execute(EventTriggerType.Drag, eventData);
    }

    /// <summary>
    /// Called by the EventSystem once dragging ends.
    /// </summary>
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!IsTriggerable(eventData))
            return;

        Execute(EventTriggerType.EndDrag, eventData);
    }

    /// <summary>
    /// Called by the EventSystem when a new Scroll event occurs.
    /// </summary>
    public override void OnScroll(PointerEventData eventData)
    {
        Execute(EventTriggerType.Scroll, eventData);
    }
}