using UnityBCL;

namespace Engine.Procedural {
	public class ProceduralMapHandler : Singleton<ProceduralMapHandler, ProceduralMapHandler> {
		RoomDrawHelper   _drawHelper;
		public RoomData Data         { get; private set; }
		public MapSpans MapSpansData { get; private set; }
		public string   Seed         { get; private set; }

		public void DrawRoom() => _drawHelper.Draw();


		public void Inject(RoomData data, MapSpans mapSpansData, int seed) {
			var dto = new RoomProcessorDto(data, seed, mapSpansData);
			Data         = data;
			_drawHelper   = new RoomDrawHelper(dto);
			MapSpansData = mapSpansData;
			Seed         = seed.ToString();
		}
	}
}