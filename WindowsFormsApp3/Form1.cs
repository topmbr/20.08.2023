using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        private List<Question> questions;
        private int currentQuestionIndex = 0;
        private int totalScore = 0;
        public Form1()
        {
            InitializeComponent();
            InitializeQuestions();
            DisplayQuestion(currentQuestionIndex);
            answersListBox.KeyDown += answersListBox_KeyDown;
            submitButton.Click += submitButton_Click;
        }
        private void InitializeQuestions()
        {
            questions = new List<Question>
    {
        new Question("Сколько будет 2 + 2?",
                     new List<string> { "3", "4", "5" },
                     new List<int> { 1 },
                     QuestionType.SingleChoice),

        new Question("Выберите цвета радуги.",
                     new List<string> { "Красный", "Зеленый", "Синий", "Розовый", "Черный" },
                     new List<int> { 0, 1, 2 },
                     QuestionType.MultipleChoice),

        new Question("Что обозначает HTML?",
                     new List<string> { "Hypertext Markup Language", "High Tech Machine Learning", "Hyper Transfer Mechanism Language" },
                     new List<int> { 0 },
                     QuestionType.SingleChoice),

        new Question("Какой год был провозглашен Годом космонавтики?",
                     new List<string> { "1957", "1961", "1969" },
                     new List<int> { 1 },
                     QuestionType.SingleChoice),

        new Question("Что означает аббревиатура CPU?",
                     new List<string> { "Central Processing Unit", "Computer Processing Unit", "Critical Processing Unit" },
                     new List<int> { 0 },
                     QuestionType.SingleChoice),

        new Question("Введите название текущего года:",
                     new List<string>(),
                     new List<int>(),
                     QuestionType.FreeText),

        new Question("Что обозначает аббревиатура PDF?",
                     new List<string> { "Print Document Format", "Portable Document Format", "Personal Data File" },
                     new List<int> { 1 },
                     QuestionType.SingleChoice),

        new Question("Выберите крупнейшую планету в Солнечной системе.",
                     new List<string> { "Марс", "Юпитер", "Венера", "Уран" },
                     new List<int> { 1 },
                     QuestionType.SingleChoice),

        new Question("Какое из чисел является простым?",
                     new List<string> { "12", "7", "18", "25", "33" },
                     new List<int> { 1 },
                     QuestionType.SingleChoice),

        new Question("Какое из животных может летать?",
                     new List<string> { "Крокодил", "Слон", "Обезьяна", "Летучая мышь", "Акула" },
                     new List<int> { 3 },
                     QuestionType.SingleChoice),

        new Question("Введите название столицы Франции:",
                     new List<string>(),
                     new List<int>(),
                     QuestionType.FreeText),

        new Question("Сколько месяцев в году имеют 31 день?",
                     new List<string> { "1", "5", "7", "12" },
                     new List<int> { 2, 3 },
                     QuestionType.MultipleChoice),

        new Question("Какое химическое обозначение у воды?",
                     new List<string> { "H2O", "O2", "CO2", "NaCl" },
                     new List<int> { 0 },
                     QuestionType.SingleChoice),

        new Question("Выберите столицу Японии.",
                     new List<string> { "Пекин", "Сеул", "Токио", "Москва" },
                     new List<int> { 2 },
                     QuestionType.SingleChoice),

        new Question("Как называется самая длинная река в мире?",
                     new List<string> { "Амазонка", "Нил", "Янцзы", "Миссисипи" },
                     new List<int> { 1 },
                     QuestionType.SingleChoice),

        new Question("Введите кодовое имя самого известного агента в кино:",
                     new List<string>(),
                     new List<int>(),
                     QuestionType.FreeText),

    };
        }

        private void DisplayQuestion(int index)
        {
            if (index < questions.Count)
            {
                Question question = questions[index];
                questionTextLabel.Text = question.Text;
                answersListBox.Items.Clear();
                answersListBox.SelectionMode = question.Type == QuestionType.MultipleChoice ? SelectionMode.MultiSimple : SelectionMode.One;
                answersListBox.Items.AddRange(question.Answers.ToArray());
                answersListBox.SelectedIndex = -1;
                answersListBox.Enabled = true;
                userAnswerTextBox.Visible = question.Type == QuestionType.FreeText;
                submitButton.Enabled = true;
            }
            else
            {
                questionTextLabel.Text = "Тест завершен!";
                answersListBox.Items.Clear();
                answersListBox.Enabled = false;
                userAnswerTextBox.Visible = false;
                submitButton.Enabled = false;
                resultLabel.Text = $"Ваш счет: {totalScore} баллов из {questions.Count}";
            }
        }

        private void answersListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && answersListBox.SelectedIndex != -1)
            {
                ProcessAnswer();
            }
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            ProcessAnswer();
        }

        private void ProcessAnswer()
        {
            Question currentQuestion = questions[currentQuestionIndex];

            if (currentQuestion.Type == QuestionType.FreeText)
            {
                string userAnswer = userAnswerTextBox.Text.Trim();
                totalScore += currentQuestion.GetScore(userAnswer);
            }
            else
            {
                List<int> selectedIndices = new List<int>();
                foreach (int index in answersListBox.SelectedIndices)
                {
                    selectedIndices.Add(index);
                }

                totalScore += currentQuestion.GetScore(selectedIndices);
            }

            currentQuestionIndex++;
            DisplayQuestion(currentQuestionIndex);
        }
    }

    public class Question
    {
        public string Text { get; }
        public List<string> Answers { get; }
        public List<int> CorrectAnswerIndices { get; }
        public QuestionType Type { get; }

        public Question(string text, List<string> answers, List<int> correctAnswerIndices, QuestionType type)
        {
            Text = text;
            Answers = answers;
            CorrectAnswerIndices = correctAnswerIndices;
            Type = type;
        }

        public int GetScore(List<int> selectedIndices)
        {
            int score = 0;

            if (Type == QuestionType.MultipleChoice)
            {
                bool allCorrect = true;
                foreach (int index in selectedIndices)
                {
                    if (!CorrectAnswerIndices.Contains(index))
                    {
                        allCorrect = false;
                        break;
                    }
                }
                if (allCorrect)
                {
                    score = 1;
                }
            }
            else if (Type == QuestionType.SingleChoice && selectedIndices.Count == 1)
            {
                if (CorrectAnswerIndices.Contains(selectedIndices[0]))
                {
                    score = 1;
                }
            }

            return score;
        }

        public int GetScore(string userAnswer)
        {
            int score = 0;
            if (Type == QuestionType.FreeText)
            {
                score = 1;
            }

            return score;
        }
    }

    public enum QuestionType
    {
        SingleChoice,
        MultipleChoice,
        FreeText
    }

}
