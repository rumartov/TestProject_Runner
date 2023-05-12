using Services.Input;
using UnityEngine;

namespace Logic.Hero
{
    public class HeroMove : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 5;

        private IInputService _inputService;
        private Vector3 _movementVector;

        private void Awake()
        {
            _movementVector = Vector3.forward;
        }

        private void Update()
        {
            _inputService.DetectInput(Input.mousePosition);

            transform.Translate(_movementVector * movementSpeed * Time.deltaTime);
        }

        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
            _inputService.OnMoveEvent += Move;
        }

        private void Move(InputService.MoveDirection direction)
        {
            var position = transform.position;

            switch (direction)
            {
                case InputService.MoveDirection.Left:
                    MoveLeft(position);
                    Debug.Log("Left");
                    break;
                case InputService.MoveDirection.Right:
                    MoveRight(position);
                    Debug.Log("Right");
                    break;
            }
        }

        private void MoveRight(Vector3 position)
        {
            if (transform.position.x >= 1)
                return;

            transform.position = new Vector3(
                position.x + 1,
                position.y,
                position.z);
        }

        private void MoveLeft(Vector3 position)
        {
            if (transform.position.x <= -1)
                return;

            transform.position = new Vector3(
                position.x - 1,
                position.y,
                position.z);
        }
    }
}