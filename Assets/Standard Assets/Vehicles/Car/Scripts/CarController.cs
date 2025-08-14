using System;
using UnityEngine;

#pragma warning disable 649
namespace UnityStandardAssets.Vehicles.Car
{
    internal enum CarDriveType
    {
        FrontWheelDrive,
        RearWheelDrive,
        FourWheelDrive
    }

    internal enum SpeedType
    {
        MPH,
        KPH
    }

    public class CarController : MonoBehaviour
    {
        [SerializeField] private CarDriveType m_CarDriveType = CarDriveType.FourWheelDrive;
        [SerializeField] private WheelCollider[] m_WheelColliders = new WheelCollider[4];
        [SerializeField] private GameObject[] m_WheelMeshes = new GameObject[4];
        [SerializeField] private WheelEffects[] m_WheelEffects = new WheelEffects[4];
        [SerializeField] private Vector3 m_CentreOfMassOffset;
        [SerializeField] private float m_MaximumSteerAngle;
        [Range(0, 1)][SerializeField] private float m_SteerHelper;
        [Range(0, 1)][SerializeField] private float m_TractionControl;
        [SerializeField] private float m_FullTorqueOverAllWheels;
        [SerializeField] private float m_ReverseTorque;
        [SerializeField] private float m_MaxHandbrakeTorque;
        [SerializeField] private float m_Downforce = 100f;
        [SerializeField] private SpeedType m_SpeedType;
        [SerializeField] public static float m_Topspeed = 200;
        [SerializeField] private static int NoOfGears = 5;
        [SerializeField] private float m_RevRangeBoundary = 1f;
        [SerializeField] private float m_SlipLimit;
        [SerializeField] private float m_BrakeTorque;

        private Quaternion[] m_WheelMeshLocalRotations;
        private Vector3 m_Prevpos, m_Pos;
        private float m_SteerAngle;
        private int m_GearNum;
        private float m_GearFactor;
        private float m_OldRotation;
        private float m_CurrentTorque;
        private Rigidbody m_Rigidbody;
        private const float k_ReversingThreshold = 0.01f;

        public bool Skidding { get; private set; }
        public float BrakeInput { get; private set; }
        public float CurrentSteerAngle { get { return m_SteerAngle; } }
        public float CurrentSpeed { get { return m_Rigidbody != null ? m_Rigidbody.linearVelocity.magnitude * 2.23693629f : 0f; } }
        public float MaxSpeed { get { return m_Topspeed; } }
        public float Revs { get; private set; }
        public float AccelInput { get; private set; }

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            if (m_Rigidbody == null)
                Debug.LogError("CarController requires a Rigidbody component!");
        }

        private void Start()
        {
            CarController.m_Topspeed = 200;
            m_WheelMeshLocalRotations = new Quaternion[4];
            for (int i = 0; i < 4; i++)
            {
                if (m_WheelMeshes[i] != null)
                    m_WheelMeshLocalRotations[i] = m_WheelMeshes[i].transform.localRotation;
            }

            if (m_WheelColliders[0] != null && m_WheelColliders[0].attachedRigidbody != null)
                m_WheelColliders[0].attachedRigidbody.centerOfMass = m_CentreOfMassOffset;

            m_MaxHandbrakeTorque = float.MaxValue;

            m_CurrentTorque = m_FullTorqueOverAllWheels - (m_TractionControl * m_FullTorqueOverAllWheels);
        }

        private void GearChanging()
        {
            float f = Mathf.Abs(CurrentSpeed / MaxSpeed);
            float upgearlimit = (1 / (float)NoOfGears) * (m_GearNum + 1);
            float downgearlimit = (1 / (float)NoOfGears) * m_GearNum;

            if (m_GearNum > 0 && f < downgearlimit)
                m_GearNum--;

            if (f > upgearlimit && (m_GearNum < (NoOfGears - 1)))
                m_GearNum++;
        }

        private static float CurveFactor(float factor)
        {
            return 1 - (1 - factor) * (1 - factor);
        }

        private static float ULerp(float from, float to, float value)
        {
            return (1.0f - value) * from + value * to;
        }

        private void CalculateGearFactor()
        {
            float f = (1 / (float)NoOfGears);
            var targetGearFactor = Mathf.InverseLerp(f * m_GearNum, f * (m_GearNum + 1), Mathf.Abs(CurrentSpeed / MaxSpeed));
            m_GearFactor = Mathf.Lerp(m_GearFactor, targetGearFactor, Time.deltaTime * 5f);
        }

