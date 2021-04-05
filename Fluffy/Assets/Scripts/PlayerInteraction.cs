using System.Collections.Generic;
using System.Linq;
using Brackeys;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Fluffy
{
    [RequireComponent(typeof(CharacterController2D), typeof(PlayerInput), typeof(Animator))]
    public class PlayerInteraction : MonoBehaviour
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        
        private CharacterController2D controller;
        private PlayerInput input;
        private Animator animator;

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
            animator = GetComponent<Animator>();

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
            animator.SetFloat(Speed, Mathf.Abs(move));
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
            if (interactionTarget == null) return;
            
            Debug.Log("Interacting");
            interactionTarget.StartInteraction(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}
