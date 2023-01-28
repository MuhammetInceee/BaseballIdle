using MuhammetInce.DesignPattern.Singleton;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private UpgradableAttributeSettings playerSettings;
    
    [Header("About Player Controller:"), Space]
    [SerializeField] private FloatingJoystick joystick;
    public GameObject camTr;
    public GameObject jsTr;

    private void Awake() => AwakeInit();
    private void FixedUpdate() => FixedUpdateInit();
    private void Update()
    {
        camTr.transform.position = transform.position;
        if (GameManager.instance.isMatchScreen)
        {
            joystick.gameObject.SetActive(false);
        }
        else
        {
            joystick.gameObject.SetActive(true);
        }
    }
    private void AwakeInit()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdateInit()
    {
        MovementPlayer();
    }
    
    private void MovementPlayer()
    {
        if (joystick.Direction.magnitude > 0.05f)
        {
            Vector3 direction = jsTr.transform.forward * joystick.Vertical + jsTr.transform.right * joystick.Horizontal;
           
            CalculateSpeed(direction);

            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
                playerAnimator.SetBool(IsRunning, true);
                transform.rotation = Quaternion.LookRotation(_rb.velocity);
            }
        }
        else
        {
            playerAnimator.ResetTrigger(IsRunning);
            _rb.velocity = Vector3.zero;
        }

    }

    private void CalculateSpeed(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0)
        {
            if(!PlayerPrefs.HasKey("PlayerMovementSpeed"))
                _rb.velocity = direction * playerSettings.playerMovementSpeedList[0];
            else
            {
                _rb.velocity = PlayerPrefs.GetFloat("PlayerMovementSpeed") switch
                {
                    0 => direction * playerSettings.playerMovementSpeedList[0],
                    1 => direction * playerSettings.playerMovementSpeedList[1],
                    2 => direction * playerSettings.playerMovementSpeedList[2],
                    3 => direction * playerSettings.playerMovementSpeedList[3],
                    4 => direction * playerSettings.playerMovementSpeedList[4],
                    _ => _rb.velocity
                };
            }
        }
    }
    
}
