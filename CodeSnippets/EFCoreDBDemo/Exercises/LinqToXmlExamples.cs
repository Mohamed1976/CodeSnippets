using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;
using System.Xml;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.XPath;

/* The first component of an XML Document is typically known as the XML declaration. The XML
declaration isn’t a required component, but you will typically see it in an XML Document. The
two things it almost always includes are the XML version and the encoding. A typical declaration
looks like the following:
<?xml version="1.0" encoding="utf-8" ?>

You need to understand the concept of “well-formedness” and validating XML. To be wellformed,
the following conditions need to be met:
■■ There should be one and only one root element.
■■ Elements that are opened must be closed and must be closed in the order they were
opened.
■■ Any element referenced in the document must also be well-formed.
The core structures of XML are elements and attributes. Elements are structures that represent
a component of data. They are delineated by less-than and greater-than signs at the
beginning and end of a string.

Attributes differ from elements in both syntax and nature. For instance, you might have the following structure that describes a “Name”:
<?xml version="1.0" encoding="utf-8" ?>
<Name>
<FirstName>John</FirstName>
<MiddleInitial>Q</MiddleInitial>
<LastName>Public</LastName>
</Name>
Name in this case is its own element, and FirstName, MiddleInitial, and LastName are their own elements, 
but have context and meaning only within a Name element. You could do the same thing with attributes, 
although they are necessarily part of the element to which they belong:
<Name FirstName="John" MiddleInitial="Q" LastName="Public"></Name>
If you tried to consume this data, the way you access it would differ, but the end result would be that 
you’d retrieve information about a Name, and the data would be identical.

You delineate a comment with the following character sequence:
<!-- Then you end it with the following:--> 

Namespaces are a little more involved. Assume that you want to use an element name—something common. 
If namespaces didn’t exist, it would mean that, after an element name was used, it couldn’t be used 
for anything else. You can imagine how much difficulty this would cause when you’re dealing with different 
vendors, all adding to an existing snippet of XML. This would be particularly problematic even if you didn’t 
have different vendors but had a case in which different XML fragments were used. If you are familiar with 
DLL Hell, this is its evil cousin.

<DocumentCore xmlns:johnco="http://www.yourcompany.com/Companies" xmlns:billco="http://www.mycompany.com/Customers">
    <johnco:Name>
        <johnco:Company>JohnCo</johnco:Company>
    </johnco:Name>
    <billco:Name>
        <billco:FirstName>John</billco:FirstName>
        <billco:MiddleInitial>Q</billco:MiddleInitial>
        <billco:LastName>Public</billco:LastName>
    </billco:Name>
</DocumentCore> 

You can also define namespaces at the element level instead.
<DocumentCore>
    <johnco:Name xmlns:johnco="http://www.yourcompany.com/Companies">
        <johnco:Company>JohnCo</johnco:Company>
    </johnco:Name>
    <billco:Name xmlns:billco="http://www.mycompany.com/Customers">
        <billco:FirstName>John</billco:FirstName>
        <billco:MiddleInitial>Q</billco:MiddleInitial>
        <billco:LastName>Public</billco:LastName>
    </billco:Name>
</DocumentCore> 

 */

namespace EFCoreDBDemo.Exercises
{
    /*
    You’ll find the classes for the LINQ-to-XML API in the System.Xml.Linq namespace.
    The XElement class is one of the core classes of the LINQ-to-XML API and something you should be familiar with. 
    It has five constructor overloads:
    public XElement(XName someName);
    public XElement(XElement someElement);
    public XElement(XName someName, Object someValue);
    public XElement(XName someName, params Object[] someValueset);
    public XElement(XStreamingElement other);
    */

    public class LinqToXmlExamples
    {
        public async Task Run()
        {
            //Example01();
            //Example02();
            //Example03();
            //Example04();
            //Example05();
            //Example06();
            //Example07();
            //Example08();
        }

