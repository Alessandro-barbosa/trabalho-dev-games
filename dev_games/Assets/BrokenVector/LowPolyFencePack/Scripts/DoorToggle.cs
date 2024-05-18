using UnityEngine;

namespace BrokenVector.LowPolyFencePack
{
    /// <summary>
    /// This class toggles the door animation.
    /// The gameobject of this script has to have the DoorController script which needs an Animator component
    /// and some kind of Collider which detects your mouse click applied.
    /// </summary>
    [RequireComponent(typeof(DoorController))]
	public class DoorToggle : MonoBehaviour
    {
        public GameObject porta;
        private DoorController doorController;
        private bool playerInsideTriggerDoor = false;

        void Awake()
        {
            doorController = GetComponent<DoorController>();
        }
        //void OnMouseDown()
        //{
        //    doorController.ToggleDoor();
        //}
        void Update()
        {
            if (playerInsideTriggerDoor && Input.GetKeyDown(KeyCode.E))
            {
                doorController.ToggleDoor();
            }
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInsideTriggerDoor = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInsideTriggerDoor = false;
            }
        }
    }
}