using System;
using NeatProjectiles.Core.Scripts.SODefinitions;
using NeatProjectiles.Core.Scripts.WeaponSystem;
using NeatProjectiles.Core.Scripts.WeaponSystem.ProjectileStatePattern;
using SharpNeat.Phenomes;
using Unity.Mathematics;
using UnityEngine;

namespace NeatProjectiles.WeaponDemo.Scripts {
    public class ForceArrow : MonoBehaviour {

        // Assign from another script
        [SerializeField] public WeaponParams WeaponParamsLocal;
        [HideInInspector] public Transform OriginTransform;
        public float SignX;
        public float SignY;
        public IBlackBox Box;
        public ISignalArray InputArr;
        public ISignalArray OutputArr;
        [HideInInspector] public ArrowSpawner ArrowSpawner;

        public SpriteRenderer[] SpriteRenderers;
        private void Awake() {
            SpriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
        }
        private void Start() {
            foreach (SpriteRenderer sr in SpriteRenderers) 
                sr.enabled = false;
        }

        public void ActivateBlackBoxCircle() {
            Box.ResetState();

            float maxDistance = WeaponParamsLocal.NNControlDistance * math.SQRT2;

            InputArr[0] = Mathf.Lerp(-1f, 1f, Math.Abs(RelativePos.x) / WeaponParamsLocal.NNControlDistance);
            InputArr[1] = Mathf.Lerp(-1f, 1f, Math.Abs(RelativePos.y) / WeaponParamsLocal.NNControlDistance);
            InputArr[2] = Mathf.Lerp(-1f, 1f, DistanceFromOrigin / maxDistance);

            Box.Activate();
        }

        public void ActivateBlackBoxRect() {
            Box.ResetState();

            float maxX = WeaponParamsLocal.RectDimensions.x * WeaponParamsLocal.NNControlDistance;
            float maxY = WeaponParamsLocal.RectDimensions.y * WeaponParamsLocal.NNControlDistance;
            float maxDistance = maxX > maxY ? maxX : maxY;

            InputArr[0] = Mathf.Lerp(-1f, 1f, Math.Abs(RelativePos.x) / maxX);
            InputArr[1] = Mathf.Lerp(-1f, 1f, Math.Abs(RelativePos.y) / maxY);
            InputArr[2] = Mathf.Lerp(-1f, 1f, DistanceFromOrigin / maxDistance);

            Box.Activate();
        }

        public void ActivateBlackBoxPolar() {
            Box.ResetState();

            float maxPhiRad = WeaponParamsLocal.MaxPolarAngleDeg * Mathf.Deg2Rad;
            float NNControlDistance = WeaponParamsLocal.NNControlDistance;

            float x = Math.Abs(DistanceFromOrigin * Mathf.Sin(PhiRad));
            float y = Math.Abs(DistanceFromOrigin * Mathf.Cos(PhiRad));

            float maxX = maxPhiRad * Mathf.Rad2Deg >= 90f ? NNControlDistance : NNControlDistance * Mathf.Sin(maxPhiRad);

            InputArr[0] = Mathf.Lerp(-1f, 1f, x / maxX);
            InputArr[1] = Mathf.Lerp(-1f, 1f, y / NNControlDistance);
            InputArr[2] = Mathf.Lerp(-1f, 1f, DistanceFromOrigin / NNControlDistance);

            Box.Activate();
        }

        private float _hue;
        private Vector2 _forceDir;
        public void ReadDataFromBlackBox() {
            float x = Mathf.Lerp(-1f, 1f, (float)OutputArr[0]) * SignX;
            float y = Mathf.Lerp(-1f, 1f, (float)OutputArr[1]) * SignY;

            switch (WeaponParamsLocal.ReadMode) {
                case ReadMode.Default:
                    _forceDir = OriginTransform.TransformDirection(x, y, 0f);
                    break;

                case ReadMode.Rotator:
                    Vector2 rotatingDir = Vector2.Perpendicular(RelativePosDir).normalized;
                    _forceDir = x * rotatingDir + y * RelativePosDir.normalized;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            float angleDeg = Vector3.SignedAngle(transform.up, _forceDir, Vector3.forward);
            transform.rotation = Quaternion.AngleAxis(angleDeg, Vector3.forward);

            _hue = Mathf.Lerp(WeaponParamsLocal.HueRange.x, WeaponParamsLocal.HueRange.y, (float)OutputArr[2]);
            foreach (SpriteRenderer sr in SpriteRenderers) {
                sr.color = Color.HSVToRGB(_hue, WeaponParamsLocal.Saturation, WeaponParamsLocal.Brightness);
            }

            // transform.localScale = new Vector3(
            //     Mathf.Lerp(0.2f, ArrowSpawner.CellDimensions.x, (float)OutputArr[4]),
            //     Mathf.Lerp(0.3f, ArrowSpawner.CellDimensions.y, (float)OutputArr[4]),
            //     1f
            // );
        }

        public Vector2 RelativePos;
        public Vector2 RelativePosDir;
        public float DistanceFromOrigin;
        public float PhiRad;
        public void CalcProjectileStats() {
            RelativePosDir = transform.position - OriginTransform.position;
            RelativePos = transform.localPosition;
            DistanceFromOrigin = RelativePos.magnitude;
            PhiRad = Vector2.Angle(OriginTransform.up, RelativePosDir) * Mathf.Deg2Rad;
        }


    }
}