using System;
using System.Collections;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BCL;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Pathfinding;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	/// <summary>
	/// The TaskLogic arguments specify a NavGraph & flag
	/// The NavGraph is the Astar generated graph; the flag is whether to run TaskLogic asynchronously
	/// This will run synchronously if not awaited/forgotten
	/// </summary>
	internal class GraphScanner : AsyncUnitOfWork<GraphScanner.Args> {
		protected override async UniTask TaskLogic(Args args, CancellationToken token) {
			if (args.IsAsync) {
				await ScanGraphAsync(args, token);
			}
			else {
				ScanGraph(args.Graph);
			}
		}

		static void ScanGraph(NavGraph graph) {
			graph.active.Scan();
		}

		static IEnumerator ScanGraphAsync(Args args, CancellationToken token) {
			Physics.SyncTransforms();
			var sB = new StringBuilder();

			foreach (var progress in AstarPath.active.ScanAsync(args.Graph)) {
#if UNITY_EDITOR || UNITY_STANDALONE
				sB.Clear();
				sB.Append(progress.description);
				sB.Append(GenLogging.SPACE);
				sB.Append("-");
				sB.Append(GenLogging.SPACE);
				sB.Append(progress.progress * 100);

#endif

				if (token.IsCancellationRequested) {
#if UNITY_EDITOR || UNITY_STANDALONE
					sB.Clear();
					sB.Append("Cancellation was requested. Cleaning up Pathfinding.");

#endif
					yield break;
				}

				yield return null;
			}
			
			args.OnCompleteCtx?.Invoke();
			
#if UNITY_EDITOR || UNITY_STANDALONE
			sB.Clear();
			sB.Append("Pathfinding has completed.");


			sB.Clear();
			sB.Append(PATHFINDING_NODES);
			sB.Append(args.Graph.CountNodes());

#endif
		}

		internal GraphScanner() {
		}

		internal readonly struct Args {
			internal             NavGraph Graph         { get; }
			[CanBeNull] internal Action   OnCompleteCtx { get; }
			internal             bool     IsAsync       { get; }

			internal Args(NavGraph graph, bool isAsync) : this(graph, default, isAsync) {
			}

			internal Args(NavGraph graph, [CanBeNull] Action onCompleteCtx = default, bool isAsync = false) {
				Graph         = graph;
				IsAsync       = isAsync;
				OnCompleteCtx = onCompleteCtx;
			}
		}

		const string PATHFINDING_NODES = "Total pathfinding nodes: ";
	}
}