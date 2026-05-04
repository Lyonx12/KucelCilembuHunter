using UnityEngine;

public class KucelController : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("UI Menang")]
    public GameObject winPanel; // Tempat memasukkan WinPanel dari Unity

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput.x > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput.x < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    // --- FITUR BARU: MAKAN UBI ---
    // Fungsi ini dipanggil otomatis oleh Unity jika Kucel menyentuh objek ber-Trigger
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Cek apakah yang disentuh itu punya Tag "Ubi"
        if (collision.CompareTag("Ubi"))
        {
            // Hilangkan ubinya (dimakan)
            Destroy(collision.gameObject);
            
            // Panggil fungsi menang
            MisiSelesai();
        }
    }

    void MisiSelesai()
    {
        Debug.Log("Nyam! Ubi Cilembu mantap.");
        
        // Munculkan layar menang
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        // Hentikan semua pergerakan (Kucel langsung tidur)
        Time.timeScale = 0f;
    }
}