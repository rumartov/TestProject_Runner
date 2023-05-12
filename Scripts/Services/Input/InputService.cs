using System.Collections.Generic;
using UnityEngine;

namespace Services.Input
{
    public class InputService : IInputService
    {
        public delegate void MoveDelegate(MoveDirection direction);

        public enum MoveDirection
        {
            Left,
            Right,
            Up,
            Down
        }

        private readonly float _swipeDeltaThreshold = 10;

        private readonly List<Vector2> points = new();

        public MoveDelegate OnMoveEvent { get; set; }


        public void DetectInput(Vector3 mousePosition)
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                points.Clear();
                points.Add(mousePosition);
            }

            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                points.Add(mousePosition);
                DetectSwipe();
            }
        }

        private void DetectSwipe()
        {
            var swipeDelta = points[points.Count - 1] - points[0];

            if (Mathf.Abs(swipeDelta.x) < _swipeDeltaThreshold && Mathf.Abs(swipeDelta.y) < _swipeDeltaThreshold)
                return;

            OnMoveEvent?.Invoke(Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y) ? swipeDelta.x < 0
                    ?
                    MoveDirection.Left
                    : MoveDirection.Right :
                swipeDelta.y < 0 ? MoveDirection.Down : MoveDirection.Up);
        }
    }
}