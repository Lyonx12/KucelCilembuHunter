using UnityEngine;

public class KucelController : MonoBehaviour
{
    [Header("Pergerakan Kucel")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("Animasi Goyang Jalan")]
    public float kecepatanGoyang = 15f;
    public float sudutGoyang = 12f;     

    [Header("Skill & Jumpscare")]
    public float melasRadius = 2.5f;
    public float scareRadius = 5f;        
    public float kekuatanPentalan = 25f;  
    
    [Header("Status Stealth (Ubi)")]
    public bool sudahMakanUbi = false; 
    public Color warnaTransparan = new Color(1f, 1f, 1f, 0.5f); 
    public Color warnaNormal = Color.white; 

    [Header("Ekspresi Kucel (Aset Gambar)")]
    public SpriteRenderer sr;
    public Sprite spriteNormal;   
    public Sprite spriteMakan;    
    public Sprite spriteSembunyi; 
    public Sprite spriteKaget;    
    public Sprite spriteWin; // Masukkan gambar Kucel pas menang ke sini nanti di Inspector

    [Header("UI Menang")]
    public GameObject winPanel;

    private bool isHiding = false;
    private bool isGameOver = false;

    // --- VARIABEL KONTROL MOBILE ---
    private bool isMoveLeft, isMoveRight, isMoveUp, isMoveDown;

    void Start()
    {
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody2D>();
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        sr.sprite = spriteNormal; 
        sr.color = warnaNormal; 
    }

