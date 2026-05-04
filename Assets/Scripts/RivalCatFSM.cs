using UnityEngine;

public class RivalCatFSM : MonoBehaviour
{
    // Tiga State Utama FSM untuk Kucing Saingan
    public enum RivalState { Patroling, Chasing, Awkward }
    public RivalState currentState = RivalState.Patroling;

    [Header("Area Kekuasaan (Patrol)")]
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 2f;
    private Transform targetWaypoint;

    [Header("Mode Ngegas (Seek/Chase)")]
    public float chaseSpeed = 3.5f;
    public float detectionRadius = 4f; // Jarak pandang si Judes
    private Transform playerKucel;

    [Header("Efek Muka Melas Mas Bro")]
    private float awkwardTimer = 0f;

    void Start()
    {
        targetWaypoint = pointA;
        // Mencari Kucel di arena
        playerKucel = GameObject.Find("Player_Kucel").transform;
    }

    void Update()
    {
        // Mesin FSM berjalan terus setiap frame[cite: 3, 7]
        switch (currentState)
        {
            case RivalState.Patroling:
                Patrol();
                CheckForKucel();
                break;
            case RivalState.Chasing:
                Chase();
                CheckDistance();
                break;
            case RivalState.Awkward:
                FeelAwkward();
                break;
        }
    }

    void Patrol()
    {
        // Kucing saingan berpatroli menjaga wilayah
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, patrolSpeed * Time.deltaTime);
        
        // Balik arah jika sudah sampai ujung
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            targetWaypoint = targetWaypoint == pointA ? pointB : pointA;
        }
    }

    void CheckForKucel()
    {
        // Mendeteksi Kucel memakai kalkulasi jarak vektor[cite: 6]
        if (Vector2.Distance(transform.position, playerKucel.position) <= detectionRadius)
        {
            currentState = RivalState.Chasing; // Transisi: Langsung ngegas ngajak ribut!
        }
    }

    void Chase()
    {
        // Steering Behavior: Seek ke arah Kucel[cite: 6, 7]
        transform.position = Vector2.MoveTowards(transform.position, playerKucel.position, chaseSpeed * Time.deltaTime);
    }

    void CheckDistance()
    {
        // Jika Kucel menjauh (malas meladeni), musuh kembali patroli
        if (Vector2.Distance(transform.position, playerKucel.position) > detectionRadius * 1.5f)
        {
            currentState = RivalState.Patroling; 
        }
    }

    void FeelAwkward()
    {
        // Kucing saingan terdiam karena Kucel masang muka melas/datar
        awkwardTimer -= Time.deltaTime;
        if (awkwardTimer <= 0)
        {
            currentState = RivalState.Patroling; // Ilfeel selesai, lanjut patroli lagi
        }
    }

    // Fungsi ini dipanggil saat pemain menekan tombol Skill "Muka Melas" di Kucel
    public void KenaJurusMelas()
    {
        currentState = RivalState.Awkward;
        awkwardTimer = 3f; // Kucing musuh bengong/ilfeel selama 3 detik
    }
}