using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 450f;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;

    private CharacterController controller;
    private Quaternion targetRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (input != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(input);
            transform.eulerAngles = Vector3.up * Mathf
                .MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);
        }

        Vector3 motion = input;
        motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? 0.7f : 1f; // if moving diagonally, reduce speed to 70% to prevent faster movement
        motion *= Input.GetButton("Run") ? runSpeed : walkSpeed; // if run button is held, use run speed, otherwise use walk speed
        motion += Vector3.up * -8; // add gravity

        controller.Move(motion * Time.deltaTime);
    }
}
