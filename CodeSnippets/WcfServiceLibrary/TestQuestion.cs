using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary
{
    //When using DataContracts, you indicate that the .NET type decorated with the
    //DataContract attribute is serialized. You can use the KnownTypeAttribute 
    //(or just KnownType) to specify that a given type be included during deserialization.
    //Fortunately, all you need to do to let the deserialization engine know about a type is mark
    //the type with the KnownType attribute.Use the KnownType attribute in conjunction with the
    //DataContract attribute, and any ambiguity is removed.
    //Although using the DataContract attribute is normally sufficient, it’s critical to remember
    //the previous rules, particularly when you’re working with polymorphic types.
    [DataContract(Namespace = "http://www.williamgryan.mobi/Book/70-487")]
    [KnownType(typeof(AnswerSet))]
    [KnownType(typeof(AnswerDetails))]
    public class TestQuestion
    {
        [DataMember]
        public Int32 QuestionId { get; set; }
        [DataMember]
        public Int32 QuestionText { get; set; }
        [DataMember]
        public AnswerSet[] AvailableAnswers { get; set; }
        [DataMember]
        public AnswerDetails Answers { get; set; }
    }
}
