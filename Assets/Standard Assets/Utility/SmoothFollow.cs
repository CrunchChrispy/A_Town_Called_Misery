using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class SmoothFollow : MonoBehaviour
    {

        // The target we are following
        [SerializeField]
        private Transform target;
        // The distance in the x-z plane to the target
        [SerializeField]
        private float distance = 10.0f;
        // the height we want the camera to be above the target
        [SerializeField]
        private float height = 5.0f;
        public Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);
        [SerializeField]
        private float rotationDamping;
        [SerializeField]
        private float heightDamping;


        private float wantedRotationAngle;// = target.eulerAngles.y;
        private float wantedHeight;// = target.position.y + height;

        private float currentRotationAngle;// = transform.eulerAngles.y;
        private float currentHeight;// = transform.position.y;

        private Quaternion currentRotation;// = Quaternion.Euler(0, currentRotationAngle, 0);

        // Use this for initialization
        void Start() { }

        // Update is called once per frame
        void LateUpdate()
        {
            // Early out if we don't have a target
            if (!target)
                return;

            // Calculate the current rotation angles
            wantedRotationAngle = target.eulerAngles.y;
            wantedHeight = target.position.y + height;

            currentRotationAngle = transform.eulerAngles.y;
            currentHeight = transform.position.y;

            // Damp the rotation around the y-axis
            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

            // Damp the height
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

            // Convert the angle into a rotation
            currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target
            transform.position = target.position + offset;
            transform.position -= currentRotation * Vector3.forward * distance;

            // Set the height of the camera
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            // Always look at the target
            transform.LookAt(target);
        }
    }
}