using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DSHI_diplom.Model;

public partial class DiplomContext : DbContext
{
    public DiplomContext()
    {
    }

    public DiplomContext(DbContextOptions<DiplomContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<ApplicationFile> ApplicationFiles { get; set; }

    public virtual DbSet<AudioFile> AudioFiles { get; set; }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<CollectionOfNote> CollectionOfNotes { get; set; }

    public virtual DbSet<CollectionOfNotesWithNote> CollectionOfNotesWithNotes { get; set; }

    public virtual DbSet<CollectionOfThMlWithThM> CollectionOfThMlWithThMs { get; set; }

    public virtual DbSet<CollectionOfTheoreticalMaterial> CollectionOfTheoreticalMaterials { get; set; }

    public virtual DbSet<Composer> Composers { get; set; }

    public virtual DbSet<Instrument> Instruments { get; set; }

    public virtual DbSet<MusicalForm> MusicalForms { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<TestResult> TestResults { get; set; }

    public virtual DbSet<TheoreticalMaterial> TheoreticalMaterials { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=LAPTOP-LQJASQ0O\\SQLDEV;Database=diplom;Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Answers__3213E83FF2267E9F");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.Rightt).HasColumnName("rightt");
            entity.Property(e => e.Variant)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("variant");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__Answers__questio__59FA5E80");
        });

        modelBuilder.Entity<ApplicationFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Files__3213E83F15EA6BEB");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Path)
                .IsUnicode(false)
                .HasColumnName("path");
        });

        modelBuilder.Entity<AudioFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AudioFil__3213E83FC660FB40");

            entity.HasIndex(e => e.NotesId, "UQ__AudioFil__8D8E58676E487054").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NotesId).HasColumnName("notes_id");
            entity.Property(e => e.Path)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("path");

            entity.HasOne(d => d.Notes).WithOne(p => p.AudioFile)
                .HasForeignKey<AudioFile>(d => d.NotesId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__AudioFile__notes__619B8048");
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Author__3213E83FC766F95D");

            entity.ToTable("Author");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Classes__3213E83FE581837F");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<CollectionOfNote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Collecti__3213E83FE4218A92");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.CollectionOfNotes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Collectio__user___4D94879B");
        });

        modelBuilder.Entity<CollectionOfNotesWithNote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Collecti__3213E83F9B4BCABA");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CollectionId).HasColumnName("collection_id");
            entity.Property(e => e.NotesId).HasColumnName("notes_id");

            entity.HasOne(d => d.Collection).WithMany(p => p.CollectionOfNotesWithNotes)
                .HasForeignKey(d => d.CollectionId)
                .HasConstraintName("FK__Collectio__colle__6477ECF3");

            entity.HasOne(d => d.Notes).WithMany(p => p.CollectionOfNotesWithNotes)
                .HasForeignKey(d => d.NotesId)
                .HasConstraintName("FK__Collectio__notes__656C112C");
        });

        modelBuilder.Entity<CollectionOfThMlWithThM>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Collecti__3213E83F820FC709");

            entity.ToTable("CollectionOfThMlWithThM");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CollectionId).HasColumnName("collection_id");
            entity.Property(e => e.ThmId).HasColumnName("thm_id");

            entity.HasOne(d => d.Collection).WithMany(p => p.CollectionOfThMlWithThMs)
                .HasForeignKey(d => d.CollectionId)
                .HasConstraintName("FK__Collectio__colle__68487DD7");

            entity.HasOne(d => d.Thm).WithMany(p => p.CollectionOfThMlWithThMs)
                .HasForeignKey(d => d.ThmId)
                .HasConstraintName("FK__Collectio__thm_i__693CA210");
        });

        modelBuilder.Entity<CollectionOfTheoreticalMaterial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Collecti__3213E83F25F9F509");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.CollectionOfTheoreticalMaterials)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Collectio__user___5070F446");
        });

        modelBuilder.Entity<Composer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Composer__3213E83F093F00A4");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Instrument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Instrume__3213E83FB04E9B03");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<MusicalForm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MusicalF__3213E83F2FAE9E9B");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notes__3213E83FEF78DC35");

            entity.HasIndex(e => e.FileId, "UQ__Notes__07D884C778AC092C").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.ComposerId).HasColumnName("composer_id");
            entity.Property(e => e.DateOfCreate).HasColumnName("date_of_create");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.FileId).HasColumnName("file_id");
            entity.Property(e => e.InstrumentId).HasColumnName("instrument_id");
            entity.Property(e => e.MusicalformId).HasColumnName("musicalform_id");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("name");

            entity.HasOne(d => d.Class).WithMany(p => p.Notes)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Notes__class_id__3C69FB99");

            entity.HasOne(d => d.Composer).WithMany(p => p.Notes)
                .HasForeignKey(d => d.ComposerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Notes__composer___3B75D760");

            entity.HasOne(d => d.File).WithOne(p => p.Note)
                .HasForeignKey<Note>(d => d.FileId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Notes__file_id__3E52440B");

            entity.HasOne(d => d.Instrument).WithMany(p => p.Notes)
                .HasForeignKey(d => d.InstrumentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Notes_Instruments");

            entity.HasOne(d => d.Musicalform).WithMany(p => p.Notes)
                .HasForeignKey(d => d.MusicalformId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Notes__musicalfo__3D5E1FD2");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3213E83F72125BDD");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TestId).HasColumnName("test_id");
            entity.Property(e => e.Text)
                .HasColumnType("text")
                .HasColumnName("text");

            entity.HasOne(d => d.Test).WithMany(p => p.Questions)
                .HasForeignKey(d => d.TestId)
                .HasConstraintName("FK__Questions__test___571DF1D5");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3213E83FE6B265BF");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subject__3213E83F59AB958D");

            entity.ToTable("Subject");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tests__3213E83FBFD9FEF0");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");

            entity.HasOne(d => d.Class).WithMany(p => p.Tests)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Tests__class_id__534D60F1");

            entity.HasOne(d => d.Subject).WithMany(p => p.Tests)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Tests__subject_i__5441852A");
        });

        modelBuilder.Entity<TestResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestResu__3213E83FDB40387F");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CorrectAnswers).HasColumnName("correct_answers");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.TestId).HasColumnName("test_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Test).WithMany(p => p.TestResults)
                .HasForeignKey(d => d.TestId)
                .HasConstraintName("FK__TestResul__test___5CD6CB2B");

            entity.HasOne(d => d.User).WithMany(p => p.TestResults)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__TestResul__user___5DCAEF64");
        });

        modelBuilder.Entity<TheoreticalMaterial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Theoreti__3213E83F02C50FB0");

            entity.ToTable("TheoreticalMaterial");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.DateOfCreate).HasColumnName("date_of_create");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.FileId).HasColumnName("file_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");

            entity.HasOne(d => d.Author).WithMany(p => p.TheoreticalMaterials)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Theoretic__autho__49C3F6B7");

            entity.HasOne(d => d.Class).WithMany(p => p.TheoreticalMaterials)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Theoretic__class__48CFD27E");

            entity.HasOne(d => d.File).WithMany(p => p.TheoreticalMaterials)
                .HasForeignKey(d => d.FileId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Theoretic__file___4AB81AF0");

            entity.HasOne(d => d.Subject).WithMany(p => p.TheoreticalMaterials)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Theoretic__subje__47DBAE45");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3213E83F619A065D");

            entity.HasIndex(e => e.Login, "UQ__Users__7838F2724186B0EF").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Login)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("login");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("middle_name");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Users__role_id__276EDEB3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
