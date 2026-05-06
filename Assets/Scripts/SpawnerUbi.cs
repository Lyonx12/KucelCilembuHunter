using UnityEngine;

public class SpawnerUbi : MonoBehaviour
{
    [Header("Pengaturan Spawner")]
    public GameObject ubiPrefab;      
    public float jedaMunculBaru = 2f; // Kucel nunggu 2 detik buat dapet jatah ubi baru
    
    [Header("Posisi Muncul (Langit)")]
    public float posisiLangitY = 6f;  
    public float batasKiri = -7f;
    public float batasKanan = 7f;

    [Header("Posisi Mendarat (Jalan Hutan)")]
    public float tanahAtas = 1f;      
    public float tanahBawah = -1f;    

    // Variabel untuk melacak ubi yang sedang ada di layar
    private GameObject ubiSaatIni = null;
    private float timerJeda;

    void Start()
    {
        timerJeda = jedaMunculBaru;
    }

    void Update()
    {
        // JIKA UBI KOSONG (Belum muncul atau baru saja dimakan Kucel)
        if (ubiSaatIni == null)
        {
            timerJeda -= Time.deltaTime; // Mulai hitung mundur

            if (timerJeda <= 0f)
            {
                MunculkanUbiDariLangit();
                timerJeda = jedaMunculBaru; // Reset timer buat ubi berikutnya (kalau nanti dimakan lagi)
            }
        }
    }

    void MunculkanUbiDariLangit()
    {
        float randomX = Random.Range(batasKiri, batasKanan);
        Vector2 posisiMuncul = new Vector2(randomX, posisiLangitY);

        // Munculkan ubi dan SIMPAN statusnya agar Spawner tahu ada ubi di layar
        ubiSaatIni = Instantiate(ubiPrefab, posisiMuncul, Quaternion.identity);
        
        UbiJatuh skripJatuh = ubiSaatIni.GetComponent<UbiJatuh>();
        if (skripJatuh != null)
        {
            skripJatuh.batasTanah = Random.Range(tanahBawah, tanahAtas);
        }
    }
}