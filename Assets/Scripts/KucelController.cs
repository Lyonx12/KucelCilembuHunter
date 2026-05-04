using UnityEngine;

public class KucelController : MonoBehaviour
{
    [Header("Pergerakan Kucel")]
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("Skill Muka Melas")]
    public float melasRadius = 2.5f; // Jarak jangkauan muka melas
    public float skillCooldown = 5f; // Harus nunggu 5 detik sebelum bisa melas lagi
    private float currentCooldown = 0f;

    [Header("UI Menang")]
    public GameObject winPanel;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Membaca Input Pergerakan
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput.x > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput.x < 0) transform.localScale = new Vector3(-1, 1, 1);

        // 2. Mengurus Cooldown Skill
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }

        // 3. Menekan Tombol Skill (SPASI)
        if (Input.GetKeyDown(KeyCode.Space) && currentCooldown <= 0)
        {
            GunakanMukaMelas();
        }
    }

    void FixedUpdate()
    {
        // Pergerakan Fisika Kucel
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    // --- FUNGSI SPATIAL SENSING (OVERLAP DETECTION) ---
    void GunakanMukaMelas()
    {
        Debug.Log("Kucel: *Pasang muka melas tingkat dewa*");
        currentCooldown = skillCooldown; // Mulai cooldown

        // Membuat lingkaran tak kasat mata untuk mendeteksi semua objek di sekitar Kucel
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, melasRadius);

        foreach (Collider2D obj in objectsInRange)
        {
            // Jika objek itu adalah musuh
            if (obj.CompareTag("Enemy"))
            {
                // Ambil skrip FSM musuh dan panggil fungsi KenaJurusMelas()
                RivalCatFSM rival = obj.GetComponent<RivalCatFSM>();
                if (rival != null)
                {
                    rival.KenaJurusMelas();
                    Debug.Log("Kucing Judes: 'Ih apaan sih gajelas', lalu pergi.");
                }
            }
        }
    }

    // --- FITUR MAKAN UBI ---
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ubi"))
        {
            Destroy(collision.gameObject); 
            MisiSelesai();
        }
    }

    void MisiSelesai()
    {
        Debug.Log("Nyam! Ubi Cilembu mantap.");
        
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        Time.timeScale = 0f; 
    }

    // (Opsional) Menggambar garis lingkaran radius melas di Editor Unity
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, melasRadius);
    }
}