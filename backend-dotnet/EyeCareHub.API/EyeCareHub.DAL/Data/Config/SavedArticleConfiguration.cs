using EyeCareHub.DAL.Entities.Content_Education;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Data.Config
{
    public class SavedArticleConfiguration : IEntityTypeConfiguration<SavedArticle>
    {
        public void Configure(EntityTypeBuilder<SavedArticle> builder)
        {
            
            builder
                .HasKey(sa => new { sa.UserEmail, sa.ArticleId });

            builder.Property(sa => sa.UserEmail).IsRequired();

            builder
                .HasOne(sa => sa.Article)
                .WithMany(a => a.SavedArticles) 
                .HasForeignKey(sa => sa.ArticleId);
        }
    }
}