        private void CalculateRevs()
        {
            CalculateGearFactor();
            var gearNumFactor = m_GearNum / (float)NoOfGears;
            var revsRangeMin = ULerp(0f, m_RevRangeBoundary, CurveFactor(gearNumFactor));
            var revsRangeMax = ULerp(m_RevRangeBoundary, 1f, gearNumFactor);
            Revs = ULerp(revsRangeMin, revsRangeMax, m_GearFactor);
        }

        public void Move(float steering, float accel, float footbrake, float handbrake)
        {
            for (int i = 0; i < 4; i++)
            {
                if (m_WheelColliders[i] == null) continue;

                Quaternion quat;
                Vector3 position;
                m_WheelColliders[i].GetWorldPose(out position, out quat);

                if (m_WheelMeshes[i] != null)
                {
                    m_WheelMeshes[i].transform.position = position;
                    m_WheelMeshes[i].transform.rotation = quat;
                }
            }

            steering = Mathf.Clamp(steering, -1, 1);
            AccelInput = accel = Mathf.Clamp(accel, 0, 1);
            BrakeInput = footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);
            handbrake = Mathf.Clamp(handbrake, 0, 1);

            m_SteerAngle = steering * m_MaximumSteerAngle;
            if (m_WheelColliders[0] != null) m_WheelColliders[0].steerAngle = m_SteerAngle;
            if (m_WheelColliders[1] != null) m_WheelColliders[1].steerAngle = m_SteerAngle;

            SteerHelper();
            ApplyDrive(accel, footbrake);
            CapSpeed();

            if (handbrake > 0f)
            {
                float hbTorque = handbrake * m_MaxHandbrakeTorque;
                if (m_WheelColliders[2] != null) m_WheelColliders[2].brakeTorque = hbTorque;
                if (m_WheelColliders[3] != null) m_WheelColliders[3].brakeTorque = hbTorque;
            }

            CalculateRevs();
            GearChanging();

