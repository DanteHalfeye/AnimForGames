using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponCollisionAdjustment : MonoBehaviour
{
   struct RayResult
   {
      public Ray Ray;
      public bool Result;
      public RaycastHit HitInfo;
   }
   
   [SerializeField] private Transform handIK;
   
   [SerializeField] private Transform weaponReference;
   [SerializeField] private float weaponLength;
   [SerializeField] private float profileThickness;
   
   [SerializeField] private LayerMask collisionMask;

   private Animator animator;
   private RayResult _rayResult;
   private float _offset;
   
   private void SolveOffset()
   {
      RayResult result = new RayResult();
      result.Ray = new Ray(weaponReference.position, weaponReference.forward);
      result.Result = Physics.SphereCast(result.Ray, profileThickness, out result.HitInfo, weaponLength, collisionMask);
      _rayResult = result;
      _offset = Mathf.Max(weaponLength - Vector3.Distance(_rayResult.HitInfo.point,weaponReference.position)) * -1f;
      
   }

   private void Awake()
   {
      animator = GetComponent<Animator>();
   }

   private void OnAnimatorIK(int layerIndex)
   {
      Vector3 originalPosition = animator.GetIKPosition(AvatarIKGoal.RightHand);
      animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
      animator.SetIKPosition(AvatarIKGoal.RightHand, originalPosition + weaponReference.forward * _offset);
   }

   private void FixedUpdate()
   {
      SolveOffset();
   }

   private void Update()
   {
      handIK.Translate(transform.forward * _offset);
   }

#if UNITY_EDITOR
   private void OnDrawGizmos()
   {
      if (!weaponReference) return;
      Vector3 startPos = weaponReference.position;
      Vector3 endPos = weaponReference.position + weaponReference.forward * weaponLength;
      Gizmos.DrawWireSphere(startPos, profileThickness);
      Gizmos.DrawWireSphere(endPos, profileThickness);
      Gizmos.DrawLine(startPos, endPos);
   }
#endif
}
