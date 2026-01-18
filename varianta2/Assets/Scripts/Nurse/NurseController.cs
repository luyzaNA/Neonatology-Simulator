using Assets.Scripts.Babies;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Nurse
{
    public class NurseController : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 2f;
        public float rotationSpeed = 8f;

        [Header("Carrying")]
        public Transform carryPoint;
        public float pickupRange = 1.5f;
        public float pickupHeightOffset = 0.8f;

        private bool isCarryingBaby;
        private Babies.Baby carriedBaby;

        private Rigidbody rb;
        private Vector3 moveInput;
        private Camera mainCam;
        private Animator animator;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            mainCam = Camera.main;

            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

            if (!carryPoint)
                Debug.LogError("[NurseController] CarryPoint not assigned!", this);

            Debug.Log("[NurseController] Initialized");
        }

        void Update()
        {
            if (Time.timeScale == 0) return;

            if (EventSystem.current != null &&
            EventSystem.current.currentSelectedGameObject != null &&
            EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null)
            {
                moveInput = Vector3.zero;
                animator.SetFloat("Speed", 0);
                return;
            }

            HandleMoveInput();
            UpdateAnimator();

            if (Input.GetKeyDown(KeyCode.C))
            {
                if (carriedBaby == null)
                    TryPickUpBaby();
                else
                    DropBaby();
            }

            //if (Input.GetMouseButtonDown(0))
            //    InteractWithBaby();
        }

        void TryPickUpBaby()
        {
            Vector3 center = transform.position + Vector3.up * pickupHeightOffset;
            Collider[] hits = Physics.OverlapSphere(center, pickupRange);

            foreach (Collider hit in hits)
            {
                Babies.Baby baby = hit.GetComponentInParent<Babies.Baby>();
                if (baby == null)
                    continue;

                carriedBaby = baby;
                isCarryingBaby = true;

                Rigidbody babyRb = baby.GetComponent<Rigidbody>();
                if (babyRb)
                    babyRb.isKinematic = true;

                Collider babyCollider = baby.GetComponent<Collider>();
                if (babyCollider)
                    babyCollider.enabled = false;

                baby.transform.SetParent(carryPoint);
                baby.transform.localPosition = Vector3.zero;
                baby.transform.localRotation = Quaternion.identity;

                Debug.Log($"Picked up baby: {baby.babyName}");
                return;
            }
        }

        void DropBaby()
        {
            if (!carriedBaby)
                return;

            carriedBaby.transform.SetParent(null);
            carriedBaby.transform.position = transform.position + transform.forward;

            Rigidbody babyRb = carriedBaby.GetComponent<Rigidbody>();
            if (babyRb)
                babyRb.isKinematic = false;

            Collider babyCollider = carriedBaby.GetComponent<Collider>();
            if (babyCollider)
                babyCollider.enabled = true;

            Debug.Log($"Dropped baby: {carriedBaby.babyName}");

            carriedBaby = null;
            isCarryingBaby = false;
        }


        // ================= MOVEMENT =================
        void HandleMoveInput()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 camForward = mainCam.transform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = mainCam.transform.right;
            camRight.y = 0;
            camRight.Normalize();

            moveInput = (camForward * v + camRight * h).normalized;
            animator.SetFloat("Speed", moveInput.magnitude);

            if (moveInput != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveInput);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRot,
                    rotationSpeed * Time.deltaTime
                );
            }
        }

        void FixedUpdate()
        {
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        }

        // ================= ANIMATOR =================
        void UpdateAnimator()
        {
            int carryLayer = animator.GetLayerIndex("Carry");
            if (carryLayer >= 0)
                animator.SetLayerWeight(carryLayer, isCarryingBaby ? 1f : 0f);
        }

        // ================= INTERACTION =================
        void InteractWithBaby()
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit, 5f))
                return;

            Babies.Baby baby = hit.collider.GetComponentInParent<Babies.Baby>();
            if (!baby)
                return;

            Debug.Log($"[NurseController] Interacting with {baby.babyName}");

            if (baby.hunger > 50)
                baby.Feed();
            else
                baby.ChangeDiaper();
        }

        // ================= DEBUG =================
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Vector3 center = transform.position + Vector3.up * pickupHeightOffset;
            Gizmos.DrawWireSphere(center, pickupRange);
        }
    }
}
