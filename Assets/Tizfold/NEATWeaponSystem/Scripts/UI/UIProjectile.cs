using Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.ProjectileStatePattern;

namespace Tizfold.NEATWeaponSystem.Scripts.UI {
    public class UIProjectile : Projectile {
        // public CanvasDemoWeapon DemoWeapon;
        // private void Awake()
        // {
        //     _camera = Camera.main;
        //     Rigidbody = GetComponent<Rigidbody2D>();
        //     _spriteRenderer = GetComponent<SpriteRenderer>();
        //     _pivotPoint = transform.Find("PivotPoint");
        //     Damage = 5f;
        //     IsUI = true;
        //     StateMachine = new ProjectileStateMachine(this);
        // }
        //
        // private void Start()
        // {
        //     _inputArr = Box.InputSignalArray;
        //     _outputArr = Box.OutputSignalArray;
        //     
        //     StateMachine.Initialize(StateMachine.StraightFlight);
        //     
        //     _birthTime = Time.time;
        //     DemoWeapon.OnChangeDemoWeaponEvent += ImmediateDestroy;
        // }
        //
        // private void OnDestroy() {
        //     DemoWeapon.OnChangeDemoWeaponEvent -= ImmediateDestroy;
        // }
        //
        // private void Update() {
        //     StateMachine.Update();
        // }
        //
        // private void FixedUpdate() {
        //     CalcProjectileStats();
        //     StateMachine.FixedUpdate();
        //     transform.up = Rigidbody.velocity;
        //     // CheckCollision();
        // }

        private void LateUpdate() {
            StateMachine.LateUpdate();
        }

        private void ImmediateDestroy() {
            Destroy(gameObject);
        }
    
    }
}
