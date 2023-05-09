using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Maze : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [SerializeField] Image background;
    [SerializeField] float rotationSpeed = 0.2f;
    [SerializeField] Rigidbody2D wallCollider;
    [SerializeField] Transform maze;

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
        oldEulerZ = wallCollider.rotation;
        oldAngle = Mathf.Atan2(eventData.position.y - screenPos.y, eventData.position.x - screenPos.x) * Mathf.Rad2Deg;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            float angle = Mathf.Atan2(eventData.position.y - screenPos.y, eventData.position.x - screenPos.x) * Mathf.Rad2Deg;
            float angleDiff = (angle - oldAngle) % 360;
            if (angleDiff > 180)
                angleDiff -= 360;
            else if (angleDiff < -180)
                angleDiff += 360;

            float eulerZ = oldEulerZ + angleDiff * rotationSpeed;

            wallCollider.MoveRotation(eulerZ);
            maze.localEulerAngles = new Vector3(0, 0, eulerZ);
            oldAngle = angle;
            oldEulerZ = eulerZ;
        }
    }
}
