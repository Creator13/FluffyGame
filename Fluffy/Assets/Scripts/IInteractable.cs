using UnityEngine;

namespace Fluffy
{
    public interface IInteractable
    {
        void OnTargeted();
        void OnUntargeted();
        void StartInteraction(GameObject interactor);
        void EndInteraction();
    }
}
