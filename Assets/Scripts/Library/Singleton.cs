using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
///     제네릭 싱글톤 클래스 - MonoBehaviour를 상속받는 어떤 클래스든 싱글톤으로 만들 수 있습니다.
/// </summary>
/// <typeparam name="T">싱글톤으로 만들 클래스 타입</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // 싱글톤 인스턴스
    private static T _instance;

    // 씬 전환 시에도 유지되도록 하는 옵션 (필요에 따라 사용)
    [FormerlySerializedAs("DontDestroyOnLoadFlag")]
    public bool dontDestroyOnLoadFlag = true;

    // 싱글톤 인스턴스에 대한 읽기 전용 공개 접근자
    public static T instance
    {
        get
        {
            // 인스턴스가 없는 경우 찾아보기
            if (_instance) return _instance;
            // 씬에서 해당 타입의 오브젝트 찾기
            _instance = FindFirstObjectByType<T>();

            // 씬에 인스턴스가 없는 경우
            if (_instance == null)
            {
                // 새 게임오브젝트 생성
                var singletonObject = new GameObject(typeof(T).Name + " (Singleton)");
                _instance = singletonObject.AddComponent<T>();

                Debug.Log($"[Singleton] {typeof(T).Name} 인스턴스 생성됨.");
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // 이미 인스턴스가 있는지 확인
        if (_instance && _instance != this)
        {
            // 중복된 인스턴스는 파괴
            Debug.LogWarning($"[Singleton] {typeof(T).Name}의 인스턴스가 이미 존재합니다. 중복 객체를 파괴합니다.");
            Destroy(gameObject);
            return;
        }

        // 이 객체를 싱글톤 인스턴스로 설정
        _instance = this as T;

        // 씬 전환 시에도 파괴되지 않도록 설정 (옵션)
        if (dontDestroyOnLoadFlag)
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }

        // 초기화 로직
        OnSingletonAwake();
    }

    // 상속 클래스에서 오버라이드할 가상 메서드
    protected virtual void OnSingletonAwake()
    {
        // 상속 클래스에서 초기화 작업을 구현
    }
}