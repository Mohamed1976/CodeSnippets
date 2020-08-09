using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary
{
    [ServiceContract(Namespace = "http://www.williamgryan.mobi/Books/70-487")]
    public interface ISchoolService
    {
        [OperationContract]
        AnswerDetails GetAnswerDetails(Int32 questionNumber);
        [OperationContract]
        AnswerSet[] GetQuestionAnswers(Int32 questionNumber);
        [FaultContract(typeof(IndexOutOfRangeException))]
        [OperationContract]
        String GetQuestionText(Int32 questionNumber);
        [OperationContract]
        String[] GetExamOutline(String examName);
        [OperationContract]
        TestQuestion GetTestQuestion(int questionNumber);
    }
}
