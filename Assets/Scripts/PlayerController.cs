using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 450f;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;

    private CharacterController controller;
    private Quaternion targetRotation;
    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ControlerMouse();
        // ControlWASD();
    }

    void ControlerMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = mainCamera
            .ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.transform.position.y - transform.position.y));
        targetRotation = Quaternion.LookRotation(
            mousePosition - new Vector3(transform.position.x, 0, transform.position.z)
        );
        transform.eulerAngles = Vector3.up * Mathf
            .MoveTowardsAngle(
                transform.eulerAngles.y,
                targetRotation.eulerAngles.y,
                rotationSpeed * Time.deltaTime
                );

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        Vector3 motion = input;
        motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? 0.7f : 1f; // if moving diagonally, reduce speed to 70% to prevent faster movement
        motion *= Input.GetButton("Run") ? runSpeed : walkSpeed; // if run button is held, use run speed, otherwise use walk speed
        motion += Vector3.up * -8; // add gravity

        controller.Move(motion * Time.deltaTime);
    }

    void ControlWASD()
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
