using UnityEngine;

public class UbiJatuh : MonoBehaviour
{
    public float kecepatanJatuh = 3f;
    [HideInInspector] public float batasTanah;
    private bool sudahDiTanah = false;

    void Update()
    {
        if (!sudahDiTanah)
        {
            // Bergerak turun
            transform.Translate(Vector3.down * kecepatanJatuh * Time.deltaTime);

            if (transform.position.y <= batasTanah)
            {
                sudahDiTanah = true;
                transform.position = new Vector2(transform.position.x, batasTanah);
                Debug.Log("Ubi mendarat dan setia menunggu Kucel!");
            }
        }
    }
}