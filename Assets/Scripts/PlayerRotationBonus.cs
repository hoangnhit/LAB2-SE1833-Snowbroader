using UnityEngine;

public class PlayerRotationBonus : MonoBehaviour
{
    private float rotationAccumulator = 0f; // Biến tích lũy lượng xoay
    private float lastZRotation;            // Góc quay của frame trước đó
    private GameManager gameManager;        // Để cộng điểm bonus

    // Số điểm thưởng mỗi lần xoay 360 độ
    private int bonusScore = 50;

    void Start()
    {
        // Lấy góc quay ban đầu của đối tượng
        lastZRotation = transform.eulerAngles.z;
        // Tìm đối tượng GameManager đã được gắn script
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        // Lấy góc quay hiện tại của đối tượng theo trục Z
        float currentZRotation = transform.eulerAngles.z;
        // Tính delta giữa góc hiện tại và góc của frame trước đó
        float deltaRotation = currentZRotation - lastZRotation;

        // Xử lý wrap-around: nếu delta vượt quá 180, điều chỉnh lại
        if (deltaRotation > 180f)
        {
            deltaRotation -= 360f;
        }
        else if (deltaRotation < -180f)
        {
            deltaRotation += 360f;
        }

        // Cộng dồn delta vào biến tích lũy
        rotationAccumulator += deltaRotation;
        lastZRotation = currentZRotation;

        // Nếu đã tích lũy đủ 360 độ (theo bất kỳ hướng nào)
        if (Mathf.Abs(rotationAccumulator) >= 360f)
        {
            // Tính số vòng xoay đầy đủ đã thực hiện
            int fullRotations = (int)(Mathf.Abs(rotationAccumulator) / 360f);
            // Cộng 50 điểm bonus mỗi lần xoay 360 độ
            gameManager.AddScore(bonusScore * fullRotations);
            // Giảm đi số độ đã cộng điểm, giữ lại phần dư cho lần tính sau
            rotationAccumulator = rotationAccumulator % 360f;
        }
    }
}
