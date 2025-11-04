using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Model.Tenant.Client.ViewModel
{
    /// <summary>
    /// Represents the detailed view of a client questionnaire,
    /// including its metadata and associated sections.
    /// </summary>
    public class ClientQuestionnaireDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier (RowId) of the questionnaire.
        /// </summary>
        public Guid QuestionnaireRowId { get; set; }

        /// <summary>
        /// Gets or sets the display name of the questionnaire.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the list of sections that belong to this questionnaire.
        /// </summary>
        public List<ClientQuestionnaireSectionViewModel> Sections { get; set; } = new();
    }

    /// <summary>
    /// Represents a section within a client questionnaire.
    /// Each section may contain one or more questions.
    /// </summary>
    public class ClientQuestionnaireSectionViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the title or heading of the questionnaire section.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets the list of questions contained in this section.
        /// </summary>
        public List<QuestionBankViewModel> Questions { get; set; } = new();
    }

    /// <summary>
    /// Represents an individual question from the question bank
    /// that is part of a client questionnaire section.
    /// </summary>
    public class QuestionBankViewModel
    {
        /// <summary>
        /// Gets or sets the unique numeric identifier of the question.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the unique RowId (GUID) of the question.
        /// </summary>
        public Guid RowId { get; set; }

        /// <summary>
        /// Gets or sets the display text or description of the question.
        /// </summary>
        public string Text { get; set; } = null!;

        /// <summary>
        /// Gets or sets the display render type of the question.
        /// </summary>
        public string RenderType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of available answer options for the question.
        /// </summary>
        public List<string>? Options { get; set; }

        /// <summary>
        /// Gets or sets the identifier of a linked question,
        /// if this question’s response determines the visibility or behavior of another question.
        /// </summary>
        public long? LinkedQuestion { get; set; }

        /// <summary>
        /// Gets or sets the list of actions to perform when this question is answered.
        /// </summary>
        public List<string>? OnAction { get; set; }
    }
}