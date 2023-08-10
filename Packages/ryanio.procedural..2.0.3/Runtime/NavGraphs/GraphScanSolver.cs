using System;
using System.Collections;
using System.Text;
using System.Threading;
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
	public class GraphScanner : AsyncUnitOfWork<GraphScanner.Args> {
		StopWatchWrapper StopWatch { get; }

		protected override async UniTask TaskLogic(Args args, CancellationToken token) {
			if (args.IsAsync) {
				await ScanGraphAsync(args, token);
				
			}
			else {
				
				ScanGraph(args.Graph);
			}
		}

		public void ScanGraph(NavGraph graph) => AstarPath.active.Scan(graph);

		IEnumerator ScanGraphAsync(Args args, CancellationToken token) {
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

				GenLogging.Instance.LogWithTimeStamp(LogLevel.Normal, StopWatch.TimeElapsed, sB.ToString(),
					"GraphScanAsync");
#endif

				if (token.IsCancellationRequested) {
#if UNITY_EDITOR || UNITY_STANDALONE
					sB.Clear();
					sB.Append("Cancellation was requested. Cleaning up Pathfinding.");

					GenLogging.Instance.LogWithTimeStamp(
						LogLevel.Error, StopWatch.TimeElapsed, sB.ToString(), "GraphScanAsync_Cancelled");
#endif
					yield break;
				}

				yield return null;
			}
			
			args.OnCompleteCtx?.Invoke();
			
#if UNITY_EDITOR || UNITY_STANDALONE
			sB.Clear();
			sB.Append("Pathfinding has completed.");

			GenLogging.Instance.LogWithTimeStamp(LogLevel.Normal, StopWatch.TimeElapsed, sB.ToString(),
				"GraphScanAsync_Complete");

			sB.Clear();
			sB.Append(PATHFINDING_NODES);
			sB.Append(args.Graph.CountNodes());

			GenLogging.Instance.LogWithTimeStamp(LogLevel.Normal, StopWatch.TimeElapsed, sB.ToString(),
				"GraphScanAsync_TotalNodes");
#endif
		}

		public GraphScanner(StopWatchWrapper stopWatch) {
			StopWatch = stopWatch;
		}

		public readonly struct Args {
			public             NavGraph Graph         { get; }
			[CanBeNull] public Action   OnCompleteCtx { get; }
			public             bool     IsAsync       { get; }

			public Args(NavGraph graph, [CanBeNull] Action onCompleteCtx, bool isAsync) {
				Graph         = graph;
				IsAsync       = isAsync;
				OnCompleteCtx = onCompleteCtx;
			}
		}

		const string PATHFINDING_NODES = "Total pathfinding nodes: ";
	}
}