// ProceduralGeneration

namespace ProceduralGeneration {
	/// <summary>
	///  This needs work... will need to be refactored and overall architecture needs to be rethought
	/// </summary>
	internal interface IActions : IProceduralLogging, IComponents, IConfigurations, ICollections, IIdentifiers, IData,
	                              ISerializable, IDeserializable, ILayers, ITiles {
		float GetTimeElapsed();
		void  StopTimer();
	}
}