using UnityEngine;
using UnityEngine.SceneManagement; // Wajib untuk pindah scene

public class MenuManager : MonoBehaviour
{
    [Header("Panel Pengaturan")]
    public GameObject settingsPanel;

    // Fungsi untuk Tombol Play
    public void MulaiMain()
    {
        // Memuat scene bernama "Gameplay"
        SceneManager.LoadScene("Gameplay");
    }

    // Fungsi untuk Tombol Settings
    public void BukaPengaturan()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    // Fungsi untuk Tombol Tutup di Panel Settings
    public void TutupPengaturan()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    // Fungsi untuk Tombol Keluar
    // Catatan: Application.Quit() tidak terlihat efeknya di WebGL browser, 
    // tapi standar di dunia game harus tetap ada.
    public void KeluarGame()
    {
        Debug.Log("Kucel mau lanjut tidur (Keluar Game)");
        Application.Quit();
    }
}