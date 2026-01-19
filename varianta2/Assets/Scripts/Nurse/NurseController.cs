using Assets.Scripts.Cribs;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Nurse
{
    public class NurseController : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 1f;
        public float rotationSpeed = 8f;

        [Header("Carrying")]
        public Transform carryPoint;
        public float pickupRange = 1.5f;
        public float pickupHeightOffset = 0.8f;

        private bool isCarryingBaby;
        private BabyController carriedBaby;

        private Rigidbody rb;
        private Vector3 moveInput;
        private Camera mainCam;
        private Animator animator;

        public Incubator incubator;
        public Bed bed;

        [Header("Waypoint Movement")]
        public Transform[] waypoints;     // assign in inspector
        public Transform[] waypointsTemperatureCheck;     // assign in inspector
        public Transform[] waypointsCT;     // assign in inspector
        public Transform[] waypointsInternare;     // assign in inspector
        public float waypointThreshold = 0.5f; // distance to consider "reached"
        private int currentWaypoint = 0;
        public bool followWaypoints = false;   // enable automatic movement
        public bool followWaypointsTemperature = false;
        public bool followWaypointsInternare = false;

        public bool followWaypointsCT = false;// enable automatic movement


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

            if (followWaypoints && waypoints.Length > 0)
            {
                MoveAlongWaypoints();
                UpdateAnimator();
                animator.SetFloat("Speed", 1);
                return;
            }

            if (followWaypointsTemperature && waypointsTemperatureCheck.Length > 0)
            {
                MoveAlongWaypointsTemperature();
                UpdateAnimator();
                animator.SetFloat("Speed", 1);
                return;
            }

            if (followWaypointsCT && waypointsCT.Length > 0)
            {
                MoveAlongWaypointsCT();
                UpdateAnimator();
                animator.SetFloat("Speed", 1);
                return;
            }

            if (followWaypointsInternare && waypointsInternare.Length > 0)
            {
                MoveAlongWaypointsInternare();
                UpdateAnimator();
                animator.SetFloat("Speed", 1);
                return;
            }

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
        }

        void MoveAlongWaypointsCT()
        {
            if (currentWaypoint >= waypointsCT.Length) return;
            Vector3 targetPos = waypointsCT[currentWaypoint].position;
            targetPos.y = transform.position.y;
            Vector3 direction = (targetPos - transform.position);
            direction.y = 0;
            direction.Normalize();

            if (direction != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }

            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

            Vector3 flatDistance = targetPos - transform.position;
            flatDistance.y = 0;
            if (flatDistance.magnitude < waypointThreshold)
            {
                currentWaypoint++;
                if (currentWaypoint >= waypointsCT.Length)
                {
                    followWaypointsCT = false;
                    animator.SetFloat("Speed", 0);
                    currentWaypoint = 0;
                    CheckbabyCT();
                }
            }
        }

        void MoveAlongWaypointsInternare()
        {
            if (currentWaypoint >= waypointsInternare.Length) return;
            Vector3 targetPos = waypointsInternare[currentWaypoint].position;
            targetPos.y = transform.position.y;
            Vector3 direction = (targetPos - transform.position);
            direction.y = 0;
            direction.Normalize();

            if (direction != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }

            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

            Vector3 flatDistance = targetPos - transform.position;
            flatDistance.y = 0;
            if (flatDistance.magnitude < waypointThreshold)
            {
                currentWaypoint++;
                if (currentWaypoint >= waypointsInternare.Length)
                {
                    followWaypointsInternare = false;
                    animator.SetFloat("Speed", 0);
                    currentWaypoint = 0;
                    InterneazaBebelus();
                }
            }
        }

        void MoveAlongWaypointsTemperature()
        {
            if (currentWaypoint >= waypointsTemperatureCheck.Length) return;
            Vector3 targetPos = waypointsTemperatureCheck[currentWaypoint].position;
            targetPos.y = transform.position.y;
            Vector3 direction = (targetPos - transform.position);
            direction.y = 0;
            direction.Normalize();

            if (direction != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }

            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

            Vector3 flatDistance = targetPos - transform.position;
            flatDistance.y = 0;
            if (flatDistance.magnitude < waypointThreshold)
            {
                currentWaypoint++;
                if (currentWaypoint >= waypointsTemperatureCheck.Length)
                {
                    followWaypointsTemperature = false;
                    animator.SetFloat("Speed", 0);
                    currentWaypoint = 0;
                    CheckbabyTemperature();
                }
            }
        }

        void MoveAlongWaypoints()
        {
            if (currentWaypoint >= waypoints.Length) return;
            Vector3 targetPos = waypoints[currentWaypoint].position;
            targetPos.y = transform.position.y;
            Vector3 direction = (targetPos - transform.position);
            direction.y = 0;
            direction.Normalize();

            if (direction != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }

            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

            Vector3 flatDistance = targetPos - transform.position;
            flatDistance.y = 0;
            if (flatDistance.magnitude < waypointThreshold)
            {
                currentWaypoint++;
                if (currentWaypoint >= waypoints.Length)
                {
                    followWaypoints = false;
                    animator.SetFloat("Speed", 0);
                    currentWaypoint = 0;
                    PutBabyInIncubator();
                }
            }
        }



        public void TryPickUpBaby()
        {
            Vector3 center = transform.position + Vector3.up * pickupHeightOffset;
            Collider[] hits = Physics.OverlapSphere(center, pickupRange);

            foreach (Collider hit in hits)
            {
                BabyController baby = hit.GetComponentInParent<BabyController>();
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
                return;
            }
        }

        void CheckbabyTemperature()
        {
            if (!carriedBaby)
                return;
            carriedBaby.ShowDecisionPanelTemperature();
        }

        void CheckbabyCT()
        {
            if (!carriedBaby)
                return;
            carriedBaby.ShowDecisionPanelCT();
        }

        void InterneazaBebelus()
        {
            if (!carriedBaby || !bed)
                return;
            bed.PlaceBabyInBed(carriedBaby);
            carriedBaby = null;
            isCarryingBaby = false;
        }

        void PutBabyInIncubator()
        {
            if (!carriedBaby || !incubator)
                return;
            incubator.PlaceBabyIncubator(carriedBaby);
            carriedBaby.ShowDecisionPanelIncubator();
            carriedBaby = null;
            isCarryingBaby = false;
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

            BabyController baby = hit.collider.GetComponentInParent<BabyController>();
            if (!baby)
                return;
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
