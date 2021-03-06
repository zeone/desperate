using UnityEditor;
using UnityEngine;
using System.Collections;

namespace RootMotion.FinalIK {

	/*
	 * Custom inspector for LookAtIK.
	 * */
	[CustomEditor(typeof(LookAtIK))]
	public class LookAtIKInspector : IKInspector {
		
		private LookAtIK script { get { return target as LookAtIK; }}

		protected override MonoBehaviour GetMonoBehaviour(out int executionOrder) {
			executionOrder = 9997;
			return script;
		}

		protected override SerializedContent[] FindContent() {
			return IKSolverLookAtInspector.FindContent(solver);
		}
		
		protected override void OnApplyModifiedProperties() {
			if (!Application.isPlaying) script.solver.Initiate(script.transform);
		}
		
		protected override void AddInspector() {
			// Draw the inspector for IKSolverTrigonometric
			IKSolverLookAtInspector.AddInspector(solver, !Application.isPlaying, true, content);
		}	
		
		void OnSceneGUI() {
			if (Application.isPlaying && !script.isAnimated) return;

			// Draw the scene veiw helpers
			IKSolverLookAtInspector.AddScene(script.solver, new Color(0f, 1f, 1f, 1f), true);
		}
	}
}

