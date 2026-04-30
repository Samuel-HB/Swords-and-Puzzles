using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    Vector2 offset;

    public void OnDrag(PointerEventData eventData)
    {
        offset = transform.position - (Vector3)eventData.position;
        transform.SetAsLastSibling();
        //offset = transform.position - Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.position = (Vector3)eventData.position + (Vector3)offset;
        //transform.position = Camera.main.ScreenToWorldPoint(eventData.position) + (Vector3)offset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