        /* XPath https://www.w3.org/TR/xpath/
        One feature of navigating through a document is XPath, a kind of query language for XML documents. 
        XPath stands for XML Path Language. It’s a language that is specifically designed for addressing 
        parts of an XML document. XmlDocument implements IXPathNavigable so you can retrieve an XPathNavigator 
        object from it. The XPathNavigator offers an easy way to navigate through an XML file. You can use 
        methods similar to those on an XmlDocument to move from one node to another or you can use an XPath 
        query. This allows you to select elements or attributes with certain values.

        <?xml version="1.0" encoding="utf-8" ?>
        <People>
            <Person firstName="John" lastName="Doe">
                <ContactDetals>
                    <EmailAddress>john@unknown.com</EmailAddress>
                </ContactDetals>
            </Person>
            <Person firstName="Jane" lastName="Doe">
                <ContactDetals>
                    <EmailAddress>jane@unknown.com</EmailAddress>
                    <PhoneNunmber>001122334455</PhoneNunmber>
                </ContactDetals>
            </Person>
        </People>

        You can now use an XPath query to select a Person by name:
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);
        XPathNavigator nav = doc.CreateNavigator();
        string query = "//People/Person[@firstName='Jane']";
        XPathNodeIterator iterator = nav.Select(query);
        Console.WriteLine(iterator.Count); // Displays 1
        
        while(iterator.MoveNext())
        {
            string firstName = iterator.Current.GetAttribute("firstName","");
            string lastName = iterator.Current.GetAttribute("lastName","");
            Console.WriteLine("Name: {0} {1}", firstName, lastName);
        }*/
        private void Example08()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XPathNavigator nav = doc.CreateNavigator();
            string query = "//Contacts/Contact";
            XPathNodeIterator iterator = nav.Select(query);
            Console.WriteLine(iterator.Count);
            while (iterator.MoveNext())
            {
                string age = iterator.Current.GetAttribute("Age", "");
                string birthPlace = iterator.Current.GetAttribute("BirthPlace", "");
                Console.WriteLine("{0} {1}", age, birthPlace);
            }
        }

        /*  XmlDocument class
            The XmlDocument class is the parent of the others in some ways, but it’s even easier to use. You typically do the following:
            ■■ Instantiate a new XmlDocument class.
            ■■ Call the Load method pointing to a file or one of the many overloaded items.
            ■■ Extract the list of nodes.
            ■■ Iterate.
        */
        private void Example07()
        {
            //Writing data using the XmlDocument class works intuitively.There’s a CreateElement method 
            //that accepts a string as a parameter.This method can be called on the document itself 
            //(the first of which creates the root node) or any given element.So creating an initial 
            //document and then adding a root node named Customers that contains one element named Customer 
            //is created like this:
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement customers = xmlDoc.CreateElement("Customers");
            XmlElement customer = xmlDoc.CreateElement("Customer");
            XmlElement customer2 = xmlDoc.CreateElement("Customer");
            //The CreateElement method simply creates the element; it does nothing else. So if you want to create an element named FirstName and then add a value of John to it, use the following syntax:
            //XmlElement FirstNameJohn = DocumentInstance.CreateElement("FirstName");
            //FirstNameJohn.InnerText = "John";
            XmlElement firstNameJohn = xmlDoc.CreateElement("FirstName");
            XmlElement middleInitialQ = xmlDoc.CreateElement("MiddleInitial");
            XmlElement lastNamePublic = xmlDoc.CreateElement("LastName");
            firstNameJohn.InnerText = "John";
            middleInitialQ.InnerText = "Q";
            lastNamePublic.InnerText = "Public";

            XmlElement firstNameBob = xmlDoc.CreateElement("FirstName");
            XmlElement middleInitialX5 = xmlDoc.CreateElement("MiddleInitial");
            XmlElement lastNameGreen = xmlDoc.CreateElement("LastName");
            firstNameBob.InnerText = "Bob";
            middleInitialX5.InnerText = "X5";
            lastNameGreen.InnerText = "Green";

            customer.SetAttribute("Age", 29.ToString());
            customer.AppendChild(firstNameJohn);
            customer.AppendChild(middleInitialQ);
            customer.AppendChild(lastNamePublic);

            customer2.SetAttribute("Age", 47.ToString());
            customer2.AppendChild(firstNameBob);
            customer2.AppendChild(middleInitialX5);
            customer2.AppendChild(lastNameGreen);

            customers.AppendChild(customer);
            customers.AppendChild(customer2);

            xmlDoc.AppendChild(customers);
            Console.WriteLine(xmlDoc.OuterXml);

            //If you wanted to add additional Customer elements, you’d follow the same style, 
            //appending them to the corresponding parent element in the same manner as you did here.
            //For attributes, there’s a SetAttribute method that accepts two strings as parameters 
            //and can be called on any given element. The first string is the attribute name; the 
            //second is the attribute value.

            TextReader xmlContent = new StringReader(xml);
            XmlDocument documentInstance = new XmlDocument();
            documentInstance.Load(xmlContent);
            XmlNodeList currentNodes = documentInstance.DocumentElement.ChildNodes;
            //walk through a Node collection and extracts the InnerText property 
            //(which is the value contained in the nodes).
            foreach (XmlNode myNode in currentNodes)
            {
                Console.WriteLine(myNode.InnerText);
            }
        }

