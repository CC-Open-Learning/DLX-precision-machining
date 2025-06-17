using UnityEngine;
using UnityEngine.Events;

namespace RemoteEducation.UI
{
    public class Toggle : MonoBehaviour
    {

        /// <summary>Property which manipulates the current toggle state of this UI element.</summary>
        public bool On { set { Set(value); } get { return on; } }
        [Tooltip("Indicates the current toggle state of this UI element.")]
        [SerializeField] private bool on;

        /// <summary>If you enable this, then any sibling GameObjects (i.e. same parent GameObject) will automatically be turned off.
		/// This allows you to make radio boxes, or force only one panel to show at a time, etc.</summary>
		public bool TurnOffSiblings { set { if (turnOffSiblings = value) TurnOffSiblingsNow(); } get { return turnOffSiblings; } }
        [Tooltip("When enabled, any Toggle with the same parent will be turned off when this Toggle is turned on")]
        [SerializeField] private bool turnOffSiblings;

        /// <summary>Event fired when the Toggle is switched on</summary>
        public UnityEvent OnOn { get { if (onOn == null) onOn = new UnityEvent(); return onOn; } }
        [SerializeField] private UnityEvent onOn;


        /// <summary>Event fired when the Toggle is switched off</summary>
        public UnityEvent OnOff { get { if (onOff == null) onOff = new UnityEvent(); return onOff; } }
        [SerializeField] private UnityEvent onOff;


        public void Set(bool value)
        {
            if (value)
            {
                TurnOn();
            }
            else
            {
                TurnOff();
            }
        }

        /// <summary>
        ///     Inverts the current state of the Toggle
        /// </summary>
        [ContextMenu("Invert")]
        public void Invert()
        {
            On = !On;
        }

        /// <summary>
        ///     Attempts to turn on the Toggle, if it is off
        /// </summary>
        [ContextMenu("Turn On")]
        public void TurnOn()
        {
            if (on) { return; }

            TurnOnImmediate();
        }

        /// <summary>
        ///     Attempts to turn off the Toggle, if it is on
        /// </summary>
        [ContextMenu("Turn Off")]
        public void TurnOff()
        {
            if (!on) { return; }
            
            TurnOffImmediate();
        }


        /// <summary>
        ///     Checks for sibling (same parent) GameObjects with attached <see cref="Toggle"/>
        ///     components and turns them off
        /// </summary>
        public virtual void TurnOffSiblingsNow()
        {
            // Toggles which are top-level GameObjects will not consider sibling objects
            if (!transform.parent) { return; }

            var ignore = transform.GetSiblingIndex();

            for (var idx = transform.parent.childCount - 1; idx >= 0; idx--)
            {
                if (idx == ignore) { continue; }

                var sibling = transform.parent.GetChild(idx).GetComponent<Toggle>();

                if (sibling)
                {
                    sibling.TurnOff();
                }
            }
        }

        /// <summary>
        ///     Invokes the event indicating the Toggle is in the On state
        /// </summary>
        protected virtual void TurnOnImmediate()
        {
            if (turnOffSiblings)
            {
                TurnOffSiblingsNow();
            }

            on = true;

            // Safe event invokation
            onOn?.Invoke();
        }

        /// <summary>
        ///     Invokes the event indicating te Toggle is in the Off state
        /// </summary>
        protected virtual void TurnOffImmediate()
        {
            on = false;

            onOff?.Invoke();
        }
    }
}