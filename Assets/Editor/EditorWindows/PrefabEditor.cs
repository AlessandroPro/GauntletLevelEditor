using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class PrefabEditor : EditorWindow
{
    static protected EditorWindow _window = null;
    static protected GauntletLevelEditor parentWindow = null;
    static protected float width = 450;

    protected VisualElement root;
    protected VisualElement dataRoot;
    protected VisualElement spriteRoot;

    // Properties
    protected TextField nameTextField;
    protected ObjectField objectData;
    protected ObjectField objectTileSprite;
    protected Image objectTileSpriteImage;

    public class Spacer : Label
    {
        public Spacer(int space)
        {
            style.marginTop = space;
        }
    }

    public static EditorWindow createWindow<T>(GauntletLevelEditor _parentWindow, string windowTitle) where T : EditorWindow
    {
        if (_window != null) _window.Close();
        _window = GetWindow<T>();
        _window.titleContent = new GUIContent(windowTitle);
        _window.maxSize = new Vector2(width, _parentWindow.position.height);
        _window.minSize = new Vector2(width, _parentWindow.position.height);
        _window.Repaint();
        parentWindow = _parentWindow;
        return _window;
    }

    public void setupWindow()
    {
        root = this.rootVisualElement;
        root.style.flexDirection = FlexDirection.Row;
        root.style.paddingTop = 10;
        root.style.paddingBottom = 10;
        root.style.paddingLeft = 10;
        root.style.paddingRight = 10;


        dataRoot = new VisualElement()
        {
            style =
            {
                width = _window.maxSize.x * 0.7f,
                paddingRight = 30
            }
        };
        spriteRoot = new VisualElement()
        {
            style =
            {
                alignContent = Align.Center,
                width = _window.maxSize.x * 0.3f
            }
        };

        root.Add(dataRoot);
        root.Add(spriteRoot);
    }

    protected void OnDestroy()
    {
        if (parentWindow)
        {
            parentWindow.assetWindow = null;
        }
        parentWindow = null;
    }

    protected void addSlider(ref VisualElement dataRoot, int minVal, int maxVal, string label, string bindingPath)
    {
        dataRoot.Add(new Spacer(30));
        var slider = new SliderInt(minVal, maxVal);
        var sliderLabel = new Label(label + slider.value);
        dataRoot.Add(sliderLabel);
        slider.RegisterCallback<ChangeEvent<int>>((evt) =>
        {
            sliderLabel.text = label + slider.value;
        });
        slider.bindingPath = bindingPath;
        dataRoot.Add(slider);
    }
}
