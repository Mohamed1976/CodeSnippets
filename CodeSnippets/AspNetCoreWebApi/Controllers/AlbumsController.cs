using DataLibrary.MusicStore.Data;
using DataLibrary.MusicStore.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AlbumsController : ODataController
    {
        private readonly MusicContext _context;
        public AlbumsController(MusicContext context)
        {
            _context = context;
            if (!context.Albums.Any())
            {
                _context.Database.EnsureCreated();
            }
        }

        //[EnableQuery]
        //public IActionResult Get()
        //[ODataRoute]
        //public IQueryable<Album> Get()
        //{
        //    var albums = _context.Albums.Include("Songs").AsQueryable();
        //    return albums;
        //    //return Ok(albums);
        //}

        [EnableQuery]
        public IActionResult Get()
        {
            var albums = _context.Albums;
            return Ok(albums);
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var album = _context.Albums.Include(a => a.Songs).Where(a => a.Id == key);
            if (album == null)
            {
                return NotFound();
            }
            return Ok(album);
        }
    }
}

/*
There are some difficulties using swagger and OData. To use OData uncomment OData in startup.cs.
You can then use postman to query this controller using odata syntax, as shown below. 

Used References:

1) Using OData Controller in .NET Core APIs
https://medium.com/@sddkal/using-odata-controller-in-net-core-apis-63b688585eaf

https://weblogs.asp.net/ricardoperes/asp-net-core-odata-part-1
https://devblogs.microsoft.com/odata/asp-net-core-odata-now-available/
https://devblogs.microsoft.com/odata/supercharging-asp-net-core-api-with-odata/
https://devblogs.microsoft.com/odata/experimenting-with-odata-in-asp-net-core-3-1/
https://devblogs.microsoft.com/odata/select-enhancement-in-asp-net-core-odata/
https://github.com/hassanhabib/ODataDemo
https://docs.microsoft.com/en-us/aspnet/web-api/overview/odata-support-in-aspnet-web-api/odata-v4/create-an-odata-v4-endpoint
https://docs.microsoft.com/en-us/aspnet/web-api/overview/odata-support-in-aspnet-web-api/supporting-odata-query-options
https://docs.microsoft.com/en-us/dynamics-nav/using-filter-expressions-in-odata-uris

If you use app.UseMvc(routeBuilder => to configure OData, you need to configure
services.AddMvc(options => options.EnableEndpointRouting = false); as described in link below
https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-3.1&tabs=visual-studio#use-mvc-without-endpoint-routing

Hot to configure OData with Swagger? 
https://github.com/OData/WebApi/issues/2024
https://voidnish.wordpress.com/2018/08/17/asp-net-core-odata-and-swashbuckle-workaround-for-error/
https://devblogs.microsoft.com/odata/enabling-endpoint-routing-in-odata/
https://stackoverflow.com/questions/57236413/how-enable-swagger-for-asp-net-core-odata-api
https://joonasw.net/view/hide-actions-from-swagger-openapi-documentation-in-aspnet-core


$orderby
Defines a sort order to be used. You can use one or more properties that need to be separated by commas if more than one will be used:
http://servicehost/ExamPrepService.svc/Questions?$orderby=Id,Description
$top
The number of entities to return in the feed. Corresponds to the TOP function in SQL:
http://servicehost/ExamPrepService.svc/Questions?$top=5
$skip
Indicates how many records should be ignored before starting to return values. Assume that there are 30 topics in the previous data set, and you want to skip the first 10 and return the next 10 after that. The following would do it:
http://servicehost/ExamPrepService.svc/Questions?$skip=10&$top=5
$filter
Specifies a condition or conditions to filter on:
http://servicehost/ExamPrepService.svc/Questions?$filter=Id gt 5
$expand
Indicates which related entities are returned by the query. They will be included either as a feed or as an entry inline return with the query:
http://servicehost/ExamPrepService.svc/Questions?$expand=Answers
$select
By default, all properties of an entity are returned in the feed. This is the equivalent of SELECT * in SQL. By using $select, you can indicate a comma separated list to just return the fields you want back: http://servicehost/ExamPrepService.svc/Questions&$select=Id, Text,Description,Author
$inlinecount
Returns the number of entries returned (in the <count> element). If the following collection had 30 total values, the feed would have a <count> element indicating 30:
http://servicehost/ExamPrepService.svc/Questions?$inlinecount=allpages

Request: https://localhost:5001/odata
Response: {
    "@odata.context": "https://localhost:5001/odata/$metadata",
    "value": [
        {
            "name": "Albums",
            "kind": "EntitySet",
            "url": "Albums"
        },
        {
            "name": "Songs",
            "kind": "EntitySet",
            "url": "Songs"
        }
    ]
}

Normal Query
https://localhost:5001/odata/Albums
{
    "@odata.context": "https://localhost:5001/odata/$metadata#Albums",
    "value": [
        {
            "Id": 1,
            "Name": "The Court of the Crimson King"
        },
        {
            "Id": 2,
            "Name": "Smile (Katy Perry song)"
        }
    ]
}

Query by ID
https://localhost:5001/odata/Albums(2)
{
    "@odata.context": "https://localhost:5001/odata/$metadata#Albums",
    "value": [
        {
            "Id": 2,
            "Name": "Smile (Katy Perry song)"
        }
    ]
}

Expand
https://localhost:5001/odata/Albums(2)?$Expand=Songs
{
    "@odata.context": "https://localhost:5001/odata/$metadata#Albums(Songs())",
    "value": [
        {
            "Id": 2,
            "Name": "Smile (Katy Perry song)",
            "Songs": [
                {
                    "Id": 3,
                    "Name": "Happy days are coming to you",
                    "Duration": 89
                },
                {
                    "Id": 4,
                    "Name": "Fly through the wind",
                    "Duration": 46
                },
                {
                    "Id": 5,
                    "Name": "Love like its the last day",
                    "Duration": 71
                }
            ]
        }
    ]
}

https://localhost:5001/odata/Albums(2)?$Expand=Songs($Select=Name)
{
    "@odata.context": "https://localhost:5001/odata/$metadata#Albums(Songs(Name))",
    "value": [
        {
            "Id": 2,
            "Name": "Smile (Katy Perry song)",
            "Songs": [
                {
                    "Name": "Happy days are coming to you"
                },
                {
                    "Name": "Fly through the wind"
                },
                {
                    "Name": "Love like its the last day"
                }
            ]
        }
    ]
}

Projection using Select
https://localhost:5001/odata/Albums(2)?$Expand=Songs($Select=Name,Duration)
{
    "@odata.context": "https://localhost:5001/odata/$metadata#Albums(Songs(Name,Duration))",
    "value": [
        {
            "Id": 2,
            "Name": "Smile (Katy Perry song)",
            "Songs": [
                {
                    "Name": "Happy days are coming to you",
                    "Duration": 89
                },
                {
                    "Name": "Fly through the wind",
                    "Duration": 46
                },
                {
                    "Name": "Love like its the last day",
                    "Duration": 71
                }
            ]
        }
    ]
}

Skipping Records
https://localhost:5001/odata/Albums(2)?$Expand=Songs($Select=Name,Duration;$skip=1)
{
    "@odata.context": "https://localhost:5001/odata/$metadata#Albums(Songs(Name,Duration))",
    "value": [
        {
            "Id": 2,
            "Name": "Smile (Katy Perry song)",
            "Songs": [
                {
                    "Name": "Fly through the wind",
                    "Duration": 46
                },
                {
                    "Name": "Love like its the last day",
                    "Duration": 71
                }
            ]
        }
    ]
}

Limit Number of Results via Top
https://localhost:5001/odata/Albums(2)?$Expand=Songs($Select=Name,Duration;$top=1)
{
    "@odata.context": "https://localhost:5001/odata/$metadata#Albums(Songs(Name,Duration))",
    "value": [
        {
            "Id": 2,
            "Name": "Smile (Katy Perry song)",
            "Songs": [
                {
                    "Name": "Happy days are coming to you",
                    "Duration": 89
                }
            ]
        }
    ]
}

OrderBy
https://localhost:5001/odata/Albums(2)?$Expand=Songs($Select=Name,Duration;$top=3;$OrderBy=Duration asc)
{
    "@odata.context": "https://localhost:5001/odata/$metadata#Albums(Songs(Name,Duration))",
    "value": [
        {
            "Id": 2,
            "Name": "Smile (Katy Perry song)",
            "Songs": [
                {
                    "Name": "Fly through the wind",
                    "Duration": 46
                },
                {
                    "Name": "Love like its the last day",
                    "Duration": 71
                },
                {
                    "Name": "Happy days are coming to you",
                    "Duration": 89
                }
            ]
        }
    ]
}


Filter
https://localhost:5001/odata/Albums(2)?$Expand=Songs($Select=Name,Duration;$top=3;$OrderBy=Duration asc;$filter=startswith(Name,'Love'))
{
    "@odata.context": "https://localhost:5001/odata/$metadata#Albums(Songs(Name,Duration))",
    "value": [
        {
            "Id": 2,
            "Name": "Smile (Katy Perry song)",
            "Songs": [
                {
                    "Name": "Love like its the last day",
                    "Duration": 71
                }
            ]
        }
    ]
}

Count
https://localhost:5001/odata/Albums(2)?$Expand=Songs($Count=true)
{
    "@odata.context": "https://localhost:5001/odata/$metadata#Albums(Songs())",
    "value": [
        {
            "Id": 2,
            "Name": "Smile (Katy Perry song)",
            "Songs@odata.count": 3,
            "Songs": [
                {
                    "Id": 3,
                    "Name": "Happy days are coming to you",
                    "Duration": 89
                },
                {
                    "Id": 4,
                    "Name": "Fly through the wind",
                    "Duration": 46
                },
                {
                    "Id": 5,
                    "Name": "Love like its the last day",
                    "Duration": 71
                }
            ]
        }
    ]
}

filter
https://localhost:5001/odata/Albums?$Expand=Songs($filter=Duration gt 100 and startswith(Name,'21st');$orderby=name desc)
{
    "@odata.context": "https://localhost:5001/odata/$metadata#Albums(Songs())",
    "value": [
        {
            "Id": 1,
            "Name": "The Court of the Crimson King",
            "Songs": [
                {
                    "Id": 1,
                    "Name": "21st Century Schzoid Man",
                    "Duration": 110
                }
            ]
        },
        {
            "Id": 2,
            "Name": "Smile (Katy Perry song)",
            "Songs": []
        }
    ]
}

Longest song in each album
https://localhost:5001/odata/Albums?$Expand=Songs($orderby=Duration desc;$top=1)
{
    "@odata.context": "https://localhost:5001/odata/$metadata#Albums(Songs())",
    "value": [
        {
            "Id": 1,
            "Name": "The Court of the Crimson King",
            "Songs": [
                {
                    "Id": 2,
                    "Name": "I Talk to the Wind",
                    "Duration": 140
                }
            ]
        },
        {
            "Id": 2,
            "Name": "Smile (Katy Perry song)",
            "Songs": [
                {
                    "Id": 3,
                    "Name": "Happy days are coming to you",
                    "Duration": 89
                }
            ]
        }
    ]
}

https://localhost:5001/odata/Albums(2)
{
    "@odata.context": "https://localhost:5001/odata/$metadata#Albums",
    "value": [
        {
            "Id": 2,
            "Name": "Smile (Katy Perry song)"
        }
    ]
}

skip and top
https://localhost:5001/odata/Albums?$Expand=Songs($Orderby=Duration desc;$skip=1;$top=2)
{
    "@odata.context": "https://localhost:5001/odata/$metadata#Albums(Songs())",
    "value": [
        {
            "Id": 1,
            "Name": "The Court of the Crimson King",
            "Songs": [
                {
                    "Id": 1,
                    "Name": "21st Century Schzoid Man",
                    "Duration": 110
                }
            ]
        },
        {
            "Id": 2,
            "Name": "Smile (Katy Perry song)",
            "Songs": [
                {
                    "Id": 5,
                    "Name": "Love like its the last day",
                    "Duration": 71
                },
                {
                    "Id": 4,
                    "Name": "Fly through the wind",
                    "Duration": 46
                }
            ]
        }
    ]
}

https://localhost:5001/odata/Albums?$select=name
{
    "@odata.context": "https://localhost:5001/odata/$metadata#Albums(Name)",
    "value": [
        {
            "Name": "The Court of the Crimson King"
        },
        {
            "Name": "Smile (Katy Perry song)"
        }
    ]
}


https://localhost:5001/odata/Albums(2)?$select=name
{
    "@odata.context": "https://localhost:5001/odata/$metadata#Albums(Name)",
    "value": [
        {
            "Name": "Smile (Katy Perry song)"
        }
    ]
}

*/ 