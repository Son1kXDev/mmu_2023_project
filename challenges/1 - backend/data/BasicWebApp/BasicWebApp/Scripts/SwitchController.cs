namespace BasicWebApp.Scripts;

public static class SwitchController
{
    public static bool CurrentState { get; private set; }
    public static readonly string DataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "state.txt");
    
    public static void ChangeSwitchState()
    {
        ReadCurrentState();
        CurrentState = !CurrentState;
        WriteCurrentState();
    }

    public static void SetSwitchState(bool state)
    {
        CurrentState = state;
        WriteCurrentState();
    }
    
    private static void ReadCurrentState()
    {
        CheckFile(DataFilePath);
        
        var dataString = System.IO.File.ReadAllText(DataFilePath);
        CurrentState = Convert.ToBoolean(dataString);
    }

    private static void WriteCurrentState()
    {
        File.WriteAllText(DataFilePath, CurrentState.ToString());
    }
    
    private static void CheckFile(string path)
    {
        if (File.Exists(path) == true) return;

        File.Create(path);
        File.WriteAllText(path, "False");
        Console.Write(DataFilePath);
    }
}