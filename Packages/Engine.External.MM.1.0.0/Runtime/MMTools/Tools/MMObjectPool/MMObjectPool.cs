using System.Collections.Generic;
using UnityEngine;

namespace MMTools {
    public class MMObjectPool : MonoBehaviour
    {
        [MMReadOnly]
        public List<GameObject> PooledGameObjects;
    }
}