// using Sirenix.OdinInspector;
// using UnityEngine;
// using Logger = Engine.Tools.Logging.Logger;
//
// namespace Engine.Procedural {
// 	public class MonoTesting : MonoBehaviour {
// 		[SerializeField] ProceduralMapConfiguration _configuration;
//
// 		[Button]
// 		void TestGeneratorStatus() {
// 			var generatorStatus = new ProceduralExitHandler();
// 			generatorStatus.OnQuit.Register(LogTestMsg);
// 			var shouldQuit = generatorStatus.DetermineQuit(() => _configuration == null);
//
// 			if (shouldQuit)
// 				Logger.Msg("We should quit!");
// 			else
// 				Logger.Msg("We should NOT quit.");
//
// 			generatorStatus.OnQuit.Unregister(LogTestMsg);
// 		}
//
// 		void LogTestMsg() => Logger.Test("This isa test method");
// 	}
// }