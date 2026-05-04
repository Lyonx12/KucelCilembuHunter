using UnityEngine;
using System.Collections; // Wajib untuk fungsi jeda waktu (Coroutine)

public class KresekTrap : MonoBehaviour
{
    private bool sudahTerpicu = false;

    // Fungsi ini bagian dari Trigger Detection (Materi Pertemuan 4)
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Jika Kucel menginjak area kresek dan belum pernah terpicu
        if (collision.CompareTag("Player") && !sudahTerpicu)
        {
            sudahTerpicu = true;
            Debug.Log("*SRESEK SRESEK* Wah, ada kresek nih!");
            
            // Ambil komponen KucelController untuk menghentikan dia
            KucelController kucel = collision.GetComponent<KucelController>();
            if (kucel != null)
            {
                StartCoroutine(EfekDistract(kucel));
            }
        }
    }

    IEnumerator EfekDistract(KucelController kucel)
    {
        // Simpan kecepatan asli Kucel
        float kecepatanAsli = kucel.moveSpeed;
        
        // Kucel berhenti mendadak karena nyariin suara kresek
        kucel.moveSpeed = 0f;
        Debug.Log("Kucel: 'Eh, ada yang bawa makanan ya?' (Berhenti 2 detik)");

        // Tunggu 2 detik
        yield return new WaitForSeconds(2f);

        // Kecepatan Kucel kembali normal
        kucel.moveSpeed = kecepatanAsli;
        Debug.Log("Kucel: 'Ah elah, cuma angin.' (Lanjut jalan)");
        
        // Hancurkan objek kresek biar ga ngetrigger lagi
        Destroy(gameObject);
    }
}