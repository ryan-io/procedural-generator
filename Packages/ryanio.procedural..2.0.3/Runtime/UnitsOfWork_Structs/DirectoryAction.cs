
using System;
using System.IO;
using JetBrains.Annotations;
using UnityBCL.Serialization;

namespace Engine.Procedural.Runtime {
	public readonly struct DirectoryAction {
		[CanBeNull]
		public string NewMapDirectory(string name) {
			try {
				var isSerialized = new ValidationSerializedName().Validate(name);

				if (!isSerialized)
					throw new Exception($"{name} {Message.NAME_NOT_SERIALIZED}");

				var location        = UnitySaveLocation.GetDefault;
				var directory       = location.SaveLocation + Constants.BACKSLASH + name;
				var directoryExists = Directory.Exists(directory);

				if (!directoryExists)
					Directory.CreateDirectory(directory);

				return directory;
			}
			catch (Exception) {
				// ignored
			}

			return string.Empty;
		}
	}
}