using UnityEngine;

public class KucelController : MonoBehaviour
{
    [Header("Pergerakan Kucel")]
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("Skill Sembunyi / Muka Melas")]
    public float melasRadius = 2.5f;
    public float skillCooldown = 5f;
    private float currentCooldown = 0f;

    [Header("UI Menang")]
    public GameObject winPanel;

    // --- VARIABEL KONTROL MOBILE ---
    private bool isMoveLeft, isMoveRight, isMoveUp, isMoveDown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Membaca Input Keyboard (Untuk PC)
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (moveInput.magnitude > 0) Debug.Log("Kucel sedang berusaha jalan!");

        // 2. Menimpa dengan Input Mobile (Jika tombol layar ditekan)
        if (isMoveLeft) inputX = -1f;
        if (isMoveRight) inputX = 1f;
        if (isMoveUp) inputY = 1f;
        if (isMoveDown) inputY = -1f;

        moveInput = new Vector2(inputX, inputY);

        // 3. Mengatur Hadapan Kucel (Flip Sprite)
        if (moveInput.x > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput.x < 0) transform.localScale = new Vector3(-1, 1, 1);

        // 4. Cooldown Skill
        if (currentCooldown > 0) currentCooldown -= Time.deltaTime;

        // 5. Input Keyboard untuk Skill Melas (Spasi)
        if (Input.GetKeyDown(KeyCode.Space) && currentCooldown <= 0)
        {
            TombolSembunyiMelas();
        }
    }

    void FixedUpdate()
    {
        // Menggerakkan fisik Kucel
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    // ==========================================
    // FUNGSI UNTUK TOMBOL MOBILE (TOUCH SCREEN)
    // ==========================================
    public void PointerDownLeft() { isMoveLeft = true; }
    public void PointerUpLeft() { isMoveLeft = false; }

    public void PointerDownRight() { isMoveRight = true; }
    public void PointerUpRight() { isMoveRight = false; }

    public void PointerDownUp() { isMoveUp = true; }
    public void PointerUpUp() { isMoveUp = false; }

    public void PointerDownDown() { isMoveDown = true; }
    public void PointerUpDown() { isMoveDown = false; }

    public void TombolSembunyiMelas() // Tombol Skill
    {
        if (currentCooldown <= 0)
        {
            Debug.Log("Kucel: *Sembunyi / Pasang muka melas*");
            currentCooldown = skillCooldown; 
            Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, melasRadius);
            foreach (Collider2D obj in objectsInRange)
            {
                if (obj.CompareTag("Enemy"))
                {
                    RivalCatFSM rival = obj.GetComponent<RivalCatFSM>();
                    if (rival != null) rival.KenaJurusMelas();
                }
            }
        }
    }

    public void TombolJump() // Tombol Lompat
    {
        Debug.Log("Kucel melompat! (Fitur lompat aktif)");
        // Karena game top-down, lompat biasanya berupa 'Dash' ke depan.
        // Nanti kita tambahkan fisika lompatnya jika rintangan sudah siap.
    }

    // --- FITUR MAKAN UBI ---
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ubi"))
        {
            Destroy(collision.gameObject); 
            if (winPanel != null) winPanel.SetActive(true);
            Time.timeScale = 0f; 
        }
    }
}