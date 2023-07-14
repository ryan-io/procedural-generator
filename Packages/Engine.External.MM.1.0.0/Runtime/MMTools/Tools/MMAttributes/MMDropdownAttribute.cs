using UnityEngine;

namespace MMTools {
    public class MMDropdownAttribute : PropertyAttribute
    {
        public readonly object[] DropdownValues;

        public MMDropdownAttribute(params object[] dropdownValues)
        {
            DropdownValues = dropdownValues;
        }
    }
}