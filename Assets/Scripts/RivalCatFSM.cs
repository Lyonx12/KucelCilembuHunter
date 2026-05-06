using UnityEngine;

public class RivalCatFSM : MonoBehaviour
{
    public enum RivalState { Diam, Patroling, PingsanFade }
    public RivalState currentState = RivalState.Diam;

    [Header("Area Patroli")]
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 2f;
    private Transform targetWaypoint;

    [Header("Efek Pingsan & Portal")]
    public float kecepatanMemudar = 1f;  // Seberapa cepat Pico menghilang
    public GameObject portalLevel;       // Masukkan objek Portal_Win ke sini
    
    private SpriteRenderer sr;

    void Start()
    {
        targetWaypoint = pointA; 
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (currentState == RivalState.Patroling)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, patrolSpeed * Time.deltaTime);
            
            if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                targetWaypoint = targetWaypoint == pointA ? pointB : pointA;
                BalikBadan(); 
            }
        }
        else if (currentState == RivalState.PingsanFade)
        {
            // PROSES FADE AWAY (Memudar perlahan)
            Color warna = sr.color;
            warna.a -= kecepatanMemudar * Time.deltaTime; // Kurangi transparansi
            sr.color = warna;

            // Jika sudah 100% transparan (lenyap)
            if (warna.a <= 0f)
            {
                Debug.Log("Pico lenyap! Portal terbuka!");

                // 1. Pindahkan Portal tepat ke posisi Pico saat ini
                if (portalLevel != null)
                {
                    portalLevel.transform.position = transform.position;
                    portalLevel.SetActive(true); // Nyalakan portalnya!
                }

                // 2. Hancurkan objek Pico
                Destroy(gameObject);
            }
        }
    }

    void BalikBadan()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1; 
        transform.localScale = scale;
    }

    public void MulaiPatroli()
    {
        if (currentState == RivalState.Diam)
        {
            currentState = RivalState.Patroling;
            BalikBadan(); 
        }
    }

    public void KenaJumpscare()
    {
        if (currentState != RivalState.PingsanFade)
        {
            currentState = RivalState.PingsanFade; // Ganti status jadi pingsan
            
            // Animasi Pingsan: Putar badan Pico 90 derajat jadi tiduran/terguling
            transform.rotation = Quaternion.Euler(0, 0, 90f); 
            
            Debug.Log("Pico: WADAAAW! *Pingsan*");
        }
    }
}