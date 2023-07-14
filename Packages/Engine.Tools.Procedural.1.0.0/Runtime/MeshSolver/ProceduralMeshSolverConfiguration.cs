// using System;
// using Sirenix.OdinInspector;
// using UnityEngine;
// using UnityEngine.AddressableAssets;
//
// namespace Engine.Procedural {
// 	// [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
// 	// [CreateAssetMenu(menuName = "Procedural/Mesh Configuration", fileName = "procedural-mesh-configuration")]
// 	// public class ProceduralMeshSolverConfiguration : ScriptableObject {
// 	// 	[SerializeField] [Title("Collider Solver Setup")] [EnumToggleButtons]
// 	// 	public ColliderSolverType CollisionSolverType =
// 	// 		ColliderSolverType
// 	// 		   .Box;
// 	//
// 	// 	[field: SerializeField]
// 	// 	[field: ShowIf("@IsBox")]
// 	// 	[field: HideLabel]
// 	// 	public BoxColliderModel BoxColliderModel { get; private set; }
// 	//
// 	// 	[field: SerializeField]
// 	// 	[field: ShowIf("@IsEdge")]
// 	// 	[field: HideLabel]
// 	// 	public EdgeColliderModel EdgeColliderModel { get; private set; }
// 	//
// 	// 	[field: SerializeField]
// 	// 	[field: ShowIf("@IsPrimitive")]
// 	// 	[field: HideLabel]
// 	// 	public PrimitiveColliderModel PrimitiveColliderModel { get; private set; }
// 	//
// 	// 	bool IsBox       => CollisionSolverType == ColliderSolverType.Box;
// 	// 	bool IsEdge      => CollisionSolverType == ColliderSolverType.Edge;
// 	// 	bool IsPrimitive => CollisionSolverType == ColliderSolverType.PrimitiveCombo;
// 	// }
// 	//
// 	// [Serializable]
// 	// public class EdgeColliderModel {
// 	// 	[field: SerializeField]
// 	// 	[field: Title("Edge Collider Solver")]
// 	// 	[field: Range(0.1f, 1f)]
// 	// 	public float EdgeColliderRadius { get; private set; } = 0.5f;
// 	//
// 	// 	[field: SerializeField] public Vector2 EdgeColliderOffset { get; private set; } = new(0.5f, -0.5f);
// 	// }
// 	//
// 	// [Serializable]
// 	// public class BoxColliderModel {
// 	// 	[field: SerializeField]
// 	// 	[field: Title("Box Collider Solver")]
// 	// 	[field: Required]
// 	// 	public AssetReference BoxColliderPrefab { get; private set; }
// 	//
// 	// 	[field: SerializeField]
// 	// 	[field: Range(0.1f, 1.5f)]
// 	// 	public float BoxColliderSkinWidth { get; private set; } = 0.1f;
// 	// }
//
// 	// [Serializable]
// 	// public class PrimitiveColliderModel {
// 	// 	[field: SerializeField]
// 	// 	[field: Range(0.1f, 1f)]
// 	// 	public float PrimitiveColliderRadius { get; private set; } = 0.1f;
// 	//
// 	// 	[field: SerializeField]
// 	// 	[field: Range(0.1f, 1f)]
// 	// 	public float PrimitiveColliderSkinWidth { get; private set; } = 0.5f;
// 	// }
// }