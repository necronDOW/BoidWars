using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {

    public float speed;
    public GameObject Spawn_Point;

    private int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    private float camRayLength = 1000f;          // The length of the ray from the camera into the scene.
    public int spawnDelay;
    private Rigidbody _Player;

    public Object shot;
    public float fireRate = 0.8f;
    public int shotCount;

    private float nextFire;
    private bool m_isAxisInUse = false;
    public int index = -1;

    public CameraController camScript;
    public GameObject cam;

    public int health;
    private int _respawnTimer;
    private int healthstart;
    private int startindex;
    private bool dead;

    void Awake()
    {
        healthstart = health;
        _respawnTimer = 0;
        shotCount = 4;
        fireRate = 0.8f;
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor");

        _Player = GetComponent<Rigidbody>();
        _Player.drag = 2.5f;

        cam = GameObject.FindGameObjectWithTag("MainCamera");
        camScript = cam.GetComponent<CameraController>();
        
        index = Manager.Instance.NewTeam(gameObject);
        startindex = index;

        //use index to grab correct camera
        switch (index)
        {
            case 0:
                cam = GameObject.FindGameObjectWithTag("MainCamera");
                camScript = cam.GetComponent<CameraController>();
                break;
            case 1:
                cam = GameObject.FindGameObjectWithTag("Player2_cam");
                camScript = cam.GetComponent<CameraController>();
                break;
        }
    }

    void Update()
    { 

        if (index != 0 && dead == false)
        {
            if (Input.GetAxis("SHOOT") != 0 && Time.time > nextFire)
                Shoot(0.01f, 3.0f);
        }

        if (index == 0 && dead == false)
        {
            if (Input.GetAxis("Fire1") != 0 && Time.time > nextFire)
                Shoot(0.01f, 3.0f);
        }

        if (health <= 0)
        {
            dead = true;
            _Player.velocity = Vector3.zero;
            transform.position = Spawn_Point.transform.position;

            _respawnTimer++;
            if (_respawnTimer >= spawnDelay)
            {
                Respawn();
                _respawnTimer = 0;
            }
        }

        transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
    }

    void FixedUpdate()
    {
        float moveHorizontal = 0;
        float moveVertical = 0;
        // Controller
        if (index != 0)
        {
            moveHorizontal = Input.GetAxis("HorizontalJ");
            moveVertical = Input.GetAxis("VerticalJ");
            
            Vector3 InputDirection = Vector3.zero;
            InputDirection.x = Input.GetAxis("Horizontal1");
            InputDirection.z = Input.GetAxis("Vertical1");

            _Player.transform.LookAt(_Player.transform.position + InputDirection);
        }
        //Mouse and Keyboard
        if (index == 0)
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");

            Turning();
        }

        Vector3 movement = new Vector3(moveHorizontal * speed, 0.0f, moveVertical * speed);

        _Player.AddForce(movement);

    }

    void Turning()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse *= -1;
            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
      
            // Set the player's rotation to this new rotation.
            _Player.MoveRotation(newRotation);
        }
    }

    private void Respawn()
    {
        dead = false;
        health = healthstart;
        index = startindex;

    }

    private void Shoot(float shake, float recoil)
    {
        // Call your event function here.
        nextFire = Time.time + fireRate;
        for (int i = 0; i < shotCount; i++)
        {
            GameObject bullet = (GameObject)Instantiate(shot, transform.position - transform.forward, transform.rotation);
            bullet.GetComponentInChildren<Shot>().team = index;
        }       
       
        camScript.Shake(shake, 0.002f);

        Vector3 velRef = Vector3.zero;
        GetComponent<Rigidbody>().AddForce(transform.forward * recoil, ForceMode.Impulse);
    }
}
