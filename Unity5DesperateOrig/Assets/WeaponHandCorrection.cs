using UnityEngine;
using System.Collections;

public class WeaponHandCorrection : MonoBehaviour
{


    public Transform WeaponCorrector;
    public float ReloadCorrection;
    public float WeightPosition = 1f;
    public float WeightRotation = 0f;
    private Animator _animator;
    private float _weightCorrector;
    public float DellayTimmer = 0.4f;
    private float _timmer;

    public static WeaponHandCorrection entity { get; set; }
    // Use this for initialization
    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Weapon.entity && Weapon.entity.HandCorrector != WeaponCorrector)
        {
            WeaponCorrector = Weapon.entity.HandCorrector;
            ReloadCorrection = Weapon.entity.ReloadCorrectionWeight;
        }
    }

    private void OnAnimatorIK()
    {
        if (WeaponCorrector != null)
        {
            //_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, WeightPosition);
            //_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, WeightRotation);

            //_animator.SetIKPosition(AvatarIKGoal.LeftHand, WeaponCorrector.position);
            //_animator.SetIKRotation(AvatarIKGoal.LeftHand, WeaponCorrector.rotation);

            // if (!Weapon.entity.isReload && WeaponCorrector != null)
            if (!Weapon.entity.isReload && WeaponCorrector != null)
            {

                if (_weightCorrector < WeightPosition) _weightCorrector += 0.01f;
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, _weightCorrector);
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, WeightRotation);

                _animator.SetIKPosition(AvatarIKGoal.LeftHand, WeaponCorrector.position);
                _animator.SetIKRotation(AvatarIKGoal.LeftHand, WeaponCorrector.rotation);
            }
            else
            {

                if (_weightCorrector != ReloadCorrection)
                {
                    _weightCorrector -= 0.02f;
                    if (_weightCorrector > ReloadCorrection) _weightCorrector = ReloadCorrection;
                    if (_weightCorrector < 0) _weightCorrector = ReloadCorrection;
                }

                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, _weightCorrector);
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                _animator.SetIKPosition(AvatarIKGoal.LeftHand, WeaponCorrector.position);
                _animator.SetIKRotation(AvatarIKGoal.LeftHand, WeaponCorrector.rotation);
            }
        }
    }
}
