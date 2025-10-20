using UnityEngine;

public class Background : MonoBehaviour
{
    public Transform mainCam;
    public Transform midBg;
    public Transform sideBg;
    public float length;

    // Tốc độ cuộn thị sai (Parallax Speed)
    // Giá trị < 1.0f: Background sẽ cuộn chậm hơn Camera (hiệu ứng xa)
    // Giá trị = 0.5f là một điểm khởi đầu tốt.
    [Range(0f, 1f)]
    public float parallaxMultiplier = 0.5f;

    private float offset;
    // Biến để lưu trữ vị trí X TẠM THỜI của Camera trong frame trước
    private float lastCameraX;

    void Start()
    {
        // Tính toán khoảng cách hoán đổi (2 lần chiều dài một mảnh)
        offset = length * 2f;

        // Khởi tạo vị trí Camera hiện tại
        lastCameraX = mainCam.position.x;
    }

    void Update()
    {
        // 1. TÍNH TOÁN ĐỘ DỊCH CHUYỂN CỦA CAMERA
        float deltaCameraX = mainCam.position.x - lastCameraX;

        // 2. TÍNH TOÁN ĐỘ DỊCH CHUYỂN CỦA BACKGROUND (Parallax)
        // Background chỉ di chuyển một phần của độ dịch chuyển Camera
        float parallaxMovement = deltaCameraX * parallaxMultiplier;

        // 3. DI CHUYỂN BACKGROUNDS
        // Áp dụng dịch chuyển cho cả hai mảnh Background
        midBg.position += Vector3.right * parallaxMovement;
        sideBg.position += Vector3.right * parallaxMovement;

        // 4. CẬP NHẬT VỊ TRÍ CUỐI CÙNG CỦA CAMERA
        lastCameraX = mainCam.position.x;

        // 5. KIỂM TRA VÀ HOÁN ĐỔI VỊ TRÍ (Looping/Tái chế)
        // Kiểm tra vị trí của mảnh background chính (midBg) so với Camera
        CheckAndSwapBackgrounds();
    }

    // Hàm này giữ nguyên chức năng hoán đổi background
    void CheckAndSwapBackgrounds()
    {
        // Điểm cần kiểm tra là vị trí Camera vượt qua giữa midBg và sideBg
        float midBgEndPosition = midBg.position.x + (length / 2f);
        float midBgStartPosition = midBg.position.x - (length / 2f);

        // Cuộn Sang Phải (Camera đi tới)
        if (mainCam.position.x > midBgEndPosition)
        {
            UpdateBackgroundPosition(Vector3.right);
        }
        // Cuộn Sang Trái (Camera đi lùi - nếu game cho phép)
        else if (mainCam.position.x < midBgStartPosition)
        {
            UpdateBackgroundPosition(Vector3.left);
        }
    }

    void UpdateBackgroundPosition(Vector3 direction)
    {
        // Di chuyển mảnh đối diện (sideBg) đến vị trí mới (trước/sau midBg)
        sideBg.position = midBg.position + direction * offset;

        // Hoán đổi Tham Chiếu
        Transform temp = midBg;
        midBg = sideBg;
        sideBg = temp;
    }
}