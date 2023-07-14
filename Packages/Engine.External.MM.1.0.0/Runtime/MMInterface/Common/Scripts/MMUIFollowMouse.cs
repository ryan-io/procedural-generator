﻿using UnityEngine;

namespace MMInterface {
    public class MMUIFollowMouse : MonoBehaviour
    {
        public    Canvas  TargetCanvas { get; set; }
        protected Vector2 _newPosition;

        protected virtual void LateUpdate()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(TargetCanvas.transform as RectTransform, Input.mousePosition, TargetCanvas.worldCamera, out _newPosition);
            transform.position = TargetCanvas.transform.TransformPoint(_newPosition);
        }
    }
}