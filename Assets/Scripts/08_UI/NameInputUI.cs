using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameInputUI : MonoBehaviour
{
    [Header("이 패널(이름 입력 UI)")]
    [SerializeField] private GameObject uiPanel;

    [Header("스프라이트 미리보기 (월드 씬)")]
    [SerializeField] private Image previewWorldSprite;

    [Header("엔트리 애니메이션 오브젝트")]
    [SerializeField] private GameObject entryFX;
    [SerializeField] private Animator entryAnimator;

    [Header("Fade 설정")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("이름 입력 필드")]
    [SerializeField] private TMP_InputField nameField;

    private Player player;
    private StatsUI statsUI;
    private PlayerUI playerUI;

    public void Init(Player player, StatsUI statsUI, PlayerUI playerUI)
    {
        this.player = player;
        this.statsUI = statsUI;
        this.playerUI = playerUI;
    }

    private void Awake()
    {
        // entryFX, fadeImage 초기 상태
        if (entryFX) entryFX.SetActive(false);
        if (fadeImage)
        {
            fadeImage.gameObject.SetActive(false);
            Color color = fadeImage.color;
            color.a = 1f;
            fadeImage.color = color;
        }

        if (previewWorldSprite)
            previewWorldSprite.gameObject.SetActive(false);
    }

    // GenderSelectUI에서 할당할 미리보기 스프라이트
    public void SetPreviewSprite(Sprite sprite)
    {
        if (previewWorldSprite == null) return;
        previewWorldSprite.sprite = sprite;
        previewWorldSprite.gameObject.SetActive(true);
    }

    public void OnConfirmButton()
    {
        string enteredName = nameField.text;

        if (!string.IsNullOrEmpty(enteredName))
        {
            player?.SetPlayerName(enteredName);
            statsUI?.RefreshUI();
            playerUI?.RefreshUI();
        }

        StartCoroutine(PlayEntryFade());

        // 이름 입력 UI 및 페이드인 UI 비활성화
        if (uiPanel) uiPanel.SetActive(false);
        if (previewWorldSprite) previewWorldSprite.gameObject.SetActive(false);
        if (fadeImage) fadeImage.gameObject.SetActive(false);
    }

    private IEnumerator PlayEntryFade()
    {
        // FX 애니메이션
        if (entryFX && entryAnimator)
        {
            entryFX.SetActive(true);
            entryAnimator.Play("Complete_Entry", 0, 0f);

            // 애니 길이만큼 대기
            var state = entryAnimator.GetCurrentAnimatorStateInfo(0);
            float animLen = state.length / entryAnimator.speed;
            yield return new WaitForSeconds(animLen);
            entryFX.SetActive(false);
        }

        // 이름입력 UI 비활성화 / 페이드 시작
        if (uiPanel) uiPanel.SetActive(false);
        if (fadeImage)
        {
            fadeImage.gameObject.SetActive(true);
            Color color = fadeImage.color;
            color.a = 1f;
            fadeImage.color = color;

            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                color.a = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                fadeImage.color = color;
                yield return null;
            }

            // 페이드 비활성화
            fadeImage.color = new Color(color.r, color.g, color.b, 0f);
            fadeImage.gameObject.SetActive(false);
        }
    }
}