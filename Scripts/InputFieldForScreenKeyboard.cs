/*
    Original Creator: Vitaliy Polchuk (https://stackoverflow.com/users/900026/vitaliy-polchuk)
    Edit Recommendations: 
        olonge (https://stackoverflow.com/users/2374782/olonge)
        Valerie (https://stackoverflow.com/users/574623/valerie)
    This Version: kfblake (https://stackoverflow.com/users/1473564/kfblake)
*/

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class InputFieldForScreenKeyboardPanelAdjuster : MonoBehaviour {


    // Assign panel here in order to adjust its height when TouchScreenKeyboard is shown
    public GameObject panel;

    // ## Works better if your container (i.e. panel) is positioned in the center of the screen
    public bool UseAnchorPosition3D = true;

    private InputField inputField;
    private RectTransform panelRectTrans;
    private Vector2 panelOffsetMinOriginal;
    private Vector3 panelAnchor3DOriginal;
    private float panelHeightOriginal;
    private float currentKeyboardHeightRatio;



    public void Start() {
        if (panel == null) return;
        inputField = transform.GetComponent<InputField>();
        panelRectTrans = panel.GetComponent<RectTransform>();
        panelOffsetMinOriginal = panelRectTrans.offsetMin;
        panelAnchor3DOriginal = panelRectTrans.anchoredPosition3D;
        panelHeightOriginal = panelRectTrans.rect.height;
    }

    public void LateUpdate () {
        if (panel == null) return;
        if (inputField.isFocused) {
            float newKeyboardHeightRatio = GetKeyboardHeightRatio();
            if (currentKeyboardHeightRatio != newKeyboardHeightRatio) {
                Debug.Log("InputFieldForScreenKeyboardPanelAdjuster: Adjust to keyboard height ratio: " + newKeyboardHeightRatio);
                currentKeyboardHeightRatio = newKeyboardHeightRatio;
                SetPanelOffset(currentKeyboardHeightRatio);
            }
        } else if (currentKeyboardHeightRatio != 0f) {
            if (panelRectTrans.offsetMin != panelOffsetMinOriginal) {
                Invoke("DelayedReset", 0.5f);
            }
            currentKeyboardHeightRatio = 0f;
        }
    }

    private void SetPanelOffset(float ratio){
        if (UseAnchorPosition3D){
            panelRectTrans.anchoredPosition3D = new Vector3(panelAnchor3DOriginal.x,panelHeightOriginal * ratio,panelAnchor3DOriginal.z); 
        } else {
            panelRectTrans.offsetMin = new Vector2(panelOffsetMinOriginal.x, panelHeightOriginal * ratio);
        }
    }

    private void ResetPanelOffset(){
        if (UseAnchorPosition3D){
            panelRectTrans.anchoredPosition3D = panelAnchor3DOriginal; 
        } else {
            panelRectTrans.offsetMin = panelOffsetMinOriginal; 
        }
    }

    private bool CheckForOtherUses(){
        InputFieldForScreenKeyboardPanelAdjuster[] uses = panel.GetComponentsInChildren<InputFieldForScreenKeyboardPanelAdjuster>();
        Debug.Log("OtherUsesFound: "+uses.Length);
        bool isFocused = false;
        foreach (InputFieldForScreenKeyboardPanelAdjuster u in uses){
            if (u.IsFocused) { isFocused = true; break; }
        }
        return isFocused;
    }

    public bool IsFocused {
        get { return inputField.isFocused; }
    }
    protected void DelayedReset() { 
        if (CheckForOtherUses()) return;
        Debug.Log("InputFieldForScreenKeyboardPanelAdjuster: Revert to original "); 
        ResetPanelOffset();
    }

    private float GetKeyboardHeightRatio() {
        #if UNITY_EDITOR
        return 0.4f; // fake TouchScreenKeyboard height ratio for debug in editor        
        #elif UNITY_ANDROID
        using (AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
            AndroidJavaObject View = UnityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");
            using (AndroidJavaObject rect = new AndroidJavaObject("android.graphics.Rect")) {
                View.Call("getWindowVisibleDisplayFrame", rect);
                return (float)(Screen.height - rect.Call<int>("height")) / Screen.height;
            }
        }
        #elif UNITY_IOS
        // return (float)TouchScreenKeyboard.area.height / Screen.height;
        return (float)TouchScreenKeyboard.area.height / Screen.safeArea.height;
        #else
        return 0f;
        #endif
    }

}
