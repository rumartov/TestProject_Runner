using UnityEngine;

namespace Services.Input
{
    public interface IInputService : IService
    {
        InputService.MoveDelegate OnMoveEvent { get; set; }
        void DetectInput(Vector3 mousePosition);
    }
}