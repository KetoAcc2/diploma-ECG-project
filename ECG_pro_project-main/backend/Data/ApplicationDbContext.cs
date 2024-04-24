using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
public class ApplicationDbContext : IdentityDbContext<User, Role, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    private static User Register(User user)
    {
        CreatePasswordHash(user.Password!, out byte[] passwordHash, out byte[] passwordSalt);

        return new User
        {
            Id = user.Id,
            Email = user.Email,
            Password = user.Password,
            UserSalt = passwordSalt,
            UserHash = passwordHash,
            Role = user.Role,
            CreatedGroups = user.CreatedGroups,
            EmailConfirmed = user.EmailConfirmed
        };
    }
    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        User u = Register(new User { Id = 99, Email = "admin@test.com", Password = "admin123", Role = UserRoleType.PSEUDO_ADMIN, EmailConfirmed = true });
        User u1 = Register(new User { Id = 1, Email = "john@test.com", Password = "teacher123", Role = UserRoleType.TEACHER, EmailConfirmed = true });
        User u2 = Register(new User { Id = 5, Email = "johnwick@test.com", Password = "teacher123", Role = UserRoleType.TEACHER, EmailConfirmed = true });
        User u3 = Register(new User { Id = 10, Email = "apachehelicopter@gmail.com", Password = "youwishyouknew", Role = UserRoleType.TEACHER, EmailConfirmed = true });
        Group g1 = new Group { GroupId = 1, GroupName = "2022_Cardilogy_3c", GroupCode = "xdtyjs", GroupOwner = u1.Id };
        Group g2 = new Group { GroupId = 2, GroupName = "2022_Physiology_4a", GroupCode = "poiuyt", GroupOwner = u2.Id };
        Group g3 = new Group { GroupId = 3, GroupName = "2022_Cardilogy_10c", GroupCode = "123456", GroupOwner = u3.Id };

        base.OnModelCreating(builder);
        builder.Entity<User>((Action<Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User>>)(entity =>
        {
            entity.HasKey(e => e.Id).HasName("id_pk");
            entity.ToTable<User>("Users");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasMany(d => d.CreatedGroups)
            .WithOne(d => d.IdGroupOwnerNavigation)
            .HasForeignKey(d => d.GroupOwner)
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasData(
                u,
               u1,
               Register(new User { Id = 2, Email = "dopinder@test.com", Password = "student123", Role = UserRoleType.STUDENT, EmailConfirmed = true }),
               Register(new User { Id = 3, Email = "godzilla@test.com", Password = "teacher123", Role = UserRoleType.TEACHER, EmailConfirmed = true }),
               Register(new User { Id = 4, Email = "mike.wazowski@test.com", Password = "student123", Role = UserRoleType.STUDENT, EmailConfirmed = true }),
               u2,
               Register(new User { Id = 6, Email = "alibaba@test.com", Password = "student123", Role = UserRoleType.STUDENT, EmailConfirmed = true }),
               Register(new User { Id = 7, Email = "hacker_master@test.com", Password = "student123", Role = UserRoleType.STUDENT, EmailConfirmed = true }),
               Register(new User { Id = 8, Email = "hacker_master123@test.com", Password = "student123", Role = UserRoleType.STUDENT, EmailConfirmed = true }),
               Register(new User { Id = 9, Email = "pewdiepie@test.com", Password = "student123", Role = UserRoleType.STUDENT, EmailConfirmed = true }),
               u3
            );
        }));
        builder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("groupId_pk");
            entity.ToTable("Groups");
            entity.Property(e => e.GroupId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.IdGroupOwnerNavigation)
            .WithMany(d => d.CreatedGroups)
            .HasForeignKey(d => d.GroupOwner);

            entity.HasMany(d => d.JoinedGroups)
            .WithOne(d => d.IdGroupNavigation)
            .HasForeignKey(d => d.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasData(
                g1, g2, g3
            );
        }
        );

        builder.Entity<User_Group>(entity =>
        {
            entity.HasKey(d => d.User_GroupId);
            entity.ToTable("Users_Groups");
            entity.Property(e => e.User_GroupId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.IdGroupNavigation)
            .WithMany(d => d.JoinedGroups)
            .HasForeignKey(d => d.GroupId);

            entity.HasOne(d => d.IdUserNavigation)
            .WithMany(d => d.JoinedGroups)
            .HasForeignKey(d => d.UserId);

            entity.HasData(
                new User_Group { User_GroupId = 1, UserId = 1, GroupId = 1 },
                new User_Group { User_GroupId = 2, UserId = 1, GroupId = 2 },
                new User_Group { User_GroupId = 3, UserId = 1, GroupId = 3 },
                new User_Group { User_GroupId = 4, UserId = 2, GroupId = 1 },
                new User_Group { User_GroupId = 5, UserId = 2, GroupId = 3 },
                new User_Group { User_GroupId = 6, UserId = 3, GroupId = 1 },
                new User_Group { User_GroupId = 7, UserId = 4, GroupId = 1 },
                new User_Group { User_GroupId = 8, UserId = 5, GroupId = 1 },
                new User_Group { User_GroupId = 9, UserId = 6, GroupId = 1 },
                new User_Group { User_GroupId = 10, UserId = 7, GroupId = 2 },
                new User_Group { User_GroupId = 11, UserId = 8, GroupId = 2 },
                new User_Group { User_GroupId = 12, UserId = 9, GroupId = 2 },
                new User_Group { User_GroupId = 13, UserId = 10, GroupId = 2 },
                new User_Group { User_GroupId = 14, UserId = 10, GroupId = 1 },
                new User_Group { User_GroupId = 15, UserId = 9, GroupId = 3 }
            );
        });
        builder.Entity<Task>(entity =>
        {
            entity.HasKey(d => d.TaskId);
            entity.ToTable("Tasks");
            entity.Property(e => e.TaskId).ValueGeneratedOnAdd();

            entity.HasMany(d => d.GroupsAssignedWithThisTask)
            .WithOne(d => d.TaskAssigned)
            .HasForeignKey(d => d.TaskAssignedId)
            .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Task_Group>(entity =>
        {
            entity.HasKey(d => d.TaskGroupId);
            entity.ToTable("Tasks_Groups");
            entity.Property(e => e.TaskGroupId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.TaskAssigned)
            .WithMany(d => d.GroupsAssignedWithThisTask)
            .HasForeignKey(d => d.TaskAssignedId);

            entity.HasOne(d => d.AssignedUserGroup)
            .WithMany(d => d.TaskGroups)
            .HasForeignKey(d => d.AssignedUserGroupId);

            entity.HasOne(d => d.ECGDiagramNavigation)
            .WithMany(d => d.TaskGroups)
            .HasForeignKey(d => d.ECGDiagramId);
        });

        builder.Entity<Task_Question>(entity =>
        {
            entity.HasKey(d => d.TaskQuestionId);
            entity.ToTable("Tasks_Questions");
            entity.Property(e => e.TaskQuestionId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.UserNavigation)
            .WithMany(d => d.TaskQuestions)
            .HasForeignKey(d => d.UserId);

            entity.HasOne(d => d.BelongedTaskGroup)
            .WithMany(d => d.TaskQuestions)
            .HasForeignKey(d => d.BelongedTaskGroupId);
        });


        builder.Entity<Question_Type>(entity =>
        {
            entity.HasKey(d => d.Question_TypeId);
            entity.ToTable("Question_Type");
            entity.Property(e => e.Question_TypeId).ValueGeneratedOnAdd();

            entity.HasMany(d => d.ECGDiagramsNavigation)
            .WithOne(d => d.QuestionType)
            .HasForeignKey(d => d.ECGDiagramId);

            entity.HasData(
                new Question_Type { Question_TypeId = 1, QuestionTypeText = "Hyponatraemia 1" }
            );

        });

        builder.Entity<ECGDiagram>(e =>
        {
            e.HasKey(d => d.ECGDiagramId);
            e.ToTable("ECGDiagrams");
            e.Property(e => e.ECGDiagramId).ValueGeneratedOnAdd();

            e.HasData(new ECGDiagram { ECGDiagramId = 1, Image = "./Assets/Images/case1_ecg.png", QuestionTypeId = 1, Komor = 64, PR = 930, PQ = 160, QT = 400, QTC = 412});

        });

        builder.Entity<CheatSheet>(e =>
        {
            e.HasKey(d => d.CheatSheetId);
            e.ToTable("CheatSheets");
            e.Property(d => d.CheatSheetId).ValueGeneratedOnAdd();

            e.HasOne(d => d.ECGDiagramNavigation)
            .WithMany(d => d.CheatSheets)
            .HasForeignKey(d => d.ECGDiagramId);

            //params
            //int ecgId, int cheatId, int parentNumber, int questionNumber, int answerNumber, string ansStr
            e.HasData(
                //for Hyponatraemia 1
                NewCheatSheet(1, 1, 1, 1, 1, "64"),
                NewCheatSheet(1, 2, 2, 1, 1, true.ToString()),
                NewCheatSheet(1, 3, 3, 1, 1, true.ToString()),
                NewCheatSheet(1, 4, 5, 1, 1, true.ToString()),
                NewCheatSheet(1, 5, 6, 1, 1, true.ToString()),
                NewCheatSheet(1, 6, 7, 1, 1, true.ToString()),
                NewCheatSheet(1, 7, 8, 1, 1, true.ToString()),
                NewCheatSheet(1, 8, 9, 1, 1, true.ToString()),
                NewCheatSheet(1, 9, 10, 1, 1, true.ToString()),
                NewCheatSheet(1, 10, 11, 1, 1, true.ToString()),
                NewCheatSheet(1, 11, 12, 1, 1, true.ToString()),
                NewCheatSheet(1, 12, 13, 1, 1, true.ToString()),
                NewCheatSheet(1, 13, 14, 1, 1, true.ToString()),
                NewCheatSheet(1, 14, 15, 1, 1, true.ToString())
            );
        });
        builder.Entity<Docs>(e =>
        {
            e.HasKey(d => d.DocId);
            e.ToTable("Docs");
            e.Property(d => d.DocId).ValueGeneratedOnAdd();

            e.HasData(
                new Docs
                {
                    DocId = 1,
                    DocPath = "/Assets/Docs/it_instruction.docx"
                },
                new Docs
                {
                    DocId = 2,
                    DocPath = "/Assets/Docs/teacher-manual.docx"
                },
                new Docs
                {
                    DocId = 3,
                    DocPath = "/Assets/Docs/student-manual.docx"
                }
            );
        });
    }

    private CheatSheet NewCheatSheet(int ecgId, int cheatId, int parentNumber, int questionNumber, int answerNumber, string ansStr)
    {
        return new CheatSheet { ECGDiagramId = ecgId, CheatSheetId = cheatId, ParentQuestionNumber = parentNumber, QuestionNumber = questionNumber, AnswerNumber = answerNumber, Answer = ansStr };
    }

    public DbSet<Docs> Docs { get; set; } = default!;
    public DbSet<Group> Groups { get; set; } = default!;
    public DbSet<User_Group> Users_Groups { get; set; } = default!;
    public DbSet<Task> Tasks { get; set; } = default!;
    public DbSet<Task_Question> Tasks_Questions { get; set; } = default!;
    public DbSet<Task_Group> Tasks_Groups { get; set; } = default!;
    public DbSet<Question_Type> Question_Types { get; set; } = default!;
    public DbSet<Patient> Patients { get; set; } = default!;
    public DbSet<ECGDiagram> ECGDiagrams { get; set; } = default!;

    public DbSet<CheatSheet> CheatSheets { get; set; } = default!;
}