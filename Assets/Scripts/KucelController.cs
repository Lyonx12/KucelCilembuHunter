using UnityEngine;

public class KucelController : MonoBehaviour
{
    public float moveSpeed = 3f; // Kucel gendut, jalannya santai saja
    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Membaca input WASD atau Panah
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // Flip Sprite Kucel biar tidak jalan mundur
        if (moveInput.x > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput.x < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    void FixedUpdate()
    {
        // Pergerakan halus menggunakan Rigidbody sesuai hukum fisika
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}