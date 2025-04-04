
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class WeaponShoot : MonoBehaviour
{
    public Rigidbody2D rb;

    public Camera cam;
    public Vector3 mouseposFixed;

    // Where should the bullet spawn
    public Transform bulletSpawnPoint;

    // Bullet prefab
    public GameObject bulletPrefab;

    // Reference to the layer you want the Raycast to interact with (optional)
    public LayerMask targetLayer;

    // Bullet speed
    float bulletSpeed = 10f;

    float fireRate = 0.4f;

    float damage = 1f;

    float life = 0.6f;

    public bool justFired = false;

    bool flipSprite;

    public bool usingdirShooting = false;

    AudioManage audioManager;

    private Vector2 lastMovementDirection;
    Quaternion oldRotation;
    Quaternion angleFixed;

    Vector2 shootdir;

    public ItemBuffs Buffs;

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManage>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        Buffs = GameObject.FindWithTag("buffs").GetComponent<ItemBuffs>();
    }

    void Update()
    {

        if (usingdirShooting)
        {
            // Get the player's current movement direction (assuming it's based on velocity)
            Vector2 currentDirection = rb.velocity;

            // Update lastMovementDirection if the player is moving
            if (currentDirection != Vector2.zero)
            {
                lastMovementDirection = currentDirection.normalized;
            }

            // Check if the player presses the fire button (usually the left mouse button or a specific key)
            if (Input.GetButtonDown("Fire1") && !justFired)
            {
                Shoot();
                justFired = true;
                StartCoroutine(WaitAndAllowShoot((fireRate*Buffs.firerateChange)/Buffs.firerateChangeDiv));
            }
        }
        else
        {
            if (!justFired)
            {
                if (Input.GetKey(KeyCode.UpArrow)) { shootdir = Vector2.up; flipSprite = false; }
                else if (Input.GetKey(KeyCode.DownArrow)) { shootdir = Vector2.down; flipSprite = false; }
                else if (Input.GetKey(KeyCode.LeftArrow)) { shootdir = Vector2.left; flipSprite = false; }
                else if (Input.GetKey(KeyCode.RightArrow))  { shootdir = Vector2.right; flipSprite = true; }

                if (shootdir != Vector2.zero)
                {
                    justFired = true;
                    audioManager.PlaySFX(audioManager.playerShoot);
                    Shoot();
                    StartCoroutine(WaitAndAllowShoot((fireRate*Buffs.firerateChange)/Buffs.firerateChangeDiv));
                }
            }


        }

        void Shoot()
        {
            /*
            Vector3 mousepos = Input.mousePosition;

            Vector3 mouseposWorld = cam.ScreenToWorldPoint(mousepos);

            mouseposFixed = new Vector3(mouseposWorld.x, mouseposWorld.y, 0);

            // Calculate the direction from the player to the mouse
            Vector3 direction = mouseposFixed - bulletSpawnPoint.transform.position;

            // Calculate the angle in degrees cuz u know
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            //why the fyuck does it need to bee a quaternion - ahhhh:-(
            quaternion angleFixed = Quaternion.Euler(0, 0, angle);


            old code for 360 shooting
            */

            if (usingdirShooting)
            {
                // Calculate the angle from the last movement direction
                float angle = Mathf.Atan2(lastMovementDirection.y, lastMovementDirection.x) * Mathf.Rad2Deg;

                // Create a rotation quaternion based on this angle
                angleFixed = Quaternion.Euler(0, 0, angle);
            }
            else if (!usingdirShooting)
            {
                // Calculate the angle from the last movement direction
                float angle = Mathf.Atan2(-shootdir.y, -shootdir.x) * Mathf.Rad2Deg;

                // Create a rotation quaternion based on this angle
                angleFixed = Quaternion.Euler(0, 0, angle + (Random.Range(-2f, 2f) * Buffs.acuraty));

                shootdir = Vector2.zero;
            }

            // Spawn the bullet object
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, angleFixed);

            // Attach a script to the bullet to handle movement and raycast and cool shit
            bullet.AddComponent<BulletMovement>().Initialize(bulletSpeed*Buffs.bulletSpeed, targetLayer, (damage*Buffs.damageChange)/Buffs.damageChangeDiv, life*Buffs.rangeChange, flipSprite);
            bullet.transform.localScale = new Vector3((0.2f+Buffs.damageChange / 4), (0.2f + Buffs.damageChange / 4), (0.2f + Buffs.damageChange/4));
        }
        IEnumerator WaitAndAllowShoot(float tTime)
        {
            yield return new WaitForSeconds(tTime);
            justFired = false;
        }
    }

    public class BulletMovement : MonoBehaviour
    {
        private float bulletSpeed;
        private float damage;
        private float life;
        private LayerMask targetLayer;
        private Vector2 previousPosition;
        public Enemybase enemybase;
        
        public SpriteRenderer spriteRenderer;

        // Initialize the bullet's speed and target layer
        public void Initialize(float speed, LayerMask layer, float newdamage, float tlife, bool tflipSprite)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.flipY = tflipSprite;
            bulletSpeed = speed;
            targetLayer = layer;
            damage = newdamage;
            life = tlife;
            previousPosition = transform.position;
            StartCoroutine(LifeTime());
        }

        void Update()
        {
            // Calculate the current position
            Vector2 currentPosition = transform.position;

            // Calculate the direction the bullet is moving in
            Vector2 direction = (currentPosition - previousPosition).normalized;

            // Perform the Raycast from the previous position to the current position
            RaycastHit2D hit = Physics2D.Raycast(previousPosition, direction, Vector2.Distance(previousPosition, currentPosition), targetLayer);

            // Check if the Raycast hits something
            if (hit.collider != null)
            {
                // Debug log to show what was hit
                Debug.Log("Hit: " + hit.collider.name);
                if (hit.collider.CompareTag("enemy") || hit.collider.CompareTag("enemyObs"))
                {
                    enemybase = hit.collider.gameObject.GetComponent<Enemybase>();
                    if (enemybase != null)
                    {
                        enemybase.TakeDamage(damage);
                    }
                }

                // Destroy the bullet
                Destroy(gameObject);

                // Optionally destroy the object that was hit (if applicable)
                // Destroy(hit.collider.gameObject);
            }

            // Visualize the Raycast in the editor for debugging purposes
            Debug.DrawRay(previousPosition, direction * Vector2.Distance(previousPosition, currentPosition), Color.red, 1f);

            // Update the previous position to the current position
            previousPosition = currentPosition;

            // Move the bullet forward
            transform.Translate(Vector2.left * bulletSpeed * Time.deltaTime);
        }
        IEnumerator LifeTime()
        {
            yield return new WaitForSeconds(life);
            Destroy(gameObject);
        }
    }
 
}