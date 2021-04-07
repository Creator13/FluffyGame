using UnityEngine;
using UnityEngine.InputSystem;

namespace Fluffy
{
    [RequireComponent(typeof(PlayerInteraction), typeof(PlayerInput))]
    public class InteractionController : MonoBehaviour
    {
        private PlayerInput input;
        private PlayerInteraction playerInteraction;
        
        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            playerInteraction = GetComponent<PlayerInteraction>();
        }
        
        public void Deactivate()
        {
            // playerInteraction.enabled = false;

            input.actions["Walk"].Disable();
            input.actions["Interact"].Disable();
        }

        public void Activate()
        {
            // playerInteraction.enabled = true;

            input.actions["Walk"].Enable();
            input.actions["Interact"].Enable();
        }
    }
}
