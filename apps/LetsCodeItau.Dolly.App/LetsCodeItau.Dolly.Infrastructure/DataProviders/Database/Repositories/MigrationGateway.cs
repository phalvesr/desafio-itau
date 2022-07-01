using System.Data;
using LetsCodeItau.Dolly.Application.Gateways;

namespace LetsCodeItau.Dolly.Infrastructure.DataProviders.Database.Repositories;

public class MigrationGateway : IMigrationGateway
{
    private readonly IDbConnection dbConnection;

    public MigrationGateway(IDbConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public void EnsureCreation()
    {
        AssureTablesCreation();
    }

    private void AssureTablesCreation()
    {
        try
        {
            var query = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Points INTEGER NOT NULL DEFAULT 0,
                    Username TEXT NOT NULL,
                    DisplayName TEXT NOT NULL DEFAULT username,
                    UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                    GlobalId TEXT NOT NULL,
                    RegisterDate DATETIME NOT NULL,
                    LastLogin DATETIME NULL,
                    Deleted BIT NULL DEFAULT NULL
                );

                CREATE TABLE IF NOT EXISTS Ratings (
                    RatingId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                    UserId INTEGER NOT NULL,
                    MovieId INTEGER NOT NULL,
                    Rate REAL NOT NULL,
                    FOREIGN KEY(UserId) REFERENCES Users(UserId)
                );

                CREATE TABLE IF NOT EXISTS Movies (
                    MovieId INTEGER PRIMARY KEY AUTOINCREMENT, 
                    ImdbId TEXT NOT NULL,
                    Title TEXT NOT NULL,
                    Plot TEXT NOT NULL,
                    RuntimeMinutes INTEGER NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Comments (
                    CommentId INTEGER PRIMARY KEY AUTOINCREMENT,
                    MovieId INTEGER NOT NULL,
                    UserId INTEGER NOT NULL,
                    Content TEXT NOT NULL,
                    ReplyId INTEGER NULL,
                    Deleted BIT NOT NULL,
                    FOREIGN KEY(UserId) REFERENCES Users(UserId),
                    FOREIGN KEY(MovieId) REFERENCES Movies(MovieId)
                );
                
                CREATE TABLE IF NOT EXISTS CommentReactions (
                    CommentReactionId INTEGER PRIMARY KEY AUTOINCREMENT,
                    AuthorId INTEGER NOT NULL,
                    ReactId INTEGER NOT NULL,
                    Reaction INTEGER NOT NULL,
                    FOREIGN KEY(AuthorId) REFERENCES Users(UserId),
                    FOREIGN KEY(ReactId) REFERENCES Comments(CommentId)
                );
                
                CREATE TABLE IF NOT EXISTS CommentReactionDomain (
                    DomainId INTEGER PRIMARY KEY AUTOINCREMENT,
                    ReactionDescription TEXT NOT NULL,
                    DomainRepresentation INTEGER NOT NULL
                );

                INSERT INTO CommentReactionDomain 
                    (ReactionDescription, DomainRepresentation)
                VALUES ('LIKED', 1), ('DISLIKED', 2);";

            dbConnection.Open();
            var command = dbConnection.CreateCommand();
            command.CommandText = query;
            command.ExecuteReader();
        }
        finally
        {
            dbConnection.Close();
        }
    }
}
