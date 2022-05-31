using UnityEngine;


public class PlayerMovementController : MonoBehaviour
{
    public GameObject PlayerHolder;

    private float? lastMousePoint = null;
    public float RestirictionX = 3.5f;
    private Vector3 oldPos;
    private Quaternion oldRot;
    private bool mouseControl;
    private PlayerManager _playerManager;

    private void Awake()
    {
        _playerManager = PlayerManager.Instance;
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * _playerManager.PlayerSpeed);

        if (!mouseControl)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastMousePoint = Input.mousePosition.x;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                lastMousePoint = null;
            }
            if (lastMousePoint != null)
            {
                float difference = Input.mousePosition.x - lastMousePoint.Value;
                PlayerHolder.transform.localPosition = new Vector3(PlayerHolder.transform.localPosition.x + (difference / 188) * Time.deltaTime * _playerManager.PlayerSpeed, PlayerHolder.transform.localPosition.y, PlayerHolder.transform.localPosition.z);
                lastMousePoint = Input.mousePosition.x;
            }

            float xPos = Mathf.Clamp(PlayerHolder.transform.localPosition.x, -RestirictionX, RestirictionX);
            PlayerHolder.transform.localPosition = new Vector3(xPos, PlayerHolder.transform.localPosition.y, PlayerHolder.transform.localPosition.z);
            Vector3 movement = oldRot * (PlayerHolder.transform.localPosition - oldPos);
        }
    }

}