        private string xml =
                    @"<Contacts>
                        <Contact Age=""20"" BirthPlace=""Washington"">
                            <Name>Patrick Hines</Name>
                            <Phone Type=""home"">206-555-0144</Phone>
                            <Phone Type=""work"">425-555-0145</Phone>
                            <Address>
                            <Street1>123 Main St</Street1>
                            <City>Mercer Island</City>
                            <State>WA</State>
                            <Postal>68042</Postal>
                            </Address>
                            <NetWorth>10</NetWorth>
                        </Contact>
                        <Contact Age=""30"" BirthPlace=""New York"">
                            <Name>Gretchen Rivas</Name>
                            <Phone Type=""mobile"">206-555-0163</Phone>
                            <Address>
                            <Street1>123 Main St</Street1>
                            <City>Mercer Island</City>
                            <State>WA</State>
                            <Postal>68042</Postal>
                            </Address>
                            <NetWorth>11</NetWorth>
                        </Contact>
                    </Contacts>";

        private string myXML = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                    "<myDataz xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                             "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                        "<listS>" +
                            "<sog>" +
                                "<field1>123</field1>" +
                                "<field2>a</field2>" +
                                "<field3>b</field3>" +
                            "</sog>" +
                            "<sog>" +
                                "<field1>456</field1>" +
                                "<field2>c</field2>" +
                                "<field3>d</field3>" +
                            "</sog>" +
                        "</listS>" +
                    "</myDataz>";

