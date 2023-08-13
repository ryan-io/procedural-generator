// ProceduralGeneration

namespace ProceduralGeneration {
	internal class ColliderGameObject {
		IActions Actions { get; }

		internal void Setup() {
			var colliderGo = new ColliderGameObjectCreator().Create(Actions.GetOwner());
			Actions.SetColliderGameObject(colliderGo);
		}

		public ColliderGameObject(IActions actions) {
			Actions = actions;
		}
	}
}