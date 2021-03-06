using UnityEngine;
using System.Collections;
using System;

	namespace RootMotion.FinalIK {

	/// <summary>
	/// %IK system for multiple branched %FABRIK chains.
	/// </summary>
	[System.Serializable]
	public class IKSolverFABRIKRoot : IKSolver {
		
		#region Main Interface
		
		/// <summary>
		/// Solver iterations.
		/// </summary>
		public int iterations = 4;
		/// <summary>
		/// The weight of all chains being pinned to root position.
		/// </summary>
		public float rootPin = 0f;
		/// <summary>
		/// The %FABRIK chains.
		/// </summary>
		public FABRIKChain[] chains = new FABRIKChain[0];
		
		public override bool IsValid(bool log) {
			if (chains.Length == 0) {
				LogWarning("IKSolverFABRIKRoot contains no chains.");
				return false;
			}

			foreach (FABRIKChain chain in chains) if (!chain.IsValid(LogWarning)) return false;
			
			return true;
		}
		
		#endregion Main Interface
		
		private bool zeroWeightApplied;
		
		protected override void OnInitiate() {
			for (int i = 0; i < chains.Length; i++) chains[i].Initiate();
		}
		
		protected override void OnUpdate() {
			if (IKPositionWeight <= 0 && zeroWeightApplied) return;
			
			// Set weight of all IK solvers
			for (int i = 0; i < chains.Length; i++) chains[i].SetWeight(IKPositionWeight);
			
			if (IKPositionWeight <= 0) {
				zeroWeightApplied = true;
				return;
			}
			
			zeroWeightApplied = false;
			
			for (int i = 0; i < iterations; i++) {
				// Solve trees from their targets
				for (int c = 0; c < chains.Length; c++) chains[c].Stage1();
				
				// Get centroid of all tree roots
				Vector3 centroid = GetCentroid();

				// Start all trees from the centroid
				for (int c = 0; c < chains.Length; c++) chains[c].Stage2(centroid);
			}
		}
		
		public override IKSolver.Point[] GetPoints() {
			IKSolver.Point[] array = new IKSolver.Point[0];
			for (int i = 0; i < chains.Length; i++) AddPointsToArray(ref array, chains[i]);
			return array;
		}
		
		public override IKSolver.Point GetPoint(Transform transform) {
			IKSolver.Point p = null;
			for (int i = 0; i < chains.Length; i++) {
				p = chains[i].ik.solver.GetPoint(transform);
				if (p != null) return p;
			}
			
			return null;
		}
		
		private void AddPointsToArray(ref IKSolver.Point[] array, FABRIKChain chain) {
			IKSolver.Point[] chainArray = chain.ik.solver.GetPoints();
			Array.Resize(ref array, array.Length + chainArray.Length);
			
			int a = 0;
			for (int i = array.Length - chainArray.Length; i < array.Length; i++) {
				array[i] = chainArray[a];
				a++;
			}
		}
		
		/*
		 * Gets the centroid position of all chains respective of their pull weights
		 * */
		private Vector3 GetCentroid() {		
			Vector3 centroid = root.position;
			if (rootPin >= 1) return centroid;

			float pullSum = 0f;
			for (int i = 0; i < chains.Length; i++) pullSum += chains[i].pull;
			
			for (int i = 0; i < chains.Length; i++) {
				if (pullSum > 0) centroid += (chains[i].ik.solver.bones[0].solverPosition - root.position) * (chains[i].pull / Mathf.Clamp(pullSum, 1f, pullSum));
			}

			return Vector3.Lerp(centroid, root.position, rootPin);
		}
	}
}
