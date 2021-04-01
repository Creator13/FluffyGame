namespace Fluffy
{
    public interface IInteractable
    {
        void OnTargeted();
        void OnUntargeted();
        void StartInteraction();
        void EndInteraction();
    }
}
