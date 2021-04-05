using CameraBounding;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fluffy
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class RoomPortal : MonoBehaviour, IInteractable
    {
        [FormerlySerializedAs("targetPosition")] [SerializeField] private Transform destinationPosition;
        [FormerlySerializedAs("targetRoomBounds")] [SerializeField] private CameraBounds2D destinationRoomBounds;
        [SerializeField] private string destinationName;
        [SerializeField] private FollowPlayer2D followCamera;

        public void OnTargeted()
        {
            // TODO show popup
        }

        public void OnUntargeted()
        {
            // TODO hide popup
        }

        public void StartInteraction(GameObject interactor)
        {
            followCamera.Bounds = destinationRoomBounds;
            interactor.transform.position = destinationPosition.position;
        }

        public void EndInteraction()
        {
            // unused
        }
    }
}
