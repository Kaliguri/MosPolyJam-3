using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        Vector3 mousePosition = Input.mousePosition;

        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        mousePosition.z = 0;

        transform.position = mousePosition;
    }
}
