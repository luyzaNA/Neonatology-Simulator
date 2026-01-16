using UnityEngine;

namespace Assets.Scripts.Nurse
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        [Header("Target")]
        public Transform target; // NurseRoot to follow

        [Header("Rotation Settings")]
        public float rotateSpeed = 5f;
        public float minYAngle = -20f;
        public float maxYAngle = 180f;

        [Header("Zoom Settings")]
        public float zoomSpeed = 5f;
        public float minZoomDistance = 1.5f;
        public float maxZoomDistance = 6f;
        public float zoomSmoothTime = 0.1f;

        private float yaw;
        private float pitch;

        private float currentZoom;
        private float zoomVelocity;

        private Transform cam;

        private Vector3 pivotOffset; // offset from target to camera rig

        void Start()
        {
            cam = GetComponentInChildren<Camera>().transform;

            // Initialize rotation
            Vector3 angles = transform.eulerAngles;
            yaw = angles.y;
            pitch = angles.x;

            // Calculate initial offset from target (so we respect Scene placement)
            pivotOffset = transform.position - target.position;

            // Initialize zoom based on current camera local z
            currentZoom = -cam.localPosition.z;
        }

        void FollowTarget()
        {
            // Keep the pivot at its initial offset from the target
            transform.position = target.position + pivotOffset;
        }


        void LateUpdate()
        {
            FollowTarget();
            HandleRotation();
            HandleZoom();
        }

      

        void HandleRotation()
        {
            if (Input.GetMouseButton(1)) // Hold RMB
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                yaw += mouseX * rotateSpeed;
                pitch -= mouseY * rotateSpeed;
                pitch = Mathf.Clamp(pitch, minYAngle, maxYAngle);

                transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
            }
        }

        void HandleZoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.001f)
            {
                currentZoom -= scroll * zoomSpeed;
                currentZoom = Mathf.Clamp(currentZoom, minZoomDistance, maxZoomDistance);
            }

            // Smooth zoom factor
            float smoothZoom = Mathf.SmoothDamp(cam.localPosition.magnitude, currentZoom, ref zoomVelocity, zoomSmoothTime);

            // Calculate new local position relative to camera rotation
            Vector3 zoomDir = (cam.localPosition).normalized; // direction from pivot to camera
            if (zoomDir == Vector3.zero) zoomDir = -Vector3.forward; // fallback

            cam.localPosition = zoomDir * smoothZoom;
        }

    }
}