        /*XERCES can also be used to parse XML file.
         * XmlReader class
        The XmlReader is the counterpart to the XmlWriter, and it’s equally simple to use. 
        Although there are several different cases you can check for (attributes, comments, namespace declarations, and so on), 
        in its simplest form, you simply do the following:
        ■■ Instantiate a new XmlReader instance passing in the file name of the XML file you want to read.
        ■■ Create a while loop using the Read method.
        ■■ While it iterates the elements, check for whatever you want to check looking at the XmlNodeType enumeration. 
        There’s no need to go through each item available in the XmlNodeType enumeration, but you can become familiar 
        with the available items on MSDN: https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlnodetype?redirectedfrom=MSDN&view=netcore-3.1
         */
        private void Example06()
        {

            //Console.WriteLine(myXML + "\n\n");
            //Console.WriteLine(xml);

            TextReader xmlContent = new StringReader(xml);

            XmlTextReader reader = new XmlTextReader(xmlContent);

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        Console.Write("<" + reader.Name);
                        Console.WriteLine(">");
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        Console.WriteLine(reader.Value);
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        Console.Write("</" + reader.Name);
                        Console.WriteLine(">");
                        break;
                }
            }
        }

        /* XmlWriter class The XmlWriter class can be used to write out XmlDocuments. 
         * It’s intuitive to use and needs little explanation.        
        */
        private void Example05()
        {
            List<Customer> customers = default;

            using (AdventureWorksContext adventureWorksContext = new AdventureWorksContext())
            {
                customers = adventureWorksContext.Customer
                    .OrderBy(c => c.FirstName)
                    .Take(5)
                    .ToList();
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamReader streamReader = new StreamReader(memoryStream))
                {
                    //Create a new instance of the XmlWriter Class. This is accomplished by calling the 
                    //static Create method of the XmlWriter class (and for convenience, passing in a file 
                    //name as a parameter).
                    using (XmlWriter writerInstance = XmlWriter.Create(memoryStream))
                    {
                        //Call the WriteStartDocument method to create the initial document.
                        writerInstance.WriteStartDocument();
                        //Call the WriteStartElement, passing in a string name for the element name for the root element.
                        writerInstance.WriteStartElement("Customers");
                        foreach (Customer customer in customers)
                        {
                            //Use the WriteStartElement again to create an instance of each child element 
                            //of the root element you just created in the previous step.
                            writerInstance.WriteStartElement("Customer");
                            writerInstance.WriteAttributeString("Age", 23.ToString());
                            //Use the WriteElementString method passing in the element name and value as parameters.
                            writerInstance.WriteElementString("FirstName", customer.FirstName);
                            writerInstance.WriteElementString("MiddleInitial", customer.MiddleName);
                            writerInstance.WriteElementString("LastName", customer.LastName);
                            writerInstance.WriteEndElement();
                        }
                        writerInstance.WriteEndElement();
                        writerInstance.WriteEndDocument();
                    }

                    memoryStream.Position = 0;
                    string xml = streamReader.ReadToEnd();
                    Console.WriteLine(xml);
                }
            }
        }

        /*XML is a hierarchical collection of data. LINQ to XML provides an in-memory XML API that leverages 
         * LINQ capabilities. LINQ to XML is actually a whole set of XML operation APIs stored in the System.Xml.Linq 
         * namespace. Compared with the APIs for XML DOM in the System.Xml, the LINQ to XML APIs is lightweight and 
         * more flexible. For example, in XML DOM APIs, the classes that represent XML elements and attributes are 
         * XmlNode and XmlAttribute; while in LINQ to XML they are XElement and XAttribute. When composing an XML 
         * element, the LINQ to XML syntax is also simpler than the XML DOM APIs.
         * Using LINQ to XML APIs to create XML elements and attributes is pretty straightforward:
         * The constructor of XElement allows you to put the attribute objects and child-element objects in the argument list, after the element name. 
        */
        private void Example04()
        {
            using (AdventureWorksContext adventureWorksContext = new AdventureWorksContext())
            {
                //The two queries below result in the same IQueryable<ProductCategory> collection  
                IQueryable<ProductCategory> productCategories = from g in adventureWorksContext.ProductCategory
                                                                .Include(p => p.Product)
                                                                where !g.Name.Contains("Test")
                                                                select g;

                //IQueryable<ProductCategory> productCategories_ = adventureWorksContext.ProductCategory
                //    .Include(p => p.Product);

                //XElement cannot contain spaces, (The ' ' character, hexadecimal value 0x20, cannot be included in a name.)
                // Construct XML root element
                XElement rootElement = new XElement("ProductCategories");
                foreach(ProductCategory p in productCategories)
                {
                    //XElement productCategory = new XElement("ProductCategory",
                    //    new XAttribute("ModifiedDate", p.ModifiedDate),
                    //    new XElement("Name", p.Name));

                    //This results in the same XML doc as above.  
                    XElement productCategory = new XElement("ProductCategory");
                    productCategory.Add(new XAttribute("ModifiedDate", p.ModifiedDate));
                    //productCategory.Add(new XElement("Name", p.Name));
                    productCategory.Add(new XAttribute("Name", p.Name));
                    //Add products
                    foreach(Product prod in p.Product)
                    {
                        XElement product = new XElement("Product");
                        product.Add(new XElement("Name", prod.Name));
                        product.Add(new XElement("Price", prod.ListPrice));
                        productCategory.Add(product);
                    }
                    
                    rootElement.Add(productCategory);                
                }
                // Persist in-memory XML data to XML file
                //var writer = XmlWriter.Create(@"C:\Projects\producst.xml");
                //rootElement.WriteTo(writer);
                //writer.Flush();
                //writer.Close();
                //System.Console.WriteLine("DONE!");

                Console.WriteLine(rootElement.ToString() + "\n\n");

                //How to query Data with LINQ to XML
                //Similar to the LINQ to Objects, when querying the XML data, we use LINQ to XML to 
                //convert the hierarchical XML data structure to the linear collection data structure, 
                //then use standard LINQ to query the data we want.
                // Query 1:
                var _productCategories = rootElement.Elements()
                    .Select(e => e.Attribute("Name").Value);
                foreach (var grp in _productCategories)
                {
                    Console.WriteLine(grp);
                }

                //As we see in the code, the Elements method can get the direct child-elements; the Descendants 
                //can get the direct and indirect child-elements - both of these methods can convert the hierarchical 
                //XML data to linear collection. After getting a linear collection, we can use standard LINQ to query 
                //the data again. In Query 2, we alternately use LINQ to XML and standard LINQ.
                var _products = rootElement.Elements()
                    .Single(e => e.Attribute("Name").Value == "Cranksets")
                    .Descendants("Product")
                    .Select(e => e.Element("Name").Value);

                Console.WriteLine("\n\n");
                foreach (var p in _products)
                {
                    Console.WriteLine(p);
                    //LL Crankset
                    //ML Crankset
                    //HL Crankset
                }

                //The average price 
                var priceAvg = rootElement.Elements()
                    .Single(e => e.Attribute("Name").Value == "Cranksets")
                    .Descendants("Product")
                    .Average(e => decimal.Parse(e.Element("Price").Value));

                Console.WriteLine($"Average Crankset price: {priceAvg}");

                /*<ProductCategories> 
                  <ProductCategory ModifiedDate="2002-06-01T00:00:00" Name="Chains">
                    <Product>
                      <Name>Chain</Name>
                      <Price>20.2400</Price>
                    </Product>
                  </ProductCategory>
                  <ProductCategory ModifiedDate="2002-06-01T00:00:00" Name="Cranksets">
                    <Product>
                      <Name>LL Crankset</Name>
                      <Price>175.4900</Price>
                    </Product>
                    <Product>
                      <Name>ML Crankset</Name>
                      <Price>256.4900</Price>
                    </Product>
                    <Product>
                      <Name>HL Crankset</Name>
                      <Price>404.9900</Price>
                    </Product>
                  </ProductCategory>
                </ProductCategories>*/
            }
        }

        /*
        So the first thing to know is that XmlConvert automatically escapes reserved items. 
        It also does more than that. Think of the Convert class in the System namespace. 
        It has several methods, such as ToInt32, ToDateTime, ToBoolean, and many more. 
        Think of the XmlConvert class as its Xml obsessed sibling. XmlConvert.ToDateTime, 
        XmlConvert.ToDecimal, XmlConvert.ToGuid, and any other method that contains “To,” 
        followed by a framework type, should be self-explanatory.
        There are countless overloads for members, such as ToInt32 or ToDateTime, and they 
        behave just as you expect and are easy to follow. The following code illustrates the 
        encoding and decoding issues
        */
        private void Example03()
        {
            String encodedFirstName = XmlConvert.EncodeName("First Name");
            Console.WriteLine("Encoded FirstName: {0}", encodedFirstName);
            String decodedFirstName = XmlConvert.DecodeName(encodedFirstName);
            Console.WriteLine("Encoded FirstName: {0}", decodedFirstName);
            String encodedFirstNameWithColon = XmlConvert.EncodeLocalName("First:Name");
            Console.WriteLine("Encoded FirstName with Colon: {0}", encodedFirstNameWithColon);
            decodedFirstName = XmlConvert.DecodeName(encodedFirstNameWithColon);
            Console.WriteLine("Encoded FirstName with Colon: {0}", decodedFirstName);
        }

        /*
        The XDocument class almost seems redundant when compared with the XElement class.
        The XDocument class contains the information necessary for a valid XML
        document. This includes an XML declaration, processing instructions and
        comments. Note that you only have to create XDocument objects if you
        require the specific functionality provided by the XDocument class. In many
        cases you can work with the XElement. Working directly with XElement is a
        simpler programming mode.
        The following list summarizes the basic components of an XDocument instance:
        ■■  One XDeclaration object The declaration enables you to specify the version of
            XML being used, the encoding, and whether the document contains a document type
            definition.
        ■■  One XElement object Because a valid document must contain one root node, there
            must be one XElement present. Note that, although you need to use an XElement to
            use an XDocument, the reverse is not the case.
        ■■  XProcessingInstruction objects Represents an XML processing instruction.
        ■■  XComment objects As with XProcessingInstruction, you can have one or more.
            According to MSDN, the only caveat is that this can’t be the first argument in the constructor
            list. Valid documents can’t start with a comment. The irony here is that there
            are no warnings generated if you use it as the first argument; the document parses
            correctly, and MSDN’s own examples show example after example that specifically
            violate this rule.
        */
        private void Example02()
        {
            //The XAttribute class is so simple that it doesn’t need much discussion. 
            //To declare one, you simply instantiate it, passing in the name of the attribute and the value:
            //XAttribute sampleAttribute = new XAttribute("FirstName", "John"); Attributes, by definition, 
            //have no meaning outside of the context of an element, so they are obviously used only in 
            //conjunction with an XElement.

            //If you want to use an XNamespace in conjunction with an XElement, you simply append it to the Element Name.
            XNamespace billCoNamespace = "http://www.billco.com/Samples";
            XNamespace jimmyCoNamespace = "http://www.jimmyco.com/Samples";
            
            XDocument sampleDoc = new XDocument(
                new XComment("This is a comment sample"),
                new XElement("Customers",
                    new XElement(jimmyCoNamespace + "Customer",
                        new XAttribute("Age", "21"),
                        new XElement("FirstName", "John"),
                        new XElement("LastName", "Deer")),
                    new XElement(billCoNamespace + "Customer",
                        new XAttribute("Age", "35"),
                        new XElement("FirstName", "Neil"),
                        new XElement("LastName", "Bridges"))));

            //sampleDoc.Save("CommentFirst.xml");
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamReader streamReader = new StreamReader(memoryStream))
                {
                    sampleDoc.Save(memoryStream);
                    memoryStream.Position = 0;
                    string xml = streamReader.ReadToEnd();
                    Console.WriteLine(xml);
                    /*<?xml version="1.0" encoding="utf-8"?>
                    <!--This is a comment sample-->
                    <Customers>
                        <Customer Age="21" xmlns="http://www.jimmyco.com/Samples">
                        <FirstName xmlns="">John</FirstName>
                        <LastName xmlns="">Deer</LastName>
                        </Customer>
                        <Customer Age="35" xmlns="http://www.billco.com/Samples">
                        <FirstName xmlns="">Neil</FirstName>
                        <LastName xmlns="">Bridges</LastName>
                        </Customer>
                    </Customers> */
                }
            }

            //Parsing XML document
            String documentData = @"<Customers>
                                        <Customer>
                                            <FirstName>John</FirstName>
                                        </Customer>
                                        <Customer>
                                            <FirstName>Bill</FirstName>
                                        </Customer>
                                        <Customer>
                                            <FirstName>Joe</FirstName>
                                        </Customer>
                                    </Customers>";

            XDocument docSample = XDocument.Parse(documentData);
            var descendantsQuery = from desc in docSample.Root.Descendants("Customer")
                                   select desc;
            var elementsQuery = from elem in docSample.Root.Elements("Customer")
                                select elem;
            int descendantsCount = descendantsQuery.Count();
            int elementsCount = elementsQuery.Count();

            //The output in both cases is 3.
            //Descendants return whatever elements you choose from the entire subtree of each element. 
            //Elements, on the other hand, yield only the child elements. The only time it matters 
            //is when there are not child elements inside one of the ones you are searching for. 
            //It behaves differently only if there are child elements that also have the same name 
            //as the element you are looking for. 
            
            //The output in both cases is 3.
            Console.WriteLine(descendantsCount.ToString());
            Console.WriteLine(elementsCount.ToString());
        }

        private void Example01()
        {
            XElement customers = 
                new XElement("Customers", 
                    new XElement("Customer", 
                        new XElement("FirstName", "John"), 
                        new XElement("MiddleInitial", "Q"),
                        new XElement("LastName", "Public")),
                    new XElement("Customer",
                        new XElement("FirstName", "Donald"),
                        new XElement("MiddleInitial", "Q7"),
                        new XElement("LastName", "Deer")
                    ));

            Console.WriteLine(customers.ToString());

            //You can easily reference items inside the node. Although these are all strings, 
            //you can easily cast them to different .NET types as needed if you query against 
            //a different object type or structure. You can iterate through all the nodes.
            //Example below shows just one node.
            string flattened = customers.Element("Customer").Element("FirstName").ToString() +
                customers.Element("Customer").Element("MiddleInitial").ToString() +
                customers.Element("Customer").Element("LastName").ToString();
            //This code produces the corresponding output:
            //<FirstName>John</FirstName><MiddleInitial>Q</MiddleInitial><LastName>Public</LastName>
            Console.WriteLine(flattened);

            /*
                Like any language, XML has characters that define it. You might come across situations in
                which you need to use those characters (and many of them happen to be characters that
                are used quite often). Greater-than and less-than symbols are part of the language syntax;
                ampersands are as well. Normally, you have to manually escape these. &gt; is used instead
                of a greater-than symbol; &lt needs to be used in place of a less-than symbol; &amp replaces
                an ampersand character. XElement automatically calls the XmlConvert class, which
                escapes these for you. As such, the following code snippet would work just fine:
            */

            XElement customers_ = new XElement("CustomerNote", 
                "The customer really likes to use < and > in their correspondence.They love using & and </ div > as well");
            Console.WriteLine(customers_.ToString());
        }
    }
}
