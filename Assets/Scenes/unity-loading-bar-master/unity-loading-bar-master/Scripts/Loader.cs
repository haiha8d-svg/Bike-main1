using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image fill;
    public int sceneToLoad = 1;

    void Start()
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        StartCoroutine(LoadAsync(sceneToLoad));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        // Tạm thời chặn KHÔNG CHO CHUYỂN CẢNH ngay cả khi đã load xong dữ liệu
        operation.allowSceneActivation = false;

        // Biến này để lưu độ dài thực tế đang hiển thị của thanh màu đỏ (từ 0 đến 1)
        float visualProgress = 0f;

        while (!operation.isDone)
        {
            // Tiến trình thực sự của Unity (đã load xong bao nhiêu phần trăm)
            float targetProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // Bắt thanh màu chạy TỪ TỪ đuổi theo tiến trình thật
            // Số 0.5f ở dưới là tốc độ chạy (mất khoảng 2 giây để đầy). 
            // Nếu bạn muốn nó chạm trễ hơn nữa thì đổi thành 0.2f, muốn nhanh hơn thì để 1f
            visualProgress = Mathf.MoveTowards(visualProgress, targetProgress, 0.5f * Time.deltaTime);

            if (fill != null)
            {
                fill.fillAmount = visualProgress;
            }

            // Chỉ khi nào thanh màu CHẠY ĐẦY (visualProgress vươn tới 1) 
            // thì chúng ta mới mở khóa cho phép chuyển sang Scene đua xe
            if (visualProgress >= 1f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
