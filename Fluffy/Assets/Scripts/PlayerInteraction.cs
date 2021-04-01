using System.Collections.Generic;
using System.Linq;
using Brackeys;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Fluffy
{
    [RequireComponent(typeof(CharacterController2D), typeof(PlayerInput))]
    public class PlayerInteraction : MonoBehaviour
    {
        private CharacterController2D controller;
        private PlayerInput input;

        [SerializeField] private float moveSpeedMultiplier = 1;
        [SerializeField] private float interactionRange = 3;
        [SerializeField] private LayerMask interactionLayers;

        private List<IInteractable> targetsInRange = new List<IInteractable>();
        private IInteractable interactionTarget;
        private float move;

        private void Start()
        {
            controller = GetComponent<CharacterController2D>();
            input = GetComponent<PlayerInput>();

            SetupInput();
        }

        private void Update()
        {
            UpdateMove();
            UpdateInteractionTargets();
        }

        private void FixedUpdate()
        {
            PerformMove();
        }

        private void SetupInput()
        {
            input.actions["Interact"].performed += Interact;
        }

        private void UpdateMove()
        {
            move = input.actions["Walk"].ReadValue<float>() * moveSpeedMultiplier;
        }

        private void PerformMove()
        {
            controller.Move(move, false, false);
        }

        private void UpdateInteractionTargets()
        {
            var overlap = Physics2D.OverlapCircleAll(transform.position, interactionRange, interactionLayers);
            
            var interactables = overlap.Where(obj => obj.GetComponent<IInteractable>() != null).ToArray();
            if (!interactables.Any())
            {
                interactionTarget?.OnUntargeted();
                interactionTarget = null;
                return;
            }
            
            interactables = interactables.OrderBy(obj => Vector2.Distance(obj.transform.position, transform.position)).ToArray();

            targetsInRange = interactables.Select(obj => obj.GetComponent<IInteractable>()).ToList();
            if (interactionTarget != targetsInRange[0])
            {
                interactionTarget?.OnUntargeted();
                interactionTarget = targetsInRange[0];
                interactionTarget.OnTargeted();
            }
        }

        private void Interact(InputAction.CallbackContext ctx)
        {
            Debug.Log("Interacting");
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}
