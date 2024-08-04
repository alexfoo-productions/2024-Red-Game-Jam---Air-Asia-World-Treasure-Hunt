using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    public class ObjectPreviewer : MonoBehaviour
    {
        [SerializeField] Transform pivotYaw;
        [SerializeField] Transform pivotPitch;
        [SerializeField] Transform pivotRoll;
        [SerializeField] Transform pivotDolly;

        [SerializeField] bool invertOrbitY;                
        [SerializeField] Vector2 yawLimits;
        [SerializeField] Vector2 pitchLimits;
        [SerializeField] Vector2 dollyLimits;

        public float orbitAmountMultiplier;
        public float dollyAmountMultipler;
        public float FollowRotationSpeed = 1f;
        public float FollowDollySpeed = 1f;


        float _orbitMultiplier;
        float pivotYawRotation;
        float pivotPitchRotation;        
        Quaternion YawGoal;
        Quaternion YawOriginal;
        Quaternion PitchGoal;
        Quaternion PitchOriginal;
        Vector3 DollyGoal;
        Vector3 DollyOriginal;
        
        public void init()
        {
            DollyGoal = DollyOriginal = pivotDolly.localPosition;
            YawOriginal = pivotYaw.localRotation;
            PitchOriginal = pivotPitch.localRotation;

            pivotYawRotation = pivotYaw.localRotation.eulerAngles.y;
            pivotPitchRotation = pivotPitch.localRotation.eulerAngles.x;

            updateOrbitMultiplier();

            //LogUtil.Log(WebglUtils.CheckIfMobile());
            //LogUtil.Log(Screen.dpi);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public void updateOrbitMultiplier()
        {
            _orbitMultiplier = 96f / Screen.dpi * orbitAmountMultiplier;                
        }

        public void dolly(float _dollyAmount)
        {
            //pivotDollyAmount.z = _dollyAmount;

            ////if (dollyAmountLast == pivotDollyAmount.z)
            ////  return;

            //pivotDolly.Translate(pivotDollyAmount * dollyAmountMultiplier * Time.deltaTime);
            //pivotDollyPosition.z = Mathf.Clamp(pivotDolly.localPosition.z, dollyLimits.x, dollyLimits.y);

            //pivotDolly.localPosition = pivotDollyPosition;

            //dollyAmountLast = pivotDollyAmount.z;

            DollyGoal.z += _dollyAmount * dollyAmountMultipler * Time.deltaTime;
            DollyGoal.z = Mathf.Clamp(DollyGoal.z, dollyLimits.x, dollyLimits.y);
        }

        public void orbit( Vector2 _orbitAmount )
        {
            //pivotYawAmount.y = _orbitAmount.x;
            //pivotPitchAmount.x = (invertOrbitY)?_orbitAmount.y:(-1*_orbitAmount.y);
            
            pivotYawRotation += _orbitAmount.x * _orbitMultiplier * Time.deltaTime;
            if(yawLimits != Vector2.zero)               
                pivotYawRotation = Mathf.Clamp(pivotYawRotation, yawLimits.x, yawLimits.y);

            pivotPitchRotation += (invertOrbitY) ? _orbitAmount.y : (-1 * _orbitAmount.y) * _orbitMultiplier * Time.deltaTime;
            pivotPitchRotation = Mathf.Clamp(pivotPitchRotation, pitchLimits.x, pitchLimits.y);

            //pivotYawRotation %= 360f;
            //pivotPitchRotation %= 360f;

            YawGoal = Quaternion.Euler(0, pivotYawRotation, 0);
            PitchGoal = Quaternion.Euler(pivotPitchRotation, 0, 0);

            //pivotYaw.localRotation = Quaternion.Euler(0, pivotYawRotation, 0);
            //pivotPitch.localRotation = Quaternion.Euler(pivotPitchRotation, 0, 0);
        }

        public void resetCamera()
        {
            YawGoal = YawOriginal;
            PitchGoal = PitchOriginal;
            DollyGoal = DollyOriginal;

            pivotYawRotation = YawOriginal.eulerAngles.y;
            pivotPitchRotation = PitchOriginal.eulerAngles.x;

            //pivotDollyAmount = Vector3.zero;

            //pivotYaw.localRotation = Quaternion.Euler(Vector3.zero);
            //pivotPitch.localRotation = Quaternion.Euler(Vector3.zero);

            //Vector3 dollyReset = Vector3.zero;
            //dollyReset.z = pivotDollyOriginal;
            //pivotDolly.transform.localPosition = dollyReset;
        }

        // Update is called once per frame
        void Update()
        {
            pivotYaw.localRotation = Quaternion.Slerp(pivotYaw.localRotation, YawGoal, FollowRotationSpeed);
            pivotPitch.localRotation = Quaternion.Slerp(pivotPitch.localRotation, PitchGoal, FollowRotationSpeed);

            pivotDolly.localPosition = Vector3.Lerp(pivotDolly.localPosition, DollyGoal, FollowDollySpeed);

            //pivotYaw.Rotate(pivotYawAmount * orbitAmountMultiplier * Time.deltaTime);
            //pivotPitch.Rotate(pivotPitchAmount * orbitAmountMultiplier * Time.deltaTime);

            //pivotPitchRotation = Mathf.Clamp(pivotPitch.localRotation.eulerAngles.x, pitchLimits.x, pitchLimits.y);            
            //pivotPitch.localRotation = Quaternion.Euler(pivotPitchRotation+90f, 0, 0);

            /*if (pivotPitch.localRotation.eulerAngles.x <= 90f)
            {
                pivotPitchRotation = Mathf.Clamp(pivotPitch.localRotation.eulerAngles.x, 0, pitchLimits.y);
                pivotPitch.localRotation = Quaternion.Euler(pivotPitchRotation, 0, 0);
            }
            if (pivotPitch.localRotation.eulerAngles.x >= 270f)
            {                
                pivotPitchRotation = Mathf.Clamp(pivotPitch.localRotation.eulerAngles.x, 360+pitchLimits.x, 360f);
                pivotPitch.localRotation = Quaternion.Euler(pivotPitchRotation, 0, 0);
            }*/
        }        
    }
}