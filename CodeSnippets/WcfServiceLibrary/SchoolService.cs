using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary
{
    //Because each service must include a contract, Microsoft decided to use interfaces to
    //implement this feature.Because interfaces are contracts by their nature, they're a natural and
    //intuitive way to implement the requirement. Note, one service can implement several different interfaces.
    public class SchoolService : ISchoolService
    {
        public AnswerDetails GetAnswerDetails(int questionNumber)
        {
            AnswerDetails CurrentDetails = new AnswerDetails();
            
            switch(questionNumber)
            {
                case 1:
                    CurrentDetails = AnswerDetails.A;
                    break;
                case 2:
                    CurrentDetails = AnswerDetails.B;
                    break;
                case 3:
                    CurrentDetails = AnswerDetails.C;
                    break;
                case 4:
                    CurrentDetails = AnswerDetails.D;
                    break;
                case 5:
                    CurrentDetails = AnswerDetails.All;
                    break;
                default:
                    CurrentDetails = AnswerDetails.A;
                    break;
            }

            // Method implementation
            return CurrentDetails;
        }

        public string[] GetExamOutline(string examName)
        {
            string[] OutlineItems = new string[]
            {
                "Exam line 1",
                "Exam line 2",
                "Exam line 3",
                "Exam line 4",
                "Exam line 5",
            };
            // Method implementation
            return OutlineItems;
        }

        public AnswerSet[] GetQuestionAnswers(int questionNumber)
        {
            AnswerSet[] CurrentAnswers = new AnswerSet[]
            {
                new AnswerSet() { AnswerId=Guid.NewGuid(), QuestionId=1, AnswerText= "Answer Text 1" },
                new AnswerSet() { AnswerId=Guid.NewGuid(), QuestionId=2, AnswerText= "Answer Text 2" },
                new AnswerSet() { AnswerId=Guid.NewGuid(), QuestionId=3, AnswerText= "Answer Text 3" },
            };
            // Method implementation
            return CurrentAnswers;
        }

        public string GetQuestionText(int questionNumber)
        {
            if (questionNumber <= 0)
            {
                string OutOfRangeMessage = "Question Ids must be a positive value greater than 0";
                IndexOutOfRangeException InvalidQuestionId = new IndexOutOfRangeException(OutOfRangeMessage);
                
                throw new FaultException<IndexOutOfRangeException>(InvalidQuestionId, OutOfRangeMessage);
            }

            string AnswerText = $"Answer to question: {questionNumber}.";
            // Method implementation
            return AnswerText;
        }

        public TestQuestion GetTestQuestion(int questionNumber)
        {
            AnswerSet[] answers = new AnswerSet[]
            {
                new AnswerSet() { AnswerId=Guid.NewGuid(), QuestionId=1, AnswerText= $"Answer Text 1, q: {questionNumber}" },
                new AnswerSet() { AnswerId=Guid.NewGuid(), QuestionId=2, AnswerText= $"Answer Text 2, q: {questionNumber}" },
                new AnswerSet() { AnswerId=Guid.NewGuid(), QuestionId=3, AnswerText= $"Answer Text 3, q: {questionNumber}" },
            };

            return new TestQuestion()
            {
                QuestionId = 12,
                Answers = AnswerDetails.B,
                QuestionText = 22,
                AvailableAnswers = answers
            };
        }
    }
}
