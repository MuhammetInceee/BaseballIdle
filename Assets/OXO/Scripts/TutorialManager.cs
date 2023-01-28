using Sirenix.OdinInspector;
using MuhammetInce.DesignPattern.Singleton;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public Transform cursor;
    private Vector3 cursorOffset;
    public float arrowRadius;

    public GameObject ballMachine;
    public GameObject twinPlayer;
    public GameObject player;


    public Vector3 PlayerTransform => player.transform.position;

    private void Awake()
    {
        cursorOffset = cursor.transform.position - PlayerTransform;
        cursor.transform.parent = null;
    }
    [Button]
    public void SetCursorRadius()
    {
        cursor.transform.position = PlayerTransform+ cursor.transform.forward * arrowRadius + cursorOffset;
    }
    [Button]
    public void ResetArrowPos()
    {
        cursor.transform.position = PlayerTransform;
    }
}