    void Update()
    {
        if (isGameOver || isHiding) 
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); 
            return; 
        }

        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (isMoveLeft) inputX = -1f;
        if (isMoveRight) inputX = 1f;
        if (isMoveUp) inputY = 1f;
        if (isMoveDown) inputY = -1f;

        moveInput = new Vector2(inputX, inputY);

        if (moveInput.magnitude > 0)
        {
            if (moveInput.x > 0) transform.localScale = new Vector3(1, 1, 1);
            else if (moveInput.x < 0) transform.localScale = new Vector3(-1, 1, 1);

            float angle = Mathf.Sin(Time.time * kecepatanGoyang) * sudutGoyang;
            if (moveInput.x > 0) angle -= 5f; 
            else if (moveInput.x < 0) angle += 5f; 
            
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        // Skill Sembunyi pakai Spasi (Untuk tes di PC)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TombolSembunyiMelas();
        }
    }

    void FixedUpdate()
    {
        if (isGameOver || isHiding) return;
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    // --- KONTROL MOBILE ---
    public void PointerDownLeft() { isMoveLeft = true; }
    public void PointerUpLeft() { isMoveLeft = false; }
    public void PointerDownRight() { isMoveRight = true; }
    public void PointerUpRight() { isMoveRight = false; }
    public void PointerDownUp() { isMoveUp = true; }
    public void PointerUpUp() { isMoveUp = false; }
    public void PointerDownDown() { isMoveDown = true; }
    public void PointerUpDown() { isMoveDown = false; }

    public void TombolJump() 
    { 
        if (isHiding) KeluarKardusDanKagetin();
        else Debug.Log("Kucel lompat biasa");
    }

    public void TombolSembunyiMelas() 
    {
        if (!isGameOver && !isHiding)
        {
            Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, melasRadius);
            bool dekatKardus = false;

            foreach (Collider2D obj in objectsInRange)
            {
                if (obj.CompareTag("Kardus")) { dekatKardus = true; break; }
            }

            if (dekatKardus)
            {
                if (sudahMakanUbi)
                {
                    isHiding = true;
                    sr.sprite = spriteSembunyi; 
                    sr.color = warnaNormal; 
                    Debug.Log("Berhasil masuk kardus!");

                    GameObject pico = GameObject.FindGameObjectWithTag("Enemy");
                    if (pico != null)
                    {
                        RivalCatFSM skripPico = pico.GetComponent<RivalCatFSM>();
                        if (skripPico != null) skripPico.MulaiPatroli();
                    }
                }
                else
                {
                    Debug.Log("Kucel belum makan ubi! Dia tidak berani masuk kardus karena masih kelihatan!");
                }
            }
        }
    }

    void KeluarKardusDanKagetin()
    {
        isHiding = false;
        sr.sprite = spriteKaget; 
        sr.color = warnaNormal; 
        sudahMakanUbi = false; 

        Debug.Log("Kucel: BAA! (Mencoba nakutin)");

        // 1. Cari objek Pico
        GameObject pico = GameObject.FindGameObjectWithTag("Enemy");
        
        if (pico != null)
        {
            // 2. Hitung jarak antara Kucel dan Pico
            float jarakKePico = Vector2.Distance(transform.position, pico.transform.position);

            // 3. Hanya kaget jika jaraknya kurang dari atau sama dengan scareRadius
            if (jarakKePico <= scareRadius) 
            {
                RivalCatFSM rival = pico.GetComponent<RivalCatFSM>();
                if (rival != null) 
                {
                    rival.KenaJumpscare(); 
                    Debug.Log("Pico kena serangan jantung! Dia terlalu dekat (Jarak: " + jarakKePico + ")");
                }
            }
            else
            {
                // Kalau kejauhan, Pico cuma bingung tapi tidak pingsan
                Debug.Log("Pico: 'Eh, kayak ada suara kucing... tapi jauh ah.' (Jarak: " + jarakKePico + ")");
            }
        }

        Invoke("KembaliMukaNormal", 1f); 
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // PENGAMAN: Kalau sudah menang/kalah, jangan deteksi apa-apa lagi!
        if (isGameOver) return;

        if (collision.CompareTag("Ubi") && !sudahMakanUbi)
        {
            Debug.Log("Nyam! Kucel makan Ubi!");
            sudahMakanUbi = true;
            sr.color = warnaTransparan; 
            sr.sprite = spriteMakan; 
            Invoke("KembaliMukaNormal", 0.5f); 
            Destroy(collision.gameObject); 
        }
        else if (collision.CompareTag("Portal")) 
        {
            MenangDong(); 
        }
    }

    void KembaliMukaNormal()
    {
        if (!isGameOver && !isHiding) sr.sprite = spriteNormal;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isHiding)
        {
            sr.sprite = spriteKaget; 
            isGameOver = true;
            
            rb.linearVelocity = Vector2.zero;
            moveInput = Vector2.zero;

            RivalCatFSM otakPico = collision.gameObject.GetComponent<RivalCatFSM>();
            if (otakPico != null) otakPico.KenaJumpscare(); 

            Rigidbody2D rbPico = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rbPico != null) rbPico.linearVelocity = Vector2.zero;

            Debug.Log("GAME OVER! Kucel ketahuan Pico sebelum sempat sembunyi.");
            Invoke("PauseGame", 0.5f);
        }
    }

    void PauseGame() { Time.timeScale = 0f; }

    void MenangDong()
    {
        if (isGameOver) return; // Mencegah pemanggilan ganda
        isGameOver = true; 
        
        if (spriteWin != null) sr.sprite = spriteWin; 

        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true; // Biar Kucel gak kegeser-geser lagi

        Debug.Log("Hore! Kucel menang. Menunggu Panel muncul...");
        Invoke("TampilWinPanel", 1.5f);
    }

    void TampilWinPanel()
    {
        if (winPanel != null) 
        {
            winPanel.SetActive(true); 
            Debug.Log("Win Panel AKTIF!");
        }
        else 
        {
            // Jika ini muncul, berarti kamu lupa drag objek ke Inspector
            Debug.LogError("ERROR: Win Panel belum dimasukkan ke Inspector Player_Kucel!");
        }
        
        // Hentikan waktu
        Time.timeScale = 0f; 
    }
}