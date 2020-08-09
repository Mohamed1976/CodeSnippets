using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary
{
    //Types that participate in WCF operations must be serializable.
    //When using DataContracts, you indicate that the .NET type decorated with the
    //DataContract attribute is serialized.
    //The DataContract explicitly puts forth all necessary information about how the 
    //data it represents will be serialized.Precisely because it’s a contract, both 
    //the client and server can agree and know what’s being sent and received,
    //and they can do this without having to share the types.
    [DataContract(Name = "Answers", Namespace = "http://www.williamgryan.mobi/Book/70-487")]
    public class AnswerSet
    {
        [DataMember(Name = "QuestionId", IsRequired = true)]
        public Int32 QuestionId { get; set; }
        [DataMember]
        public Guid AnswerId { get; set; }
        [DataMember]
        public String AnswerText { get; set; }
    }
}