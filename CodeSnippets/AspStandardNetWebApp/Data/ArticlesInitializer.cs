using AspStandardNetWebApp.Data;
using AspStandardNetWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspStandardNetWebApp.Data
{
    public class ArticlesInitializer :
        System.Data.Entity.CreateDatabaseIfNotExists<AspStandardNetWebAppContext>
    //System.Data.Entity.DropCreateDatabaseIfModelChanges<AspStandardNetWebAppContext>
    {
        protected override void Seed(AspStandardNetWebAppContext context)
        {
            List<Article> articles = new List<Article>()
            {
                new Article()
                {
                    //ID=1,
                    AuthorEmail="John.Doe@gmail.com",
                    CreateDate = new DateTime(2019,12,31),
                    Description = "Description1",
                    NumberOfAuthors=1,
                    Price= 9.99m,
                    Title ="Title1"
                },

                new Article()
                {
                    //ID=2,
                    AuthorEmail="John.Doe2@gmail.com",
                    CreateDate = new DateTime(2019,11,31),
                    Description = "Description2",
                    NumberOfAuthors=2,
                    Price= 19.99m,
                    Title ="Title2"
                },

                new Article()
                {
                    //ID=3,
                    AuthorEmail="John.Doe3@gmail.com",
                    CreateDate = new DateTime(2018,12,31),
                    Description = "Description3",
                    NumberOfAuthors=3,
                    Price= 29.99m,
                    Title ="Title3"
                },

                new Article()
                {
                    //ID=4,
                    AuthorEmail="John.Doe4@gmail.com",
                    CreateDate = new DateTime(2017,12,31),
                    Description = "Description4",
                    NumberOfAuthors=4,
                    Price= 49.99m,
                    Title ="Title4"
                }
            };

            articles.ForEach(s => context.Articles.Add(s));
            context.SaveChanges();
        }
    }
}