            AddDownForce();
            CheckForWheelSpin();
            TractionControl();
        }

        private void CapSpeed()
        {
            if (m_Rigidbody == null) return;

            float speed = m_Rigidbody.linearVelocity.magnitude;
            switch (m_SpeedType)
            {
                case SpeedType.MPH:
                    speed *= 2.23693629f;
                    if (speed > m_Topspeed)
                        m_Rigidbody.linearVelocity = (m_Topspeed / 2.23693629f) * m_Rigidbody.linearVelocity.normalized;
                    break;

                case SpeedType.KPH:
                    speed *= 3.6f;
                    if (speed > m_Topspeed)
                        m_Rigidbody.linearVelocity = (m_Topspeed / 3.6f) * m_Rigidbody.linearVelocity.normalized;
                    break;
            }
        }

        private void ApplyDrive(float accel, float footbrake)
        {
            if (m_WheelColliders == null) return;

            float thrustTorque;
            switch (m_CarDriveType)
            {
                case CarDriveType.FourWheelDrive:
                    thrustTorque = accel * (m_CurrentTorque / 4f);
                    for (int i = 0; i < 4; i++)
                        if (m_WheelColliders[i] != null) m_WheelColliders[i].motorTorque = thrustTorque;
                    break;

                case CarDriveType.FrontWheelDrive:
                    thrustTorque = accel * (m_CurrentTorque / 2f);
                    if (m_WheelColliders[0] != null) m_WheelColliders[0].motorTorque = thrustTorque;
                    if (m_WheelColliders[1] != null) m_WheelColliders[1].motorTorque = thrustTorque;
                    break;

                case CarDriveType.RearWheelDrive:
                    thrustTorque = accel * (m_CurrentTorque / 2f);
                    if (m_WheelColliders[2] != null) m_WheelColliders[2].motorTorque = thrustTorque;
                    if (m_WheelColliders[3] != null) m_WheelColliders[3].motorTorque = thrustTorque;
                    break;
            }

            if (m_Rigidbody == null) return;

            for (int i = 0; i < 4; i++)
            {
                if (m_WheelColliders[i] == null) continue;

                if (CurrentSpeed > 5 && Vector3.Angle(transform.forward, m_Rigidbody.linearVelocity) < 50f)
                    m_WheelColliders[i].brakeTorque = m_BrakeTorque * footbrake;
                else if (footbrake > 0)
                {
                    m_WheelColliders[i].brakeTorque = 0f;
                    m_WheelColliders[i].motorTorque = -m_ReverseTorque * footbrake;
                }
            }
        }

        private void SteerHelper()
        {
            if (m_Rigidbody == null) return;

            for (int i = 0; i < 4; i++)
            {
                if (m_WheelColliders[i] == null) continue;

                WheelHit wheelhit;
                m_WheelColliders[i].GetGroundHit(out wheelhit);
                if (wheelhit.normal == Vector3.zero) return;
            }

            if (Mathf.Abs(m_OldRotation - transform.eulerAngles.y) < 10f)
            {
                var turnadjust = (transform.eulerAngles.y - m_OldRotation) * m_SteerHelper;
                Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
                m_Rigidbody.linearVelocity = velRotation * m_Rigidbody.linearVelocity;
            }
            m_OldRotation = transform.eulerAngles.y;
        }

        private void AddDownForce()
        {
            if (m_WheelColliders[0] != null && m_WheelColliders[0].attachedRigidbody != null)
                m_WheelColliders[0].attachedRigidbody.AddForce(-transform.up * m_Downforce *
                                                               m_WheelColliders[0].attachedRigidbody.linearVelocity.magnitude);
        }

        private void CheckForWheelSpin()
        {
            for (int i = 0; i < 4; i++)
            {
                if (m_WheelColliders[i] == null) continue;

                WheelHit wheelHit;
                m_WheelColliders[i].GetGroundHit(out wheelHit);

                if (Mathf.Abs(wheelHit.forwardSlip) >= m_SlipLimit || Mathf.Abs(wheelHit.sidewaysSlip) >= m_SlipLimit)
                {
                    if (m_WheelEffects[i] != null) m_WheelEffects[i].EmitTyreSmoke();

                    if (m_WheelEffects[i] != null && !AnySkidSoundPlaying())
                        m_WheelEffects[i].PlayAudio();
                    continue;
                }

                if (m_WheelEffects[i] != null && m_WheelEffects[i].PlayingAudio)
                    m_WheelEffects[i].StopAudio();

                if (m_WheelEffects[i] != null) m_WheelEffects[i].EndSkidTrail();
            }
        }

        private void TractionControl()
        {
            WheelHit wheelHit;
            switch (m_CarDriveType)
            {
                case CarDriveType.FourWheelDrive:
                    for (int i = 0; i < 4; i++)
                        if (m_WheelColliders[i] != null && m_WheelColliders[i].GetGroundHit(out wheelHit))
                            AdjustTorque(wheelHit.forwardSlip);
                    break;

                case CarDriveType.RearWheelDrive:
                    if (m_WheelColliders[2] != null && m_WheelColliders[2].GetGroundHit(out wheelHit))
                        AdjustTorque(wheelHit.forwardSlip);
                    if (m_WheelColliders[3] != null && m_WheelColliders[3].GetGroundHit(out wheelHit))
                        AdjustTorque(wheelHit.forwardSlip);
                    break;

                case CarDriveType.FrontWheelDrive:
                    if (m_WheelColliders[0] != null && m_WheelColliders[0].GetGroundHit(out wheelHit))
                        AdjustTorque(wheelHit.forwardSlip);
                    if (m_WheelColliders[1] != null && m_WheelColliders[1].GetGroundHit(out wheelHit))
                        AdjustTorque(wheelHit.forwardSlip);
                    break;
            }
        }

        private void AdjustTorque(float forwardSlip)
        {
            if (forwardSlip >= m_SlipLimit && m_CurrentTorque >= 0)
                m_CurrentTorque -= 10 * m_TractionControl;
            else
            {
                m_CurrentTorque += 10 * m_TractionControl;
                if (m_CurrentTorque > m_FullTorqueOverAllWheels)
                    m_CurrentTorque = m_FullTorqueOverAllWheels;
            }
        }

        private bool AnySkidSoundPlaying()
        {
            for (int i = 0; i < 4; i++)
                if (m_WheelEffects[i] != null && m_WheelEffects[i].PlayingAudio)
                    return true;
            return false;
        }
    }
}
