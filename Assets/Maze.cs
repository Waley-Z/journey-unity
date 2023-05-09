using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Maze : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [SerializeField] Image background;
    [SerializeField] float rotationSpeed = 0.2f;

    float oldAngle;
    float oldEulerZ;
    Vector2 screenPos;

    void Start()
    {
        background.alphaHitTestMinimumThreshold = 0.5f;
        screenPos = Camera.main.WorldToScreenPoint(transform.position);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        oldEulerZ = transform.localRotation.eulerAngles.z;
        oldAngle = Mathf.Atan2(eventData.position.y - screenPos.y, eventData.position.x - screenPos.x) * Mathf.Rad2Deg;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnMouseDrag");

        if (eventData.dragging)
        {
            float angle = Mathf.Atan2(eventData.position.y - screenPos.y, eventData.position.x - screenPos.x) * Mathf.Rad2Deg;
            float angleDiff = (angle - oldAngle) % 360;
            if (angleDiff > 180)
                angleDiff -= 360;
            else if (angleDiff < -180)
                angleDiff += 360;

            print($"{angle}, {oldAngle}, {(angle - oldAngle) % 360}, {angleDiff}");

            float eulerZ = oldEulerZ + angleDiff * rotationSpeed;

            transform.localRotation = Quaternion.Euler(0, 0, eulerZ);
            oldAngle = angle;
            oldEulerZ = eulerZ;
        }
    }
}
