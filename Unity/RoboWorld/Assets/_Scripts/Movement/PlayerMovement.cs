using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Références")]
    public Transform cameraTransform;
    public Image barreEnduranceUI;
    public Animator animatorRobot;
    private Rigidbody rb;

    [Header("Mouvements")]
    public float vitesseMarche = 8f;
    public float vitesseSprint = 13f;
    public float turnSmoothTime = 0.1f;
    public float maxSlopeAngle = 50f;
    float turnSmoothVelocity;

    [Header("Physique (Saut & Gravité)")]
    public float forceSaut = 16f;
    public float gravityMultiplier = 2.5f;
    public float fallMultiplier = 2f;
    public float groundDrag = 6f;
    public float airDrag = 1f;
    public int maxJumpCount = 2;
    private int currentJumpCount;
    private float lastJumpTime;

    [Header("Système Escaliers")]
    public float stepHeight = 0.5f;
    public float stepSmooth = 0.2f;

    [Header("Endurance")]
    public float enduranceMax = 100f;
    public float enduranceActuelle;
    private bool estEpuise = false;

    [Header("Détection Sol")]
    public LayerMask layerSol;
    public float distanceSol = 0.9f;

    bool estAuSol;
    RaycastHit slopeHit;

    // KICKBACK
    private float knockbackTimer;

    public void ApplyKnockback(Vector3 force)
    {
        knockbackTimer = 0.2f; // On bloque les controlles pendant 0.2s
        rb.AddForce(force, ForceMode.Impulse);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;
        enduranceActuelle = enduranceMax;

        // CORRECTION 2 : Empêcher de s'accrocher aux murs (Friction 0)
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            PhysicMaterial zeroFrictionMat = new PhysicMaterial();
            zeroFrictionMat.dynamicFriction = 0f;
            zeroFrictionMat.staticFriction = 0f;
            zeroFrictionMat.frictionCombine = PhysicMaterialCombine.Minimum;
            zeroFrictionMat.name = "ZeroFriction_Auto";
            col.material = zeroFrictionMat;
        }
    }

    void Update()
    {
        estAuSol = Physics.Raycast(transform.position, Vector3.down, distanceSol + 0.2f, layerSol, QueryTriggerInteraction.Ignore);

        rb.drag = estAuSol ? groundDrag : airDrag;

        // Reset saut
        if (estAuSol && Time.time - lastJumpTime > 0.2f)
        {
            currentJumpCount = 0;
        }

        // SAUT
        if (Input.GetButtonDown("Jump"))
        {
            if (estAuSol || currentJumpCount < maxJumpCount)
            {
                estAuSol = false;
                lastJumpTime = Time.time;
                currentJumpCount++;

                rb.velocity = new Vector3(rb.velocity.x, Mathf.Max(rb.velocity.y, 0), rb.velocity.z);
                rb.AddForce(Vector3.up * forceSaut, ForceMode.VelocityChange);
                if (animatorRobot) 
                {
                    animatorRobot.ResetTrigger("Jump");
                    animatorRobot.SetTrigger("Jump");
                }
            }
        }

        // Saut variable
        if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // Gestion du timer de knockback
        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
        }
        else
        {
            Mouvement();
            ControlSpeed();
        }

        if (!estAuSol)
        {
            rb.AddForce(Physics.gravity * (gravityMultiplier - 1), ForceMode.Acceleration);
        }
    }

    void Mouvement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(x, 0f, z).normalized;

        // ENDURANCE
        bool toucheSprint = Input.GetKey(KeyCode.LeftShift);
        if (enduranceActuelle <= 0) estEpuise = true;
        if (enduranceActuelle >= enduranceMax * 0.1f) estEpuise = false;

        bool peutSprinter = toucheSprint && !estEpuise && direction.magnitude >= 0.1f;
        float vitesseActuelle = peutSprinter ? vitesseSprint : vitesseMarche;

        if (peutSprinter) enduranceActuelle -= 15 * Time.deltaTime;
        else enduranceActuelle += 10 * Time.deltaTime;

        enduranceActuelle = Mathf.Clamp(enduranceActuelle, 0, enduranceMax);
        if (barreEnduranceUI) barreEnduranceUI.fillAmount = enduranceActuelle / enduranceMax;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (OnSlope())
            {
                moveDir = GetSlopeMoveDirection(moveDir);
            }

            if (estAuSol)
            {
                rb.AddForce(moveDir * vitesseActuelle, ForceMode.VelocityChange);
            }
            else
            {
                // Anti-Stick : On ne pousse pas si un mur est juste devant (0.6f car Rayon Capsule ~0.5f)
                if (!Physics.Raycast(transform.position, moveDir, 0.7f, layerSol))
                {
                    rb.AddForce(moveDir * vitesseActuelle * 0.4f, ForceMode.VelocityChange);
                }
            }
        }

        // ANIMATION
        if (animatorRobot)
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            animatorRobot.SetBool("isRunning", flatVel.magnitude > 0.1f);
            animatorRobot.SetFloat("VitesseMulti", flatVel.magnitude / vitesseMarche);
            animatorRobot.SetBool("isGrounded", estAuSol);
        }
    }

    void ControlSpeed()
    {
        bool sprint = Input.GetKey(KeyCode.LeftShift) && !estEpuise;
        float targetSpeed = sprint ? vitesseSprint : vitesseMarche;

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > targetSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * targetSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, distanceSol + 0.3f, layerSol, QueryTriggerInteraction.Ignore))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle > 0 && angle < maxSlopeAngle;
        }
        return false;
    }

    Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}
