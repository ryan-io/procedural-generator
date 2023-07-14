﻿using UnityEngine;
using UnityEngine.UI;

namespace MMTools {
    /// <summary>
    /// A class used to bind a button to a MMDebugMenu
    /// </summary>
    public class MMDebugMenuItemButton : MonoBehaviour
    {
        [Header("Bindings")]
        /// the associated button 
        public Button TargetButton;
        /// the button's text comp
        public Text ButtonText;
        /// the button's background image
        public Image ButtonBg;
        /// the name of the event bound to this button
        public string ButtonEventName = "Button";

        protected bool _listening = false;

        /// <summary>
        /// Triggers a button event using the button's event name
        /// </summary>
        public virtual void TriggerButtonEvent()
        {
            MMDebugMenuButtonEvent.Trigger(ButtonEventName);
        }

        protected virtual void OnMMDebugMenuButtonEvent(string checkboxEventName, bool active, MMDebugMenuButtonEvent.EventModes eventMode)
        {
            if ((eventMode            == MMDebugMenuButtonEvent.EventModes.SetButton)
                && (checkboxEventName == ButtonEventName)
                && (TargetButton      != null))
            {
                TargetButton.interactable = active;
            }
        }

        /// <summary>
        /// Starts listening for events
        /// </summary>
        public virtual void OnEnable()
        {
            if (!_listening)
            {
                _listening = true;
                MMDebugMenuButtonEvent.Register(OnMMDebugMenuButtonEvent);
            }
        }

        /// <summary>
        /// Stops listening for events
        /// </summary>
        public virtual void OnDestroy()
        {
            _listening = false;
            MMDebugMenuButtonEvent.Unregister(OnMMDebugMenuButtonEvent);
        }
    }
}