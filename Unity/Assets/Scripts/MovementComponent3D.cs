using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MovementKey
{
    UP = 0,
    Left = 1,
    Down = 2,
    Right = 3,
    Jump = 4,
    StandUp = 5
}

enum MovementState
{
    Stand = 0,
    Walk = 1,
    Run = 2,
    Jump = 3,
    Down = 4
}
public class MovementComponent3D : MonoBehaviour
{
    [Tooltip("This notation is meter per second")]
    public float movementSpeed = 1;
    public float currentSpeed = 0.0f;
    [Tooltip("This notion is newton x mass")]
    public float jumpPower = 200;
    [Tooltip("This notation is meter")]
    public float jumpableDistanceFromGround = 1f;
    public int jumpCombo_Max = 2;
    [Tooltip("Gameobject is stand up when pressed any key")]
    public bool isAutoStandup = true;

    private MovementState currentState;
    private int countOfinputMovementKey=0;


    int jumpCombo_Current = 0;
    void Update()
    {
        Vector3 startPos = transform.position;

        countOfinputMovementKey = 0;
        if (Input.anyKeyDown)
        {
            if (IsInputThat(MovementKey.Jump)) Jump();
            if (IsInputThat(MovementKey.StandUp)) StandUp();
        }
        else if (Input.anyKey)
        {
            if (IsInputThat(MovementKey.UP)) transform.transform.position += transform.forward * movementSpeed * 0.001f;
            if (IsInputThat(MovementKey.Left)) transform.transform.position -= transform.right * movementSpeed * 0.001f;
            if (IsInputThat(MovementKey.Down)) transform.transform.position -= transform.forward *  movementSpeed * 0.001f;
            if (IsInputThat(MovementKey.Right)) transform.transform.position += transform.right * movementSpeed * 0.001f;
        }

        bool isAutoStandupAble = countOfinputMovementKey > 0 && isAutoStandup;
        if (isAutoStandupAble) StandUp();

        if (currentSpeed!=0) currentSpeed = Vector3.Distance(startPos, transform.position) * 1000;
    }

    bool IsInputThat(MovementKey movementKey)
    {
        countOfinputMovementKey++;
        if (movementKey == MovementKey.UP && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.W - 32) || Input.GetKey(KeyCode.UpArrow))) return true;
        if (movementKey == MovementKey.Left && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.A - 32) || Input.GetKey(KeyCode.LeftArrow))) return true;
        if (movementKey == MovementKey.Down && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.S - 32) || Input.GetKey(KeyCode.DownArrow))) return true;
        if (movementKey == MovementKey.Right && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.D - 32) || Input.GetKey(KeyCode.RightArrow))) return true;
        if (movementKey == MovementKey.Jump && Input.GetKeyDown(KeyCode.Space)) return true;
        if (movementKey == MovementKey.StandUp && Input.GetKeyDown(KeyCode.LeftControl)) return true;
        countOfinputMovementKey--;

        return false;
    }

    void Jump()
    {
        Collider boxCollider = GetComponent<Collider>();
        bool isGotBox = boxCollider != null;

        if (isGotBox)
        {
            float sclaeZ = transform.localScale.z;
            Vector3 footLocation = transform.position - new Vector3(.0f, GetComponent<BoxCollider>().size.y * 0.5f * sclaeZ - 0.05f, .0f);
            RaycastHit[] hits = Physics.RaycastAll(footLocation, transform.up * -1, jumpableDistanceFromGround);
            bool isOnGround = Physics.RaycastAll(footLocation, transform.up * -1, 0.06f).Length > 0;
            bool isJumpable = hits.Length > 0 && jumpCombo_Current < jumpCombo_Max;

            if (isOnGround) jumpCombo_Current = 0;
            if (isJumpable)
            {
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower, ForceMode.Acceleration);
                jumpCombo_Current++;
            }
        } else
        {
            Debug.Log("Please add component 'Collider'. Recommend to BoxCollider");
        }
    }

    void StandUp()
    {
        transform.localRotation = Quaternion.Euler(.0f, .0f, .0f);
    }
}
