using UnityEngine;

namespace Controllers
{
    public class RotateCamHolder : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed;

        private void Update()
        {
            transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
        }
    }
}