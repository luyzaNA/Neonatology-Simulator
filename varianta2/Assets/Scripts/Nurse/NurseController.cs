using UnityEngine;
using Assets.Scripts.Babies;

namespace Assets.Scripts.Nurse
{
    public class NurseController : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 2f;
        public float rotationSpeed = 2f;

        private Rigidbody rb;
        private Vector3 moveInput;
        private Camera mainCam;

        private Animator animator;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            mainCam = Camera.main;

            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            HandleMoveInput();

            if (Input.GetMouseButtonDown(0))
            {
                InteractWithBaby();
            }
        }

        void HandleMoveInput()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            float speed = new Vector3(h, 0f, v).magnitude;
            animator.SetFloat("Speed", speed);

            // Camera-relative forward
            Vector3 camForward = mainCam.transform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            // Character-relative right
            Vector3 right = transform.right;
            right.y = 0f;
            right.Normalize();

            // Forward follows camera, strafe follows character
            moveInput = (camForward * v + right * h).normalized;

            // Rotate nurse toward movement direction
            if (moveInput != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveInput);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }

        void FixedUpdate()
        {
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        }

        void InteractWithBaby()
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Babies.Baby baby = hit.collider.GetComponent<Babies.Baby>();
                if (baby != null)
                {
                    if (baby.hunger > 50)
                        baby.Feed();
                    else
                        baby.ChangeDiaper();
                }
            }
        }
    }
}
