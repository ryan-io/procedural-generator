﻿using MMTools;
using Source.Events;
using UnityEngine;

namespace InventoryEngine {
    /// <summary>
    /// This class shows examples of how you can listen to MMInventoryEvents, from any class
    /// </summary>
    public class EventTester : MonoBehaviour, IEngineEventListener<MMInventoryEvent>
    {
        /// <summary>
        /// When we catch a MMInventoryEvent, we filter on its type and display info about the item used
        /// </summary>
        /// <param name="inventoryEvent"></param>
        public virtual void OnEventHeard(MMInventoryEvent inventoryEvent)
        {
            if (inventoryEvent.InventoryEventType == MMInventoryEventType.ItemUsed)
            {
                MMDebug.DebugLogTime("item used");
                MMDebug.DebugLogTime("ItemID : "    +inventoryEvent.EventItem.ItemID);
                MMDebug.DebugLogTime("Item name : " +inventoryEvent.EventItem.ItemName);
            }
        }
    
        /// <summary>
        /// On enable we start listening for MMInventoryEvents 
        /// </summary>
        protected virtual void OnEnable()
        {
            this.StartListeningToEvents();
        }
    
        /// <summary>
        /// On disable we stop listening for MMInventoryEvents 
        /// </summary>
        protected virtual void OnDisable()
        {
            this.StopListeningToEvents();
        }
    }
}