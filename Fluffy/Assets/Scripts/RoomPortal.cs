using CameraBounding;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fluffy
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class RoomPortal : Interactable
    {
        [SerializeField] private Transform destinationPosition;
        [SerializeField] private CameraBounds2D destinationRoomBounds;
        [SerializeField] private string destinationName;
        [SerializeField] private FollowPlayer2D followCamera;
        [SerializeField] private Vector2 interactionBubbleOffset;
        [SerializeField] private InteractionPromptProvider interactionPromptProvider;

        public override Vector2 InteractionBubbleOffset => interactionBubbleOffset;

        public override void OnTargeted()
        {
            interactionPromptProvider.ShowPrompt(this, destinationName);
        }

        public override void OnUntargeted()
        {
            interactionPromptProvider.HidePrompt(this);
        }

        public override void StartInteraction(GameObject interactor)
        {
            followCamera.Bounds = destinationRoomBounds;
            interactor.transform.position = destinationPosition.position;
        }

        public override void EndInteraction()
        {
            // unused
        }
    }
}
