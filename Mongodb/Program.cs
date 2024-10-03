using System.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongodb
{
    public class Program
    {
        static async Task Main()
        {
            // Read MongoDB connection string from app.config
            string connectionString = ConfigurationManager.AppSettings["mongodbconnectionstring"];
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("GardenGroup"); // Database name
            var usersCollection = database.GetCollection<User>("Employees"); // Collection name

            // Fetch all users with plaintext passwords
            var users = await usersCollection.Find(new BsonDocument()).ToListAsync();

            // Loop through each user, hash their password, and update the record
            foreach (var user in users)
            {
                Console.WriteLine($"Processing user: {user.Username}");

                if (string.IsNullOrEmpty(user.Password))
                {
                    Console.WriteLine($"User {user.Username} has an empty password, skipping.");
                    continue; // Skip users with empty passwords
                }

                // Hash the plaintext password
                string hashedPassword = HashPassword(user.Password);
                Console.WriteLine($"Hashed Password for {user.Username}: {hashedPassword}");

                // Check if the current password is different from the hashed password
                if (user.Password != hashedPassword)
                {
                    // Create an update definition to update the password field 
                    var update = Builders<User>.Update.Set(u => u.Password, hashedPassword);

                    // Apply the update to the user in MongoDB
                    var updateResult = await usersCollection.UpdateOneAsync(
                        Builders<User>.Filter.Eq(u => u.Username, user.Username), // Change to Username for testing (_id might give errors)
                        update
                    );
                    
                    //Messages just to know where the script is at and troubleshooting messages
                    Console.WriteLine($"Matched Count: {updateResult.MatchedCount}, Modified Count: {updateResult.ModifiedCount}");
                    if (updateResult.ModifiedCount > 0)
                    {
                        Console.WriteLine($"User {user.Username}'s password has been hashed and updated."); //Password has been successfuly hashed and updated
                    }
                    else
                    {
                        Console.WriteLine($"No changes made for user {user.Username}."); //Something went wrong and password has not been hashed or updated
                    }
                }
                else
                {
                    Console.WriteLine($"Password for user {user.Username} is already hashed, skipping."); //Password is already hashed
                }
            }

            Console.WriteLine("Password hashing completed for all users."); //End of the hashing script
        }
        //Using BCrypt to hash the password
        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}