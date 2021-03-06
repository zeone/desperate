using UnityEngine;
using System.Collections;

namespace RootMotion.FinalIK {

	/// <summary>
	/// Maps a single bone to a node in %IK Solver
	/// </summary>
	[System.Serializable]
	public class IKMappingBone: IKMapping {
		
		#region Main Interface
		
		/// <summary>
		/// The bone transform.
		/// </summary>
		public Transform bone;
		
		/// <summary>
		/// The weight of maintaining the bone's rotation after solver has finished.
		/// </summary>
		public float maintainRotationWeight = 1f;
		
		/// <summary>
		/// Determines whether this IKMappingBone is valid.
		/// </summary>
		public override bool IsValid(IKSolver solver, Warning.Logger logger = null) {
			if (!base.IsValid(solver, logger)) return false;
			
			if (bone == null) {
				if (logger != null) logger("IKMappingBone's bone is null.");
				return false;
			}

			return true;
		}
		
		#endregion Main Interface
		
		private BoneMap boneMap = new BoneMap();
		
		public IKMappingBone() {}
		
		public IKMappingBone(Transform bone) {
			this.bone = bone;
		}
		
		/*
		 * Initiating and setting defaults
		 * */
		protected override void OnInitiate() {
			boneMap.Initiate(bone, solver);
		}
		
		/*
		 * Pre-solving
		 * */
		public override void ReadPose() {
			boneMap.MaintainRotation();
		}
		
		public override void WritePose() {
			// Rotating back to the last maintained rotation
			boneMap.RotateToMaintain(solver.GetIKPositionWeight() * maintainRotationWeight);
		}
	}
}
