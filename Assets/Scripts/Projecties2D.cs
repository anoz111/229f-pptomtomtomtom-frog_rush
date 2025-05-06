using UnityEngine;

public class Projecties2D : MonoBehaviour
{
    [SerializeField] Transform shootPoint;
    [SerializeField] GameObject target;
    [SerializeField] Rigidbody2D bulletPrefab;
    [SerializeField] float shootCooldown = 1f; // ระยะเวลาคูลดาวน์
    float shootCooldownTimer = 0f;
    public int damage = 1;

    Vector2 CalculateProjectileVelocity(Vector2 origin,Vector2 target,float time)
    {
        Vector2 distance = target - origin;
        float velocityX = distance.x / time;
        float velocityY = distance.y / time + 0.5f * Mathf.Abs(Physics2D.gravity.y) * time;

        Vector2 projectileVelocity = new Vector2(velocityX,velocityY);
        return projectileVelocity;
    }
    void Update()
{
    // ลดเวลาคูลดาวน์ลง
    if (shootCooldownTimer > 0)
    {
        shootCooldownTimer -= Time.deltaTime;
    }

    // เมื่อคลิกซ้ายและคูลดาวน์หมด
    if (Input.GetMouseButtonDown(0) && shootCooldownTimer <= 0f)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red, 5f);

        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

        if (hit.collider != null)
        {
            target.transform.position = new Vector2(hit.point.x, hit.point.y);
            Debug.Log("hit " + hit.collider.name);

            Vector2 projectileVelocity = CalculateProjectileVelocity(shootPoint.position, hit.point, 1f);
            Rigidbody2D shootBullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            shootBullet.linearVelocity = projectileVelocity;

            shootCooldownTimer = shootCooldown; // รีเซ็ตคูลดาวน์
        }
    }
}

}