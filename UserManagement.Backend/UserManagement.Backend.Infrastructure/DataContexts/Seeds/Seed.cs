using Microsoft.EntityFrameworkCore;
using UserManagement.Backend.Domain.Database;
using UserManagement.Backend.Domain.LookupCodes;
using UserManagement.Backend.Infrastructure.DataContext;

namespace UserManagement.Backend.Infrastructure.DataContexts.Seeds;

public class Seed
{
    public static void SeedDataAndMigrate(ManagementContext context)
    {

        context.Database.Migrate();
        if (!context.Permissions.Any())
        {
            var permissions = new List<Permission>()
            {
                Permission.Create("PEM0000001", "Can manage all other Cats"),
                Permission.Create("PEM0000002", "Can summon unlimited catnip from thin air"),
                Permission.Create("PEM0000003", "Has the power to hypnotize humans into providing endless belly rubs"),
                Permission.Create("PEM0000004", "Can communicate with birds to gather intelligence on the neighborhood squirrels' activities"),
                Permission.Create("PEM0000005", "Grants the ability to instantly teleport to any warm and sunny spot in the house"),
                Permission.Create("PEM0000006", "Allows the cat to enter stealth mode, becoming invisible to humans but not to other cats"),
                Permission.Create("PEM0000007", "Grants the authority to declare 'nap time' at any moment, with immediate compliance from all feline subjects"),
            };

            var groups = new List<Group>()
            {
                Group.Create("GRP0000001", "Generalissimo Feline", "Supreme leader of the feline army, with absolute authority over all cat operations"),
                Group.Create("GRP0000002", "Cosmic Commander", "Master strategist and explorer of the cosmos, leading the space cat division to new frontiers"),
                Group.Create("GRP0000003", "Sergeant Purrfect", "Expert in tactical maneuvers and skilled in coordinating missions for the normal cat battalion"),
                Group.Create("GRP0000004", "Snooze Sergeant", "Champion of leisure and relaxation, setting the pace for the lazy cat squadron"),
            };

            var groupPermissions = new List<GroupPermission>()
            {
                //Generalissimo Feline
                GroupPermission.Create("GRP0000001","PEM0000001"),
                GroupPermission.Create("GRP0000001","PEM0000002"),
                GroupPermission.Create("GRP0000001","PEM0000003"),
                GroupPermission.Create("GRP0000001","PEM0000004"),
                GroupPermission.Create("GRP0000001","PEM0000005"),
                GroupPermission.Create("GRP0000001","PEM0000006"),
                GroupPermission.Create("GRP0000001","PEM0000007"),

                //Cosmic Commander
                GroupPermission.Create("GRP0000002","PEM0000002"),
                GroupPermission.Create("GRP0000002","PEM0000003"),
                GroupPermission.Create("GRP0000002","PEM0000004"),

                //Sergeant Purrfect
                GroupPermission.Create("GRP0000003","PEM0000004"),
                GroupPermission.Create("GRP0000003","PEM0000005"),
                GroupPermission.Create("GRP0000003","PEM0000006"),
                
                //Sergeant Purrfect
                GroupPermission.Create("GRP0000004","PEM0000007"),
            };

            var loginStatuses = new List<LoginStatusLookup>()
            {
                LoginStatusLookup.Create(LoginStatuses.Success,"The log in attempt was successful"),
                LoginStatusLookup.Create(LoginStatuses.FailedIncorrectEmail,"The log in attempt was not successful due to invalid email"),
                LoginStatusLookup.Create(LoginStatuses.FailedIncorrectPassword,"The log in attempt was not successful due to invalid password"),
            };


            var user = User.Create("DarthMeow", "Meowlord", "meow@meow.com", "meow");

            context.Users.Add(user);
            context.Permissions.AddRange(permissions);
            context.LoginStatusLookup.AddRange(loginStatuses);
            context.Groups.AddRange(groups);
            context.GroupPermission.AddRange(groupPermissions);
            
            //Call it here to get the user Id
            context.SaveChanges();
            context.UserGroups.Add(UserGroup.Create(user.EntryId, "GRP0000001"));
            context.SaveChanges();


        }
    }
}

