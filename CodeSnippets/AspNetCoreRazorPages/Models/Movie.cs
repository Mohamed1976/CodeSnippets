using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

//The DataAnnotations namespace provides a set of built-in validation attributes that are applied 
//declaratively to a class or property.DataAnnotations also contains formatting attributes like 
//DataType that help with formatting and don't provide any validation.
//Update the Movie class to take advantage of the built-in Required, StringLength, 
//RegularExpression, and Range validation attributes.

//Having validation rules automatically enforced by ASP.NET Core helps make your app more robust.
//It also ensures that you can't forget to validate something and inadvertently let bad data into the database.

//Notice how the form has automatically rendered a validation error message in each field 
//containing an invalid value.The errors are enforced both client-side (using JavaScript and jQuery) 
//and server-side(when a user has JavaScript disabled).
//A significant benefit is that no code changes were necessary in the Create or Edit pages.
//Once DataAnnotations were applied to the model, the validation UI was enabled.
//The Razor Pages created in this tutorial automatically picked up the validation rules 
//(using validation attributes on the properties of the Movie model class). 
//Test validation using the Edit page, the same validation is applied.
//The form data isn't posted to the server until there are no client-side validation errors.

//[DataType(DataType.Date)]
//[DataType(DataType.Currency)]
//The DataType attributes only provide hints for the view engine to format the data(and supplies 
//attributes such as <a> for URL's and <a href="mailto:EmailAddress.com"> for email). Use the 
//RegularExpression attribute to validate the format of the data. The DataType attribute is used 
//to specify a data type that's more specific than the database intrinsic type. DataType attributes 
//are not validation attributes. In the sample application, only the date is displayed, without time.
//The DataType Enumeration provides for many data types, such as Date, Time, PhoneNumber, Currency, 
//EmailAddress, and more. The DataType attribute can also enable the application to automatically 
//provide type-specific features. For example, a mailto: link can be created for DataType.EmailAddress.
//A date selector can be provided for DataType.Date in browsers that support HTML5. The DataType 
//attributes emits HTML 5 data- (pronounced data dash) attributes that HTML 5 browsers consume.
//The DataType attributes do not provide any validation.
//DataType.Date doesn't specify the format of the date that's displayed. By default, the data 
//field is displayed according to the default formats based on the server's CultureInfo.

//The[Column(TypeName = "decimal(18, 2)")] data annotation is required so Entity Framework 
//Core can correctly map Price to currency in the database

//[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
//The DisplayFormat attribute is used to explicitly specify the date format:
//The ApplyFormatInEditMode setting specifies that the formatting should be applied 
//when the value is displayed for editing. You might not want that behavior for some fields. 
//For example, in currency values, you probably don't want the currency symbol in the edit UI.
//The DisplayFormat attribute can be used by itself, but it's generally a good idea to use the DataType
//attribute. The DataType attribute conveys the semantics of the data as opposed to how to render it
//on a screen, and provides the following benefits that you don't get with DisplayFormat:
//The browser can enable HTML5 features(for example to show a calendar control, the locale-appropriate currency symbol, email links, etc.)
//By default, the browser will render data using the correct format based on your locale.
//The DataType attribute can enable the ASP.NET Core framework to choose the right field template to render the data.The DisplayFormat if used by itself uses the string template.

//[Range(typeof(DateTime), "1/1/1966", "1/1/2020")]
//Note: jQuery validation doesn't work with the Range attribute and DateTime. For example, 
//the following code will always display a client-side validation error, even when the date 
//is in the specified range:
//It's generally not a good practice to compile hard dates in your models, so using the 
//Range attribute and DateTime is discouraged.

namespace AspNetCoreRazorPages.Models
{
    public class Movie
    {
        //The ID field is required by the database for the primary key.
        public int ID { get; set; }

        //The Required and MinimumLength attributes indicate that a property must have a value; 
        //but nothing prevents a user from entering white space to satisfy this validation.
        [StringLength(60, MinimumLength = 3), Required]
        //[Required]
        public string Title { get; set; }

        //DataAnnotations is covered in the next tutorial.The Display attribute specifies what to display 
        //for the name of a field(in this case "Release Date" instead of "ReleaseDate"). 
        //The DataType attribute specifies the type of the data(Date), so the time information stored 
        //in the field isn't displayed.
        //[DataType(DataType.Date)]: The DataType attribute specifies the type of the data (Date).
        //With this attribute:
        //The user is not required to enter time information in the date field.
        //Only the date is displayed, not time information.
        //[Display(Name = "Release Date")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Release Date"), DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        //The RegularExpression attribute is used to limit what characters can be input. 
        //Must only use letters. The first letter is required to be uppercase.
        //Genre consist only of letters A-Z 
        //[RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        //[Required]
        //[StringLength(30)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$"), Required, StringLength(30)]
        public string Genre { get; set; }

        //You may not be able to enter decimal commas in the Price field.To support jQuery validation 
        //for non-English locales that use a comma(",") for a decimal point and for non US-English date 
        //formats, the app must be globalized.For globalization instructions, see this GitHub issue.
        //https://jqueryvalidation.org/
        //https://github.com/dotnet/AspNetCore.Docs/issues/4076#issuecomment-326590420

        //https://docs.microsoft.com/en-us/ef/core/modeling/entity-properties?tabs=data-annotations%2Cwithout-nrt#column-data-types
        //The [Column(TypeName = "decimal(18, 2)")] data annotation enables Entity Framework Core to 
        //correctly map Price to currency in the database.
        //The Range attribute constrains a value to within a specified range.
        //Value types (such as decimal, int, float, DateTime) 
        //are inherently required and don't need the [Required] attribute.
        //[Range(1, 100)]
        //[DataType(DataType.Currency)]
        [Range(1, 100), DataType(DataType.Currency), Column(TypeName = "decimal(18, 2)")]
        //[Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        //The RegularExpression "Rating":
        //Requires that the first character be an uppercase letter.
        //Rating may consist of letters A-Z, 0-9, " ' - and space
        //[RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        //[StringLength(5)]
        //[Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$"), StringLength(5), Required]
        public string Rating { get; set; }
    }
}
