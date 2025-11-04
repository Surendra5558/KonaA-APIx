namespace KonaAI.Master.Model.Common.Constants;

public struct Constants
{
    public static string DefaultDateFormat { get; } = "MM/dd/yyy";

    //Regular Expressions
    public static string PhoneNumberRegex { get; } = @"^\+?[0-9]{7,15}$";
    public static string DatabaseNameSanitizationRegex { get; } = @"[^a-zA-Z0-9]";

    public static string ChildQuestionRenderType = "TextArea";

    // Project Scheduler Deployment Paths
    public static string DacPacPath { get; } = @"C:\Users\LikithaPasumarthi\Downloads\Spectrum_Project_Db.dacpac";
    public static string SqlScriptPath { get; } = @"C:\Users\LikithaPasumarthi\Downloads\Spectrum_Project_Db_Create (1).sql";
}