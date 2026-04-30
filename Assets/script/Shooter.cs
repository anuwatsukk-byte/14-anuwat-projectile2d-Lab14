using UnityEngine;
using UnityEngine.InputSystem; // ใช้ Input แบบใหม่ตามที่ Lab กำหนด

public class Shooter : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;   // จุดที่ยิงออก
    [SerializeField] private GameObject target;     // เป้าเล็ง / Crosshair
    [SerializeField] private GameObject bulletPrefab; // ลูกบอล (Prefab)

    void Update()
    {
        
        Vector2 screenPos = Mouse.current.position.ReadValue();

       
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // ยิง Rayจากกล้องไปยังตำแหน่งเมาส์
            Ray ray = Camera.main.ScreenPointToRay(screenPos);

            
            Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red, 5f);

            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                
                target.transform.position = new Vector2(hit.point.x, hit.point.y);
                Debug.Log($"Hit: {hit.collider.gameObject.name}");

                Vector2 projectileVelocity = CalculateProjectileVelocity(shootPoint.position, hit.point, 1f);
               
                GameObject bulletObj = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
                Rigidbody2D shootBullet = bulletObj.GetComponent<Rigidbody2D>();

                if (shootBullet != null)
                {
                    
                    shootBullet.linearVelocity = projectileVelocity;
                }

            }
                

                
        }
    }

    // ฟังก์ชันคำนวณความเร็วแบบวิถีโค้ง
    Vector2 CalculateProjectileVelocity(Vector2 origin, Vector2 targetPos, float time)
    {
        
        Vector2 direction = targetPos - origin;

        // คำนวณความเร็วแนวแกน X และ Y
        float vx = direction.x / time;
        float vy = (direction.y / time) + (0.5f * Mathf.Abs(Physics2D.gravity.y) * time);

        return new Vector2(vx, vy);
    }
}