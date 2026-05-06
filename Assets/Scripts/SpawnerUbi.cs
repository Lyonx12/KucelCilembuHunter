using UnityEngine;

public class SpawnerUbi : MonoBehaviour
{
    [Header("Pengaturan Spawner")]
    public GameObject ubiPrefab;      // Slot untuk Prefab Ubi
    public float waktuSpawn = 4f;     // Ubi muncul setiap 4 detik
    
    [Header("Area Muncul (X dan Y)")]
    public float batasKiri = -7f;
    public float batasKanan = 7f;
    public float batasAtas = 4f;
    public float batasBawah = -4f;

    private float timer;

    void Update()
    {
        // Timer berjalan setiap frame
        timer += Time.deltaTime;

        // Jika waktu sudah mencapai batas (4 detik)
        if (timer >= waktuSpawn)
        {
            MunculkanUbi();
            timer = 0f; // Reset timer
        }
    }

    void MunculkanUbi()
    {
        // Mencari posisi acak di dalam batas area
        float randomX = Random.Range(batasKiri, batasKanan);
        float randomY = Random.Range(batasBawah, batasAtas);
        Vector2 posisiAcak = new Vector2(randomX, randomY);

        // Fungsi Instantiate adalah inti dari konsep Spawner
        Instantiate(ubiPrefab, posisiAcak, Quaternion.identity);
        
        Debug.Log("Satu Ubi Cilembu panas baru saja muncul!");
    }
}