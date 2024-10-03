using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Mongodb;

public class User //Add the parameters of each of the fields in the database
{
    [BsonId]
    [BsonElement("_id")]
    public Guid Id { get; set; } // UUID type

    [BsonElement("first_name")]
    public string FirstName { get; set; }

    [BsonElement("last_name")]
    public string LastName { get; set; }

    [BsonElement("email")]
    public string Email { get; set; }

    [BsonElement("phone")]
    public string Phone { get; set; }

    [BsonElement("user_type")]
    public string UserType { get; set; }

    [BsonElement("password")]
    public string Password { get; set; } // Hashed password

    [BsonElement("username")]
    public string Username { get; set; }
}