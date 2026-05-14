using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [HideInInspector] public GameState state = new GameState();
    [HideInInspector] public GameState previousState = new GameState();

    private void Awake()
    {
        Locator.gameStateManager = this;
    }

    private void Start()
    {
        EventManager.enteringDialogue += SetDialogueState;
        EventManager.exitingDialogue += SetPlayState;
        EventManager.pausingGame += SetMenuState;
        EventManager.resumingGame += SetPlayState;
    }

    private void SetPlayState()
    {
        if (previousState == GameState.Dialogue)
        {
            previousState = state;
            state = GameState.Dialogue;
            return;
        }

        previousState = state;
        state = GameState.Play;
        EventManager.ActivatePlayerInputs();
    }

    private void SetDialogueState()
    {
        previousState = state;
        state = GameState.Dialogue;
        EventManager.DeactivatePlayerInputs();
    }

    private void SetMenuState()
    {
        previousState = state;
        state = GameState.Menu;
        EventManager.DeactivatePlayerInputs();
    }

    private void OnDestroy()
    {
        EventManager.enteringDialogue -= SetDialogueState;
        EventManager.exitingDialogue -= SetPlayState;
        EventManager.pausingGame -= SetMenuState;
        EventManager.resumingGame -= SetPlayState;
    }
}
