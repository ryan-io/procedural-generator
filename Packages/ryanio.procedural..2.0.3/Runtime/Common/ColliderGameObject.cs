// ProceduralGeneration

namespace ProceduralGeneration {
	internal class ColliderGameObject {
		IActions Actions           { get; }
		string   SerializeableName { get; }

		internal void Setup() {
			var colliderGo = new ColliderGameObjectCreator().Create(Actions.GetOwner());
			Actions.SetColliderGameObject(colliderGo);

			new SetColliderObjName()
			   .Set(colliderGo, Constants.SAVE_COLLIDERS_PREFIX + SerializeableName);
		}

		public ColliderGameObject(IActions actions) {
			Actions           = actions;
			SerializeableName = actions.GetSerializationName();
		}
	}
}