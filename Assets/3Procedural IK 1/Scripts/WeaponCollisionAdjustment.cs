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

   [SerializeField] private AvatarIKGoal triggerHand = AvatarIKGoal.RightHand;
   [SerializeField] private AvatarIKGoal holdingHand = AvatarIKGoal.LeftHand; 
   [SerializeField] private Transform weaponReference;
   [SerializeField] private Transform weaponHandle;
   [SerializeField] private float weaponLength;
   [SerializeField] private float profileThickness;
   
   [SerializeField] private LayerMask collisionMask;

   private Animator animator;
   private RayResult _rayResult;
   [SerializeField]private FloatDampener _offset;
   
   Character character;
   
   private void SolveOffset()
   {
      RayResult result = new RayResult();
      result.Ray = new Ray(weaponReference.position, weaponReference.forward);
      result.Result = Physics.SphereCast(result.Ray, profileThickness, out result.HitInfo, weaponLength, collisionMask);
      _rayResult = result;
      _offset.TargetValue = Mathf.Max(0, weaponLength - Vector3.Distance(_rayResult.HitInfo.point, weaponReference.position)) * -1f;
      
   }

   private void Awake()
   {
      animator = GetComponent<Animator>();
      character = GetComponent<Character>();
   }

   private void OnAnimatorIK(int layerIndex)
   {
      _offset.Update();
      if (character.IsAiming)
      {
         Vector3 originalPosition = animator.GetIKPosition(triggerHand);
         animator.SetIKPositionWeight(triggerHand, 1);
         animator.SetIKPosition(triggerHand, originalPosition + weaponReference.forward * _offset.CurrentValue);

         animator.SetIKPositionWeight(holdingHand, 1);
         animator.SetIKPosition(holdingHand, weaponHandle.position);
      }
   }

   private void FixedUpdate()
   {
      SolveOffset();
   }
   

#if UNITY_EDITOR
   private void OnDrawGizmos()
   {
      if (!weaponReference) return;
      Gizmos.color =  _rayResult.Result ? Color.green : Color.red;
      Vector3 startPos = weaponReference.position;
      Vector3 endPos = weaponReference.position + weaponReference.forward * weaponLength;
      Gizmos.DrawWireSphere(startPos, profileThickness);
      Gizmos.DrawWireSphere(endPos, profileThickness);
      Gizmos.DrawLine(startPos, endPos);
   }
#endif
}
