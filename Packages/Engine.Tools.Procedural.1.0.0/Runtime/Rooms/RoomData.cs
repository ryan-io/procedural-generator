using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityBCL;

namespace Engine.Procedural {
	[Serializable]
	[HideMonoScript]
	public class RoomData : SerializedScriptableObject {
		public List<Room> Rooms => _rooms;

		public void Inject(List<Room> rooms, string fileName) {
			if (rooms.IsEmptyOrNull() || string.IsNullOrWhiteSpace(fileName)) {
#if UNITY_STANDALONE || UNITY_EDITOR
				var log = new UnityLogging();
				log.Error(CannotSaveError);
#endif
				return;
			}

			_rooms = rooms;

			// var generic = new GenericSaver(LevelGeneratorRoomData);
			// generic.Save(this, fileName, true);
		}

#region PLUMBING

		[NonSerialized] [OdinSerialize] [ReadOnly] [ListDrawerSettings(HideRemoveButton = true, HideAddButton = true)]
		List<Room> _rooms;

		[ShowInInspector] int RoomCount => _rooms.Count;

		const string CannotSaveError =
			"The provided 'rooms' collection was null or empty or you did not provide a proper asset name.";

		const string LevelGeneratorRoomData = "Level Generator Room Data";

#endregion
	}
}