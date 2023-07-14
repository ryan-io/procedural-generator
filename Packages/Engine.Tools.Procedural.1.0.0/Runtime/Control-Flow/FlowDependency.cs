using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityBCL;
using UnityEngine;

namespace Engine.Procedural {
	[Serializable]
	public class FlowDependency {
		[field: SerializeField]
		[field: HideLabel]
		public FlowComponents FlowComponents { get; private set; } = new();

		public List<ICreation> GetComponentsAsCreation => GetPropertyValues<ICreation>(false);
		public List<IProgress> GetComponentsAsProgress => GetPropertyValues<IProgress>(false);
		public List<object>    GetComponents           => GetPropertyValues<object>();

		List<T> GetPropertyValues<T>(bool addNull = true) where T : class
			=> FlowComponents.GetPropertyValues<T>(addNull);
	}
}